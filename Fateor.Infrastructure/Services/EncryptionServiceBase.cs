using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Fateor.Infrastructure.Services
{
    public abstract class EncryptionServiceBase : IEncryptionService
    {
        public virtual T DecryptBase64<T>(string s)
        {
            // First we get the bytes from the encrypted string
            byte[] encryptedBytes = Convert.FromBase64String(s);
            return Decrypt<T>(encryptedBytes);
        }

        public virtual T Decrypt<T>(byte[] encryptedBytes)
        {
            return DeserializeBinary<T>(DecryptBytes(encryptedBytes));
        }

        public virtual T DecryptHex<T>(string s)
        {
            byte[] encryptedBytes = HexadecimalStringToBytes(s);
            return Decrypt<T>(encryptedBytes);
        }

        protected T DeserializeBinary<T>(byte[] bytes)
        {
            var str = Encoding.UTF8.GetString(bytes);
            var t = JsonSerializer.Deserialize<T>(str);
            if (t is not null)
                return t;
            throw new ArgumentNullException(nameof(T));
        }

        public abstract byte[] DecryptBytes(byte[] encryptedBytes);

        public byte[] HexadecimalStringToBytes(string hex)
        {
            byte[] bytes = new byte[hex.Length / 2 + hex.Length % 2];
            for (int i = 0, j = 0; i < hex.Length; i += 2, j++)
            {
                bytes[j] = Convert.ToByte(hex.Substring(i, 2), 16);
            }
            return bytes;
        }

        protected byte[] SerializeBinary<T>(T t)
        {
            // First, get a json serialized string from the object
            var str = JsonSerializer.Serialize(t);
            return Encoding.UTF8.GetBytes(str);
        }

        public virtual byte[] Encrypt<T>(T t)
        {
            return Encrypt(SerializeBinary<T>(t));
        }

        public abstract byte[] Encrypt(byte[] bytes);

        public string BytesToHexadecimalString(byte[] bytes)
        {
            StringBuilder sb = new();
            foreach (byte b in bytes)
            {
                sb.AppendFormat("{0:X2}", b);
            }
            return sb.ToString();
        }

        public string EncryptBase64<T>(T t)
        {
            byte[] encryptedBytes = Encrypt(t);
            return Convert.ToBase64String(encryptedBytes);
        }

        public string EncryptHex<T>(T t)
        {
            byte[] encryptedBytes = Encrypt(t);
            return BytesToHexadecimalString(encryptedBytes);
        }

    }
}
