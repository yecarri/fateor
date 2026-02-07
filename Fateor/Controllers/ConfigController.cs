using Fateor.Application.Config.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fateor.Controllers
{

    [Authorize]
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

        [HttpPost("/config")]
        public IActionResult Index(Models.ConfigViewModel model)
        {
            if (ModelState.IsValid)
            {
                var configsToUpdate = new List<Fateor.Application.Config.Entities.Config>();
                foreach (var currentConfig in model.Current)
                {
                    var initialConfig = model.Initial.FirstOrDefault(c => c.ConfigCode == currentConfig.ConfigCode);
                    if (initialConfig == null || !initialConfig.Equals(currentConfig))
                    {
                        var configEntity = _Mapper.Map<Models.ConfigModel, Fateor.Application.Config.Entities.Config>(currentConfig);
                        configsToUpdate.Add(configEntity);
                    }
                }
                if (configsToUpdate.Count > 0)
                {
                    _ConfigService.UpdateConfigs(configsToUpdate);
                }
                model.Messages= ["Configurations updated successfully."];
                return RedirectToAction("Index");
            }
            model.Messages = ["There were errors in the submitted data."];
            return View(model);
        }
    }
}
