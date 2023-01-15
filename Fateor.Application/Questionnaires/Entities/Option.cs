using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Entities
{
    public class Option
    {
        public int OptionId { get; set; }
        public int QuestionId { get; set; }
        public string? OptionText { get; set; }
        public int Ord { get; set; }
        public decimal? Points { get; set; }
    }
}
