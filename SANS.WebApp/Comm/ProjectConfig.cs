using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.Comm
{
    /// <summary>
    /// 工程配置类
    /// </summary>
    public class ProjectConfig
    {
        /// <summary>
        /// kafka地址
        /// </summary>
        public static string KafkaUrl { get; set; }
        /// <summary>
        /// kafka组名称
        /// </summary>
        public static string KafkaGroup { get; set; }
        /// <summary>
        /// 系统名称
        /// </summary>
        public static string SysName { get; set; }
    }
}
