using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Google.Protobuf.Reflection.SourceCodeInfo.Types;

namespace Fateor.Application.Mappers
{
    public class AutoMapperProfile : AutoMapper.Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<UserAccess.Dto.User, UserAccess.Entities.User>().ReverseMap();

            CreateMap<Location.Dto.Region, Location.Entities.Region>().ReverseMap();
            CreateMap<Location.Dto.Country, Location.Entities.Country>().ReverseMap();

            CreateMap<Organization.Dto.Member, Organization.Entities.Member>().ReverseMap();
            CreateMap<Organization.Dto.IdentityDocumentType, Organization.Entities.IdentityDocumentType>().ReverseMap();
            CreateMap<Organization.Dto.Unit, Organization.Entities.Unit>().ReverseMap();
            CreateMap<Organization.Dto.Status, Organization.Entities.Status>().ReverseMap();
            CreateMap<Organization.Dto.MemberStatus, Organization.Entities.MemberStatus>()
                   .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => src.StatusDescription))
                   .ReverseMap();
            CreateMap<Organization.Dto.MemberFee, Organization.Entities.MemberFee>().ReverseMap();
            CreateMap<Organization.Dto.MemberFeeBatch, Organization.Entities.MemberFeeBatch>().ReverseMap();

            CreateMap<Questionnaires.Dto.Option, Questionnaires.Entities.Option>().ReverseMap();

            CreateMap<Questionnaires.Dto.Question, Questionnaires.Entities.Question>()
                .ForMember(m => m.QuestionaireId, opt => opt.MapFrom(m1 => m1.QuestionaireId))
                .ReverseMap();

            CreateMap<Questionnaires.Dto.Questionnaire, Questionnaires.Entities.Questionnaire>()
                .ForMember(m => m.QuestionnaireId, opt => opt.MapFrom(m1 => m1.QuestionaireId))
                .ForMember(m => m.QuestionnaireName, opt => opt.MapFrom(m1 => m1.QuestionaireName))
                .ReverseMap();

            CreateMap<Questionnaires.Dto.QuestionType, Questionnaires.Entities.QuestionType>().ReverseMap();


            CreateMap<Questionnaires.Dto.Answer, Questionnaires.Entities.Answer>().ReverseMap();
            CreateMap<Questionnaires.Dto.AnswerOption, Questionnaires.Entities.AnswerOption>().ReverseMap();
            CreateMap<Questionnaires.Dto.Response, Questionnaires.Entities.Response>()
                .ForMember(m => m.QuestionnaireId, opt => opt.MapFrom(m1 => m1.QuestionaireId))
                .ReverseMap();

            CreateMap<Organization.Dto.MemberFee, Organization.Entities.MemberFee>().ReverseMap();
            CreateMap<Organization.Dto.Member, Organization.Entities.MemberFee>().ReverseMap();

            //CreateMap<Bulletin.Dto.Bulletin, Bulletin.Entities.Bulletin>().ReverseMap();
            //CreateMap<Bulletin.Dto.BulletinCountryFilter, Bulletin.Entities.BulletinCountryFilter>().ReverseMap();
            //CreateMap<Bulletin.Dto.BulletinStatusFilter, Bulletin.Entities.BulletinStatusFilter>().ReverseMap();
            //CreateMap<Bulletin.Dto.BulletinUnitFilter, Bulletin.Entities.BulletinUnitFilter>().ReverseMap();
            //CreateMap<Bulletin.Dto.BulletinMember, Bulletin.Entities.BulletinMember>().ReverseMap();
        }
    }
}
