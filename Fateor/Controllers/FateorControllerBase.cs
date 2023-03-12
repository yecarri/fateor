using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Fateor.Controllers
{
    public class FateorControllerBase : Controller
    {
        protected readonly ILogger _logger;
        protected readonly IMapper _mapper;
        protected readonly Settings _settings;
        private readonly IEncryptionService _encryptionService;
        protected User? CurrentUser { get; set; }

        public FateorControllerBase(ILogger logger, IMapper mapper, IEncryptionService encryptionService, Settings settings)
        {
            _logger=logger;
            _mapper=mapper;
            _encryptionService=encryptionService;
            _settings=settings; 
            CurrentUser = null;            
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Site = _settings.Site;
            var url = filterContext.HttpContext.Request.GetDisplayUrl();
            if (User.Identity?.IsAuthenticated ?? false)
            {
                var identity = (ClaimsIdentity?)User.Identity;
                _logger.LogInformation("{url} executing {authentication}", url, $"authenticated as {identity?.Name}");

                CurrentUser = new User
                {
                    Login = identity?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? "",
                    UserId = int.Parse(identity?.Claims?.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? "")
                };

                ViewBag.User = CurrentUser.Login;
            }
            else
            {
                _logger.LogInformation("{url} executing {authentication}", url, "not authenticated");
            }
        }

        public User DecryptUserIdentityToken(string s)
        {
            return _encryptionService.DecryptBase64<User>(s);
        }

        public string EncryptUserIdentityToken(User t)
        {
            return _encryptionService.EncryptBase64(t);
        }

    }
}
