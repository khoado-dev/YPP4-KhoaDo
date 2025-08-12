using Dapper;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitTestForTrello.Models.DTOs;

namespace UnitTestForTrello.Repositories
{
    public class BoardRepository
    {
        private readonly IDbConnection _con;
        private readonly IDbTransaction _tran;

        public BoardRepository(IDbConnection con, IDbTransaction tran)
        {
            _con = con;
            _tran = tran;
        }

        public IEnumerable<BoardDTO> GetStarredBoardsByUser(int userId)
        {
            const string sql = @"
            SELECT
                usb.UserId,
                brd.Id AS BoardId,
                brd.BackgroundUrl,
                brd.BoardName,
                brd.BoardStatus
            FROM UserStarredBoard usb
            JOIN Board brd ON brd.Id = usb.BoardId
            WHERE usb.UserId = @UserId
              AND brd.BoardStatus = 'active'
            ORDER BY usb.CreatedAt DESC";

            return _con.Query<BoardDTO>(sql, new { UserId = userId }, _tran);
        }
    }
}
