using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Model
{
    [ProtoContract]
    public class ModelUpdateModel
    {
        [ProtoMember(1)]
        public string ModelName { get; set; }
        [ProtoMember(2)]
        public string ModelFile { get; set; }//模型文件路径
    }
}
