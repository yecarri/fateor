using Castle.Core.Logging;
using Fateor.Application.Location.Entities;
using Fateor.Application.Notification.Entities;
using Fateor.Application.Notification.Services;
using Fateor.Application.UserAccess.Repositories.MySql;
using Fateor.Application.UserAccess.Services;
using Fateor.Infrastructure.Services;
using Microsoft.Extensions.Logging;
using Moq;
using oSoft.Mail.Entities;
using oSoft.Mail.Services;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Tests.Application
{
    [TestClass]
    public class Notification
    {
        [TestMethod]
        public void GetActiveUsers()
        {
            using (var repo = new UserRepository("Server=127.0.0.1;Database=lvs_fateor;Uid=root;Pwd=MysREDACTED;"))
            {
                var users = repo.GetActiveUsers();
                foreach (var user in users)
                {
                    Assert.IsFalse(user.IsDeleted);
                }
            }
        }
       

        [TestMethod]
        public void SendNotification()
        {
            using (var repo = new UserRepository("Server=127.0.0.1;Database=lvs_fateor;Uid=root;Pwd=MysREDACTED;"))
            {
                var users = repo.GetActiveUsers();
                foreach (var user in users)
                {
                    Assert.IsFalse(user.IsDeleted);
                }
            }

            var mockUserService = new Mock<IUserService>();
            mockUserService
                .Setup(m => m.GetActiveUsers())
                .Returns(new List<Fateor.Application.UserAccess.Entities.User>() { new Fateor.Application.UserAccess.Entities.User() {
                UserId=1,
                Email="yecarri@gmail.com",
                IsDeleted=false,
                IsExternal=false,
                Login="yecarri"
                } });

            var mockLocationService = new Mock<Fateor.Application.Location.Services.ILocationService>();
            mockLocationService.Setup(m => m.GetCountryById(It.IsAny<int>()))
                .Returns(new Country() { CountryCode="ES", CountryId=1, CountryName="España" });

            var mockEmailService = new Mock<IEmailService>();
            mockEmailService.Setup(m => m.SendMail(It.IsAny<EmailAddress>(), It.IsAny<EmailAddress>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<EmailAddress, EmailAddress, string, string>((from, to, content, subject) =>
                {
                    Debug.WriteLine(from.Email);
                    Debug.WriteLine(to.Email);
                    Debug.WriteLine(subject);
                    Debug.WriteLine(content);
                })
                .Returns((true, new List<string>()));

            var emailConf = new Fateor.Application.Notification.Entities.EmailConfiguration()
            {
                DefaultSender=new oSoft.Mail.Entities.EmailAddress("yecarri@gmail.com"),
                DisableAllEmails=false,
                Templates = new EmailTemplate[]
                {
                    new EmailTemplate
                    {
                        Content = @"<html>
<head>
    <style>
        body, * {
            font-family: 'Lucida Sans Unicode','Lucida Sans','Sans Serif',sans-serif
        }

        .table {
            display: table;
        }

        .row {
            display: row;
        }

        .cell {
            display: cell;
        }
    </style>
</head>
<body>
    <p>Hola {user.Login},</p>
    <p>Se ha registrado un nuevo postulante en el Sistema de Miembros del MLV.</p>
    <table>
        <tr>
            <td>Nombre:</td>
            <td>{member.FullName}</td>
        </tr>
        <tr>
            <td>País:</td>
            <td>{country.CountryName}</td>
        </tr>
        <tr>
            <td>Ciudad:</td>
            <td>{member.City}</td>
        </tr>
    </table>
    <p><a href='{url}'>{url}</a></p>
    <p>¡Por la libertad!</p>
</body>
</html>",
                        Subject="Nuevo Miembro",
                        TemplateName="notifyNewMember"
                    }
                }
            };

            var mockLogger = new Mock<ILogger<NotificationService>>();

            var notificationService = new NotificationService(mockUserService.Object,
                                                              mockLocationService.Object,
                                                              mockEmailService.Object,
                                                              mockLogger.Object,
                                                              emailConf,
                                                              "http://localhost",
                                                              "");

            var (ok, messages) =notificationService.NotifyNewMember(new Fateor.Application.Organization.Entities.Member() { City="Madriz", GivenName="Yván", Surname="Ecarri", CountryId=1 });
            Assert.IsTrue(ok);
        }
    }
}
