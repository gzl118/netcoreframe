using SANS.Common;
using SANS.Log;
using SANS.WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.Helper
{
    public class ApiHelper
    {
        /// <summary>
        /// 返回的业务处理结果回调方法
        /// </summary>
        /// <param name="func"></param>
        /// <returns></returns>
        public static ApiReponseModel<T> TryInvoke<T>(Func<ApiReponseModel<T>> func)
        {
            try
            {
                return func();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
