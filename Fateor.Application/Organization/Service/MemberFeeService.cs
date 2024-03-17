using Fateor.Application.Organization.Repositories.Interfaces;
using Fateor.Application.Organization.Entities;
using Fateor.Infrastructure.Mappers;
using Microsoft.Extensions.Logging;
using System.Text;
using System.Text.RegularExpressions;

namespace Fateor.Application.Organization.Service
{
    public class MemberFeeService : IMemberFeeService
    {
        private readonly IMemberFeeUnitOfWork _memberFeeUnitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;

        public MemberFeeService(IMemberFeeUnitOfWork memberFeeUnitOfWork,
                                     IMapper mapper,
                                     ILogger<MemberFeeService> logger)
        {
            _mapper=mapper;
            _logger=logger;
            _memberFeeUnitOfWork=memberFeeUnitOfWork;
        }

        public MemberFeeBatch GetMemberFeeBatchById(int batchId, bool includeMemberFees = false)
        {
            try
            {
                _memberFeeUnitOfWork.BeginTransaction();
                var b = _memberFeeUnitOfWork.MemberFeeBatchRepository.Get(batchId);
                var result = _mapper.Map<Dto.MemberFeeBatch, Entities.MemberFeeBatch>(b);

                var members = _memberFeeUnitOfWork.MemberRepository.GetByFeeBatchId(batchId);
                if (includeMemberFees)
                {
                    result.MemberFees = _memberFeeUnitOfWork.MemberFeeRepository
                        .GetByBatchId(batchId)
                        .Join(members, mf => mf.MemberId, m => m.MemberId, (mf, m) =>
                        {
                            var e = _mapper.Map<Dto.MemberFee, Entities.MemberFee>(mf);
                            e.Surname = m.Surname;
                            e.GivenName = m.GivenName;
                            return e;
                        })
                        .ToList();
                }
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error reading Member Fee Batches");
                throw;
            }
            finally
            {
                _memberFeeUnitOfWork.Rollback();
            }
        }

        public MemberFeeBatch[] GetMemberFeeBatches(DateTime dateFrom, DateTime dateTo)
        {
            return _memberFeeUnitOfWork.MemberFeeBatchRepository
                .GetMemberFeeBatches(dateFrom, dateTo)
                .Select(b => _mapper.Map<Dto.MemberFeeBatch, Entities.MemberFeeBatch>(b))
                .ToArray();
        }

        public IEnumerable<MemberFee> GetMemberFeesByBatchId(int batchId)
        {

            var dtos = _memberFeeUnitOfWork.MemberFeeRepository.GetByBatchId(batchId);
            var fees = _mapper.MapEnumerable<Dto.MemberFee, MemberFee>(dtos);
            return fees;
        }

        public IEnumerable<MemberFee> MockFees(MemberFeeBatch batch)
        {
            var re = new Regex(@"(?:%(?<token>FD|FH)(\.(?<format>AAAA|AA|MM|DD|TR))?%|%(?<token>U)\.(?<element>N|A|ID)(?:\((?<length>\d)\))?%)");
            var allMembers = _memberFeeUnitOfWork.MemberRepository.GetByStatusIds(batch.SelectedStatuses);
            return allMembers.Select(m =>
            {
                var x = _mapper.Map<Dto.Member, Entities.MemberFee>(m);
                var sb = new StringBuilder(batch.ReferencePattern);

                x.Amout = batch.Amount;
                var matches = re.Matches(batch.ReferencePattern);
                string textToreplace;

                foreach (Match match in matches)
                {
                    var token = match.Groups["token"];
                    Group format;
                    string s;
                    switch (token.Value.ToUpper())
                    {
                        case "FD":
                            format = match.Groups["format"];
                            s= format.Value.ToUpper();
                            textToreplace=FormatDatePart(batch.DateFrom, s);
                            break;
                        case "FH":
                            format = match.Groups["format"];
                            s= format.Value.ToUpper();
                            textToreplace=FormatDatePart(batch.DateTo, s);
                            break;
                        case "U":
                            var element = match.Groups["element"];
                            var length = match.Groups["length"];
                            switch (element.Value.ToUpper())
                            {
                                case "ID":
                                    textToreplace = m.MemberId.ToString();
                                    break;
                                case "N":
                                    textToreplace = m.GivenName;
                                    break;
                                case "A":
                                    textToreplace = m.Surname;
                                    break;
                                default:
                                    textToreplace = token.Value;
                                    break;
                            }
                            if (length != null)
                            {
                                int l = int.Parse(length.Value);
                                textToreplace = (new string('0', l-textToreplace.Length) + textToreplace);
                            }
                            break;
                        default:
                            textToreplace = token.Value;
                            break;
                    }

                    sb.Replace(match.Value, textToreplace);
                }
                x.FeeReference = sb.ToString();
                return x;
            });
        }
        
        public void SaveMemberFees(IEnumerable<MemberFee> memberFees)
        {
            _memberFeeUnitOfWork.BeginTransaction();
            foreach(var memberFee in _mapper.MapEnumerable<MemberFee, Dto.MemberFee>( memberFees))
            {
                _memberFeeUnitOfWork.MemberFeeRepository.Insert(memberFee);
            }
            _memberFeeUnitOfWork.Commit();
        }
        
        private static string FormatDatePart(DateTime date, string s)
        {
            switch (s)
            {
                case "AA":
                    return date.ToString("yy");
                case "AAAA":
                    return date.ToString("yyyy");
                case "DD":
                    return date.ToString("dd");
                case "MM":
                    return date.ToString("MM");
                case "TR":
                    return ((date.Month -1) / 3 +1).ToString();
                default: return s;
            }
        }
    }
}
