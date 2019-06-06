using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Net.Http;
using System.Text;

namespace SANS.Common
{
    public class HttpClientHelper
    {
        private static HttpClient _httpClient;
        private static readonly object _locker = new object();
        /// <summary>
        /// 单例模式(单例模式的目的是为了在程序中提供类的唯一实例,而且仅提供唯一的访问点.注意与静态类的区分)
        /// </summary>
        [SuppressMessage("ReSharper", "PossibleMultipleWriteAccessInDoubleCheckLocking")]
        public static HttpClient GetClient(int timeOut = 90)//超时时间(单位：秒)
        {
            if (null != _httpClient)
                return _httpClient;
            lock (_locker)
            {
                if (null != _httpClient)
                    return _httpClient;
                var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };
                var httpClient = new HttpClient(handler) { Timeout = new TimeSpan(0, 0, timeOut) };
                httpClient.DefaultRequestHeaders.Add("keep-alive", "true");//HTTP KeepAlive设为true,设置HTTP连接保持  
                _httpClient = httpClient;
            }
            return _httpClient;
        }

        /// <summary>
        /// 动态分配HttpClient
        /// </summary>
        public static HttpClient GetDynamicClient(int timeOut = 30)
        {
            var handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };
            var httpClient = new HttpClient(handler) { Timeout = new TimeSpan(0, 0, timeOut) };
            return httpClient;
        }
    }
}
