using Fateor.Application.External;
using Fateor.Application.Location.Entities;
using Fateor.Application.Location.Services;
using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Fateor.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;

namespace Fateor.Controllers
{
    public class MemberControllerBase : FateorControllerBase
    {
        protected readonly IMemberService _memberService;
        protected readonly ILocationService _locationService;
        protected readonly IQuestionnaireService _questionaireService;
        protected readonly IIdentityDocumentService _identityDocumentService;

        public MemberControllerBase(
            IMemberService memberService,
            IIdentityDocumentService identityDocumentService,
            ILocationService locationService,
            IQuestionnaireService questionaireService,
            IEncryptionService encryptionService,
            ILogger logger,
            IMapper mapper,
            Settings settings) : base(logger, mapper, encryptionService, settings)
        {
            _locationService=locationService;
            _identityDocumentService=identityDocumentService;
            _questionaireService=questionaireService;
            _memberService=memberService;
        }

        protected void LoadModel(MemberViewModel model)
        {
            if (model.CountryId.HasValue)
                model.Regions = _locationService.GetRegionsByCountryId(model.CountryId.Value).ToList();
            else
                model.Regions = new List<Region>();

            var countries = _locationService.GetCountries().ToList();
            countries.Insert(0, new Country() { CountryId = -1, CountryName = "- selecciona un país- " });
            model.Countries = countries;

            model.DocumentTypes = _identityDocumentService.GetDocumentTypes().ToList();

            _logger.LogDebug("OK");
        }

        protected string? InitialQuestionaireToken(Member member)
        {
            var questionaire = _questionaireService.GetQuestionaireByName(_settings.Default_InitialPoll);
            if (questionaire != null)
            {
                return _questionaireService.GetQuestionaireToken(member.MemberId, questionaire.QuestionnaireId);
            }
            return null;
        }
    }

}
