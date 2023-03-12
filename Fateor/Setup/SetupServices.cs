using Fateor.Application.Credentials.Services;
using Fateor.Application.External;
using Fateor.Application.Location.Repositories.Interfaces;
using Fateor.Application.Location.Services;
using Fateor.Application.Notification;
using Fateor.Application.Organization.Repositories.Interfaces;
using Fateor.Application.Organization.Service;
using Fateor.Application.Questionnaires.Repositories.Interfaces;
using Fateor.Application.Questionnaires.Repositories.MySql;
using Fateor.Application.Questionnaires.Services;
using Fateor.Application.UserAccess.Repositories.Interfaces;
using Fateor.Application.UserAccess.Services;
using Fateor.Infrastructure.Entities;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using oSoft.Data;
using oSoft.Mail.Entities;
using oSoft.Mail.Services;

namespace Fateor.Setup
{
    public static class SetupServices
    {
        public static void AddServices(this WebApplicationBuilder builder)
        {
            var services = builder.Services;

            services.AddScoped<IUserService, UserService>(s => new UserService(s.GetRequiredService<IUserRepository>(),
                                                                               s.GetRequiredService<IEncryptionService>(),
                                                                               s.GetRequiredService<AutoMapper.IMapper>()));

            switch (builder.Configuration.GetValue<string>("Fateor:UnitSelector:TypeName"))
            {
                case "Fateor.Application.Organization.Service.MLVUnitSelector":
                    services.AddScoped<IUnitSelector, MLVUnitSelector>(s => new MLVUnitSelector(
                        s.GetRequiredService<ICountryRepository>(),
                        s.GetRequiredService<IUnitRepository>()));
                    break;
                default:
                    services.AddScoped<IUnitSelector, MLVUnitSelector>();
                    break;
            }

            switch (builder.Configuration.GetValue<string>("Fateor:Email:Service:Provider"))
            {
                case "mailgun":
                    services.AddSingleton<IEmailService, MailgunService>(s => new MailgunService(
                        builder.Configuration.GetValue<string>("Fateor:Email:Service:Domain"),
                        builder.Configuration.GetValue<string>("Fateor:Email:Service:Key")
                        ));
                    break;
                case "smtp":
                    services.AddSingleton<IEmailService, SmtpService>(s => new SmtpService(
                        builder.Configuration.GetValue<string>("Fateor:Email:Service:Server"),
                        builder.Configuration.GetValue<int>("Fateor:Email:Service:Port"),
                        builder.Configuration.GetValue<string>("Fateor:Email:Service:User"),
                        builder.Configuration.GetValue<string>("Fateor:Email:Service:Password")
                        ));
                    break;
                case "mailersend":
                    services.AddSingleton<IEmailService, MailerSendService>(s => new MailerSendService(
                        builder.Configuration.GetValue<string>("Fateor:Email:Service:Key")));
                    break;
                default:
                    services.AddScoped<IUnitSelector, MLVUnitSelector>();
                    break;
            }

            var emailConfiguration = new EmailConfiguration()
            {
                DefaultSender = new EmailAddress(
                    builder.Configuration.GetValue<string>("Fateor:Email:Sender:Email"),
                    builder.Configuration.GetValue<string>("Fateor:Email:Sender:Name")),
                DisableAllEmails = builder.Configuration.GetValue<bool>("Fateor:Email:DisableAll"),

            };



            var templates = builder.Configuration.GetSection("Fateor:Email:Templates").GetChildren().Select(t =>
            {
                var name = t.GetValue<string>("TemplateName");
                var subject = t.GetValue<string>("Subject");
                var file = t.GetValue<string>("FileName");

                var s = t.GetSection("Sender");
                EmailAddress? sender = null;
                if (s!=null)
                {
                    sender = GetEmailAddressFromConfiguration(s);
                }

                var r = t.GetSection("Recipients");
                List<EmailAddress>? recipients = null;
                if (r!=null)
                {
                    recipients = r.GetChildren().Select(e => GetEmailAddressFromConfiguration(e)).ToList();
                }

                return new EmailTemplate(sender, recipients, name, file, subject);
            });

            emailConfiguration.Templates =templates;


            string systemUrl = builder.Configuration.GetValue<string>("System:Url");

            string site = builder.Configuration.GetValue<string>("System:Site");

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

            services.AddScoped<IResponseService, ResponseService>(s => new ResponseService(
                s.GetRequiredService<IResponseUnitOfWork>(),
                s.GetRequiredService<IMapper>(),
                s.GetRequiredService<IEncryptionService>()
                ));

            services.AddScoped<ILocationService, LocationService>(s => new LocationService(
                s.GetRequiredService<ICountryRepository>(),
                s.GetRequiredService<IRegionRepository>(),
                s.GetRequiredService<IMapper>()
                ));

            services.AddScoped<IExternalMemberService, VoidExternalMemberService>(s => new VoidExternalMemberService());

            services.AddScoped<IStatusService, StatusService>(s => new StatusService(
                s.GetRequiredService<IStatusRepository>(),
                s.GetRequiredService<IMapper>()
                ));

            services.AddScoped<IUnitService, UnitService>(s => new UnitService(
                s.GetRequiredService<IUnitRepository>(),
                s.GetRequiredService<IMapper>()
                ));

            services.AddScoped<IQuestionnaireService, QuestionnaireService>(s => new QuestionnaireService(
                s.GetRequiredService<IQuestionnaireUnitOfWork>(),
                s.GetRequiredService<IMapper>(),
                s.GetRequiredService<IEncryptionService>()
                ));

            services.AddScoped<IIdentityDocumentService, IdentityDocumentService>(s => new IdentityDocumentService(
                s.GetRequiredService<IIdentityDocumentTypeRepository>(),
                s.GetRequiredService<IMapper>()
                ));

            services.AddSingleton<ICredentialService, MLVCredentialService>(s => new MLVCredentialService(
                systemUrl
                ));

            services.AddScoped<IMemberService, MemberService>(s => new MemberService(
                 s.GetRequiredService<IMemberUnitOfWork>(),
                 s.GetRequiredService<IUnitSelector>(),
                 s.GetRequiredService<IMapper>(),
                 s.GetRequiredService<ILogger<MemberService>>(),
                 builder.Configuration.GetValue<int>("Fateor:InitialStatusId")
                ));

            services.AddScoped<IMemberFeeBatchService, MemberFeeBatchService>(s => new MemberFeeBatchService(
                s.GetRequiredService<IMapper>(),
                s.GetRequiredService<ILogger<MemberFeeBatchService>>(),
                s.GetRequiredService<IMemberFeeUnitOfWork>()
                ));
        }

        private static EmailAddress GetEmailAddressFromConfiguration(IConfigurationSection s)
        {
            return new EmailAddress(s.GetValue<string>("Email"), s.GetValue<string>("Name"));
        }
    }
}
