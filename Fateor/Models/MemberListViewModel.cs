using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class MemberListViewModel : ViewModelBase
    {
        [DisplayName("País")]
        public int CountryId { get; set; } = -1;

        [DisplayName("Estado/Provincia")]
        public int RegionId { get; set; } = -1;

        [DisplayName("Sección")]
        public int UnitId { get; set; } = -1;

        [DisplayName("Estatus")]
        public int StatusId { get; set; } = -1;

        [DisplayName("Incluir miembros borrados")]
        public bool IncludeDeleted { get; set; }

        public List<Application.Organization.Entities.Member> Members { get; set; } = new List<Application.Organization.Entities.Member>();

        public List<Application.Location.Entities.Country> Countries { get; set; } = new List<Application.Location.Entities.Country>();

        public List<Application.Location.Entities.Region> Regions { get; set; } = new List<Application.Location.Entities.Region>();

        public List<Application.Organization.Entities.Unit> Units { get; set; } = new List<Application.Organization.Entities.Unit>();

        public List<Application.Organization.Entities.Status> Statuses { get; set; } = new List<Application.Organization.Entities.Status>();
    }
}