using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnitTestForTrello.Models.Interface
{
    public interface IDbTemplate
    {
        T QueryForObject<T>(string sql, Func<IDataReader, T> mapFunc, params object[] parameters);
        List<T> Query<T>(string sql, Func<IDataReader, T> mapFunc);
        int Update(string sql, params object[] parameters);
    }
}
