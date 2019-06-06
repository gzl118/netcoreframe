
using System.Data;
using System.Data.SqlClient;
using Microsoft.Data.Sqlite;

namespace SANS.DAL
{

    /// <summary>
    /// Dapper操作数据库上下文类
    /// </summary>
    public class DapperContext
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        private readonly string sqlConnection;
        /// <summary>
        /// 数据库类型(1=sqlserver,2=sqlite)
        /// </summary>
        private readonly string dataBaseType;
        public DapperContext(string sqlConnection, string dataBaseType)
        {
            this.sqlConnection = sqlConnection;
            this.dataBaseType = dataBaseType;
        }
        public IDbConnection GetConnection
        {
            get
            {
                IDbConnection conn = null;
                if (dataBaseType == "3")
                    conn = new SqlConnection(sqlConnection);
                else if (dataBaseType == "2")
                    conn = new SqliteConnection(sqlConnection);
                else if (dataBaseType == "1")
                    conn = new MySql.Data.MySqlClient.MySqlConnection(sqlConnection);
                conn.Open();
                return conn;
            }
        }

    }
}
