using System.Data.SqlClient;

namespace UnitTestForTrello.Tests
{
    public abstract class DatabaseTestBase
    {
        protected SqlConnection? _connection;
        protected SqlTransaction? _transaction;

        protected string ConnectionString => "Server=intern-khoado;Database=Trello;User Id=sa;Password=123456;TrustServerCertificate=True;"; // Replace with your test DB connection string

        [TestInitialize]
        public void BeginTransaction()
        {
            _connection = new SqlConnection(ConnectionString);
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        [TestCleanup]
        public void RollbackTransaction()
        {
            _transaction?.Rollback();
            _connection?.Close();
        }
    }
}
