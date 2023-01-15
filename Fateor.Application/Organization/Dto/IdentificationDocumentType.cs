using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Organization.Dto
{
    [Table("org_identitydocumenttype")]
    public class IdentityDocumentType
    {
        [Key]
        public int IdentityDocumentTypeId { get; set; }

        public string IdentityDocument { get; set; } = "";
    }
}