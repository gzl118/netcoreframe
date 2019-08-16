using DInjectionProvider;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SANS.DbEntity.Models;
using SANS.WebApp.Models;

namespace SANS.WebApp.Data
{
    /// <summary>
    /// 关于用户信息的操作
    /// </summary>
    public class UserAccount
    {
        private WholeInjection injection;
        private CustomConfiguration customConfiguration;
        public UserAccount(WholeInjection injection, IOptionsSnapshot<CustomConfiguration> customConfiguration)
        {
            this.injection = injection;
            this.customConfiguration = customConfiguration.Value;
        }
        /// <summary>
        /// COOKIE名常量
        /// </summary>
        private const string USER_COOKIE_NAME = "UserLogin";
        /// <summary>
        /// 得到用户登录数据
        /// </summary>
        /// <returns></returns>
        public SysUser GetUserInfo()
        {
            var user = injection.GetHttpContext.HttpContext.Session.GetSession<SysUser>(USER_COOKIE_NAME);
            return user;
        }
        /// <summary>
        /// 登录操作
        /// </summary>
        /// <returns></returns>
        public bool Login(SysUser user)
        {
            user.IsAdministrctor = JudgeUserAdmin(user);
            user.IsBusinessAdministrctor = JudgeBusinessRoleAdmin(user);
            injection.GetHttpContext.HttpContext.Session.SetSession(USER_COOKIE_NAME, user);
            return true;
        }
        /// <summary>
        /// 判断用户是否超级管理员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool JudgeUserAdmin(SysUser user)
        {
            var adminAccount = customConfiguration.AdminAccount;
            var isAdmin = user.UserName.Equals(adminAccount);
            if (!isAdmin)
            {
                isAdmin = JudgeRoleAdmin(user);
            }
            return isAdmin;
        }
        /// <summary>
        /// 通过角色判断是否是超级管理员
        /// </summary>
        /// <returns></returns>
        public bool JudgeRoleAdmin(SysUser user)
        {
            if (string.IsNullOrWhiteSpace(customConfiguration.AdminRole))
                return false;
            if (user.SysRoles != null && user.SysRoles.Count > 0)
            {
                var tempRole = user.SysRoles.Find(p => p.RoleName.Equals(customConfiguration.AdminRole));
                if (tempRole != null)
                    return true;
            }
            if (user.SysUserGroup.SysRoles != null && user.SysUserGroup.SysRoles.Count > 0)
            {
                var tempRole = user.SysUserGroup.SysRoles.Find(p => p.RoleName.Equals(customConfiguration.AdminRole));
                if (tempRole != null)
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 判断用户是否是业务管理员
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public bool JudgeBusinessRoleAdmin(SysUser user)
        {
            if (string.IsNullOrWhiteSpace(customConfiguration.BusinessAdminRole))
                return false;
            if (user.SysRoles != null && user.SysRoles.Count > 0)
            {
                var tempRole = user.SysRoles.Find(p => p.RoleName.Equals(customConfiguration.BusinessAdminRole));
                if (tempRole != null)
                    return true;
            }
            if (user.SysUserGroup.SysRoles != null && user.SysUserGroup.SysRoles.Count > 0)
            {
                var tempRole = user.SysUserGroup.SysRoles.Find(p => p.RoleName.Equals(customConfiguration.BusinessAdminRole));
                if (tempRole != null)
                    return true;
            }
            return false;
        }
    }
}
