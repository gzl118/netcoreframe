#region 版权声明
/**************************************************************** 
 * 作    者：周黎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2018/7/7 14:53:41 
 * 当前版本：1.0.0.1 
 *  
 * 描述说明： 
 * 
 * 修改历史： 
 * 
***************************************************************** 
 * Copyright @ ZhouLi 2018 All rights reserved 
 *┌──────────────────────────────────┐
 *│　此技术信息周黎的机密信息，未经本人书面同意禁止向第三方披露．　│
 *│　版权所有：周黎 　　　　　　　　　　　　　　│
 *└──────────────────────────────────┘
*****************************************************************/
#endregion

namespace SANS.WebApp.Models
{
    /// <summary>
    /// 自定义配置类
    /// </summary>
    public class CustomConfiguration
    {
        /// <summary>
        /// 超级管理账户
        /// </summary>
        public string AdminAccount { set; get; }
        /// <summary>
        /// 是否验证token
        /// </summary>
        public bool IsVerifyToken { get; set; }
        /// <summary>
        /// 容许访问的IP，多个以英文分号隔开
        /// </summary>
        public string AllowAccessIp { get; set; }
        /// <summary>
        /// 页面超时时间，单位分钟
        /// </summary>
        public int PageTimeout { get; set; }
        /// <summary>
        /// 超级管理员角色
        /// </summary>
        public string AdminRole { get; set; }
        /// <summary>
        /// 业务管理员角色
        /// </summary>
        public string BusinessAdminRole { get; set; }
    }
}
