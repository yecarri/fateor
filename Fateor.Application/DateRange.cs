using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application
{
    public class DateRange : Tuple<DateTime, DateTime>
    {
        public DateRange(DateTime item1, DateTime item2) : base(item1, item2)
        {
        }
    }
}
