using Fateor.Application.Bulletin.Services;
using Fateor.Application.Location.Services;
using Fateor.Application.Organization.Service;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Mvc;

namespace Fateor.Controllers
{

    public class BulletinController : FateorControllerBase
    {
        private readonly IBulletinService _bulletinService;
        private readonly IUnitService _unitService;
        private readonly IStatusService _statusService;
        private readonly ILocationService _locationService;

        public BulletinController(ILogger<BulletinController> logger,
                                  IMapper mapper,
                                  IEncryptionService encryptionService,
                                  IBulletinService bulletinService,
                                  IUnitService unitService,
                                  IStatusService statusService,
                                  ILocationService locationService,
                                  Settings settings) : base(logger, mapper, encryptionService, settings)
        {
            _bulletinService=bulletinService;
            _unitService=unitService;
            _statusService=statusService;
            _locationService=locationService;
        }

        // GET: Bulletin
        public ActionResult Index()
        {
            return View();
        }

        [Route("{BulletinId}")]
        public ActionResult Edit(int bulletinId)
        {
            var bulletin = _bulletinService.GetBulletin(bulletinId);

            var model = _mapper.Map<Application.Bulletin.Entities.Bulletin, BulletinViewModel>(bulletin);
            model.Units = _unitService.GetUnits().ToList();
            model.Countries = _locationService.GetCountries().ToList();
            model.Statuses = _statusService.GetStatuses().ToList();

            return View(model);

        }

        [Route("")]
        public async Task<ActionResult> List()
        {
            var bulletins = await _bulletinService.GetBulletinsAsync();
            var model = new BulletinListViewModel();
            model.Bulletins = bulletins.ToList();
            return View(model);
        }

    }
}