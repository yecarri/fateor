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
    public class CountryRepository : Repository<Country>, ICountryRepository
    {
        public CountryRepository(IDbConnection connection) : base(connection)
        {
        }

        public CountryRepository(oSoft.Data.UnitOfWork unitOfWork) : base(unitOfWork)
        {
        }

        public CountryRepository(string connectionString) : base(connectionString)
        {
        }

        public Country Get(int countryId)
        {
            return Connection.QueryFirst<Country>("SELECT * FROM loc_country WHERE CountryId=@CountryId", new { countryId });
        }

        public Country GetByIsoCode(string countryCode)
        {
            return Connection.QueryFirst<Country>("SELECT * FROM loc_country WHERE CountryCode=@countryCode", new { countryCode });
        }
    }
}