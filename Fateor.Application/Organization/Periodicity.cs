using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization
{
    public enum Periodicity : byte
    {
        [Display(Name ="Ninguna")]
        None = 0,
        [Display(Name = "Mensual")]
        Monthly = 1,
        [Display(Name = "Trimestral")]
        Quarterly = 2,
        [Display(Name = "Anual")]
        Yearly = 3
    }

    public static class PeriodicityHelpers
    {
        public static string Description(this Periodicity e)
        {
            var descriptionAttribute = e.GetType()
                .GetMember(e.ToString())[0]
                .GetCustomAttributes(typeof(DisplayAttribute), inherit: false)[0]
                as DisplayAttribute;

            return descriptionAttribute?.Name ?? e.ToString();
        }
        
    }
}
