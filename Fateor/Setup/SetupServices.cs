using Fateor.Application.Config.Services;
using Fateor.Application.Credentials.Services;
using Fateor.Application.External;
using Fateor.Application.Gdpr.Repositories.Interfaces;
using Fateor.Application.Gdpr.Services;
using Fateor.Application.Location.Repositories.Interfaces;
using Fateor.Application.Location.Services;
using Fateor.Application.Notification.Entities;
using Fateor.Application.Notification.Repositories.Interfaces;
using Fateor.Application.Notification.Services;
using Fateor.Application.Organization.Repositories.Interfaces;
using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Repositories.Interfaces;
using Fateor.Application.Questionnaires.Repositories.MySql;
using Fateor.Application.Questionnaires.Services;
using Fateor.Application.UserAccess.Repositories.Interfaces;
using Fateor.Application.UserAccess.Services;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using oSoft.Data;
using oSoft.Mail.Entities;
using oSoft.Mail.Services;
using QuestPDF.Drawing;
using QuestPDF.Infrastructure;
using System.Reflection.Metadata;

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
            var logger = sp.GetRequiredService<ILogger<Program>>();

            services.AddScoped<IUserService, UserService>(
                s => new UserService(
                    s.GetRequiredService<IUserRepository>(),
                    s.GetRequiredService<IEncryptionService>(),
                    s.GetRequiredService<AutoMapper.IMapper>()));

            switch (settings.Unit_Selector_Type)
            {
                case "Fateor.Application.Organization.Service.MLVUnitSelector":
                    logger.LogInformation("IUnitSelector: Using MLVUnitSelector");
                    services.AddScoped<IUnitSelector, MLVUnitSelector>(s => new MLVUnitSelector(
                        s.GetRequiredService<ICountryRepository>(),
                        s.GetRequiredService<IUnitRepository>()));
                    break;
                default:
                    logger.LogInformation("IUnitSelector: Using default UnitSelector");
                    services.AddScoped<IUnitSelector, UnitSelector>(s => new UnitSelector(settings.Default_UnitId));
                    break;
            }

            switch (settings.ID_Validator_Type)
            {
                case "Fateor.Application.Organization.Service.EspIdentificationNumberValidator":
                    logger.LogInformation("IIdentificationNumberValidator: Using EspIdentificationNumberValidator");
                    services.AddScoped<IIdentificationNumberValidator, EspIdentificationNumberValidator>();
                    break;
                default:
                    logger.LogInformation("IIdentificationNumberValidator: Using VoidIdentificationNumberValidator");
                    services.AddScoped<IIdentificationNumberValidator, VoidIdentificationNumberValidator>();
                    break;
            }

            switch (settings.Email_Service_Provider)
            {
                case "mailgun":
                    logger.LogInformation("IEmailService: Using Mailgun for domain {0}", settings.Email_Service_Domain);
                    services.AddSingleton<IEmailService, MailgunService>(s => new MailgunService(
                        settings.Email_Service_Domain,
                        settings.Email_Service_Key,
                        settings.Email_Service_Server
                        ));
                    break;
                case "smtp":
                    logger.LogInformation("IEmailService: Using Smtp");
                    services.AddSingleton<IEmailService, SmtpService>(s => new SmtpService(
                        settings.Email_Service_Server,
                        settings.Email_Service_Port,
                        settings.Email_Service_User,
                        settings.Email_Service_Password
                        ));
                    break;
                case "mailersend":
                    logger.LogInformation("IEmailService: Using MailerSend");
                    services.AddSingleton<IEmailService, MailerSendService>(s => new MailerSendService(
                        settings.Email_Service_Key));
                    break;
                default:
                    logger.LogInformation("IEmailService: Not using any email service, please check Email_Service_Provider in settings");
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
                systemUrl,
                templatesPath
                )); ;

            services.AddSingleton<ICredentialService, MLVCredentialService>(s => new MLVCredentialService(
                systemUrl
                ));

            services.AddScoped<IMemberService, MemberService>(s => new MemberService(
                 s.GetRequiredService<IMemberUnitOfWork>(),
                 s.GetRequiredService<IUnitSelector>(),
                 s.GetRequiredService<INotificationService>(),
                 s.GetRequiredService<IEncryptionService>(),
                 mapper: s.GetRequiredService<IMapper>(),
                 logger: s.GetRequiredService<ILogger<MemberService>>(),
                 initialStatusId: settings.Default_StatusId
                ));

            services.AddScoped<IResponseService, ResponseService>();
            services.AddScoped<ILocationService, LocationService>();
            services.AddScoped<IExternalMemberService, VoidExternalMemberService>();
            services.AddScoped<IStatusService, StatusService>();
            services.AddScoped<IUnitService, UnitService>();
            services.AddScoped<IQuestionnaireService, QuestionnaireService>();
            services.AddScoped<IIdentityDocumentService, IdentityDocumentService>();
            services.AddScoped<IMemberFeeBatchService, MemberFeeBatchService>();
            services.AddScoped<IGdprService, GdprService>();
            services.AddScoped<IConfigService, ConfigService>();

            SetupPdfGenerator();
        }

        private static void SetupPdfGenerator()
        {
            QuestPDF.Settings.License = LicenseType.Community;
            QuestPDF.Settings.UseEnvironmentFonts = false;
            QuestPDF.Settings.CheckIfAllTextGlyphsAreAvailable = true;
            FontManager.RegisterFontWithCustomName("DejaVu Sans", new MemoryStream(MLVCredential.DejaVuSans));
            FontManager.RegisterFontWithCustomName("DejaVu Sans Bold", new MemoryStream(MLVCredential.DejaVuSans_Bold));
            FontManager.RegisterFontWithCustomName("Chamelton 10 Blk", new MemoryStream(MLVCredential.Chamelton10_Blk));
        }
    }

}
