using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Dto
{
    [Table("qst_questionaire")]
    public class Questionnaire
    {
        [Key]
        public int QuestionaireId { get; set; }
        public string? QuestionaireName { get; set; }
        public string? RedirectUrl { get; set; }
        public bool IsDeleted { get; set; }
        public DateTime LastModificationDate { get; set; } = DateTime.Now;
        public int LastModificationUserId { get; set; } = 0;
    }
}
