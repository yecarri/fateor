using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Organization.Dto
{
    [Table("org_status")]
    public class Status
    {
        [Key]
        public int StatusId { get; set; }

        public string StatusDescription { get; set; } = "";

        public bool CanVote { get; set; }

        public bool CanBeElected { get; set; }

        public bool IsActiveMember { get; set; }

        public bool IsDeleted { get; set; }
    }
}