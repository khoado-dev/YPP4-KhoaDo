namespace UnitTestForTrello.Repositories
{
    public interface IRepository<T>
    {
        int Create(T entity);
        T? GetById(int id);
        List<T> GetAll();
        bool Update(T entity);
        bool Delete(int id);
    }
}