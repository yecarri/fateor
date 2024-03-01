using Fateor.Application.Location.Entities;
using Fateor.Application.Organization.Entities;
using Fateor.Application.UserAccess.Entities;

namespace Fateor.Application.Notification.Services
{
    public interface INotificationService
    {
        (bool ok, IList<string> messages) NotifyNewMember(Member member);
        (bool ok, IList<string> messages) NotifyNewMember(Member member, Country? country, IEnumerable<User> users);
        (bool ok, IList<string> messages) NotifyPasswordChangeRequest(User user);
        (bool ok, IList<string> messages) SendWelcomeEmail(Member member, string token);
    }
}