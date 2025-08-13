using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Services.IServices;

namespace UnitTestForTrello.Controllers
{
    public class MemberController
    {
        private readonly IMemberService _memberService;
        public MemberController(IMemberService memberService)
        {
            _memberService = memberService;
        }
        public IEnumerable<BoardMemberDTO> GetMembersByBoardId(int boardId)
        {
            return _memberService.GetMembersByBoardId(boardId);
        }
    }
}
