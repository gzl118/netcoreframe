using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Model
{
    /// <summary>
    /// 软件日志
    /// </summary>
    [ProtoContract]
    public class LogModel
    {
        /// <summary>
        /// 产生日志的软件名称
        /// </summary>
        [ProtoMember(1)]
        public string Source { get; set; }
        /// <summary>
        /// 系统时间
        /// </summary>
        [ProtoMember(2)]
        public double SystemTime { get; set; }
        /// <summary>
        /// 星时
        /// </summary>
        [ProtoMember(3)]
        public double SatelliteTime { get; set; }
        /// <summary>
        /// 日志消息
        /// </summary>
        [ProtoMember(4)]
        public string Message { get; set; }

    }
}
