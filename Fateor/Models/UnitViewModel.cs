using Fateor.Application.Organization.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class UnitViewModel : ViewModelBase
    {
        [Required(ErrorMessage = "{0} es requerido")]
        public int UnitId { get; set; }

        [Required(ErrorMessage = "El nombre de la sección es obligatorio")]
        [MaxLength(50)]
        public string UnitName { get; set; } = string.Empty;

        public int? ParentUnitId { get; set; }

        public int? CoordinatorId { get; set; }

        public IEnumerable<Unit> Units { get; set; } = Enumerable.Empty<Unit>();

        public IEnumerable<Member> Members { get; set; } = Enumerable.Empty<Member>();

        public bool New { get; set; }
    }
}