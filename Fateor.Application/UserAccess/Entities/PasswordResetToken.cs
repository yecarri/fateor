using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.UserAccess.Entities
{

    [Serializable]
    public class PasswordResetToken
    {
        private DateTime dateTime;

        public PasswordResetToken(int userId, DateTime validTo)
        {
            UserId=userId;
            ValidTo=validTo;
        }

        public DateTime ValidTo { get; set; }

        public int UserId { get; set; }
    }
}

