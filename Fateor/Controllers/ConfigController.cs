using Fateor.Application.Config.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Microsoft.AspNetCore.Mvc;

namespace Fateor.Controllers
{
    public class ConfigController : FateorControllerBase
    {
        private readonly IConfigService _ConfigService;
        private readonly IMapper _Mapper;

        public ConfigController(IConfigService configService,
                                IMapper mapper,
                                ILogger<ConfigController> logger,
                                IEncryptionService encryptionService,
                                Settings settings) : base(logger, mapper, encryptionService, settings)
        {
            _ConfigService = configService;
            _Mapper=mapper;
        }

        [HttpGet("/config")]
        public IActionResult Index()
        {
            var model = new Models.ConfigViewModel();
            var configs = _Mapper.MapEnumerable<Fateor.Application.Config.Entities.Config, Models.ConfigModel>(_ConfigService.GetCongfigs());
            model.Initial = configs.ToArray();
            model.Current = configs.ToArray();
            return View(model);
        }
    }
}
