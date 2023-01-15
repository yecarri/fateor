using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Organization.Entities
{
    public class Unit
    {
        public int UnitId { get; set; }

        public string UnitName { get; set; } = "";  

        public int? ParentUnitId { get; set; }

        public int? CoordinatorId { get; set; }

        public bool IsDeleted { get; set; }

        public static bool IsAncestorOf(Unit unit, Unit other, IEnumerable<Unit> units)
        {
            if (other == null)
                return false;

            if (other.ParentUnitId == unit.UnitId)
                return true;

            var parent = units.FirstOrDefault(p => p.UnitId == other.ParentUnitId);

            if (parent == null) return false;   

            return IsAncestorOf(unit, parent, units);
        }
    }
}