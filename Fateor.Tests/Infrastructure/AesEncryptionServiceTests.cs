using Fateor.Infrastructure.Services;

namespace Fateor.Tests.Infrastructure
{
    [TestClass]
    public class AesEncryptionServiceTests
    {
        class T
        {
            public string MyProperty1 { get; set; } = "";
            public int MyProperty2 { get; set; }

        }

        [TestMethod]
        public void TestMethod1()
        {
            var aes = new AesEncryptionService();
            var data = new T() { MyProperty1="AAAAA", MyProperty2=2 };
            var encrypted = aes.EncryptBase64(data);
            var decrypted = aes.DecryptBase64<T>(encrypted);
            Assert.AreEqual(data, decrypted);
        }
    }
}