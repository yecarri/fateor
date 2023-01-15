using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Dto
{
    [Table("qst_question")]
    public class Question
    {
        [Key]
        public int QuestionId { get; set; }
        public int QuestionTypeId { get; set; }
        public int? QuestionaireId { get; set; }
        public string? QuestionText { get; set; }
        public int Ord { get; set; }
        public DateTime LastModificationDate { get; set; } = DateTime.Now;
        public int LastModificationUserId { get; set; }

    }
}
