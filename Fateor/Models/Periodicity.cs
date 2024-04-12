using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.ComponentModel;

namespace Fateor.Models
{
    public class Periodicity
    {
        public byte Value { get; set; }

        public string Text { get; set; } = "";

        public Periodicity(byte value, string text)
        {
            Value=value;
            Text=text;
        }

        private static Periodicity[] _periodicities = (from Application.Organization.Periodicity p
                                                       in Enum.GetValues(typeof(Periodicity))
                                                       select new Periodicity((byte)p, p.ToString())).ToArray();

        public static Periodicity[] Periodicities => _periodicities;
    }
}
