using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class AnswerViewModel
    {
        public int QuestionId { get; set; }
        public int QuestionTypeId { get; set; }
        public int Ord { get; set; }
        public int? QuestionnaireId { get; set; }
        public string? QuestionText { get; set; }
        public List<AnswerOptionViewModel>? Options { get; set; }
        public int AnswerId { get; set; }
        public int ResponseId { get; set; }
        public int? OptionId { get; set; }
        public string? Text { get; set; }
    }
}