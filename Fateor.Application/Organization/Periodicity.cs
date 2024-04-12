using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization
{
    public enum Periodicity : byte
    {
        [Description("Ninguna")]
        None = 0,
        [Description("Mensual")]
        Monthly = 1,
        [Description("Trimestral")]
        Quarterly = 2,
        [Description("Anual")]
        Yearly = 3
    }
}
