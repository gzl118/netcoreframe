using System;

namespace SANS.WebApp.Filters
{
    /// <summary>
    /// 匿名访问标记
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AnonymousFilter : Attribute
    {
    }
}