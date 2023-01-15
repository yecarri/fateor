using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public int QuestionTypeId { get; set; }
        public int Ord { get; set; }
        public int? QuestionaireId { get; set; }
        public string? QuestionText { get; set; }
        public QuestionType? QuestionType { get; set; }
        public List<Option>? Options { get; set; }
    }
}
