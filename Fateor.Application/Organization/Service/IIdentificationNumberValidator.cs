using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Service
{
    public interface IIdentificationNumberValidator
    {
        public enum IdentificationNumberValidity:byte
        {
            Undetermined=0,
            Invalid=1,
            Valid=2
        }

        IdentificationNumberValidity IsValid(string documentType, string documentNumber);
    }
}
