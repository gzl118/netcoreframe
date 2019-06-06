#region 版权声明
/**************************************************************** 
 * 作    者：周黎 
 * CLR 版本：4.0.30319.42000 
 * 创建时间：2018/6/8 21:41:11 
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
using Newtonsoft.Json;
using System.Text;

namespace Microsoft.AspNetCore.Http//(跟.net core session 命名空间一样,这样就不需要单独引入命名空间)
{
    /// <summary>
    /// 20180603 周黎编写
    /// .net core Session扩展类
    /// </summary>
    public static class SessionExtended
    {
        /// <summary>
        /// 设置session
        /// </summary>
        /// <typeparam name="T">保存的类型</typeparam>
        /// <param name="session"></param>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void SetSession<T>(this ISession session, string key, T value)
        {
            session.Set(key, Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(value)));
        }
        /// <summary>
        /// 获取Session
        /// </summary>
        /// <typeparam name="T">获取的类型</typeparam>
        /// <param name="session"></param>
        /// <param name="key">键</param>
        /// <returns></returns>
        public static T GetSession<T>(this ISession session, string key)
        {
            byte[] b = null;
            session.TryGetValue(key, out b);
            var value = b == null ? null : Encoding.UTF8.GetString(b);
            return value == null ? default(T) :
                                  JsonConvert.DeserializeObject<T>(value);
        }
    }
}
