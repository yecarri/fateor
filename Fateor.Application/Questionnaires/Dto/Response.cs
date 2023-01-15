using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Dto
{
    [Table("qst_response")]
    public class Response
    {
        [Key]
        public int ResponseId { get; set; }

        public int QuestionaireId { get; set; }

        [Computed]
        public string? Questionaire { get; set; }

        public int MemberId { get; set; }

        [Computed]
        public string? Member { get; set; }

        public DateTime LastModificationDate { get; set; }

        public int LastModificationUserId { get; set; }

        [Computed]
        public string? LastModificationUser { get; set; }
    }
}
