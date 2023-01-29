namespace Fateor.Mappers
{
    public class ModelMapper : AutoMapper.Profile
    {
        public ModelMapper()
        {
            CreateMap<Models.User, Application.UserAccess.Entities.User>().ReverseMap();
            CreateMap<Models.UserViewModel, Application.UserAccess.Entities.User>().ReverseMap();
            CreateMap<Models.MemberViewModel, Application.Organization.Entities.Member>().ReverseMap();
            CreateMap<Models.StatusSelectViewModel, Application.Organization.Entities.Status>().ReverseMap();
            CreateMap<Models.UpdateStatusViewModel, Application.Organization.Entities.MemberStatus>().ReverseMap();
            CreateMap<Models.UnitViewModel, Application.Organization.Entities.Unit>().ReverseMap();
            CreateMap<Models.QuestionnaireViewModel, Application.Questionnaires.Entities.Questionnaire>().ReverseMap();
            CreateMap<Models.QuestionViewModel, Application.Questionnaires.Entities.Question>().ReverseMap();
            CreateMap<Models.OptionViewModel, Application.Questionnaires.Entities.Option>().ReverseMap();
            CreateMap<Models.ResponseViewModel, Application.Questionnaires.Entities.Response>().ReverseMap();
            CreateMap<Models.AnswerOptionViewModel, Application.Questionnaires.Entities.Option>().ReverseMap();
            CreateMap<Models.AnswerOptionViewModel, Application.Questionnaires.Entities.AnswerOption>().ReverseMap();
            CreateMap<Models.AnswerViewModel, Application.Questionnaires.Entities.Answer>().ReverseMap();
            CreateMap<Models.AnswerViewModel, Application.Questionnaires.Entities.Question>().ReverseMap();                        
            CreateMap<Models.OptionViewModel, Application.Questionnaires.Entities.Option>().ReverseMap();            
        }
    }
}
