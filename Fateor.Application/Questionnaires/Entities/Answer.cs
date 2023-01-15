using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Entities
{
    public class Answer
    {
        public int AnswerId { get; set; }
        public int QuestionId { get; set; }
        public int ResponseId { get; set; }
        public int? OptionId { get; set; }
        public int QuestionTypeId { get; set; }
        public string? Text { get; set; }
        public IEnumerable<AnswerOption>? Options { get; set; }
    }
}
