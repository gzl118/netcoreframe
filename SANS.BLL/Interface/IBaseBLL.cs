using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace SANS.BLL.Interface
{
    public interface IBaseBLL<T> where T : class, new()
    {
        bool Add(T t);
        bool AddRange(IEnumerable<T> t);
        bool Delete(T t);
        bool Delete(List<string> list);
        bool DeleteRange(IEnumerable<T> t);
        bool Update(T t);
        int GetCount(Expression<Func<T, bool>> WhereLambda);
        int ExecuteSql(string sql, SqlParameter parameter);
        IEnumerable<T> SqlQuery(string sql);
        List<T> GetModels(Expression<Func<T, bool>> whereLambda);
        List<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda);
        T GetModel(Expression<Func<T, bool>> whereLambda);
        List<T> GetModelsNoTracking(Expression<Func<T, bool>> whereLambda);
        T GetModelNoTracking(Expression<Func<T, bool>> whereLambda);
    }
}
