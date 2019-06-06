
using Microsoft.AspNetCore.Http;
using System;

namespace DInjectionProvider
{
    /// <summary>
    /// 依赖注入提供者类
    /// </summary>
    public class WholeInjection
    {
        public WholeInjection(IHttpContextAccessor _httpContextAccessor)
        {
            GetHttpContext = _httpContextAccessor;
        }
        /// <summary>
        /// 可以获取.net core 配置了依赖注入关系的所有实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetT<T>()
        {
            return (T)GetHttpContext.HttpContext.RequestServices.GetService(typeof(T));
        }
        public IHttpContextAccessor GetHttpContext { get; }

    }
}
