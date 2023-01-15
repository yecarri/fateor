using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Organization.Entities
{
    public class Member
    {
        public int MemberId { get; set; }
        public string Email { get; set; } = "";
        public string GivenName { get; set; } = "";
        public string Surname { get; set; } = "";
        public int IdentityDocumentTypeId { get; set; }
        public string IdentityDocumentNumber { get; set; } = "";
        public DateTime? BirthDate { get; set; }
        public string Phone { get; set; } = "";
        public string Address1 { get; set; } = "";
        public string? Address2 { get; set; }
        public string City { get; set; } = "";
        public string PostCode { get; set; } = "";
        public int? RegionId { get; set; }
        public int? CountryId { get; set; }
        public string? ExternalUserId { get; set; }
        public string? MemberCode { get; set; }
        public bool IsDeleted { get; set; }
        public IList<MemberStatus>? MemberStatuses { get; set; } = null;
        public string FullName { get => $"{GivenName} {Surname}"; }
        public string Address
        {
            get
            {
                if (string.IsNullOrEmpty(Address2))
                    return Address1;
                else
                    return $"{Address1} {Address2}";
            }
        }
        public MemberStatus? LastStatus
        {
            get => MemberStatuses?.OrderByDescending(s => s.StatusDate).FirstOrDefault();
        }
    }
}