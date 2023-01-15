using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Fateor.Models
{
    public class NolanViewModel : ViewModelBase
    {
        public double X { get; set; }
        public double Y { get; set; }
        public string Member { get; set; }
        public bool External { get; set; }
    }
}