using Fateor.Application.Bulletin.Entities;
using Fateor.Application.Location.Entities;
using Fateor.Application.Organization.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class BulletinViewModel : ViewModelBase
    {
        [DisplayName("Id")]
        public int? BulletinId { get; set; }
        [DisplayName("Comunicado")]
        public string Description { get; set; } = string.Empty;
        [DisplayName("Título")]
        public string Title { get; set; } = string.Empty;
        [DisplayName("Texto/Html")]
        public string Text { get; set; } = string.Empty;
        [DisplayName("Estado")]
        public byte BulletinStatusId { get; set; }
        [DisplayName("Estado")]
        public string BulletinStatusName { get; set; } = string.Empty;
        [DisplayName("Inicio del envío")]
        public DateTime? StartDate { get; set; }
        [DisplayName("Finalización del envío")]
        public DateTime? EndDate { get; set; }
        [DisplayName("Borrado")]
        public bool IsDeleted { get; set; }
        [DisplayName("Miembros a los que se envió")]
        public IEnumerable<BulletinMember> Members { get; set; } = new List<BulletinMember>();
        [DisplayName("Incluir miembros con estos estatus")]
        public IList<BulletinStatusFilter> StatusFilters { get; set; } = new List<BulletinStatusFilter>();
        [DisplayName("Incluir miembros en estos países")]
        public IList<BulletinCountryFilter> CountryFilters { get; set; } = new List<BulletinCountryFilter>();
        [DisplayName("Incluir miembros en de estas unidades")]
        public IList<BulletinUnitFilter> UnitFilters { get; set; } = new List<BulletinUnitFilter>();
        [DisplayName("Fecha de Modificación")]
        public DateTime LastModificationDate { get; set; }
        public IList<Status>? Statuses { get; set; }
        public IList<Unit>? Units { get; set; }
        public IList<Country>? Countries { get; set; }

        public IList<dynamic> Actions { get; private set; } = new List<dynamic>()
        {
            new { Text = "Incluir", Value = 1},
            new { Text = "Excluir", Value = 2}
        };
    }
}