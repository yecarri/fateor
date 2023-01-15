using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Organization.Dto
{
    [Table("org_member")]
    public class Member : IEquatable<Member>
    {
        [Key]
        public int MemberId { get; set; }
        public string Email { get; set; } = "";
        public string GivenName { get; set; } = "";
        public string Surname { get; set; } = "";
        public int IdentityDocumentTypeId { get; set; }
        public string IdentityDocumentNumber { get; set; } = "";
        public DateTime? BirthDate { get; set; }
        public string Phone { get; set; } = "";
        public string Address1 { get; set; } = "";
        public string Address2 { get; set; } = "";
        public string City { get; set; } = "";
        public string PostCode { get; set; } = "";
        public string MemberCode { get; set; } = "";
        public int? RegionId { get; set; }
        public int? CountryId { get; set; }
        public string ExternalUserId { get; set; } = "";
        public bool IsDeleted { get; set; }
        public int LastModificationUserId { get; set; }

        public bool Equals(Member? other)
        {
            if (other==null) return false;
            if (ReferenceEquals(this, other)) return true;
            if (MemberId != other.MemberId) return false;
            if (Email != other.Email) return false;
            if (GivenName != other.GivenName) return false;
            if (Surname!=other.Surname) return false;
            if (IdentityDocumentTypeId!=other.IdentityDocumentTypeId) return false;
            if (BirthDate!=other.BirthDate) return false;
            if (Phone != other.Phone) return false;
            if (Address1 != other.Address1) return false;
            if (Address2 != other.Address2) return false;
            if (City != other.City) return false;
            if (PostCode!= other.PostCode) return false;
            if (MemberCode != other.MemberCode) return false;
            if (RegionId != other.RegionId) return false;
            if (CountryId != other.CountryId) return false;
            if (ExternalUserId!=other.ExternalUserId) return false;
            return true;
        }
    }
}