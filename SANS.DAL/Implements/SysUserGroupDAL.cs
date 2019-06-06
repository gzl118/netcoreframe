using SANS.DAL.Interface;
using SANS.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.DAL.Implements
{
    public class SysUserGroupDAL : BaseDAL<SysUserGroup>, ISysUserGroupDAL
    {
        public SysUserGroupDAL(DapperContext dapper, MyEFContext db) : base(dapper, db) { }

        public bool Delete(List<string> list)
        {
            throw new NotImplementedException();
        }
    }
}
