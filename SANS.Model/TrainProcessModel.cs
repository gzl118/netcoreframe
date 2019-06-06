using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Model
{
    /// <summary>
    /// 训练学习进度
    /// </summary>
    [ProtoContract]
    public class TrainProcessModel
    {
        [ProtoMember(1)]
        public string ModelName { get; set; }
        /// <summary>
        /// 模型训练进度
        /// </summary>
        [ProtoMember(2)]
        public double Process { get; set; }
    }
}
