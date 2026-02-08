using Fateor.Application.Notification.Services;
using Fateor.Application.Organization.Entities;
using Fateor.Application.Organization.Repositories.Interfaces;
using Fateor.Infrastructure.Mappers;
using Fateor.Infrastructure.Services;
using Microsoft.Extensions.Logging;

namespace Fateor.Application.Organization.Service
{
    public sealed class MemberService : IMemberService, IDisposable
    {
        private readonly IMemberUnitOfWork _uow;
        private readonly IUnitSelector _unitSelector;
        private readonly INotificationService _notificationService;
        private readonly IEncryptionService _encryptionService;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly int _initialStatusId;
        private bool disposedValue;

        public MemberService(IMemberUnitOfWork memberUow,
                             IUnitSelector unitSelector,
                             INotificationService notificationService,
                             IEncryptionService encryptionService,
                             IMapper mapper,
                             ILogger<MemberService> logger,
                             int initialStatusId)
        {
            _uow=memberUow;
            _unitSelector=unitSelector;
            _mapper=mapper;
            _logger=logger;
            _initialStatusId=initialStatusId;
            _notificationService=notificationService;
            _encryptionService=encryptionService;
        }


        #region Member
        public Member GetMemberById(int id, bool withStatuses = true)
        {

            var m = _uow.MemberRepository.Get(id);
            var m1 = _mapper.Map<Organization.Dto.Member, Member>(m);

            if (withStatuses)
                m1.MemberStatuses = GetMemberStatusHistory(id);

            _logger.LogDebug("OK");
            return m1;
        }

        public IEnumerable<Member> GetAllMembers()
        {

            var members = _uow.MemberRepository.GetAll();
            var statuses = _uow.MemberStatusRepository.GetAllStatusesAt(DateTime.UtcNow);
            var result = members.Select(m =>
            {
                var x = _mapper.Map<Organization.Dto.Member, Member>(m);
                x.MemberStatuses = statuses
                    .Where(s => s.MemberId == x.MemberId)
                    .Select(s => _mapper.Map<Organization.Dto.MemberStatus, MemberStatus>(s))
                    .ToList();
                return x;
            }).ToList();
            _logger.LogDebug("OK");
            return result;

        }

        public IEnumerable<Member> GetElegibleMembers()
        {
            var members = _uow.MemberRepository.GetElegibleMembers();
            var result = members.Select(m => _mapper.Map<Organization.Dto.Member, Member>(m)).ToList();
            _logger.LogDebug("OK");
            return result;
        }

        public (bool, IList<string>) CreateMember(Member member, int currentUserId)
        {
            try
            {
                bool ok = true;
                IList<string> messages = new List<string>();

                _logger.LogDebug($"Creating Member");

                _uow.BeginTransaction();

                (ok, messages) = CreateMemberInternal(member, currentUserId);

                if (ok)
                {
                    _logger.LogDebug($"Member created: {member.MemberId}");

                    _logger.LogDebug($"Assigning status to member: {member.MemberId}");
                    var unitId = _unitSelector.SelectUnit(member);

                    var memberStatus = new Organization.Dto.MemberStatus()
                    {
                        LastModificationUserId = currentUserId,
                        MemberId = member.MemberId,
                        StatusDate = DateTime.Today,
                        StatusId = _initialStatusId,
                        UnitId = unitId
                    };

                    _uow.MemberStatusRepository.Insert(memberStatus);

                    _uow.Commit();

                    _logger.LogDebug($"Status Assigned to member: {member.MemberId}");
                }

                return (ok, messages);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                _logger.LogError(ex, "CreateMember failed");
                return (false, new List<string>() { "No se pudo crear el registro de miembro", ex.Message });
            }
        }

        public (bool, IList<string>) AssignMemberCode(Member member, int currentUserId)
        {
            try
            {
                _uow.BeginTransaction();
                var statuses = member.MemberStatuses ??
                    _uow.MemberStatusRepository.GetByMemberId(member.MemberId)
                    .Select(s => _mapper.Map<Dto.MemberStatus, Entities.MemberStatus>(s));

                var first = statuses.OrderBy(s => s.StatusDate).FirstOrDefault(s => s.IsActiveMember);

                if (first == null)
                    return (false, new List<string>() { "No se encontró ningún estatus válido para tener credencial" });

                var maxCode = _uow.MemberRepository.LastMemberCode();
                var m = _mapper.Map<Member, Organization.Dto.Member>(member);
                member.MemberCode = m.MemberCode = $"{first.StatusDate.Year}-{maxCode + 1}";
                m.LastModificationUserId = currentUserId;
                _uow.MemberRepository.Update(m);
                _uow.Commit();
                return (true, new List<string>());
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return (false, new List<string> { "No se pudo actualizar el registro", ex.Message });
            }
        }

        public (bool, IList<string>) UpdateMember(Member member, int currentUserId)
        {
            try
            {
                _uow.BeginTransaction();
                var m = _mapper.Map<Member, Organization.Dto.Member>(member);
                var m1 = _uow.MemberRepository.Get(member.MemberId);

                if (!m.Equals(m1))
                {
                    m.LastModificationUserId = currentUserId;
                    _uow.MemberRepository.Update(m);
                }
                _uow.Commit();
                return (true, new List<string>());
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return (false, new List<string> { "No se pudo actualizar el registro", ex.Message });
            }
        }

        private (bool, IList<string>) CreateMemberInternal(Member member, int userId)
        {
            bool ok = true;
            IList<string> messages = new List<string>();
            try
            {
                var m = _mapper.Map<Member, Organization.Dto.Member>(member);
                var allMembers = _uow.MemberRepository.GetAll();

                var dupe = allMembers.FirstOrDefault(x => x.IdentityDocumentNumber == member.IdentityDocumentNumber || string.Compare(x.Email, member.Email, true) == 0);
                if (dupe != null)
                {
                    ok = false;
                    messages = new List<string>() { "Ya existe un registro con esos datos." };
                    if (dupe.IdentityDocumentNumber == member.IdentityDocumentNumber)
                        messages.Add("Existe un registro con la misma cédula o pasaporte");
                    if (dupe.Email == member.Email)
                        messages.Add("Existe un registro el mismo email");
                    return (ok, messages);
                }

                m.LastModificationUserId = userId;
                member.MemberId = _uow.MemberRepository.Insert(m);
                return (ok, messages);
            }
            catch (Exception ex)
            {
                return (false, new List<string> { "No se pudo crear el registro", ex.Message });
            }
        }

        #endregion

        #region Member Status
        public MemberStatus GetMemberStatus(int memberId)
        {

            var res = _uow.MemberStatusRepository.GetMemberStatusAtInstant(memberId, DateTime.UtcNow);
            return _mapper.Map<Organization.Dto.MemberStatus, MemberStatus>(res);

        }

        public IList<MemberStatus> GetMemberStatusHistory(int id)

        {
            return _uow.MemberStatusRepository
                .GetByMemberId(id).Select(ms => _mapper.Map<Dto.MemberStatus, Entities.MemberStatus>(ms))
                .ToList();
        }

        public (bool, IList<string>) UpdateMemberStatus(MemberStatus status, int currentUserId)
        {
            var messages = new List<string>();
            try
            {


                var statuses = _uow.MemberStatusRepository.GetByMemberId(status.MemberId).OrderByDescending(s => s.StatusDate).ToList();

                var statusDto = new Organization.Dto.MemberStatus()
                {
                    MemberId = status.MemberId,
                    UnitId = status.UnitId,
                    StatusId = status.StatusId,
                    StatusDate = status.StatusDate,
                    LastModificationUserId = currentUserId,
                    LastModificationDate = DateTime.UtcNow
                };

                if (statuses.Any(x => x.StatusDate == status.StatusDate))
                {
                    _uow.MemberStatusRepository.Update(statusDto);
                }
                else if (statuses.Count == 0 || !statuses[0].IsSameAs(statusDto) || statuses[0].StatusDate > statusDto.StatusDate)
                {
                    _uow.MemberStatusRepository.Insert(statusDto);
                }
                else
                {
                    messages.Add("No se actualizó el estatus");
                    return (false, messages);
                }

                // Eliminar repetidos
                statuses = _uow.MemberStatusRepository.GetByMemberId(status.MemberId).OrderBy(s => s.StatusDate).ToList();
                {
                    var s0 = statuses[0];
                    for (int i = 1; i < statuses.Count; i++)
                    {
                        if (statuses[i].IsSameAs(s0))
                        {
                            _uow.MemberStatusRepository.Delete(statuses[i]);
                        }
                        else
                        {
                            s0 = statuses[i];
                        }
                    }
                }
                _uow.Commit();
                return (true, messages);
            }
            catch (Exception ex)
            {
                _uow.Rollback();
                return (false, new List<string>() { "No se puedo actualizar el registro", ex.Message });
            }
        }
        #endregion

        #region Login

        public void RequestMemberLogin(string email)
        {
            var dto = _uow.MemberRepository.GetMemberByEmail(email);
            if (dto is not null)
            {
                var token = new MemberLoginToken(dto.MemberId, DateTime.Now.AddMinutes(5));
                var tokenStr = _encryptionService.EncryptBase64(token, true);

#if DEBUG
                _logger.LogInformation($"Login token for {email}: {tokenStr}");
#endif

                _uow.MemberRepository.InsertMemberLoginToken(
                    new Organization.Dto.MemberLoginToken()
                    {
                        Guid = token.Guid,
                        MemberId = token.MemberId,
                        Expiration = token.Expiration
                    });
                _notificationService.SendLogin(dto.Email, tokenStr);
            }
        }

        public (Member? member, IList<string> messages) LoginMember(string token)
        {
            MemberLoginToken? tok;
            try
            {
                tok = _encryptionService.DecryptBase64<MemberLoginToken>(token, true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Invalid Token");
                return (null, ["Invalid Token"]);
            }

            if (tok.Expiration < DateTime.Now)
                return (null, new List<string>() { "El token ha expirado" });

            try
            {

                var dto = _uow.MemberRepository.GetMemberLoginToken(tok.Guid);
                if (dto is null || dto.MemberId != tok.MemberId)
                    return (null, new List<string>() { "Token inválido" });

                _uow.MemberRepository.UseMemberLoginToken(tok.Guid);
                var memberDto = _uow.MemberRepository.Get(tok.MemberId);
                if (memberDto is null)
                    return (null, new List<string>() { "No se encontró el miembro" });

                var member = _mapper.Map<Organization.Dto.Member, Member>(memberDto);
                return (member, new List<string>());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "LoginMember failed");
                return (null, ["Unable to complete login process"]);
            }
        }

        #endregion

        #region IDisposable

        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    _uow.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue=true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~MemberService()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
