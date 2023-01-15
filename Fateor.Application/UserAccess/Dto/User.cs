using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.UserAccess.Dto
{
    [Table("app_user")]
    public class User
    {
        [Key]
        public int UserId { get; set; }

        public string Login { get; set; } = "";

        public string Email { get; set; } = "";

        public string Hash { get; set; } = "";

        public bool IsExternal { get; set; }

        public bool IsDeleted { get; set; }

        public int LastModificationUserId { get; set; }
    }
}
