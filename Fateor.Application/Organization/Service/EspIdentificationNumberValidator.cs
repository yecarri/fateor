using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Service
{
    public class EspIdentificationNumberValidator : IIdentificationNumberValidator
    {
        private static char[] letters = "TRWAGMYFPDXBNJZSQVHLCKE".ToCharArray();

        public IIdentificationNumberValidator.IdentificationNumberValidity IsValid(string documentType, string documentNumber)
        {
            switch(documentType.ToLower())
            {
                case "dni": return IsValidDni(documentNumber);
                case "nie": return IsValidNie(documentNumber);
                case "cif": return IsValidCif(documentNumber);
                default: return IIdentificationNumberValidator.IdentificationNumberValidity.Undetermined;
            }
        }

        private IIdentificationNumberValidator.IdentificationNumberValidity IsValidCif(string documentNumber)
        {
            // if(primerCaracter.match(/^[FJKNPQRSUVW]$/))
            //    if(String.fromCharCode(64+unidad).toUpperCase()==cif.substr(cif.length-1,1).toUpperCase())

            ReadOnlySpan<char> span = documentNumber;

            var num = SumDigits(span.Slice(1, span.Length-2));
            char chr;

            switch (char.ToUpperInvariant(span[0]))
            {
                case char x when "ABCDEFGHJMUV".Contains(x):
                    chr = (char)(num+48);
                    break;
                case char x when "NPQRSW".Contains(x):
                    chr = (char)(num+64);
                    break;                    
                default:
                    return IIdentificationNumberValidator.IdentificationNumberValidity.Invalid;
            }
            if (span[span.Length-1] == chr)
                return IIdentificationNumberValidator.IdentificationNumberValidity.Valid;

            return IIdentificationNumberValidator.IdentificationNumberValidity.Invalid;
        }

        private IIdentificationNumberValidator.IdentificationNumberValidity IsValidNie(string documentNumber)
        {
            int add = 0;
            ReadOnlySpan<char> span = documentNumber;
            switch(Char.ToUpperInvariant(span[0]))
            {
                case 'X':
                    break;
                case 'Y':
                    add = 10000000;
                    break;
                case 'Z':
                    add = 20000000;
                    break;
                default:
                    return IIdentificationNumberValidator.IdentificationNumberValidity.Invalid;
            }
            return IsValid(span.Slice(1), add);           
        }

        private IIdentificationNumberValidator.IdentificationNumberValidity IsValidDni(string documentNumber)
        {
            ReadOnlySpan<char> span = documentNumber;
            return IsValidDni(span);
        }

        private static IIdentificationNumberValidator.IdentificationNumberValidity IsValidDni(ReadOnlySpan<char> span)
        {
            if (!char.IsDigit(span[0]))
                return IIdentificationNumberValidator.IdentificationNumberValidity.Invalid;
            return IsValid(span);
        }

        private static IIdentificationNumberValidator.IdentificationNumberValidity IsValid(ReadOnlySpan<char> span, int add=0)
        {
            var len = span.Length;

            if (!letters.Contains(span[len-1]))
                return IIdentificationNumberValidator.IdentificationNumberValidity.Invalid;

            int num;
            if (!int.TryParse(span.Slice(0, len-1), out num))
                return IIdentificationNumberValidator.IdentificationNumberValidity.Invalid;

            var mod = (add+ num) % 23;

            if (char.ToUpperInvariant(span[len-1]) == letters[mod])
                return IIdentificationNumberValidator.IdentificationNumberValidity.Valid;

            return IIdentificationNumberValidator.IdentificationNumberValidity.Invalid;
        }

       

        public byte SumDigits(ReadOnlySpan<char> span)
        {
            uint sum = 0;
            for(int i = 0; i < span.Length; i++)
            {
                if(i%2==1)
                {
                    sum += (uint)span[i]-48;
                }
                else
                {
                    uint num = 2*((uint)span[i]-48);
                    sum += (num / 10 + num % 10);
                }
            }
            return (byte)((10 - (sum % 10))%10);            
        }
    }
}
