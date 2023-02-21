using Fateor.Application.Organization.Service;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Text;

namespace Fateor.Controllers
{
    [ApiController]
    public sealed class MemberApiController : ControllerBase
    {
        private readonly IMemberService _memberService;

        public MemberApiController(IMemberService memberService)
        {
            _memberService=memberService;
        }

        [Route("/api/member/search/{term}")]
        [HttpGet]        
        public IActionResult MemberSearch(string term)
        {
            var members = _memberService.GetAllMembers().Where(m => RemoveAccents(m.FullName)?.Contains(RemoveAccents(term) ?? string.Empty) ?? false);
            var ret = members.Select(m => new { key = m.MemberId, label = m.FullName }).ToArray();
            return Ok(ret);
        }

        [Route("/api/member/instatus/")]
        [HttpPost]
        public IActionResult GetMembersByStatus( [FromBody] int[] statuses)
        {
            var allMembers = _memberService.GetAllMembers();
            var members = allMembers.Where(
                ms => statuses.Any(s => s == (ms.MemberStatuses?.OrderBy(ms => ms.StatusDate).FirstOrDefault()?.StatusId ?? int.MinValue ))).ToArray();
            return Ok(members);
        }

        private string? RemoveAccents(string s)
        {
            if (s == null) return null;

            var sb = new StringBuilder(s.Length);
            foreach (var c in s.ToLower().Normalize(NormalizationForm.FormD).ToLower())
            {
                if (char.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark)
                {
                    sb.Append(c);
                }
            }
            return sb.ToString();
        }

    }
}
