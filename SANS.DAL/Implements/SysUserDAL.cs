using SANS.DAL.Interface;
using SANS.DbEntity.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SANS.DAL.Implements
{
    public class SysUserDAL : BaseDAL<SysUser>, ISysUserDAL
    {
        public SysUserDAL(DapperContext dapper, MyEFContext db) : base(dapper, db) { }

        public bool Delete(List<string> list)
        {
            throw new NotImplementedException();
        }

        #region 设置登录用户的用户组,角色信息
        /// <summary>
        /// 设置用户的登录用户用户组,角色信息
        /// </summary>
        /// <returns></returns>
        public SysUser SetLoginSysUser(SysUser user)
        {
            user.SysUserGroup = (from t1 in db.Set<SysUserGroup>() where t1.UserGroupId.Equals(user.UserGroupId) select t1).FirstOrDefault();
            if (user.SysUserGroup != null)
            {
                user.SysUserGroup.SysRoles = (from sur in db.SysUgrRelated
                                              join sr in db.SysRole
                                              on sur.RoleId equals sr.RoleId
                                              where sur.UserGroupId.Equals(user.SysUserGroup.UserGroupId)
                                              select new SysRole
                                              {
                                                  RoleId = sr.RoleId,
                                                  RoleName = sr.RoleName,
                                                  CreateUserId = sr.CreateUserId,
                                                  DeleteSign = sr.DeleteSign,
                                                  CreateTime = sr.CreateTime,
                                                  DeleteTime = sr.DeleteTime,
                                                  EditTime = sr.EditTime,
                                                  Note = sr.Note
                                              }
                             ).ToList();
            }
            user.SysRoles = (from sur in db.SysUrRelated
                             join sr in db.SysRole
                             on sur.RoleId equals sr.RoleId
                             where sur.UserId.Equals(user.UserId)
                             select new SysRole
                             {
                                 RoleId = sr.RoleId,
                                 RoleName = sr.RoleName,
                                 CreateUserId = sr.CreateUserId,
                                 DeleteSign = sr.DeleteSign,
                                 CreateTime = sr.CreateTime,
                                 DeleteTime = sr.DeleteTime,
                                 EditTime = sr.EditTime,
                                 Note = sr.Note
                             }
                                 ).ToList();
            return user;
        }
        #endregion
    }
}
