using Fateor.Application;
using Fateor.Application.Organization;
using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Service;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Fateor.Controllers
{


    public class MemberFeeController : FateorControllerBase
    {
        private readonly IStatusService _statusService;
        private readonly IMemberFeeService _memberFeeBatchService;

        public MemberFeeController(ILogger<MemberFeeController> logger,
                                   IMapper mapper,
                                   IEncryptionService encryptionService,
                                   IStatusService statusService,
                                   IMemberFeeService memberFeeBatchService,
                                   Settings settings) : base(logger, mapper, encryptionService, settings)
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
            model.Statuses = _statusService.GetVotingStatuses().ToArray();
            model.Periodicity = Application.Organization.Periodicity.Quarterly;
            model.DateRange = GetDefaultDateRange(model.Periodicity);
            model.ReferencePattern = GetReferencePattern(model.Periodicity);
            model.BatchTitle =$"Cuota {model.Periodicity.Description()}";
            return View("Batch", model);
        }

        private DateRange GetDefaultDateRange(Application.Organization.Periodicity periodicity)
        {
            int y, m;
            y = DateTime.Today.Year;
            DateTime dateFrom, dateTo;

            switch (periodicity)
            {
                case Application.Organization.Periodicity.Quarterly:
                    m = ((DateTime.Today.Month-1) / 3)*3+1;
                    dateFrom = new DateTime(y, m, 1);
                    dateTo = new DateTime(y, m+2, DateTime.DaysInMonth(y, m+2));
                    break;
                case Application.Organization.Periodicity.Monthly:
                    m = DateTime.Today.Month;
                    dateFrom = new DateTime(y, m, 1);
                    dateTo = new DateTime(y, m, DateTime.DaysInMonth(y, m));
                    break;
                case Application.Organization.Periodicity.Yearly:
                    dateFrom=new DateTime(y, 1, 1);
                    dateTo=new DateTime(y, 12, 31);
                    break;
                default:
                    dateFrom = DateTime.Today;
                    dateTo = DateTime.Today.AddDays(30);
                    break;
            }
            return new DateRange(dateFrom, dateTo);
        }

        private string GetReferencePattern(Application.Organization.Periodicity periodicity)
        {
            switch (periodicity)
            {
                case Application.Organization.Periodicity.Quarterly:
                    return "%FD.AAAA%-Q%FD.TR%-%U.ID(4)%";
                case Application.Organization.Periodicity.Monthly:
                    return "%FD.AAAA%%FD.MM%-%U.ID(4)%";
                case Application.Organization.Periodicity.Yearly:
                    return "%FD.AAAA%-%U.ID(4)%";
                default:
                    return "%FD.AAAA%%FD.MM%%FD.DD%-%FH.AAAA%%FH.MM%%FH.DD%-%U.ID(4)%";
            }
        }

        [HttpPost("fee/batch/create")]
        public IActionResult CreateBatch(MemberFeeBatchViewModel model)
        {
            model.Statuses = _statusService.GetStatuses().Where(s => !s.IsDeleted && s.CanVote).ToArray();
            if (ModelState.IsValid)
            {
                var batch = _mapper.Map<MemberFeeBatchViewModel, MemberFeeBatch>(model);

                switch (model.Command)
                {
                    case "Generate":
                        var fees = _memberFeeBatchService.MockFees(batch);
                        model.MemberFees = fees;
                        break;
                    case "Save":
                        _memberFeeBatchService.SaveBatch(batch);
                        return Redirect($"fee/batch/edit/{batch.MemberFeeBatchId}");
                }

            }
            return View("Batch", model);
        }

        [HttpGet("fee/batch/edit/{batchFeeId}")]
        public IActionResult EditBatch(int batchFeeId)
        {
            var batch = _memberFeeBatchService.GetMemberFeeBatchById(batchFeeId, true);
            var model = _mapper.Map<MemberFeeBatch, MemberFeeBatchViewModel>(batch);
            model.Statuses = _statusService.GetVotingStatuses().ToArray();
            return View("Batch", model);
        }

        [HttpPost("fee/batch/edit/{batchFeeId}")]
        public IActionResult EditBatch(MemberFeeBatchViewModel model)
        {
            var batch = _mapper.Map<MemberFeeBatchViewModel, MemberFeeBatch>(model);
            _memberFeeBatchService.SaveBatch(batch);
            return View("Batch", model);
        }
    }
}
