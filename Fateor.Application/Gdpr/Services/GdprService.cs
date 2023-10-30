using Fateor.Application.Gdpr.Repositories.Interfaces;
using Fateor.Application.Gdpr.Repositories.MySql;
using Fateor.Application.Organization.Entities;
using Fateor.Infrastructure.Mappers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Gdpr.Services
{
    public class GdprService : IGdprService
    {
        private readonly IGdprUnitOfWork _GdprUnitOfWork;
        private readonly IMapper _Mapper;
        private readonly ILogger _logger;

        public GdprService(IGdprUnitOfWork gdprUnitOfWork, IMapper mapper, ILogger<GdprService> logger)
        {
            _GdprUnitOfWork=gdprUnitOfWork;
            _Mapper=mapper;
            _logger=logger;
        }
        public void Save(Entities.Gdpr gdpr)
        {
            try
            {
                _GdprUnitOfWork.BeginTransaction();
                var gdprDto = _Mapper.Map<Entities.Gdpr, Dto.Gdpr>(gdpr);
                _GdprUnitOfWork.GdprRepository.Insert(gdprDto);

                if (gdpr.Members != null)
                {
                    var members = gdpr.Members.Select(m => new Dto.GdprMember { GdprId = gdprDto.GdprId, MemberId = m.MemberId }).ToList();
                    foreach (var member in members)
                    {
                        _GdprUnitOfWork.GdprMemberRepository.Insert(member);
                    }
                }
                _GdprUnitOfWork.Commit();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "GdprService.Save");
                _GdprUnitOfWork.Rollback();
                throw;
            }
        }

        public void Save(int userId, IEnumerable<Member> members, bool export = false)
        {
            var gdpr = new Entities.Gdpr
            {
                UserId = userId,
                Export = export,
                Members = members.Select(m => new Entities.GdprMember { MemberId = m.MemberId }).ToList(),
                Timestamp= DateTime.Now,
            };
            Save(gdpr);
        }
    }
}
