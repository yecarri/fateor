using Fateor.Application.Organization.Entities;
using System.Collections;

namespace Fateor.Application.Credentials.Services
{
    public interface ICredentialService
    {        
        (bool, IList<string>) Generate(Stream stream, Member member);
    }
}