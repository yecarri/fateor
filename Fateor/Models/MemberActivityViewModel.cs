using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class MemberActivityViewModel
    {
        public int ActivityId { get; set; }
        [DisplayName("Participante")]
        public int MemberId { get; set; }
        [DisplayName("Forma de Participación")]
        public int Participation { get; set; }
        [DisplayName("Nombre")]
        public string FullName { get; set; }
    }
}