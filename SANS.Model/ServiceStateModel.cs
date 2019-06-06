using ProtoBuf;
using System;
using System.Collections.Generic;
using System.Text;

namespace SANS.Model
{
    /// <summary>
    /// 软件状态
    /// </summary>
    [ProtoContract]
    public class ServiceStateModel
    {
        /// <summary>
        /// 服务名称
        /// </summary>
        [ProtoMember(1)]
        public string Name { get; set; }
        /// <summary>
        /// 服务状态，正常填OK，服务发生错误时填错误信息
        /// </summary>
        [ProtoMember(2)]
        public string State { get; set; }
    }
}
