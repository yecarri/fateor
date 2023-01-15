using Fateor.Application.Organization.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class UnitListViewModel : ViewModelBase
    {
        public List<Unit> Units { get; set; } = new List<Unit>();

        public IEnumerable<Member> Coordinators { get; set; } = new List<Member>();

    }
}