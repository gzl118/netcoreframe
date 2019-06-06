using SANS.BLL.Interface;
using SANS.DAL.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Data.SqlClient;

namespace SANS.BLL.Implements
{
    public class BaseBLL<T> : IBaseBLL<T> where T : class, new()
    {
        protected IBaseDAL<T> Dal { get; set; }
        public BaseBLL(IBaseDAL<T> Dal)
        {
            this.Dal = Dal;
        }
        public bool Add(T t)
        {
            Dal.Add(t);
            return Dal.SaveChanges();
        }
        public bool AddRange(IEnumerable<T> t)
        {
            Dal.AddRange(t);
            return Dal.SaveChanges();
        }
        public bool Delete(T t)
        {
            Dal.Delete(t);
            return Dal.SaveChanges();
        }
        public bool DeleteRange(IEnumerable<T> t)
        {
            Dal.DeleteRange(t);
            return Dal.SaveChanges();
        }
        public bool Update(T t)
        {
            Dal.Update(t);
            return Dal.SaveChanges();
        }
        public int ExecuteSql(string sql, SqlParameter parameter = null)
        {
            return Dal.ExecuteSql(sql, parameter);
        }
        public int GetCount(Expression<Func<T, bool>> WhereLambda)
        {
            return Dal.GetCount(WhereLambda);
        }
        public List<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return Dal.GetModels(whereLambda).ToList();
        }

        public List<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc,
            Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda)
        {
            return Dal.GetModelsByPage(pageSize, pageIndex, isAsc, OrderByLambda, WhereLambda).ToList();
        }

        public IEnumerable<T> SqlQuery(string sql)
        {
            return Dal.SqlQuery<T>(sql);
        }
        public T GetModel(Expression<Func<T, bool>> whereLambda)
        {
            return Dal.GetModel(whereLambda);
        }
        public List<T> GetModelsNoTracking(Expression<Func<T, bool>> whereLambda)
        {
            return Dal.GetModelsNoTracking(whereLambda).ToList();
        }
        public bool Delete(List<string> list)
        {
            return Dal.Delete(list);
        }
        public T GetModelNoTracking(Expression<Func<T, bool>> whereLambda)
        {
            return Dal.GetModelNoTracking(whereLambda);
        }
    }
}
