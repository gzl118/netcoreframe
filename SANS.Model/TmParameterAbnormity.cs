using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Model
{
    /// <summary>
    /// 异常检测结果
    /// </summary>
    [ProtoContract]
    public class TmParameterAbnormity
    {
        /// <summary>
        /// 参数代号，如：IN1，TN13
        /// </summary>
        [ProtoMember(1)]
        public string Symbol { get; set; }
        /// <summary>
        /// 起始时间
        /// </summary>
        [ProtoMember(2)]
        public double BeginTime { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        [ProtoMember(3)]
        public double EndTime { get; set; }
        /// <summary>
        /// 相关测控事件，如：位保
        /// </summary>
        [ProtoMember(4)]
        public string TMCEvent { get; set; }
    }
}
