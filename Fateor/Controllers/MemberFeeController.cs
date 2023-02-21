using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Service;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

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
            model.DateFrom = new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1).AddMonths(-2);
            model.DateTo = new DateTime(DateTime.Today.Year, DateTime.Today.Month, DateTime.DaysInMonth(DateTime.Today.Year, DateTime.Today.Month));
            model.Statuses = _statusService.GetStatuses().Select(s => _mapper.Map<Status, StatusSelectViewModel>(s)).ToArray();
            model.Batches = _memberFeeBatchService.GetMemberFeeBatches(model.DateFrom, model.DateTo);
            return View(model);
        }

        [HttpPost("fee/batch/list")]
        public IActionResult BatchList(MemberFeeBatchListViewModel model)
        {
            if (ModelState.IsValid)
            {
                model.Batches = _memberFeeBatchService.GetMemberFeeBatches(model.DateFrom, model.DateTo);
            }
            return View(model);
        }

        [HttpGet("fee/batch/create")]
        public IActionResult CreateBatch()
        {
            var model = new MemberFeeBatchViewModel();
            model.Statuses = _statusService.GetStatuses().Where(s => !s.IsDeleted && s.CanVote).ToArray();
            model.Periodicity = 2;
            int y, m;
            y = DateTime.Today.Year;

            switch (model.Periodicity)
            {
                case 2:
                    m = ((DateTime.Today.Month-1) / 3)*3+1;
                    model.DateFrom = new DateTime(y, m, 1);
                    model.DateTo = new DateTime(y, m+2, DateTime.DaysInMonth(y, m+2));
                    model.ReferencePattern="%FD.AAAA%-Q%FD.TR%-%U.ID(4)%";
                    break;
                case 3:
                    m = DateTime.Today.Month;
                    model.DateFrom = new DateTime(y, m, 1);
                    model.DateTo = new DateTime(y, m, DateTime.DaysInMonth(y, m));
                    model.ReferencePattern="%FD.AAAA%%FD.MM%-%U.ID(4)%";
                    break;
                default:
                    model.ReferencePattern="%FD.AAAA%%FD.MM%%FD.DD%-%FH.AAAA%%FH.MM%%FH.DD%-%U.ID(4)%";
                    break;
            }

            model.BatchTitle ="Cuota";

            return View("Batch", model);
        }

        [HttpPost("fee/batch/create")]
        public IActionResult CreateBatch(MemberFeeBatchViewModel model)
        {
            model.Statuses = _statusService.GetStatuses().Where(s => !s.IsDeleted && s.CanVote).ToArray();
            if (ModelState.IsValid)
            {
                var batch = _mapper.Map<MemberFeeBatchViewModel, MemberFeeBatch>(model);
                // batch.StatusIds = model.SelectedStatuses;
                IEnumerable<MemberFee> fees = _memberFeeBatchService.MockFees(batch);
                model.MemberFees = fees;
            }
            return View("Batch", model);
        }

        [HttpGet("fee/batch/edit/{batchFeeId}")]
        public IActionResult EditBatch(int batchFeeId)
        {
            var batch = _memberFeeBatchService.GetMemberFeeBatchById(batchFeeId, true);
            var model = _mapper.Map<MemberFeeBatch, MemberFeeBatchViewModel>(batch);

            return View("Batch", model);
        }

        [HttpPost("fee/batch/edit/{batchFeeId}")]
        public IActionResult EditBatch(MemberFeeBatchViewModel model)
        {
            return View("Batch", model);
        }
    }
}
