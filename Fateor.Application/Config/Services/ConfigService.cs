using Fateor.Application.Config.Repositories;
using Fateor.Infrastructure.Mappers;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Fateor.Application.Config.Services
{
    public class ConfigService : IConfigService
    {
        private readonly IMapper _Mapper;
        private readonly ILogger<ConfigService> _Logger;
        private readonly IConfigRepository _ConfigRepository;

        public ConfigService(IMapper mapper, ILogger<ConfigService> logger, IConfigRepository configRepository)
        {
            _Mapper=mapper;
            _Logger=logger;
            _ConfigRepository=configRepository;
        }

        public IEnumerable<Entities.Config> GetCongfigs()
        {
            var dtos = _ConfigRepository.GetAll();
            var entities = _Mapper.MapEnumerable<Dto.Config, Entities.Config>(dtos);
            return entities;
        }
    }
}
