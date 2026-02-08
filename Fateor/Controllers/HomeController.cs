using Fateor.Application.Notification.Services;
using Fateor.Application.UserAccess.Services;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;
using Org.BouncyCastle.Asn1.Cms;
using oSoft.Mail.Services;
using System.Diagnostics;
using System.Security.Claims;

namespace Fateor.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;
        private readonly IWebHostEnvironment _environment;
        private readonly Settings _settings;

        public HomeController(ILogger<HomeController> logger,
                              IUserService userService,
                              INotificationService notificationService,
                              IWebHostEnvironment environment,
                              Settings settings)
        {
            _logger=logger;
            _userService=userService;
            _notificationService=notificationService;
            _environment=environment;
            _settings=settings;
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionFeature = HttpContext.Features
                .Get<IExceptionHandlerPathFeature>();

            if (exceptionFeature != null)
            {
                _logger.LogError(exceptionFeature.Error, "An unhandled exception occurred on path {Path}", exceptionFeature.Path);
            }
        
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Route("login")]
        public async Task<ActionResult> Login(Models.LoginViewModel model)
        {
            if (Request.Method == "GET")
                return View(model);
            else
            {
                if (ModelState.IsValid)
                {
                    List<string> messages;
                    var user = _userService.Login(model.Email, model.Password, out messages);
                    if (user != null)
                    {
                        var claims = new List<Claim>()
                        {
                            new Claim(ClaimTypes.Name , user.Login),
                            new Claim(ClaimTypes.Email, user.Email),
                            new Claim(ClaimTypes.NameIdentifier, user?.UserId?.ToString() ?? ""),
                            new Claim(ClaimTypes.Role, "admin")
                        };

                        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                        var principal = new ClaimsPrincipal(identity);

                        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

                        // Check if the query string has a ReturnUrl param and redirect there
                        if (Request.Query.ContainsKey("ReturnUrl"))
                        {
                            var returnUrl = Request.Query["ReturnUrl"].ToString();
                            return Redirect(returnUrl);
                        }
                        return (RedirectToAction("Index", "Internal"));
                    }
                    else
                    {
                        model.Messages = messages;
                        model.LoginFailed = true;
                        model.LoginMessage = "Login Incorrecto";
                        return View(model);
                    }
                }
                else
                {
                    return View(model);
                }
            }
        }

        [Route("logout")]
        public ActionResult Logout()
        {
            HttpContext.Session.Clear();
            return (RedirectToAction("Index"));
        }

        [HttpGet("user/resetpassword/{token}")]
        public ActionResult PasswordReset(string token)
        {

            try
            {
                var decrypted = _userService.DecryptPasswordResetToken(token);
                if (decrypted.ValidTo >= DateTime.UtcNow)
                {
                    var model = new PasswordResetViewModel();
                    model.UserId = decrypted.UserId;
                    return View(model);
                }
                TempData["messages"] = new List<string>() { "El tiempo para restablecer el password se venció.", "Por favor solicítalo de nuevo" };
                return RedirectToAction("RequestPasswordChange");
            }
            catch
            {
                TempData["messages"] = new List<string>() { "Ocurrió un error al restablecer el password.", "Por favor solicítalo de nuevo" };
                return RedirectToAction("RequestPasswordChange");
            }
        }

        [HttpPost]
        [Route("user/resetpassword/{token}")]
        public ActionResult PasswordReset(PasswordResetViewModel model)
        {
            if (ModelState.IsValid)
            {
                (var ok, var errors) = _userService.UpdatePassword(model.UserId, model.Password);
                if (ok)
                {
                    TempData["messages"] = new List<string> { "El password ha sido actualizado" };
                    return RedirectToAction("Login", "Home");
                }
                model.Messages = errors;
            }
            return View(model);
        }

        [HttpGet]
        [Route("user/resetpassword")]
        public ActionResult RequestPasswordChange()
        {
            return View(new RequestPasswordResetViewModel());
        }

        [HttpPost]
        [Route("user/resetpassword")]
        public ActionResult RequestPasswordChange(RequestPasswordResetViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _userService.GetUser(model.Login, model.Email);
                if (user != null && string.Compare(model.Login, user.Login, true) == 0)
                {
                    _notificationService.NotifyPasswordChangeRequest(user);
                    TempData["messages"] = new List<string>() { "Se ha enviado el correo para restablecer el password" };
                    return RedirectToAction("Login");
                }
                else
                {
                    model.Messages = new List<string>() { "El usuario no existe" };
                    return View(model);
                }
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        [Route("pages/{page}")]
        public ActionResult Pages(string page)
        {
            var model = new PageViewModel();
            var filePath = Path.Combine(_environment.WebRootPath, $"/App_Data/UserPages/{page}.html");
            model.Content = System.IO.File.ReadAllText(filePath);
            return View(model);
        }

        [HttpGet]
        [Route("imau")]
        public ActionResult Imau()
        {
            GC.Collect();
            return Ok();
        }     

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Site = _settings.Site;
            ViewBag.Title = _settings.Site_Name;
            base.OnActionExecuting(filterContext);
        }
    }
}
