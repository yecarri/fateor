using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IMemberActivityRepository : IRepository<Dto.MemberActivity>, IDisposable
    {
        IEnumerable<Dto.MemberActivity> GetByMemberId(int memberId, DateTime? dateFrom = null, DateTime? dateTo = null);

        IEnumerable<Dto.MemberActivity> GetByActivityId(int activityId);

        IEnumerable<Dto.MemberActivity> GetByDates(DateTime? dateFrom = null, DateTime? dateTo = null);

        void Update(IEnumerable<Dto.MemberActivity> participants);
    }
}
