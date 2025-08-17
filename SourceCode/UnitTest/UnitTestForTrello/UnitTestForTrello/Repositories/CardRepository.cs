using Dapper;
using System.Data;
using UnitTestForTrello.Models.DTOs;
using UnitTestForTrello.Repositories.IRepositories;
using UnitTestForTrello.Tests.Utility;

namespace UnitTestForTrello.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly IDbConnection _con;
        private readonly ICustomCache _cache;

        public CardRepository(IDbConnection con, ICustomCache cache)
        {
            _con = con;
            _cache = cache;
        }

        public IEnumerable<CardActivityDTO> GetActivitiesByCardId(int cardId)
        {
            const string sql = @"
            SELECT 
                usr.Id UserId,
                usr.PictureUrl UserPicture,
                usr.Username,
                atv.Id ActivityId,
                atv.ActivityDescription,
                atv.CreatedAt,
                owt.OwnerTypeValue Category,
                atv.OwnerId CardId
            FROM Activity atv
            JOIN OwnerType owt ON owt.Id = atv.OwnerTypeId
            JOIN Users usr ON usr.Id = atv.UserId
            WHERE owt.OwnerTypeValue = 'CARD' AND atv.OwnerId = @CardId;
            ";
            return _con.Query<CardActivityDTO>(sql, new { CardId = cardId });
        }

        public IEnumerable<CardAttachmentDTO> GetAttachmentsByCardId(int cardId)
        {
            const string sql = @"
            SELECT 
                atm.Id AttachmentId,
                att.DisplayValue AttachmentType,
                atm.AttachmentName,
                atm.AttachmentPath,
                atm.Size,
                atm.CreatedAt,
                atm.CreatedBy,
                atm.IsCover,
                atm.CardId
            FROM Attachment atm
            JOIN AttachmentType att ON att.Id = atm.AttachmentTypeId
            WHERE atm.CardId = @CardId;
            ";
            return _con.Query<CardAttachmentDTO>(sql, new { CardId = cardId });
        }

        public IEnumerable<CardCommentWithReactionCountDTO> GetCardCommentsAndReactionsCountByCardId(int cardId)
        {
            const string sql = @"
            SELECT 
              usr.Id UserId, 
              usr.PictureUrl UserPicture, 
              usr.Username, 
              cmt.Content, 
              cmt.Id CommentId, 
              cmt.CreatedAt, 
              cmt.UpdatedAt, 
              crd.Id CardId, 
              rct.Id ReactionId, 
              rct.ReactionName, 
              COUNT(rct.Id) ReactionCount 
            FROM 
              Cards crd 
              JOIN Comment cmt ON cmt.CardId = crd.Id 
              JOIN Users usr ON usr.Id = cmt.CreatedBy 
              JOIN CommentReaction cmr ON cmr.CommentId = cmt.Id 
              JOIN Reaction rct ON rct.Id = cmr.ReactionId 
            WHERE 
              crd.Id = @CardId 
            GROUP BY 
              usr.Id, 
              usr.PictureUrl, 
              usr.Username, 
              cmt.Content, 
              cmt.Id, 
              cmt.CreatedAt, 
              cmt.UpdatedAt, 
              crd.Id, 
              cmt.Id, 
              rct.Id, 
              rct.ReactionName;
            ";
            return _con.Query<CardCommentWithReactionCountDTO>(sql, new { CardId = cardId });

        }

        public IEnumerable<CardDetailDTO> GetCardDetailsByBoardId(int boardId)
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

        public CardDetailDTO? GetCardDetailsByCardId(int cardId)
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

        public IEnumerable<CardCustomFieldDTO> GetCustomFieldsByCardId(int cardId)
        {
            var cacheKey = $"card:{cardId}:custom_fields";

            if (_cache.TryGetValue(cacheKey, out IEnumerable<CardCustomFieldDTO>? cached))
                return cached!;
            const string sql = @"
            SELECT
                crd.Id CardId,
                brd.Id BoardId,
                ctf.Id CustomFieldId,
                ctf.Title CustomFieldTitle,
                dtt.DataTypeValue,
                ftm.Id FieldItemId,
                ftm.FieldItemValue,
                ctf.Position
            FROM Cards crd
            JOIN Stage stg ON stg.Id = crd.StageId
            JOIN Board brd ON brd.Id = stg.BoardId
            JOIN CustomField ctf ON ctf.BoardId = brd.Id
            JOIN DataType dtt ON dtt.Id = ctf.DataTypeId
            LEFT JOIN FieldItem ftm ON ftm.CustomFieldId = ctf.Id
            WHERE crd.Id = @CardId
            ORDER BY ctf.Position;
            ";
            var data = _con.Query<CardCustomFieldDTO>(sql, new { CardId = cardId }).ToList();
            _cache.Set(cacheKey, data, TimeSpan.FromMinutes(5));
            return data;
        }

        public IEnumerable<CardCustomFieldValueDTO> GetCustomFieldValuesByCardId(int cardId)
        {
            const string sql = @"
            WITH FieldValueCast AS (
                SELECT
                    fvl.Id,
                    fvl.CardId,
                    fvl.CustomFieldId,
                    dtt.DataTypeValue,
                    CASE
                        WHEN dtt.DataTypeValue = 'DROPDOWN'
                             AND FieldValue GLOB '[0-9]*' -- Check if value contains only digits before casting to INTEGER
                        THEN CAST(fvl.FieldValue AS INTEGER)
                        ELSE NULL
                    END AS ItemId,
                    fvl.FieldValue
                FROM FieldValue fvl
                JOIN CustomField ctf ON ctf.Id = fvl.CustomFieldId
                JOIN DataType dtt ON dtt.Id = ctf.DataTypeId
            )
            SELECT
                crd.Id AS CardId,
                brd.Id AS BoardId,
                ctf.Id AS CustomFieldId,
                ctf.Title AS CustomFieldTitle,
                fvc.DataTypeValue,
                fvc.FieldValue,
                ftm.FieldItemValue,
                ctf.Position
            FROM Cards crd
            JOIN Stage stg ON stg.Id = crd.StageId
            JOIN Board brd ON brd.Id = stg.BoardId
            JOIN CustomField ctf ON ctf.BoardId = brd.Id
            LEFT JOIN FieldValueCast fvc 
                ON fvc.CardId = crd.Id AND fvc.CustomFieldId = ctf.Id
            LEFT JOIN FieldItem ftm ON ftm.Id = fvc.ItemId
            WHERE crd.Id = @CardId
            ORDER BY ctf.Position;

            ";
            return _con.Query<CardCustomFieldValueDTO>(sql, new { CardId = cardId });
        }
    }
}
