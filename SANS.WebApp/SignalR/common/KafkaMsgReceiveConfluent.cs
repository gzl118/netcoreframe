using Confluent.Kafka;
using SANS.WebApp.Comm;
using SANS.WebApp.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SANS.WebApp.SignalR
{
    public class KafkaMsgReceiveConfluent : IMsgReceive
    {
        string kfkUrl = "";
        string kfkGroup = "";
        private readonly List<string> listTopic;
        private MsgHandling msgHandling;
        private readonly CancellationTokenSource _cts = new CancellationTokenSource();
        public KafkaMsgReceiveConfluent(MsgHandling _msgHandling)
        {
            this.msgHandling = _msgHandling;
            listTopic = new List<string>();
            listTopic.Add("engbc");
            listTopic.Add("alarm");
        }
        public void receiveMsg()
        {
            if (string.IsNullOrEmpty(kfkUrl))
            {
                kfkUrl = ProjectConfig.KafkaUrl;
                kfkGroup = ProjectConfig.KafkaGroup;
            }
            Task.Factory.StartNew(
            () =>
            {
                try
                {
                    var config = new ConsumerConfig
                    {
                        BootstrapServers = kfkUrl,
                        GroupId = kfkGroup,
                        EnableAutoCommit = false,
                        StatisticsIntervalMs = 5000,
                        SessionTimeoutMs = 6000,
                        AutoOffsetReset = AutoOffsetResetType.Latest
                    };

                    using (var consumer = new Consumer<Ignore, byte[]>(config))
                    {
                        consumer.Subscribe(listTopic);
                        while (!_cts.Token.IsCancellationRequested)
                        {
                            _cts.Token.ThrowIfCancellationRequested();
                            var consumeResult = consumer.Consume(_cts.Token);
                            consumer.Commit(consumeResult);
                            msgHandling.ProcessData(consumeResult.Topic, consumeResult.Value);
                        }
                    }
                }
                catch (Exception er)
                {
                    msgHandling.logSend(er.Message);
                }
            }, _cts.Token);
        }
        public void stopMsg()
        {
            Log.Log4netHelper.Error(this, "停止kafka监听");
            _cts.Cancel();
        }
    }
}
