using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Organization.Dto
{
    [Table("org_unit")]
    public class Unit
    {
        [Key]
        public int UnitId { get; set; }

        public string UnitName { get; set; } = "";

        public int? ParentUnitId { get; set; }

        public int? CoordinatorId { get; set; }

        public int LastModificationUserId { get; set; }
    }
}