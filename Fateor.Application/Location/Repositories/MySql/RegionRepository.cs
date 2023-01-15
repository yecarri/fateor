using Dapper;
using Fateor.Application.Location.Dto;
using Fateor.Application.Location.Repositories.Interfaces;
using oSoft.Data.MySql;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace Fateor.Application.Location.Repositories.MySql
{
    public class RegionRepository : Repository<Region>, IRegionRepository
    {
        private const string SELECT = @"SELECT r.*, c.CountryName FROM loc_region r join loc_country c on r.CountryId=c.CountryId";

        public RegionRepository(IDbConnection connection) : base(connection)
        {
        }

        public RegionRepository(oSoft.Data.UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public RegionRepository(string connectionString) : base(connectionString)
        {
        }

        public Region Get(int id)
        {
            var results = Connection.QueryFirst<Region>(
                SELECT + " WHERE r.RegionId=?", new { id }, Transaction);
            return results;
        }

        public IEnumerable<Region> GetByCountryId(int id)
        {
            var results = Connection.Query<Region>(
                SELECT + " WHERE r.CountryId=?", new { id }, Transaction);
            return results;
        }
    }
}