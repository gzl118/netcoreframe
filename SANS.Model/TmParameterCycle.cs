using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Model
{
    /// <summary>
    /// 周期检测结果
    /// </summary>
    [ProtoContract]
    public class TmParameterCycle
    {
        /// <summary>
        /// 参数代号，如：IN1，TN13
        /// </summary>
        [ProtoMember(1)]
        public string Symbol { get; set; }
        /// <summary>
        /// 周期，单位秒
        /// </summary>
        [ProtoMember(2)]
        public double Cycle { get; set; }

    }
}
