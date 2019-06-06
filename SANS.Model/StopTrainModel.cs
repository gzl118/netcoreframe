using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Model
{
    [ProtoContract]
    public class StopTrainModel
    {
        [ProtoMember(1)]
        public string ModelName { get; set; }
    }
}
