using KN.Common;
using KN.Model;
using KN.WebApp.Comm;
using KN.WebApp.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KN.WebApp.SignalR
{
    public class KafkaMsgReceive : IMsgReceive
    {
        string kfkUrl = "";
        string kfkGroup = "";
        public static CancellationTokenSource ctsKafka = new CancellationTokenSource();
        private MsgHandling msgHandling;
        public KafkaMsgReceive(MsgHandling _msgHandling,
            MsgConfig msgConfig)
        {
            kfkUrl = msgConfig.KafkaUrl;
            kfkGroup = msgConfig.KafkaGroup;
            this.msgHandling = _msgHandling;
        }
        public void receiveMsg()
        {
            //Task.Factory.StartNew(
            //() =>
            //{
            //    try
            //    {
            //        EventConsumer consumer = CreateKafkaConsumer();
            //        if (consumer == null)
            //        {
            //            ctsKafka.Cancel();
            //            return;
            //        }
            //        MessageAndError? msgee = null;
            //        while (!ctsKafka.Token.IsCancellationRequested)
            //        {
            //            //ctsKafka.Token.ThrowIfCancellationRequested();
            //            try
            //            {
            //                msgee = consumer.Consume(new TimeSpan(0, 0, 1));
            //                if(consumer==null)
            //                {
            //                    consumer = CreateKafkaConsumer();
            //                }
            //                if (msgee == null)
            //                {
            //                    continue;
            //                }
            //                if (msgee?.Error != ErrorCode.NO_ERROR)
            //                {
            //                    continue;
            //                }
            //                if (msgee != null)
            //                {
            //                    var msg = msgee.Value.Message;
            //                    msgHandling.ProcessData(msg.Topic,msg.Payload);
            //                }
            //            }
            //            catch (Exception er)
            //            {
            //                msgHandling.logSend("while:" + er.Message);
            //                continue;
            //            }
            //        }
            //        if (consumer != null)
            //        {
            //            consumer.Unsubscribe();
            //            consumer.Stop();
            //            consumer = null;
            //        }
            //    }
            //    catch (Exception er)
            //    {
            //        msgHandling.logSend(er.Message);
            //    }
            //}, ctsKafka.Token);
        }


        public void receiveMsgTest()
        {
            //var config = new Config() { GroupId = kfkGroup, EnableAutoCommit = false };
            //using (var consumer = new EventConsumer(config, kfkUrl))
            //{
            //    consumer.OnMessage += (obj, msg) =>
            //    {
            //        msgHandling.ProcessData(msg.Topic, msg.Payload);
            //        consumer.Commit(msg);
            //    };
            //    consumer.Subscribe(new List<string> { "HistoryTm","RealtimeTm", "TrainProcess", "TMCEvent", "TmParameterAbnormity", "TmParameterCycle", "ServiceState", "Log" });
            //    consumer.Start();
            //}
        }

        //private EventConsumer CreateKafkaConsumer()
        //{
        //    var config = new Config() { GroupId = kfkGroup, EnableAutoCommit = false };
        //    EventConsumer objKafkaConsumer = null;
        //    try
        //    {
        //        objKafkaConsumer = new EventConsumer(config, kfkUrl);
        //    }
        //    catch (Exception er)
        //    {
        //        msgHandling.logSend(er.Message);
        //        objKafkaConsumer = null;
        //        return null;
        //    }
        //    try
        //    {
        //        List<string> listTopic = new List<string>();
        //        listTopic.Add("HistoryTm");
        //        listTopic.Add("RealtimeTm");
        //        listTopic.Add("TrainProcess");
        //        listTopic.Add("TMCEvent");
        //        listTopic.Add("TmParameterAbnormity");
        //        listTopic.Add("TmParameterCycle");
        //        listTopic.Add("ServiceState");
        //        listTopic.Add("Log");
        //        objKafkaConsumer.Subscribe(listTopic);
        //        objKafkaConsumer.Start();

        //    }
        //    catch (Exception er)
        //    {
        //        objKafkaConsumer = null;
        //        msgHandling.logSend(er.Message);
        //    }
        //    return objKafkaConsumer;
        //}

        public void stopMsg()
        {
            ctsKafka.Cancel();
        }
    }
}
