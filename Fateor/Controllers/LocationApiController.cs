using Fateor.Application.Location.Entities;
using Fateor.Application.Location.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;

namespace Fateor.Controllers
{

    [ApiController]
    public class LocationApiController : ControllerBase
    {
        private readonly ILocationService _locationService;

        public LocationApiController(ILocationService locationService)
        {
            _locationService=locationService;
        }

        public class RegionJson
        {
            public RegionJson(int regionId, string regionName)
            {
                RegionId = regionId;
                RegionName = regionName;
            }
            [JsonPropertyName("RegionId")]
            [DataMember]
            public int RegionId { get; set; }
            [JsonPropertyName("RegionName")]
            [DataMember]
            public string RegionName { get; set; }
        }

        [Route("/api/country/{countryId:int}/regions")]
        [HttpGet]
        public IActionResult Regions(int countryId)
        {
            var regions = _locationService.GetRegionsByCountryId(countryId).Select(r => new RegionJson(r.RegionId, r.RegionName));
            return Ok(regions);
        }

        [Route("/api/country/list")]
        public IActionResult Countries()
        {
            var results = _locationService.GetCountries().Select(r => Country(r));
            return Ok(results);
        }

        [Route("/api/country/{countryId:int}")]
        public IActionResult Country(int countryId)
        {
            var results = Country(_locationService.GetCountryById(countryId));
            if (results == null)
                return BadRequest();
            return Ok(results);
        }


        [Route("/api/country/{code:regex(^[[a-zA-Z]]{{2}}$)}")]
        public IActionResult Country(string code)
        {
            var results = Country(_locationService.GetCountries().FirstOrDefault(c => string.Compare(c.CountryCode, code, true) == 0));
            if (results == null)
                return BadRequest();
            return Ok(results);
        }


        private dynamic Country(Country country)
        {
            if (country == null)
                return null;
            return new { country.CountryId, country.CountryName };
        }
    }
}
