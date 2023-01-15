using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Organization.Entities
{
    public class IdentityDocumentType
    {
        public int IdentityDocumentTypeId { get; set; }

        public string IdentityDocument { get; set; } = "";
    }
}