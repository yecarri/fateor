using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Service;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Fateor.Controllers
{


    public class MemberFeeController : FateorControllerBase
    {
        private readonly IStatusService _statusService;
        private readonly IMemberFeeBatchService _memberFeeBatchService;

        public MemberFeeController(ILogger<MemberFeeController> logger,
                                   IMapper mapper,
                                   IEncryptionService encryptionService,
                                   IStatusService statusService,
                                   IMemberFeeBatchService memberFeeBatchService) : base(logger, mapper, encryptionService)
        {
            _statusService=statusService;
            _memberFeeBatchService=memberFeeBatchService;
        }

        [HttpGet("fee/batch/list")]
        public IActionResult BatchList()
        {
            var model = new MemberFeeBatchListViewModel();
            model.DateFrom = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-2);
            model.DateTo = new DateOnly(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            //model.Statuses = _statusService.GetStatuses().Select(s => _mapper.Map<Status, StatusSelectViewModel>(s)).ToArray();
            model.Batches = _memberFeeBatchService.GetMemberFeeBatches(model.DateFrom, model.DateTo);
            return View(model);
        }

        [HttpPost("fee/batch/list")]
        public IActionResult BatchList(MemberFeeBatchViewModel memberFeeBatchViewModel)
        {
            return View();
        }
    }
}
