using Dapper.Contrib.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Questionnaires.Dto
{
    [Table("qst_answeroption")]
    public class AnswerOption
    {
        [ExplicitKey]
        public int AnswerId { get; set; }
        [ExplicitKey]
        public int OptionId { get; set; }
        public bool Selected { get; set; }
    }
}
