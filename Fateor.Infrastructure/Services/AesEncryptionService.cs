using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Infrastructure.Services
{
    public class AesEncryptionService : EncryptionServiceBase
    {
        private readonly byte[] Salt = new byte[] { 0x30, 0x36, 0x2f, 0x30, 0x37, 0x2f, 0x31, 0x39, 0x37, 0x31, 0x2f, 0x59, 0x56, 0x41, 0x4e, 0x45 };
        private readonly string Password = "MlvREDACTED!";
        private readonly byte[] _EncryptionIV;
        private readonly byte[] _EncryptionKey;

        public AesEncryptionService()
        {
            var pdb = new Rfc2898DeriveBytes(Password, Salt);
            _EncryptionIV = pdb.GetBytes(16);
            _EncryptionKey = pdb.GetBytes(32);
        }
        
        private byte[] EncryptionIV
        {
            get
            {
                return _EncryptionIV;
            }
        }

        private byte[] EncryptionKey
        {
            get
            {
                return _EncryptionKey;
            }
        }

        public override byte[] Encrypt(byte[] originalBytes)
        {
            MemoryStream m;

            // Now, encrypt the string using Rijndale
            using (m = new MemoryStream())
            using (Aes rij = Aes.Create())
            {
                rij.IV = EncryptionIV;
                rij.Key = EncryptionKey;

                var c = new CryptoStream(m, rij.CreateEncryptor(), CryptoStreamMode.Write);
                c.Write(originalBytes, 0, originalBytes.Length);
                c.Close();
            }
            
            byte[] encryptedBytes = m.ToArray();
            return encryptedBytes;
        }

        public override byte[] Encrypt<T>(T t)
        {
            return Encrypt(SerializeBinary<T>(t));
        }

        public override byte[] DecryptBytes(byte[] encryptedBytes)
        {

            // Now, decode the bytes             
            MemoryStream m;
            CryptoStream c;

            // Now, encrypt the string using Rijndale
            using (m = new MemoryStream())
            using (Aes rij = Aes.Create())
            {
                rij.IV = EncryptionIV;
                rij.Key = EncryptionKey;

                c = new CryptoStream(m, rij.CreateDecryptor(), CryptoStreamMode.Write);
                c.Write(encryptedBytes, 0, encryptedBytes.Length);
                c.Close();

                var decryptedBytes = m.ToArray();
                return decryptedBytes;
            }
        }
    }
}
