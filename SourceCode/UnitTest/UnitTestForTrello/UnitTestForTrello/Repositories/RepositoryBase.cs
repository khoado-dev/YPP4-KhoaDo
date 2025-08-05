using System.Data.SqlClient;
namespace UnitTestForTrello.Repositories
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        protected readonly SqlConnection _con;
        protected readonly SqlTransaction _tran;

        protected RepositoryBase(SqlConnection con, SqlTransaction tran)
        {
            _con = con;
            _tran = tran;
        }

        public abstract int Create(T entity);
        public abstract T? GetById(int id);
        public abstract List<T> GetAll();
        public abstract bool Update(T entity);
        public abstract bool Delete(int id);

        protected abstract T MapReaderToEntity(SqlDataReader reader);
    }
}