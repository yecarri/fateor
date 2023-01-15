using AutoMapper;
using Fateor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.VisualBasic;
using User = Fateor.Application.UserAccess.Entities.User;

namespace Fateor.Controllers
{
#pragma warning disable CS8629 // Nullable value type may be null.
    [Authorize]
    public sealed class UserController : FateorControllerBase
    {
        private readonly Infrastructure.Services.IEncryptionService _encryptionService;
        private readonly Application.UserAccess.Services.IUserService _userService;

        public UserController(ILogger<HomeController> logger,
                              Infrastructure.Mappers.IMapper mapper,
                              Infrastructure.Services.IEncryptionService encryptionService,
                              Application.UserAccess.Services.IUserService userService)
            : base(logger, mapper, encryptionService)
        {
            _userService = userService;
            _encryptionService=encryptionService;
        }

        [HttpGet]
        [Route("User")]
        public ActionResult List()
        {
            var model = _userService.GetAllUsers();
            return View(model);
        }

        [HttpGet]
        [Route("User/{id:int}")]
        [Route("User/Edit/{id:int}")]
        public ActionResult Edit(int id)
        {
            var usr = _userService.GetUser(id);
            if (usr == null)
            {
                TempData["messages"] = new List<string> { $"No se ha encontrado ningún miembro con id {id}", "Por favor revisa la dirección e intenta de nuevo." };
                return RedirectToAction("Index", "Home");
            }
            else
            {
                var model = _mapper.Map<User, UserViewModel>(usr);
                ViewBag.Title = "Editar Usuario";
                return View("Edit", model);
            }
        }

        [HttpPost]
        [Route("User/{id:int}")]
        [Route("User/Edit/{id:int}")]
        public ActionResult Edit(UserViewModel model)
        {
            var user = _mapper.Map<UserViewModel, User>(model);

            (bool ok, List<string> messages) = _userService.UpdateUser(user, CurrentUser?.UserId.Value??0);

            if (!ok)
                model.Messages = messages;

            return View("Edit", model);
        }

        [HttpGet]
        [Route("User/Create")]
        public ActionResult Create()
        {
            var model = new UserViewModel();
            return View("Edit", model);
        }

        [HttpPost]
        [Route("User/Create")]
        public ActionResult Create(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = _mapper.Map<UserViewModel, User>(model);

                (bool ok, List<string> messages) = _userService.CreateUser(user, CurrentUser?.UserId.Value??0);

                if (ok)
                {
                    model = _mapper.Map<User, UserViewModel>(user);
                    return RedirectToAction("Edit", new { id = model.UserId });
                }
                else
                {
                    model.UserId = null;
                    model.Messages = messages;
                    return View("Edit", model);
                }
            }
            else
                return View("Edit", model);
        }
    }
}
#pragma warning restore CS8629 // Nullable value type may be null.