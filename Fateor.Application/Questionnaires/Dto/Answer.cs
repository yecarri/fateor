using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Dto
{
    [Table("qst_answer")]
    public class Answer
    {
        [Key]
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int ResponseId { get; set; }
        public int? OptionId { get; set; }
        public string? Text { get; set; }
        public DateTime LastModificationDate { get; set; }
        public int LastModificationUserId { get; set; }
    }
}
