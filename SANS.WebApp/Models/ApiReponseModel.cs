using SANS.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.Models
{
    //public class ApiReponseModel
    //{
    //    /// <summary>
    //    /// 响应状态，成功1，失败2，业务处理错误3，找到不到资源4
    //    /// </summary>
    //    public ApiResponseCode Code { get; set; } = ApiResponseCode.success;
    //    /// <summary>
    //    /// 响应提示消息
    //    /// </summary>
    //    public string Message { set; get; }
    //    /// <summary>
    //    /// 响应数据
    //    /// </summary>
    //    public object Data { set; get; }
    //}
    /// <summary>
    /// 响应状态码
    /// </summary>
    public enum ApiResponseCode
    {
        /// <summary>
        /// 成功,1
        /// </summary>
        success = 1,
        /// <summary>
        /// 失败，2
        /// </summary>
        failure = 2,
        /// <summary>
        /// 业务处理错误，3
        /// </summary>
        businessfail = 3,
        /// <summary>
        /// 找到不到资源，4
        /// </summary>
        fail404
    }
    public class ApiReponseModel<T>
    {
        public ApiReponseModel() { }
        public ApiReponseModel(T result)
        {
            Code = ApiResponseCode.success;
            Data = result;
        }
        public ApiReponseModel(ApiResponseCode code, string message)
        {
            Code = code;
            Message = message;
        }
        /// <summary>
        /// 响应状态，成功1，失败2，业务处理错误3，找到不到资源4
        /// </summary>
        public ApiResponseCode Code { get; set; } = ApiResponseCode.success;
        /// <summary>
        /// 响应提示消息
        /// </summary>
        public string Message { set; get; }
        /// <summary>
        /// 响应数据
        /// </summary>
        public T Data { set; get; }
        /// <summary>
        /// 当获取分页数据时的总数据量
        /// </summary>
        public int Count { get; set; }

    }
}
