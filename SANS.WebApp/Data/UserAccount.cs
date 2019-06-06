using DInjectionProvider;
using SANS.DbEntity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using SANS.WebApp.Models;
using SANS.Common;

namespace SANS.WebApp.Data
{
    /// <summary>
    /// 关于用户信息的操作
    /// </summary>
    public class UserAccount
    {
        private WholeInjection injection;
        public UserAccount(WholeInjection injection)
        {
            this.injection = injection;
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
            return user == null ? null : user;
        }
        /// <summary>
        /// 登录操作
        /// </summary>
        /// <returns></returns>
        public bool Login(SysUser user)
        {
            user.isAdministrctor = JudgeUserAdmin(user);
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
            var adminAccount = injection.GetT<IOptionsSnapshot<CustomConfiguration>>().Value.adminAccount;
            return user.UserName.Equals(adminAccount) ? true : false;
        }
    }
}
