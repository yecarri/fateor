using Fateor.Application.Organization.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class UpdateStatusViewModel : ViewModelBase
    {
        public int MemberId { get; set; }

        [DisplayName("Unidad")]
        public int? UnitId { get; set; }

        [DisplayName("Estatus")]
        public int? StatusId { get; set; }

        [DisplayName("Fecha")]
        public DateTime StatusDate { get; set; }

        [DisplayName("Nombre")]
        public string GivenName { get; set; } = string.Empty;

        [DisplayName("Apellido")]
        public string Surname { get; set; } = string.Empty;

        public IEnumerable<Unit> Units { get; internal set; } = Enumerable.Empty<Unit>();

        public IEnumerable<Status> Statuses { get; internal set; } = Enumerable.Empty<Status>();

        [DisplayName("Historial")]
        public IList<MemberStatus> MemberStatuses { get; set; } = new List<MemberStatus>();
    }
}