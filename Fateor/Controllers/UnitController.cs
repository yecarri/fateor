using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Service;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Google.Protobuf;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;

namespace Fateor.Controllers
{
    [Authorize]
    public class UnitController : FateorControllerBase
    {
        private readonly IMemberService _memberService;
        private readonly IUnitService _unitService;

        public UnitController(ILogger<UnitController> logger,
                              IMapper mapper,
                              IEncryptionService encryptionService,
                              IMemberService memberService,
                              IUnitService unitService,
                              Settings settings) : base(logger, mapper, encryptionService, settings)
        {
            _memberService=memberService;
            _unitService=unitService;
        }

        // GET: Unit
        [HttpGet("Unit")]
        public ActionResult List()
        {
            var model = new UnitListViewModel();
            model.Coordinators = _memberService.GetElegibleMembers();
            model.Units = _unitService.GetUnits().ToList();
            return View(model);
        }

        [HttpGet("Unit/{id:int}")]
        [HttpGet("Unit/Edit/{id:int}")]
        public ActionResult Edit(int id)
        {

            var units = _unitService.GetUnits().ToList();
            var unit = units.FirstOrDefault(u => u.UnitId == id);
            if (unit == null)
            {
                TempData["messages"] = new List<string> { $"No se ha encontrado ninguna sección con id {id}", "Por favor revisa la dirección e intenta de nuevo." };
                return RedirectToAction("Index", "Home");
            }

            var model = _mapper.Map<Unit, UnitViewModel>(unit);
            model.Members = GetActiveMembersList();
            model.Units = GetPosibleParentUnitsDDL(unit, units);
            return View(model);
        }

        [HttpPost("Unit/{id:int}")]
        [HttpPost("Unit/Edit/{id:int}")]
        public ActionResult Edit(UnitViewModel model)
        {
            var units = _unitService.GetUnits().ToList();
            bool ok;
            List<string> messages = null;
            Unit unit = UnitFromViewModel(model);
            if (ModelState.IsValid)
            {                
                (ok, messages) = _unitService.UpdateUnit(unit, CurrentUser.UserId.Value);
            }
            model.Members = GetActiveMembersList();
            model.Units = GetPosibleParentUnitsDDL(unit, units);
            model.Messages = messages;
            return View(model);
        }

        [HttpGet("Unit/Create")]
        public ActionResult Create()
        {
            var model = new UnitViewModel();
            model.New = true;
            model.Units = GetUnitsDD();
            model.Members = GetActiveMembersList();
            return View("Edit", model);
        }

        [HttpPost("Unit/Create")]
        public ActionResult Create(UnitViewModel model)
        {
            model.New = true;
            var unit = UnitFromViewModel(model);
            bool ok = false;
            IList<string> messages = null;
            if (ModelState.IsValid)
            {
                try
                {
                    (ok, messages) = _unitService.CreateUnit(unit, CurrentUser.UserId.Value);
                    if (ok)
                    {
                        return RedirectToAction("Edit", new { Id = unit.UnitId });
                    }
                    else
                    {
                        model.Messages = messages;
                    }

                }
                catch (Exception ex)
                {
                    model.Messages = new List<string>()
                    {
                        "No se pudo crear el registro de la Sección",
                        ex.Message
                    };
                }
            }
            model.Units = GetUnitsDD();
            model.Members = GetActiveMembersList();
            
            if (model.Messages == null)
                model.Messages = [];

            return View("Edit", model);
        }

        private List<Unit> GetPosibleParentUnitsDDL(Unit unit, List<Unit> units)
        {
            var selectUnits = units.Where(u => u.UnitId != unit.UnitId &&
                !Unit.IsAncestorOf(unit, u, units))
                .ToList();

            selectUnits.Add(new Unit() { UnitId = -1, UnitName = "- ninguna -" });

            return selectUnits;
        }

        private IEnumerable<Unit> GetUnitsDD()
        {
            var units = _unitService.GetUnits().ToList();
            units.Insert(0, new Unit { UnitId = -1, UnitName = "- ninguna -" });
            return units;
        }

        private Unit UnitFromViewModel(UnitViewModel model)
        {
            var unit = _mapper.Map<UnitViewModel, Unit>(model);
            if (unit.ParentUnitId <= 0)
                unit.ParentUnitId = null;
            if (unit.CoordinatorId <= 0)
                unit.CoordinatorId = null;
            return unit;
        }

        private IEnumerable<Member> GetActiveMembersList()
        {
            var members = _memberService.GetElegibleMembers().OrderBy(m => m.FullName).ToList();
            members.Add(new Member { MemberId = -1, GivenName = "- ninguno -" });
            return members;
        }
    }
}