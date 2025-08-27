using Microsoft.Data.Sqlite;

namespace CustomMVC.App.Data
{
    public interface IDbConnection
    {
        SqliteConnection Open();
    }
}
