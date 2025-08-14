using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Services.IServices
{
    public class MemberService : IMemberService
    {
        private readonly IMemberRepository _memberRepository;

        public MemberService(IMemberRepository memberRepository)
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

        public IEnumerable<CardSelectableMemberDTO> GetSelectableMembersByCardId(int cardId)
        {
            return _memberRepository.GetSelectableMembersByCardId(cardId);
        }
    }
}
