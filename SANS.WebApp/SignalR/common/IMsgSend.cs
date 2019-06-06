using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.SignalR
{
    public interface IMsgSend
    {
        void send<T>(string topicName, T t);
        void sendAsync<T>(string topicName, T t);
    }
}
