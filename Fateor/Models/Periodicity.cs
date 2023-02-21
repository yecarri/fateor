using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.ComponentModel;

namespace Fateor.Models
{
    public class Periodicity
    {
        public int Value { get; set; }

        public string Text { get; set; } = "";

        public Periodicity(int value, string text)
        {
            Value=value;
            Text=text;
        }

        private static Periodicity[] _periodicities = new Periodicity[] {
            new Periodicity(1, "Variable"),
            new Periodicity(2, "Trimestral"),
            new Periodicity(3, "Mensual")
        };

        public static Periodicity[] Periodicities => _periodicities;
    }
}
