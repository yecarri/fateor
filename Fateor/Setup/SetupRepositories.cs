using Fateor.Application.UserAccess.Repositories.Interfaces;
using Fateor.Application.UserAccess.Repositories.MySql;
using Fateor.Application.Organization.Repositories.Interfaces;
using Fateor.Application.Organization.Repositories.MySql;
using Fateor.Application.External;
using Fateor.Application.Location.Services;
using Fateor.Application.Notification;
using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Services;
using Fateor.Application.Credentials.Services;
using Fateor.Application.Questionnaires.Repositories.Interfaces;
using Fateor.Application.Questionnaires.Repositories.MySql;
using Fateor.Application.Location.Repositories.Interfaces;
using Fateor.Application.Location.Repositories.MySql;
using Fateor.Application.Gdpr.Repositories.Interfaces;
using Fateor.Application.Gdpr.Repositories.MySql;

namespace Fateor.Setup
{
    public static class SetupRepositories
    {
        public static void AddRepositories(this WebApplicationBuilder builder)
        {
            builder.Services.AddScoped<IUserRepository, UserRepository>(s => new UserRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IMemberRepository, MemberRepository>(s => new MemberRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IMemberFeeBatchRepository, MemberFeeBatchRepository>(s => new MemberFeeBatchRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IResponseRepository, ResponseRepository>(s => new ResponseRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IUnitRepository, UnitRepository>(s => new UnitRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<ICountryRepository, CountryRepository>(s => new CountryRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IRegionRepository, RegionRepository>(s => new RegionRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IStatusRepository, StatusRepository>(s => new StatusRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>(s => new QuestionnaireRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>(s => new QuestionRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IOptionRepository, OptionRepository>(s => new OptionRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IResponseRepository, ResponseRepository>(s => new ResponseRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IAnswerRepository, AnswerRepository>(s => new AnswerRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>(s => new AnswerOptionRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IMemberUnitOfWork, MemberUnitOfWork>(s => new MemberUnitOfWork(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IIdentityDocumentTypeRepository, IdentityDocumentTypeRepository>(s => new IdentityDocumentTypeRepository(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IResponseUnitOfWork, ResponseUnitOfWork>(s => new ResponseUnitOfWork(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IQuestionnaireUnitOfWork, QuestionnaireUnitOfWork>(s => new QuestionnaireUnitOfWork(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IMemberFeeUnitOfWork, MemberFeeUnitOfWork>(s => new MemberFeeUnitOfWork(builder.Configuration.GetConnectionString("fateor")));
            builder.Services.AddScoped<IGdprUnitOfWork, GdprUnitOfWork>(s => new GdprUnitOfWork(builder.Configuration.GetConnectionString("fateor")));            
        }
    }
}
