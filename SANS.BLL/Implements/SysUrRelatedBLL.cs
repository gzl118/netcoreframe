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
    public class SysUrRelatedBLL : BaseBLL<SysUrRelated>, ISysUrRelatedBLL
    {
        private readonly ISysUrRelatedDAL sysUrRelated;
        /// <summary>
        /// 用于实例化父级，sysUrRelated
        /// </summary>
        /// <param name="sysUrRelated"></param>
        public SysUrRelatedBLL(ISysUrRelatedDAL sysUrRelated) : base(sysUrRelated)
        {
            this.sysUrRelated = sysUrRelated;
        }
    }
}
