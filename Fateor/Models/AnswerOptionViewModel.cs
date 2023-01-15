using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class AnswerOptionViewModel
    {
        public int AnswerId { get; set; }
        public int OptionId { get; set; }
        public bool Selected { get; set; }
        public int QuestionId { get; set; }
        public string? OptionText { get; set; }
        public int Ord { get; set; }
        public decimal? Points { get; set; }
    }
}