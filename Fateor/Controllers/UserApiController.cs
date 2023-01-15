using Fateor.Application.Notification;
using Fateor.Application.UserAccess.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace Fateor.Controllers
{
    [ApiController]
    public class UserApiController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly INotificationService _notificationService;

        public UserApiController(IUserService userService, INotificationService notificationService)
        {
            _userService=userService;
            _notificationService=notificationService;
        }

        [HttpGet(@"/api/user/{id:int}/passwordreset")]
        public IActionResult PasswordReset(int id)
        {
            var user = _userService.GetUser(id);
            if (user != null)
            {
                _notificationService.NotifyPasswordChangeRequest(user);
                return Ok();
            }
            return BadRequest();
        }
    }
}
