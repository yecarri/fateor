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
using Fateor.Application.Notification.Repositories.Interfaces;
using Fateor.Application.Notification.Repositories.MySql;
using Fateor.Application.Config.Repositories;
using System.Configuration;

namespace Fateor.Setup
{
    public static class SetupRepositories
    {
        public static void AddRepositories(this WebApplicationBuilder builder)
        {
            string? connectionString = builder.Configuration.GetConnectionString("fateor");
            if(connectionString is null)
            {
                throw new ConfigurationErrorsException("Connection string 'fateor' not found.");
            }
            builder.Services.AddScoped<IUserRepository, UserRepository>(s => new UserRepository(connectionString));
            builder.Services.AddScoped<IMemberRepository, MemberRepository>(s => new MemberRepository(connectionString));
            builder.Services.AddScoped<IMemberFeeBatchRepository, MemberFeeBatchRepository>(s => new MemberFeeBatchRepository(connectionString));
            builder.Services.AddScoped<IResponseRepository, ResponseRepository>(s => new ResponseRepository(connectionString));
            builder.Services.AddScoped<IUnitRepository, UnitRepository>(s => new UnitRepository(connectionString));
            builder.Services.AddScoped<ICountryRepository, CountryRepository>(s => new CountryRepository(connectionString));
            builder.Services.AddScoped<IRegionRepository, RegionRepository>(s => new RegionRepository(connectionString));
            builder.Services.AddScoped<IStatusRepository, StatusRepository>(s => new StatusRepository(connectionString));
            builder.Services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>(s => new QuestionnaireRepository(connectionString));
            builder.Services.AddScoped<IQuestionRepository, QuestionRepository>(s => new QuestionRepository(connectionString));
            builder.Services.AddScoped<IOptionRepository, OptionRepository>(s => new OptionRepository(connectionString));
            builder.Services.AddScoped<IResponseRepository, ResponseRepository>(s => new ResponseRepository(connectionString));
            builder.Services.AddScoped<IAnswerRepository, AnswerRepository>(s => new AnswerRepository(connectionString));
            builder.Services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>(s => new AnswerOptionRepository(connectionString));
            builder.Services.AddScoped<IMemberUnitOfWork, MemberUnitOfWork>(s => new MemberUnitOfWork(connectionString));
            builder.Services.AddScoped<IIdentityDocumentTypeRepository, IdentityDocumentTypeRepository>(s => new IdentityDocumentTypeRepository(connectionString));
            builder.Services.AddScoped<IResponseUnitOfWork, ResponseUnitOfWork>(s => new ResponseUnitOfWork(connectionString));
            builder.Services.AddScoped<IQuestionnaireUnitOfWork, QuestionnaireUnitOfWork>(s => new QuestionnaireUnitOfWork(connectionString));
            builder.Services.AddScoped<IMemberFeeUnitOfWork, MemberFeeUnitOfWork>(s => new MemberFeeUnitOfWork(connectionString));
            builder.Services.AddScoped<IGdprUnitOfWork, GdprUnitOfWork>(s => new GdprUnitOfWork(connectionString));
            builder.Services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>(s => new EmailTemplateRepository(connectionString));
            builder.Services.AddScoped<IEmailRecipientRepository, EmailRecipientRepository>(s => new EmailRecipientRepository(connectionString));
            builder.Services.AddScoped<IConfigRepository, ConfigRepository>(s => new ConfigRepository(connectionString));
        }
    }
}
