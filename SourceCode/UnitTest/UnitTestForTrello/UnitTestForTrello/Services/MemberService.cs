using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Services.IServices
{
    public class MemberService : IMemberService
    {
        private readonly MemberRepository _memberRepository;

        public MemberService(MemberRepository memberRepository)
        {
            _memberRepository = memberRepository;
        }

        public IEnumerable<BoardMemberDTO> GetMembersByBoardId(int boardId)
        {
            return _memberRepository.GetMembersByBoardId(boardId);
        }

        public IEnumerable<CardMemberDTO> GetMembersByCardId(int cardId)
        {
            return _memberRepository.GetMembersByCardId(cardId);
        }

        public IEnumerable<WorkspaceMemberDTO> GetMembersByWorkspaceId(int workspaceId)
        {
            return _memberRepository.GetMembersByWorkspaceId(workspaceId);
        }

        public IEnumerable<RolePermissionDTO> GetRolePermissions()
        {
            return _memberRepository.GetRolePermissions();
        }

        public IEnumerable<CardSelectableMemberDTO> GetSelectableMembersByCardId(int cardId)
        {
            return _memberRepository.GetSelectableMembersByCardId(cardId);
        }
    }
}
