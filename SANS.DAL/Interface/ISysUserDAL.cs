using SANS.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;

namespace SANS.DAL.Interface
{
    public interface ISysUserDAL : IBaseDAL<SysUser>
    {
        #region
        /// <summary>
        /// 获取需要登录的用户所有信息
        /// </summary>
        /// <returns></returns>
        SysUser SetLoginSysUser(SysUser user);
        #endregion
    }
}
