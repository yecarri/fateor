using Fateor.Application.Organization.Repositories.Interfaces;
using Fateor.Infrastructure.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Service
{
    public sealed class IdentityDocumentService : IIdentityDocumentService
    {
        private readonly IIdentityDocumentTypeRepository _repo;
        private readonly IMapper _mapper;

        public IdentityDocumentService(IIdentityDocumentTypeRepository repo, IMapper mapper)
        {
            _repo=repo;
            _mapper=mapper;
        }

        public IEnumerable<Organization.Entities.IdentityDocumentType> GetDocumentTypes()
        {
            var rs = _repo.GetAll();
            return rs.Select(x => _mapper.Map<
                Dto.IdentityDocumentType,
                Entities.IdentityDocumentType>(x));
        }
    }
}
