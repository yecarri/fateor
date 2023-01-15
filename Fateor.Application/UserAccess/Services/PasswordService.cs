using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.UserAccess.Services
{
    public static class PasswordService
    {
        public static string ComputeHash(string password)
        {
            System.Security.Cryptography.SHA256 pSHA256 = System.Security.Cryptography.SHA256.Create();
            var encoder = new ASCIIEncoding();
            byte[] aByte = encoder.GetBytes(password ?? "");
            aByte = pSHA256.ComputeHash(aByte);
            // http://en.wikipedia.org/wiki/Base64				//the only non alphanum chars are +/
            // Base64 chars are:
            // ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz 0123456789 +/

            // http://en.wikipedia.org/wiki/Url_encode	the only non alphanum, nonreserved chars are -_.~
            // Nonreserved url chars are:
            // ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz 0123456789 -_.~

            // so we have to replace + and / to avoid url encoding issues. 
            // - and _ shoudln't cause any problems even at postback. (We had issues with +)
            string strTmp = Convert.ToBase64String(aByte).Replace("+", "-").Replace("/", "_");
            return strTmp.Substring(0, 40);

            //// and in this case we will not need this stupid padding thing... will delete it later
            //if (strTmp.Length >= 40)
            //    return strTmp.Substring(0, 40);
            //else
            //    return strTmp + new string('x', 40 - strTmp.Length);
        }

        //internal string GetPasswordResetToken(int userId)
        //{
        //    var rij = new RijndaelEncryptionService();
        //    var token = new PasswordResetToken() { UserId = userId, ValidTo = DateTime.UtcNow.AddMinutes(10) };
        //    return rij.EncryptHex(token);
        //}
    }
}
