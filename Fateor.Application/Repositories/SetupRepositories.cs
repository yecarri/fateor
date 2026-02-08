using Dapper;
using Fateor.Application.Config.Repositories;
using Fateor.Application.Credentials.Services;
using Fateor.Application.External;
using Fateor.Application.Gdpr.Repositories.Interfaces;
using Fateor.Application.Gdpr.Repositories.MySql;
using Fateor.Application.Location.Repositories.Interfaces;
using Fateor.Application.Location.Repositories.MySql;
using Fateor.Application.Location.Services;
using Fateor.Application.Notification;
using Fateor.Application.Notification.Repositories.Interfaces;
using Fateor.Application.Notification.Repositories.MySql;
using Fateor.Application.Organization.Repositories.Interfaces;
using Fateor.Application.Organization.Repositories.MySql;
using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Repositories.Interfaces;
using Fateor.Application.Questionnaires.Repositories.MySql;
using Fateor.Application.Questionnaires.Services;
using Fateor.Application.UserAccess.Repositories.Interfaces;
using Fateor.Application.UserAccess.Repositories.MySql;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace Fateor.Application.Repositories
{
    public static class SetupRepositories
    {
        public static void AddRepositories(IServiceCollection services, string connectionString)
        {
            SqlMapper.AddTypeHandler(new GuidBinaryHandler());
            services.AddScoped<IUserRepository, UserRepository>(s => new UserRepository(connectionString));
            services.AddScoped<IMemberRepository, MemberRepository>(s => new MemberRepository(connectionString));
            services.AddScoped<IMemberFeeBatchRepository, MemberFeeBatchRepository>(s => new MemberFeeBatchRepository(connectionString));
            services.AddScoped<IResponseRepository, ResponseRepository>(s => new ResponseRepository(connectionString));
            services.AddScoped<IUnitRepository, UnitRepository>(s => new UnitRepository(connectionString));
            services.AddScoped<ICountryRepository, CountryRepository>(s => new CountryRepository(connectionString));
            services.AddScoped<IRegionRepository, RegionRepository>(s => new RegionRepository(connectionString));
            services.AddScoped<IStatusRepository, StatusRepository>(s => new StatusRepository(connectionString));
            services.AddScoped<IQuestionnaireRepository, QuestionnaireRepository>(s => new QuestionnaireRepository(connectionString));
            services.AddScoped<IQuestionRepository, QuestionRepository>(s => new QuestionRepository(connectionString));
            services.AddScoped<IOptionRepository, OptionRepository>(s => new OptionRepository(connectionString));
            services.AddScoped<IResponseRepository, ResponseRepository>(s => new ResponseRepository(connectionString));
            services.AddScoped<IAnswerRepository, AnswerRepository>(s => new AnswerRepository(connectionString));
            services.AddScoped<IAnswerOptionRepository, AnswerOptionRepository>(s => new AnswerOptionRepository(connectionString));
            services.AddScoped<IMemberUnitOfWork, MemberUnitOfWork>(s => new MemberUnitOfWork(connectionString));
            services.AddScoped<IIdentityDocumentTypeRepository, IdentityDocumentTypeRepository>(s => new IdentityDocumentTypeRepository(connectionString));
            services.AddScoped<IResponseUnitOfWork, ResponseUnitOfWork>(s => new ResponseUnitOfWork(connectionString));
            services.AddScoped<IQuestionnaireUnitOfWork, QuestionnaireUnitOfWork>(s => new QuestionnaireUnitOfWork(connectionString));
            services.AddScoped<IMemberFeeUnitOfWork, MemberFeeUnitOfWork>(s => new MemberFeeUnitOfWork(connectionString));
            services.AddScoped<IGdprUnitOfWork, GdprUnitOfWork>(s => new GdprUnitOfWork(connectionString));
            services.AddScoped<IEmailTemplateRepository, EmailTemplateRepository>(s => new EmailTemplateRepository(connectionString));
            services.AddScoped<IEmailRecipientRepository, EmailRecipientRepository>(s => new EmailRecipientRepository(connectionString));
            services.AddScoped<IConfigRepository, ConfigRepository>(s => new ConfigRepository(connectionString));
            services.AddScoped<IConfigOptionRepository, ConfigOptionRepository>(s => new ConfigOptionRepository(connectionString));
        }
    }
}
