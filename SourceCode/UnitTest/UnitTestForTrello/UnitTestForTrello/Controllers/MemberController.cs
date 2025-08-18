using UnitTestForTrello.Models;
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

        public IEnumerable<CardMemberDTO> GetMembersByCardId(int cardId)
        {
            return _memberService.GetMembersByCardId(cardId);
        }

        public IEnumerable<CardSelectableMemberDTO> GetSelectableMembersByCardId(int cardId)
        {
            return _memberService.GetSelectableMembersByCardId(cardId);
        }

        public IEnumerable<WorkspaceMemberDTO> GetMembersByWorkspaceId(int workspaceId)
        {
            return _memberService.GetMembersByWorkspaceId(workspaceId);
        }
    }
}
