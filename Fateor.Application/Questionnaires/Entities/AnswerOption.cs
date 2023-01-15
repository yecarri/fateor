using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Entities
{
    public class AnswerOption
    {
        public int AnswerId { get; set; }
        public int OptionId { get; set; }
        public bool Selected { get; set; }
        public string? OptionText { get; set; }
    }
}
