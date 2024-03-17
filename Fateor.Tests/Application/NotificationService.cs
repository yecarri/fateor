using Fateor.Application.External;
using Fateor.Application.Location.Services;
using Fateor.Application.Notification.Entities;
using Fateor.Application.Notification.Services;
using Fateor.Application.Organization.Entities;
using Fateor.Application.UserAccess.Entities;
using Fateor.Application.UserAccess.Services;
using Microsoft.Extensions.Logging;
using Moq;
using oSoft.Mail.Entities;
using oSoft.Mail.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Tests.Application
{

    [TestClass]
    public class NotificationService
    {
        [TestMethod]
        public void TestSendNotification()
        {
            var mockUserService = new Mock<IUserService>();

            mockUserService
                .Setup(m => m.GetAllUsers())
                .Returns(new List<User>() {
                    new User { Email="yecarri@gmail.com", IsDeleted=false, Login="yecarri", UserId=1},
                    new User { Email="yecarri.1@gmail.com", IsDeleted=true, Login="yecarri1", UserId=2}
                });

            IUserService userService = mockUserService.Object;

            var mockLocationService = new Mock<ILocationService>();
            mockLocationService
                .Setup(m => m.GetCountryById(It.IsAny<int>()))
                .Returns((int countryId) => new Fateor.Application.Location.Entities.Country() { CountryCode="ES", CountryId= countryId, CountryName="España" });

            ILocationService locationService = mockLocationService.Object;

            var mockEmailService = new Mock<IEmailService>();
            mockEmailService
                .Setup(m => m.SendMail(It.IsAny<EmailAddress>(), It.IsAny<EmailAddress>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns((true, new List<string>()));


            IEmailService emailService = mockEmailService.Object;

            ILogger<Fateor.Application.Notification.Services.NotificationService> logger = new Mock<ILogger<Fateor.Application.Notification.Services.NotificationService>>().Object;

            EmailConfiguration emailConfiguration = new EmailConfiguration
            {
                DefaultSender=new EmailAddress("yecarri@gmail.com", "Yván"),
                DisableAllEmails=false,
                Templates = new List<EmailTemplate>
                {
                    new EmailTemplate()
                    {
                        TemplateName="notifyNewMember",
                        Subject="Se ha registrado un nuevo miembro",
                        Content="<html>\r\n\t\t<head>\r\n\t\t\t<style>\r\n\t\t\t\tbody, * {\r\n\t\t\t\t\tfont-family: 'Lucida Sans Unicode','Lucida Sans','Sans Serif',sans-serif\r\n\t\t\t\t}\r\n\r\n\t\t\t\t.table {\r\n\t\t\t\t\tdisplay: table;\r\n\t\t\t\t}\r\n\r\n\t\t\t\t.row {\r\n\t\t\t\t\tdisplay: row;\r\n\t\t\t\t}\r\n\r\n\t\t\t\t.cell {\r\n\t\t\t\t\tdisplay: cell;\r\n\t\t\t\t}\r\n\t\t\t</style>\r\n\t\t</head>\r\n\t\t<body>\r\n\t\t\t<p>Hola {user.Login},</p>\r\n\t\t\t<p>Se ha registrado un nuevo postulante en el Sistema de Miembros del MLV.</p>\r\n\t\t\t<table>\r\n\t\t\t\t<tr>\r\n\t\t\t\t\t<td>Nombre:</td>\r\n\t\t\t\t\t<td>{member.FullName}</td>\r\n\t\t\t\t</tr>        \r\n                <tr>\r\n\t\t\t\t\t<td>País:</td>\r\n\t\t\t\t\t<td>{country.CountryName}</td>\r\n\t\t\t\t</tr>\r\n\t\t\t\t<tr>\r\n\t\t\t\t\t<td>Ciudad:</td>\r\n\t\t\t\t\t<td>{member.City}</td>\r\n\t\t\t\t</tr>\r\n\t\t\t</table>\r\n\t\t\t<p><a href='{url}'>{url}</a></p>    \r\n\t\t</body>\r\n\t\t</html>"
                    }
                }
            };
            string url = "http://localhost";

            var notificationService = new Fateor.Application.Notification.Services.NotificationService(userService,
                                                                                                       locationService,
                                                                                                       emailService,
                                                                                                       logger,
                                                                                                       emailConfiguration,
                                                                                                       url);
            var member = new Member()
            {
                GivenName="Fulanito",
                Surname="De Tal",
                CountryId=1
            };

            (var ok, var messages) = notificationService.NotifyNewMember(member);
        }
    }
}
