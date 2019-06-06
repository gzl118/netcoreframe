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
    public class SysAmRelatedBLL : BaseBLL<SysAmRelated>, ISysAmRelatedBLL
    {
        private readonly ISysAmRelatedDAL sysAmRelated;
        /// <summary>
        /// 用于实例化父级，sysAmRelated
        /// </summary>
        /// <param name="sysAmRelated"></param>
        public SysAmRelatedBLL(ISysAmRelatedDAL sysAmRelated) : base(sysAmRelated)
        {
            this.sysAmRelated = sysAmRelated;
        }
    }
}
