using Fateor.Application.Organization.Entities;

namespace Fateor.Application.Organization.Service
{
    public interface IIdentityDocumentService
    {
        IEnumerable<IdentityDocumentType> GetDocumentTypes();
    }
}