using Fateor.Application.Credentials.Services;
using Fateor.Application.Organization.Entities;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Tests.Application
{
    [TestClass]
    public class Credentials
    {
        [TestMethod]
        public void TestCredential()
        {
            var service = new MLVCredentialService("http://www.osoft.es");
            var ms = new FileStream("test.pdf", FileMode.OpenOrCreate);
            var m = new Member()
            {
                GivenName="Yván",
                Surname="Ecarri",
                MemberStatuses = new List<MemberStatus>() { new MemberStatus() { UnitName="UNIT", StatusDate=new DateTime(1971, 1, 1), IsActiveMember=true } }
            };
            service.Generate(ms, m);
            ms.Flush();
            ms.Close();
        }
    }
}
