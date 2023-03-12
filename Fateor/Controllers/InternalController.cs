using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fateor.Controllers
{
    public class InternalController : FateorControllerBase
    {
        public InternalController(ILogger<InternalController> logger,
                                  IMapper mapper,
                                  IEncryptionService encryptionService) : base(logger, mapper, encryptionService)
        {
        }

        [Route("Home")]
        [Authorize]
        public ActionResult Index()
        {
            var model = new ViewModelBase();
            if (TempData["messages"] is not null)
            {
                model.Messages = (IList<string>)(TempData["messages"]??new List<string>());
            }

            return View(model);
        }
    }
}
