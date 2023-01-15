using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.UserAccess.Entities
{
    public class User
    {
        public int? UserId { get; set; }

        public string Login { get; set; } = "";

        public string Email { get; set; } = "";

        public string Password { get; internal set; } = "";

        public bool IsExternal { get; set; }

        public bool IsDeleted { get; set; }

    }
}
