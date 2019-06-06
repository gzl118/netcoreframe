﻿using SANS.DAL.Interface;
using SANS.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.DAL.Implements
{
    public class DictAuthorityTypeDAL : BaseDAL<DictAuthorityType>, IDictAuthorityTypeDAL
    {
        public DictAuthorityTypeDAL(DapperContext dapper, MyEFContext db) : base(dapper, db)
        {
        }

        public bool Delete(List<string> list)
        {
            throw new NotImplementedException();
        }
    }
}
