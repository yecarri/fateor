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
        private readonly IConfigOptionRepository _ConfigOptionRepository;

        public ConfigService(IMapper mapper,
                             ILogger<ConfigService> logger,
                             IConfigRepository configRepository,
                             IConfigOptionRepository configOptionRepository)
        {
            _Mapper=mapper;
            _Logger=logger;
            _ConfigRepository=configRepository;
            _ConfigOptionRepository=configOptionRepository;
        }

        public IEnumerable<Entities.Config> GetCongfigs()
        {
            var dtos = _ConfigRepository.GetAll();
            var options = _ConfigOptionRepository.GetAll();
            var entities = _Mapper.MapEnumerable<Dto.Config, Entities.Config>(dtos);
            foreach(var entity in entities.Where(e=>e.ConfigTypeId==5))
            {
                entity.Options = options.Where(o => o.ConfigCode == entity.ConfigCode)
                                        .Select(o => o.Value)
                                        .ToList();
            }
            return entities;
        }

        public void UpdateConfigs(IEnumerable<Entities.Config> configsToUpdate)
        {
            var dtos = _Mapper.MapEnumerable<Entities.Config, Dto.Config>(configsToUpdate);
            _ConfigRepository.BeginTransaction();
            try
            {
                foreach (var dto in dtos)
                {
                    _ConfigRepository.Update(dto);
                }
                _ConfigRepository.CommitTransaction();
            }
            catch (Exception ex)
            {
                _ConfigRepository.RollbackTransaction();
                _Logger.LogError(ex, "Error updating configs");
                throw;
            }
        }
    }
}
