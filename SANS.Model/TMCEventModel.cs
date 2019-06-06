using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Model
{
    /// <summary>
    /// 测控事件检测结果
    /// </summary>
    [ProtoContract]
    public class TMCEventModel
    {
        /// <summary>
        /// 测控事件名称
        /// </summary>
        [ProtoMember(1)]
        public string Name { get; set; }
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
    }
}
