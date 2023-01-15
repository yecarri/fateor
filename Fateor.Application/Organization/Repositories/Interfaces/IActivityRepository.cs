using oSoft.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Repositories.Interfaces
{
    public interface IActivityRepository : IRepository<Dto.Activity>, IDisposable
    {
        IEnumerable<Dto.Activity> GetActivities(DateTime? dateFrom, DateTime? dateTo);
        IEnumerable<Dto.Activity> GetActivitiesByMemberId(int memberId, DateTime? dateFrom, DateTime? dateTo);
    }
}
