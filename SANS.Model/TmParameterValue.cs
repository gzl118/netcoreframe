
using ProtoBuf;
using System;

namespace SANS.Model
{
    /// <summary>
    /// 
    /// </summary>
    [ProtoContract]
    public class TmParameterValue
    {
        /// <summary>
        /// 参数代号，如：IN1，TN13
        /// </summary>
        [ProtoMember(1)]
        public string Symbol;
        /// <summary>
        /// 时间
        /// </summary>
        [ProtoMember(2)]
        public double Time;
        /// <summary>
        /// 参数值
        /// </summary>
        [ProtoMember(3)]
        public double Value;
        /// <summary>
        /// 参数原码
        /// </summary>
        [ProtoMember(4)]
        public ulong Code;
        /// <summary>
        /// 参数文本
        /// </summary>
        [ProtoMember(5)]
        public string Text;
    }

    /// <summary>
    /// 
    /// </summary>
    [ProtoContract]
    public class TmParameterValues
    {
        /// <summary>
        /// 
        /// </summary>
        [ProtoMember(1)]
        public TmParameterValue[] ValueList;
    }
}
