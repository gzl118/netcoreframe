using SANS.Common;
using SANS.Log;
using SANS.Model;
using SANS.RedisOperation;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SANS.DbEntity;
using SANS.DbEntity.Models;
using DInjectionProvider;
using SANS.BLL.Interface;
using SANS.WebApp.Models;
using System.Text;
using System.Linq;

namespace SANS.WebApp.SignalR
{
    public class MsgHandling
    {
        private IMsgSend msgSend;
        public MsgHandling(IMsgSend MsgSend)
        {
            this.msgSend = MsgSend;
        }
        public void ProcessData(string Topic, Byte[] Payload)
        {
            switch (Topic)
            {
                case "engbc":
                    try
                    {
                        if (Payload != null)
                        {
                            //var tmPack = ProtobufHelper.BytesToObject<TmPack>(Payload, 0, Payload.Length);                            
                        }
                    }
                    catch (Exception er)
                    {
                        logSend("engbc:" + er.Message);
                    }
                    break;
                default:
                    break;
            }
        }

        public void logSend(string mlog)
        {
            Log4netHelper.Error(this, mlog);
            LogModel lg = new LogModel();
            lg.SystemTime = DateTools.TimeToDouble(DateTime.Now);
            lg.SatelliteTime = 0;
            lg.Source = "webSystem";
            lg.Message = mlog;
            msgSend.send<LogModel>("Log", lg);

        }
    }
}
