using Dapper.Contrib.Extensions;
using Org.BouncyCastle.Asn1.Cms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Application.Organization.Dto
{
    [Table("org_member_status")]
    public class MemberStatus
    {
        [ExplicitKey]
        public int MemberId { get; set; }

        [ExplicitKey]
        public DateTime StatusDate { get; set; }

        public int UnitId { get; set; }

        public int StatusId { get; set; }

        [Computed]
        public bool IsActiveMember { get; set; }

        public DateTime LastModificationDate { get; set; } = DateTime.Now;

        public int LastModificationUserId { get; set; } = 0;

        #region Computed Attributes

        [Computed]
        public string? UnitName { get; set; }

        [Computed]
        public string? StatusDescription { get; set; }

        [Computed]
        public string? LastModificationUser { get; set; }

        #endregion
        public bool IsSameAs(MemberStatus other)
        {
            return other != null && other.StatusId == StatusId && other.UnitId == UnitId;
        }

    }
}