using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Credentials.Entities
{
    public class ValidationToken
    {
        public DateTime CredentialDate { get; set; }

        public ushort MemberId { get; set; }

        public int HashCode { get; set; }

        public override int GetHashCode()
        {
            return CredentialDate.GetHashCode() ^ MemberId;
        }

        public byte[] GetBytes()
        {
            var data = new byte[14];
            var dateAsLong = CredentialDate.ToBinary();
            for (int i = 7; i >= 0; i--)
            {
                data[i] = (byte)dateAsLong;
                dateAsLong >>= 8;
            }
            data[8] = (byte)MemberId;
            data[9] = (byte)(MemberId >> 8);

            var hash = GetHashCode();

            for (int i = 13; i >= 10; i--)
            {
                data[i] = (byte)hash;
                hash >>= 8;
            }

            return data;
        }

        public static ValidationToken FromBytes(byte[] bytes)
        {
            if (bytes is null)
                throw new ArgumentNullException(nameof(bytes));

            if (bytes.Length != 14)
                throw new ArgumentException(nameof(bytes));

            long dateAsLong = 0;
            for (int i = 0; i < 8; i++)
            {
                dateAsLong <<= 8;
                dateAsLong += bytes[i];
            }

            var memberId = (ushort)((bytes[9] << 8) + bytes[8]);

            int hash = 0;
            for (int i = 10; i < 14; i++)
            {
                hash <<= 8;
                hash += bytes[i];
            }

            return new ValidationToken() { CredentialDate = DateTime.FromBinary(dateAsLong), MemberId = memberId, HashCode = hash };
        }
    }
}
