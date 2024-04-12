using Fateor.Application.Organization.Entities;

namespace Fateor.Application.Organization.Service
{
    public interface IStatusService
    {
        IEnumerable<Status> GetStatuses();
        IEnumerable<Status> GetVotingStatuses();
    }
}