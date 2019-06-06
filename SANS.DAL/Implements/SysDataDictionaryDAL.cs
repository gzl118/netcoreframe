using SANS.DAL.Interface;
using SANS.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.DAL.Implements
{
    public class SysDataDictionaryDAL : BaseDAL<SysDataDictionary>, ISysDataDictionaryDAL
    {
        public SysDataDictionaryDAL(DapperContext dapper, MyEFContext db) : base(dapper, db) { }

        public bool Delete(List<string> list)
        {
            throw new NotImplementedException();
        }
    }
}
