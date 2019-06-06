using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Model
{
    [ProtoContract]
    public class StartTrainModel
    {
        [ProtoMember(1)]
        public string ModelName { get; set; }
        [ProtoMember(2)]
        public double BeginTime { get; set; }//起始时间
        [ProtoMember(3)]
        public double EndTime { get; set; }//结束时间
        [ProtoMember(4)]
        public double Cycle { get; set; }//周期，单位秒
    }
}
