using Fateor.Application.Credentials.Services;
using Fateor.Application.External;
using Fateor.Application.Gdpr.Services;
using Fateor.Application.Location.Repositories.Interfaces;
using Fateor.Application.Location.Services;
using Fateor.Application.Notification.Entities;
using Fateor.Application.Notification.Services;
using Fateor.Application.Organization.Repositories.Interfaces;
using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Services;
using Fateor.Application.UserAccess.Repositories.Interfaces;
using Fateor.Application.UserAccess.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using oSoft.Mail.Entities;
using oSoft.Mail.Services;

namespace Fateor.Setup
{
    public static class SetupServices
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddScoped<IEmailTemplateService, EmailTemplateService>();

            var sp = services.BuildServiceProvider();

            var settings = sp.GetRequiredService<Settings>();

            services.AddScoped<IUserService, UserService>(
                s => new UserService(
                    s.GetRequiredService<IUserRepository>(),
                    s.GetRequiredService<IEncryptionService>(),
                    s.GetRequiredService<AutoMapper.IMapper>()));

            switch (settings.Unit_Selector_Type)
            {
                case "Fateor.Application.Organization.Service.MLVUnitSelector":
                    services.AddScoped<IUnitSelector, MLVUnitSelector>(s => new MLVUnitSelector(
                        s.GetRequiredService<ICountryRepository>(),
                        s.GetRequiredService<IUnitRepository>()));
                    break;
                default:
                    services.AddScoped<IUnitSelector, UnitSelector>(s => new UnitSelector(settings.Default_UnitId));
                    break;
            }

            switch (settings.ID_Validator_Type)
            {
                case "Fateor.Application.Organization.Service.EspIdentificationNumberValidator":
                    services.AddScoped<IIdentificationNumberValidator, EspIdentificationNumberValidator>();
                    break;
                default:
                    services.AddScoped<IIdentificationNumberValidator, VoidIdentificationNumberValidator>();
                    break;
            }

            switch (settings.Email_Service_Provider)
            {
                case "mailgun":
                    services.AddSingleton<IEmailService, MailgunService>(s => new MailgunService(
                        settings.Email_Service_Domain,
                        settings.Email_Service_Key,
                        settings.Email_Service_Server
                        ));
                    break;
                case "smtp":
                    services.AddSingleton<IEmailService, SmtpService>(s => new SmtpService(
                        settings.Email_Service_Server,
                        settings.Email_Service_Port,
                        settings.Email_Service_User,
                        settings.Email_Service_Password
                        ));
                    break;
                case "mailersend":
                    services.AddSingleton<IEmailService, MailerSendService>(s => new MailerSendService(
                        settings.Email_Service_Key));
                    break;
                default:
                    services.AddScoped<IEmailService, VoidEmailService>(s => new VoidEmailService(
                        s.GetRequiredService<ILogger<VoidEmailService>>()));
                    break;
            }

            var mapper = sp.GetRequiredService<IMapper>();

            var emailConfiguration = new EmailConfiguration()
            {
                DefaultSender = new EmailAddress(settings.Email_Sender_Address, settings.Email_Sender_Name),
                DisableAllEmails = settings.Email_DisabledAll,
                Templates = sp.GetRequiredService<IEmailTemplateService>().GetEmailTemplates()
            };

            string systemUrl = settings.Url;

            string site = settings.Site;

            string templatesPath = Path.Combine(builder.Environment.WebRootPath, "sites", site, "templates");

            services.AddScoped<INotificationService, NotificationService>(s => new NotificationService(
                s.GetRequiredService<IUserService>(),
                s.GetRequiredService<ILocationService>(),
                s.GetRequiredService<IEmailService>(),
                s.GetRequiredService<ILogger<NotificationService>>(),
                emailConfiguration,
                systemUrl)); ;

            services.AddSingleton<ICredentialService, MLVCredentialService>(s => new MLVCredentialService(
                systemUrl
                ));

            services.AddScoped<IMemberService, MemberService>(s => new MemberService(
                 s.GetRequiredService<IMemberUnitOfWork>(),
                 s.GetRequiredService<IUnitSelector>(),
                 s.GetRequiredService<IMapper>(),
                 s.GetRequiredService<ILogger<MemberService>>(),
                 settings.Default_StatusId
                ));

            services.AddScoped<IResponseService, ResponseService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IExternalMemberService, VoidExternalMemberService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IQuestionnaireService, QuestionnaireService>();
            services.AddScoped<IIdentityDocumentService, IdentityDocumentService>();
            services.AddScoped<IMemberFeeService, MemberFeeService>();
            services.AddScoped<IGdprService, GdprService>();

            //services.AddScoped<IResponseService, ResponseService>(s => new ResponseService(
            //    s.GetRequiredService<IResponseUnitOfWork>(),
            //    s.GetRequiredService<IMapper>(),
            //    s.GetRequiredService<IEncryptionService>()
            //    ));

            //services.AddScoped<ILocationService, LocationService>(s => new LocationService(
            //    s.GetRequiredService<ICountryRepository>(),
            //    s.GetRequiredService<IRegionRepository>(),
            //    s.GetRequiredService<IMapper>()
            //    ));

            //services.AddScoped<IExternalMemberService, VoidExternalMemberService>(s => new VoidExternalMemberService());

            //services.AddScoped<IStatusService, StatusService>(s => new StatusService(
            //    s.GetRequiredService<IStatusRepository>(),
            //    s.GetRequiredService<IMapper>()
            //    ));

            //services.AddScoped<IUnitService, UnitService>(s => new UnitService(
            //    s.GetRequiredService<IUnitRepository>(),
            //    s.GetRequiredService<IMapper>()
            //    ));

            //services.AddScoped<IQuestionnaireService, QuestionnaireService>(s => new QuestionnaireService(
            //    s.GetRequiredService<IQuestionnaireUnitOfWork>(),
            //    s.GetRequiredService<IMapper>(),
            //    s.GetRequiredService<IEncryptionService>()
            //    ));

            //services.AddScoped<IIdentityDocumentService, IdentityDocumentService>(s => new IdentityDocumentService(
            //    s.GetRequiredService<IIdentityDocumentTypeRepository>(),
            //    s.GetRequiredService<IMapper>()
            //    ));

            //            services.AddScoped<IMemberFeeBatchService, MemberFeeBatchService>(s => new MemberFeeBatchService(
            //                s.GetRequiredService<IMemberFeeUnitOfWork>(),
            //                s.GetRequiredService<IMapper>(),
            //                s.GetRequiredService<ILogger<MemberFeeBatchService>>()));

            //            services.AddScoped<IGdprService, GdprService>(s => new GdprService(
            //                s.GetRequiredService<IGdprUnitOfWork>(),
            //                s.GetRequiredService<IMapper>(),
            //                s.GetRequiredService<ILogger<MemberFeeBatchService>>()
            //                ));
        }

        private static EmailAddress GetEmailAddressFromConfiguration(IConfigurationSection s)
        {
            return new EmailAddress(s.GetValue<string>("Email"), s.GetValue<string>("Name"));
        }
    }
}
