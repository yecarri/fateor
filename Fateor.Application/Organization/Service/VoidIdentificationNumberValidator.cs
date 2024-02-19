using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fateor.Application.Organization.Service
{
    public class VoidIdentificationNumberValidator : IIdentificationNumberValidator
    {
        public IIdentificationNumberValidator.IdentificationNumberValidity IsValid(string documentType, string documentNumber)
        {
            return IIdentificationNumberValidator.IdentificationNumberValidity.Undetermined;
        }
    }
}
