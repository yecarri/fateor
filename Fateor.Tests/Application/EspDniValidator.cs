using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Tests.Application
{
    [TestClass]
    public class EspDniValidator
    {
        [TestMethod]
        public void TestValidNie()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("nie", "X8301154V");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Valid, result);
        }

        [TestMethod]
        public void TestInvalidNie1()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("nie", "X8301154Z");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Invalid, result);
        }

        [TestMethod]
        public void TestInvalidNie2()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("nie", "8301154V");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Invalid, result);
        }

        [TestMethod]
        public void TestValidDni1()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("dni", "54440351H");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Valid, result);
        }

        [TestMethod]
        public void TestValidDni2()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("dni", "5444A0351H");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Invalid, result);
        }

        [TestMethod]
        public void TestValidDni3()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("dni", "54440351AH");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Invalid, result);
        }

        [TestMethod]
        public void TestValidCif1()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("cif", "B87208807");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Valid, result);
        }

        [TestMethod]
        public void TestValidCif2()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("cif", "P0200100F");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Valid, result);
        }

        

        [TestMethod]
        public void TestValidCif3()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("cif", "J22382949");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Valid, result);
        }

        [TestMethod]
        public void TestValidCif4()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("cif", "A00000026");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Valid, result);
        }

        [TestMethod]
        public void TestValidCif5()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("cif", "W6651114H");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Valid, result);
        }

        [TestMethod]
        public void TestValidCif6()
        {
            var validator = new Fateor.Application.Organization.Service.EspIdentificationNumberValidator();
            var result = validator.IsValid("cif", "K3951485T");
            Assert.AreEqual(Fateor.Application.Organization.Service.IIdentificationNumberValidator.IdentificationNumberValidity.Invalid, result);
        }


    }
}
