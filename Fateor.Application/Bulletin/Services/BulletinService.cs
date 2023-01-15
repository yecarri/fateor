using Fateor.Application.Bulletin.Repositories.Abstract;
using Fateor.Infrastructure.Mappers;

namespace Fateor.Application.Bulletin.Services
{
    public class BulletinService : IBulletinService
    {
        private readonly IMapper _mapper;
        private readonly IBulletinUnitOfWork _unitOfWork;

        public BulletinService(IMapper mapper, IBulletinUnitOfWork unitOfWork)
        {
            _mapper=mapper;
            _unitOfWork=unitOfWork;
        }

        public IEnumerable<Entities.Bulletin> GetBulletins()
        {
            var bulletinDtos = _unitOfWork.BulletinRepository.GetAll();

            var statusFilterDtos = _unitOfWork.BulletinStatusFilterRepository.GetAll();

            var countryFiletrDtos = _unitOfWork.BulletinCountryFilterRepository.GetAll();

            var unitFilterDtos = _unitOfWork.BulletinUnitFilterRepository.GetAll();

            var bulletins = bulletinDtos.Select(b =>
            {

                var bulletin = _mapper.Map<Dto.Bulletin, Entities.Bulletin>(b);

                bulletin.StatusFilters = _mapper.MapEnumerable<Dto.BulletinStatusFilter, Entities.BulletinStatusFilter>(statusFilterDtos);

                bulletin.CountryFilters = _mapper.MapEnumerable<Dto.BulletinCountryFilter, Entities.BulletinCountryFilter>(countryFiletrDtos);

                bulletin.UnitFilters = _mapper.MapEnumerable<Dto.BulletinUnitFilter, Entities.BulletinUnitFilter>(unitFilterDtos);

                return bulletin;
            }).ToList();

            return bulletins;
        }

        public async Task<IEnumerable<Entities.Bulletin>> GetBulletinsAsync()
        {
            var tBulletin = _unitOfWork.BulletinRepository.GetAllAsync();
            var tBulletinStatus = _unitOfWork.BulletinStatusRepository.GetAllAsync();
            var tStatusFilter = _unitOfWork.BulletinStatusFilterRepository.GetAllAsync();
            var tCountryFilter = _unitOfWork.BulletinCountryFilterRepository.GetAllAsync();
            var tUnitFilter = _unitOfWork.BulletinUnitFilterRepository.GetAllAsync();

            await Task.WhenAll(tBulletin, tStatusFilter, tCountryFilter, tUnitFilter);

            var bulletinDtos = tBulletin.Result;
            var bulletinStatusDtos = tBulletinStatus.Result;
            var statusFilterDtos = tStatusFilter.Result;
            var countryFiletrDtos = tCountryFilter.Result;
            var unitFilterDtos = tUnitFilter.Result;

            var bulletins = bulletinDtos.Select(b =>
            {

                var bulletin = _mapper.Map<Dto.Bulletin, Entities.Bulletin>(b);

                bulletin.BulletinStatusName = bulletinStatusDtos.FirstOrDefault(bs => bs.BulletinStatusId == bulletin.BulletinStatusId)?.BulletinStatusName;
                bulletin.StatusFilters = _mapper.MapEnumerable<Dto.BulletinStatusFilter, Entities.BulletinStatusFilter>(statusFilterDtos);

                bulletin.CountryFilters = _mapper.MapEnumerable<Dto.BulletinCountryFilter, Entities.BulletinCountryFilter>(countryFiletrDtos);

                bulletin.UnitFilters = _mapper.MapEnumerable<Dto.BulletinUnitFilter, Entities.BulletinUnitFilter>(unitFilterDtos);

                return bulletin;
            })
                .OrderByDescending(b => b.LastModificationDate)
            .ToList();

            return bulletins;
        }

        public Entities.Bulletin GetBulletin(int bulletinId)
        {
            var bulletinDto = _unitOfWork.BulletinRepository.Get(bulletinId);
            var statusFilterDtos = _unitOfWork.BulletinStatusFilterRepository.GetByBulletinId(bulletinId);
            var countryFiletrDtos = _unitOfWork.BulletinCountryFilterRepository.GetByBulletinId(bulletinId);
            var unitFilterDtos = _unitOfWork.BulletinUnitFilterRepository.GetByBulletinId(bulletinId);
            var bulletinStatus = _unitOfWork.BulletinStatusRepository.Get(bulletinDto.BulletinStatusId);
            var bulletin = _mapper.Map<Dto.Bulletin, Entities.Bulletin>(bulletinDto);

            bulletin.BulletinStatusName = bulletinStatus.BulletinStatusName;
            bulletin.StatusFilters = _mapper.MapEnumerable<Dto.BulletinStatusFilter, Entities.BulletinStatusFilter>(statusFilterDtos);
            bulletin.CountryFilters = _mapper.MapEnumerable<Dto.BulletinCountryFilter, Entities.BulletinCountryFilter>(countryFiletrDtos);
            bulletin.UnitFilters = _mapper.MapEnumerable<Dto.BulletinUnitFilter, Entities.BulletinUnitFilter>(unitFilterDtos);

            return bulletin;
        }

        public void Save(Entities.Bulletin bulletin, int currentUserId)
        {
            var bulletinDto = _mapper.Map<Entities.Bulletin, Dto.Bulletin>(bulletin);

            bulletinDto.LastModificationUserId = currentUserId;

            if (bulletinDto.BulletinId > 0)
            {
                _unitOfWork.BulletinRepository.Update(bulletinDto);
            }
            else
            {
                bulletin.BulletinId = bulletinDto.BulletinId = _unitOfWork.BulletinRepository.Insert(bulletinDto);
            }

            UpdateStatusFilters(bulletin.StatusFilters, bulletin.BulletinId.Value, currentUserId);

            UpdateCountryFilters(bulletin.CountryFilters, bulletin.BulletinId.Value, currentUserId);

            UpdateUnitFilters(bulletin.UnitFilters, bulletin.BulletinId.Value, currentUserId);

            _unitOfWork.Commit();
        }

        private void UpdateStatusFilters(IEnumerable<Entities.BulletinStatusFilter> filters, int bulletinId, int currentUserId)
        {
            var existing = _unitOfWork.BulletinStatusFilterRepository.GetByBulletinId(bulletinId);

            foreach (var f in filters)
            {
                f.BulletinId = bulletinId;

                var dto = ToDto(f);
                dto.LastModificationUserId = currentUserId;

                if (f.BulletinStatusFilterId.HasValue)
                {
                    _unitOfWork.BulletinStatusFilterRepository.Update(dto);
                }
                else
                {
                    f.BulletinStatusFilterId = dto.BulletinStatusFilterId = _unitOfWork.BulletinStatusFilterRepository.Insert(dto);
                }
            }

            var toDelete = existing.Where(f => !filters.Any(x => x.BulletinStatusFilterId == f.BulletinStatusFilterId)).ToList();

            foreach (var f in toDelete)
            {
                _unitOfWork.BulletinStatusFilterRepository.Delete(f);
            }

        }

        private void UpdateCountryFilters(IEnumerable<Entities.BulletinCountryFilter> filters, int bulletinId, int currentUserId)
        {
            var existing = _unitOfWork.BulletinCountryFilterRepository.GetByBulletinId(bulletinId);

            foreach (var f in filters)
            {
                f.BulletinId = bulletinId;

                var dto = ToDto(f);
                dto.LastModificationUserId = currentUserId;

                if (f.BulletinCountryFilterId.HasValue)
                {
                    _unitOfWork.BulletinCountryFilterRepository.Update(dto);
                }
                else
                {
                    f.BulletinCountryFilterId = dto.BulletinCountryFilterId = _unitOfWork.BulletinCountryFilterRepository.Insert(dto);
                }
            }

            var toDelete = existing.Where(f => !filters.Any(x => x.BulletinCountryFilterId == f.BulletinCountryFilterId)).ToList();

            foreach (var f in toDelete)
            {
                _unitOfWork.BulletinCountryFilterRepository.Delete(f);
            }

        }

        private void UpdateUnitFilters(IEnumerable<Entities.BulletinUnitFilter> filters, int bulletinId, int currentUserId)
        {
            var existing = _unitOfWork.BulletinUnitFilterRepository.GetByBulletinId(bulletinId);

            foreach (var f in filters)
            {
                f.BulletinId = bulletinId;

                var dto = ToDto(f);
                dto.LastModificationUserId = currentUserId;

                if (f.BulletinUnitFilterId.HasValue)
                {
                    _unitOfWork.BulletinUnitFilterRepository.Update(dto);
                }
                else
                {
                    f.BulletinUnitFilterId = dto.BulletinUnitFilterId = _unitOfWork.BulletinUnitFilterRepository.Insert(dto);
                }
            }

            var toDelete = existing.Where(f => !filters.Any(x => x.BulletinUnitFilterId == f.BulletinUnitFilterId)).ToList();

            foreach (var f in toDelete)
            {
                _unitOfWork.BulletinUnitFilterRepository.Delete(f);
            }

        }

        private Dto.BulletinStatusFilter ToDto(Entities.BulletinStatusFilter e)
        {
            return _mapper.Map<Entities.BulletinStatusFilter, Dto.BulletinStatusFilter>(e);
        }

        private Dto.BulletinCountryFilter ToDto(Entities.BulletinCountryFilter e)
        {
            return _mapper.Map<Entities.BulletinCountryFilter, Dto.BulletinCountryFilter>(e);
        }

        private Dto.BulletinUnitFilter ToDto(Entities.BulletinUnitFilter e)
        {
            return _mapper.Map<Entities.BulletinUnitFilter, Dto.BulletinUnitFilter>(e);
        }
    }
}
