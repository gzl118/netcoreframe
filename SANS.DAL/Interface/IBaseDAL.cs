using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace SANS.DAL.Interface
{
    public interface IBaseDAL<T> where T : class
    {
        void Add(T t);
        void AddRange(IEnumerable<T> t);
        void Delete(T t);
        void DeleteRange(IEnumerable<T> t);
        void Update(T t);
        int GetCount(Expression<Func<T, bool>> WhereLambda);
        int ExecuteSql(string sql, SqlParameter parameter = null);
        IEnumerable<TR> SqlQuery<TR>(string sql);
        T GetModel(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc, Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda);
        /// <summary>
        /// 一个业务中有可能涉及到对多张表的操作,那么可以将操作的数据,打上相应的标记,最后调用该方法,将数据一次性提交到数据库中,避免了多次链接数据库。
        /// </summary>
        bool SaveChanges();
        IQueryable<T> GetModelsNoTracking(Expression<Func<T, bool>> whereLambda);
        bool Delete(List<string> list);
        T GetModelNoTracking(Expression<Func<T, bool>> whereLambda);
    }
}
