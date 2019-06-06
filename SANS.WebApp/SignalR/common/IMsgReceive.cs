using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.SignalR
{
    public interface IMsgReceive
    {
        void stopMsg();
        void receiveMsg();
    }
}
