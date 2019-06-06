using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using Dapper;
using MySql.Data.MySqlClient;

namespace SANS.DAL.Implements
{
    public class BaseDAL<T> where T : class, new()
    {

        /// <summary>
        /// 数据库上下文
        /// </summary>
        protected readonly DapperContext dapper;
        protected readonly DbEntity.Models.MyEFContext db;
        /// <summary>
        /// 构造函数依赖注入
        /// </summary>
        /// <param name="db"></param>
        public BaseDAL(DapperContext dapper, DbEntity.Models.MyEFContext db)
        {
            this.dapper = dapper;
            this.db = db;
        }
        public void Add(T t)
        {
            db.Set<T>().Add(t);
        }
        public void AddRange(IEnumerable<T> t)
        {
            db.AddRange(t);
        }
        public void Delete(T t)
        {
            db.Set<T>().Remove(t);
        }
        public void DeleteRange(IEnumerable<T> t)
        {
            db.Set<T>().RemoveRange(t);
        }
        public int GetCount(Expression<Func<T, bool>> WhereLambda)
        {
            return db.Set<T>().AsNoTracking().Count(WhereLambda);
        }
        public void Update(T t)
        {
            db.Set<T>().Update(t);
        }
        public T GetModel(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda).FirstOrDefault();
        }
        public IQueryable<T> GetModels(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().Where(whereLambda);
        }
        public IQueryable<T> GetModelsByPage<type>(int pageSize, int pageIndex, bool isAsc,
            Expression<Func<T, type>> OrderByLambda, Expression<Func<T, bool>> WhereLambda)
        {
            if (pageSize == -1)
            {
                if (isAsc)
                {
                    return db.Set<T>().AsNoTracking().Where(WhereLambda).OrderBy(OrderByLambda);
                }
                else
                {
                    return db.Set<T>().AsNoTracking().Where(WhereLambda).OrderByDescending(OrderByLambda);
                }
            }
            else
            {
                //是否升序
                if (isAsc)
                {
                    var temp = db.Set<T>().AsNoTracking().Where(WhereLambda).OrderBy(OrderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                    return temp;
                }
                else
                {
                    return db.Set<T>().AsNoTracking().Where(WhereLambda).OrderByDescending(OrderByLambda).Skip((pageIndex - 1) * pageSize).Take(pageSize);
                }
            }
        }
        public int ExecuteSql(string sql, SqlParameter parameter = null)
        {
            if (parameter == null)
                return db.Database.ExecuteSqlCommand(sql);
            else
                return db.Database.ExecuteSqlCommand(sql, parameter);
        }
        public IEnumerable<TR> SqlQuery<TR>(string sql)
        {
            using (var conn = dapper.GetConnection)
            {
                return conn.Query<TR>(sql);
            }

        }
        public bool SaveChanges()
        {
            return db.SaveChanges() > 0;
        }
        public IQueryable<T> GetModelsNoTracking(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().AsNoTracking().Where(whereLambda);
        }
        public T GetModelNoTracking(Expression<Func<T, bool>> whereLambda)
        {
            return db.Set<T>().AsNoTracking().Where(whereLambda).FirstOrDefault();
        }
        public int ExcuteSqlByDrapper(string sql)
        {
            using (var conn = dapper.GetConnection)
            {
                MySqlCommand comd = (MySqlCommand)conn.CreateCommand();
                comd.CommandText = sql;
                comd.CommandType = System.Data.CommandType.Text;
                return comd.ExecuteNonQuery();
            }

        }
    }
}
