using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Repositories.Interfaces;
using Fateor.Infrastructure.Mappers;

namespace Fateor.Application.Organization.Service
{
    public sealed class StatusService : IStatusService
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IMapper _mapper;

        public StatusService(IStatusRepository statusRepository, IMapper mapper)
        {
            _statusRepository=statusRepository;
            _mapper=mapper;
        }

        public IEnumerable<Status> GetStatuses()
        {
            var results = _statusRepository.GetAll();
            return results.Select(x => _mapper.Map<Organization.Dto.Status, Status>(x)).ToList();
        }
    }
}
