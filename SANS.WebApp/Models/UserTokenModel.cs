using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.Models
{
    public class UserTokenModel
    {
        /// <summary>
        /// 请求用户Id
        /// </summary>
        public string ReqUserId { get; set; }
        /// <summary>
        /// 请求用户令牌
        /// </summary>
        public string ReqUserToken { get; set; }
        /// <summary>
        /// 请求有效时间
        /// </summary>
        public long ReqDateExpire { get; set; }
    }
}
