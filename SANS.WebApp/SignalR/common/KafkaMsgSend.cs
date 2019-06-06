using Confluent.Kafka;
using SANS.Common;
using SANS.WebApp.Comm;
using SANS.WebApp.Models;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SANS.WebApp.SignalR
{
    public class KafkaMsgSend : IMsgSend
    {
        static string kfkUrl = "";
        public KafkaMsgSend()
        {
            kfkUrl = ProjectConfig.KafkaUrl;
        }
        public void send<T>(string topicName, T t)
        {
            var config = new ProducerConfig { BootstrapServers = kfkUrl };
            using (var producer = new Producer<string, byte[]>(config))
            {
                byte[] data = ProtobufHelper.ObjectToBytes<T>(t);
                producer.BeginProduce(topicName, new Message<string, byte[]>() { Key = "publishKey", Value = data });
                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }
        public void sendAsync<T>(string topicName, T t)
        {
            var config = new ProducerConfig { BootstrapServers = kfkUrl };
            using (var producer = new Producer<string, byte[]>(config))
            {
                byte[] data = ProtobufHelper.ObjectToBytes<T>(t);
                var deliveryReport = producer.ProduceAsync(topicName, new Message<string, byte[]>() { Key = "publishKey", Value = data });
                deliveryReport.ContinueWith(task =>
                 {
                     Console.WriteLine("Producer: " + producer.Name + "\r\nTopic: " + topicName + "\r\nPartition: " + task.Result.Partition + "\r\nOffset: " + task.Result.Offset);
                 });
                producer.Flush(TimeSpan.FromSeconds(10));
            }
        }
    }
}
