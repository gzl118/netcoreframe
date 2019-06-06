using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using SANS.BLL.Interface;
using SANS.DAL.Interface;
using SANS.DbEntity.Models;

namespace SANS.BLL.Implements
{
    public class DictAuthorityTypeBLL : BaseBLL<DictAuthorityType>, IDictAuthorityTypeBLL
    {
        private readonly IDictAuthorityTypeDAL dictAuthorityType;
        /// <summary>
        /// 用于实例化父级，dictAuthorityType
        /// </summary>
        /// <param name="dictAuthorityType"></param>
        public DictAuthorityTypeBLL(IDictAuthorityTypeDAL dictAuthorityType) : base(dictAuthorityType)
        {
            this.dictAuthorityType = dictAuthorityType;
        }
    }
}
