using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Repositories.Interfaces;
using Fateor.Infrastructure.Mappers;
using Microsoft.Extensions.Logging;

namespace Fateor.Application.Organization.Service
{
    public class MemberFeeBatchService : IMemberFeeBatchService
    {
        private readonly IMemberFeeBatchRepository _memberFeeBatchRepository;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public MemberFeeBatchService(IMemberFeeBatchRepository memberFeeBatchRepository,
                                     IMapper mapper,
                                     ILogger<MemberFeeBatchService> logger)
        {
            _memberFeeBatchRepository=memberFeeBatchRepository;
            _mapper=mapper;
            _logger=logger;
        }

        public MemberFeeBatch[] GetMemberFeeBatches(DateTime dateFrom, DateTime dateTo)
        {
            return _memberFeeBatchRepository
                .GetMemberFeeBatches(dateFrom, dateTo)
                .Select(b => _mapper.Map<Dto.MemberFeeBatch, Entities.MemberFeeBatch>(b))
                .ToArray();
        }
    }
}
