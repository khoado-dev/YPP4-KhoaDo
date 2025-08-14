using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;

namespace UnitTestForTrello.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly IDbConnection _con;

        public CardRepository(IDbConnection con)
        {
            _con = con;
        }

        public IEnumerable<CardDetailDTO> GetCardDetailByBoardId(int boardId)
        {
            const string sql = @"
            WITH CardComment AS (
                SELECT
                    crd.Id CardId,
                    COUNT(crd.Id) NumberOfComments
                FROM Cards crd
                JOIN Comment cmt ON cmt.CardId = crd.Id
                GROUP BY crd.Id
            ),
            CardCheckListItem AS (
                SELECT
                    crd.Id CardId,
                    COUNT(crd.Id) NumberOfCheckListItem
                FROM Cards crd
                JOIN CheckList chl ON chl.CardId = crd.Id
                JOIN CheckListItem cli ON cli.CheckListId = chl.Id
                GROUP BY crd.Id
            ),
            CardAttachment AS (
                SELECT
                    crd.Id CardId,
                    COUNT(crd.Id) NumberOfAttachment
                FROM Cards crd
                JOIN Attachment atm ON atm.CardId = crd.Id
                GROUP BY crd.Id
            )
            SELECT 
                crd.Position CardPosition,
                stg.Position StagePosition,
                crd.Id CardId,
                crd.Title CardTitle,
                crd.CardLocation,
                crd.CoverValue CardCover,
                ccm.NumberOfComments,
                cci.NumberOfCheckListItem,
                cam.NumberOfAttachment,
                stg.Id StageId,
                stg.Title StageTitle,
                clr.ColorName StageColor,
                brd.Id BoardId,
                brd.BoardName
            FROM Cards crd
            JOIN Stage stg ON stg.Id = crd.StageId
            JOIN Color clr ON clr.Id = stg.ColorId 
            JOIN Board brd ON brd.Id = stg.BoardId
            LEFT JOIN CardComment ccm ON ccm.CardId = crd.Id
            LEFT JOIN CardCheckListItem cci ON cci.CardId = crd.Id
            LEFT JOIN CardAttachment cam ON cam.CardId = crd.Id
            WHERE brd.Id = @BoardId
            ORDER BY stg.Position, crd.Position
            ";
            return _con.Query<CardDetailDTO>(sql, new { BoardId = boardId });
        }

        public CardDetailDTO? GetCardDetailByCardId(int cardId)
        {
            const string sql = @"
            SELECT 
                crd.Id CardId,
                crd.Title CardTitle,
                crd.CardDescription,
                crd.CardLocation,
                stg.Title StageTitle
            FROM Cards crd
            JOIN Stage stg ON stg.Id = crd.Id
            WHERE crd.Id = @CardId
            ";
            return _con.QueryFirstOrDefault<CardDetailDTO>(sql, new { CardId = cardId });
        }

        public IEnumerable<CardLabelDTO> GetCardLabelsByCardId(int cardId)
        {
            const string sql = @"
            SELECT
                crd.Id CardId,
                lbl.Id LabelId,
                lbl.Title LabelTitle,
                clr.ColorName,
                clr.Icon LabelIcon
            FROM Cards crd
            JOIN CardLabel clb ON clb.CardId = crd.Id
            JOIN Labels lbl ON lbl.Id = clb.LabelId
            JOIN Color clr ON clr.Id = lbl.ColorId
            WHERE crd.Id = @CardId
            ";
            return _con.Query<CardLabelDTO>(sql, new { CardId = cardId });
        }

        public IEnumerable<CardSummaryDTO> GetCardSummariesByBoardId(int boardId)
        {
            const string sql = @"
            WITH CardComment AS (
                SELECT
                    crd.Id CardId,
                    COUNT(crd.Id) NumberOfComments
                FROM Cards crd
                JOIN Comment cmt ON cmt.CardId = crd.Id
                GROUP BY crd.Id
            ),
            CardCheckListItem AS (
                SELECT
                    crd.Id CardId,
                    COUNT(crd.Id) NumberOfCheckListItem
                FROM Cards crd
                JOIN CheckList chl ON chl.CardId = crd.Id
                JOIN CheckListItem cli ON cli.CheckListId = chl.Id
                GROUP BY crd.Id
            ),
            CardAttachment AS (
                SELECT
                    crd.Id CardId,
                    COUNT(crd.Id) NumberOfAttachment
                FROM Cards crd
                JOIN Attachment atm ON atm.CardId = crd.Id
                GROUP BY crd.Id
            )
            SELECT 
                crd.Position CardPosition,
                stg.Position StagePosition,
                crd.Id CardId,
                crd.Title CardTitle,
                crd.CardLocation,
                crd.CoverValue CardCover,
                ccm.NumberOfComments,
                cci.NumberOfCheckListItem,
                cam.NumberOfAttachment,
                stg.Id StageId,
                stg.Title StageTitle,
                clr.ColorName StageColor,
                brd.Id BoardId,
                brd.BoardName
            FROM Cards crd
            JOIN Stage stg ON stg.Id = crd.StageId
            JOIN Color clr ON clr.Id = stg.ColorId 
            JOIN Board brd ON brd.Id = stg.BoardId
            LEFT JOIN CardComment ccm ON ccm.CardId = crd.Id
            LEFT JOIN CardCheckListItem cci ON cci.CardId = crd.Id
            LEFT JOIN CardAttachment cam ON cam.CardId = crd.Id
            WHERE brd.Id = @BoardId
            ORDER BY stg.Position, crd.Position;
            ";

            return _con.Query<CardSummaryDTO>(sql, new { BoardId = boardId });
        }
    }
}
