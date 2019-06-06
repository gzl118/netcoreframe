using AutoMapper.Configuration;
using C513.Helper;
using Google.Protobuf.C513.RawDataProto;
using Google.Protobuf.C513.TmParameterProto;
using KN.Common;
using KN.Log;
using KN.Model;
using KN.RedisOperation;
using KN.WebApp.Comm;
using KN.WebApp.Models;
using Microsoft.AspNetCore.SignalR;
using NetMQ;
using NetMQ.Sockets;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace KN.WebApp.SignalR
{
    public class NetMQMsgReceive : IMsgReceive
    {
        public static CancellationTokenSource cts = new CancellationTokenSource();
        // 注入SignalR的消息处理器上下文，用以发送消息到客户端
        IHubContext<MessageHub> hubContext;
        string _DataPublisherServePubString = "";
        private MsgIntoDB msgIntoDB;
        private IMsgSend msgSend;
        public NetMQMsgReceive(
            IHubContext<MessageHub> hubContext,
            MsgConfig msgConfig,
            MsgIntoDB _msgIntoDB,
            IMsgSend MsgSend)
        {
            this.hubContext = hubContext;
            _DataPublisherServePubString = msgConfig.MQUrlPublisherServePubString;
            //_DataPublisherServePubString = "tcp://192.168.31.28:6556";
            this.msgIntoDB = _msgIntoDB;
            this.msgSend = MsgSend;
        }
        public void receiveMsg()
        {
            Task.Factory.StartNew(
            () =>
            {
                try
                {
                    using(var subscriber =new  SubscriberSocket())
                    {
                        subscriber.Connect(_DataPublisherServePubString);
                        subscriber.SubscribeToAnyTopic();
                        //subscriber.Subscribe("StartTrain");
                        //subscriber.Subscribe("StopTrain");
                        //while (true)
                        while (!cts.Token.IsCancellationRequested)
                        {
                            try
                            {
                                cts.Token.ThrowIfCancellationRequested();
                                var responseMessage = subscriber.ReceiveMultipartMessage();
                                if (responseMessage != null)
                                {
                                    var frame = responseMessage.Pop();
                                    var xx = frame?.ConvertToString();
                                    switch (xx)
                                    {
                                        //历史遥测数据
                                        case "HistoryTm":
                                        //实时遥测数据
                                        case "RealtimeTm":
                                        case "Group1":
                                            frame = responseMessage.Pop();
                                            var byteArray = frame?.ToByteArray();
                                            Model.TmParameterValues t1 = ProtobufHelper.BytesToObject<Model.TmParameterValues>(byteArray, 0, byteArray.Length);
                                            foreach (var item in t1.ValueList)
                                            {
                                                //RedisHelper.Default.SetStringKey<Model.TmParameterValue>("C513:" + item.Symbol + ":" + DateTools.DoubleToTime(item.Time).ToString("yyyyMMddHHmmssfff"), item);
                                                //SortedSet
                                                RedisHelper.Default.SortedSetObject<Model.TmParameterValue>("C513:" + item.Symbol + ":" + DateTools.DoubleToTime(item.Time).ToString("yyyyMMdd"), item, item.Time);
                                                //Console.WriteLine($"Topic:{xx} 内容:Symbol:{item.Symbol},Time:{item.Time},Value:{item.Value},Code:{item.Code},Text:{item.Text}");
                                            }
                                            hubContext.Clients.All.SendAsync("RecivMessage", new
                                            {
                                                MsgType = "business",
                                                Symbol = "TM",
                                                dataList = t1.ValueList
                                            });
                                            break;
                                        case "TrainProcess"://训练学习进度
                                            frame = responseMessage.Pop();
                                            var byteArray2 = frame?.ToByteArray();
                                            TrainProcessModel t2 = ProtobufHelper.BytesToObject<TrainProcessModel>(byteArray2, 0, byteArray2.Length);
                                            //Console.WriteLine($"Topic:{xx} 进度:{t2.Process.ToString()}");
                                            hubContext.Clients.All.SendAsync("RecivMessage", new
                                            {
                                                MsgType = "modeltrain",
                                                process = t2.Process
                                            });
                                            break;
                                        case "TMCEvent"://测控事件检测结果
                                            frame = responseMessage.Pop();
                                            var byteArray3 = frame?.ToByteArray();
                                            TMCEventModel t3 = ProtobufHelper.BytesToObject<TMCEventModel>(byteArray3, 0, byteArray3.Length);
                                            try
                                            {
                                                msgIntoDB.AddTMCEvent(t3);
                                            }
                                            catch (Exception er)
                                            {
                                                logSend(er.Message);
                                            }
                                            // Console.WriteLine($"Topic:{xx} Name:{t3.Name},BeginTime:{t3.BeginTime},EndTime:{t3.EndTime}");
                                            break;
                                        case "TmParameterAbnormity"://异常检测结果
                                            frame = responseMessage.Pop();
                                            var byteArray4 = frame?.ToByteArray();
                                            TmParameterAbnormity t4 = ProtobufHelper.BytesToObject<TmParameterAbnormity>(byteArray4, 0, byteArray4.Length);
                                            //Console.WriteLine($"Topic:{xx} Symbol:{t4.Symbol},TMCEvent:{t4.TMCEvent},BeginTime:{t4.BeginTime},EndTime:{t4.EndTime}");
                                            try
                                            {
                                                msgIntoDB.AddBusinessAlarm(t4);
                                            }
                                            catch (Exception er)
                                            {
                                                logSend(er.Message);
                                            }
                                            break;
                                        case "TmParameterCycle"://周期检测结果
                                            frame = responseMessage.Pop();
                                            var byteArray5 = frame?.ToByteArray();
                                            TmParameterCycle t5 = ProtobufHelper.BytesToObject<TmParameterCycle>(byteArray5, 0, byteArray5.Length);
                                            //Console.WriteLine($"Topic:{xx} Symbol:{t5.Symbol},Cycle:{t5.Cycle}");
                                            try
                                            {
                                                msgIntoDB.AddCycleResult(t5);
                                            }
                                            catch (Exception er)
                                            {
                                                logSend(er.Message);
                                            }
                                            break;
                                        case "ServiceState"://软件状态
                                            frame = responseMessage.Pop();
                                            var byteArray6 = frame?.ToByteArray();
                                            ServiceStateModel t6 = ProtobufHelper.BytesToObject<ServiceStateModel>(byteArray6, 0, byteArray6.Length);
                                            Console.WriteLine($"Topic:{xx} Name: {t6.Name},State: {t6.State}");
                                            break;
                                        case "Log"://软件日志
                                            frame = responseMessage.Pop();
                                            var byteArray7 = frame?.ToByteArray();
                                            LogModel t7 = ProtobufHelper.BytesToObject<LogModel>(byteArray7, 0, byteArray7.Length);
                                            //Console.WriteLine($"Topic:{xx} Source:{t7.Source},SatelliteTime:{t7.SatelliteTime},SystemTime:{t7.SystemTime},Message:{t7.Message}");
                                            try
                                            {
                                                msgIntoDB.AddLog(t7);
                                            }
                                            catch (Exception er)
                                            {
                                                logSend(er.Message);
                                            }
                                            break;
                                        case "StartTrain":
                                        case "StopTrain":
                                            var tt = xx;
                                            break;
                                        default:
                                            if (xx.IndexOf("BUFFER") >= 0)
                                            {
                                                frame = responseMessage.Pop();
                                                var rawData = RawData.Parser.ParseFrom(frame?.ToByteArray());
                                                if (rawData != null)
                                                {
                                                    var mv = new { MsgType = "business", rawData = rawData, Symbol = rawData.Symbol, Data = ByteConverter.BytesFormatToHexString(rawData.Data.ToByteArray()).Split(' ') };
                                                    //RedisHelper.Default.SetStringKey<string>("C513:" + rawData.Symbol + ":" + DateTools.DoubleToTime(rawData.Time).ToString("yyyyMMddHHmmssfff"), JsonConvert.SerializeObject(rawData));
                                                    //SortedSet
                                                    RedisHelper.Default.SortedSetObject<string>("C513:" + rawData.Symbol + ":" + DateTools.DoubleToTime(rawData.Time).ToString("yyyyMMdd"), JsonConvert.SerializeObject(rawData), rawData.Time);
                                                    System.Threading.Thread.Sleep(1000);
                                                    // 通过消息处理器上下文发送消息到客户端
                                                    hubContext.Clients.All.SendAsync("RecivMessage", mv);
                                                }
                                            }
                                            break;

                                    }

                                }

                            }
                            catch (Exception er)
                            {
                                Log4netHelper.Error(this, er);
                                logSend(er.Message);
                            }
                        }
                    }
                }
                catch (Exception er)
                {
                    Log4netHelper.Error(this, er);
                    logSend(er.Message);
                }
            }, cts.Token);
        }
        void logSend(string mlog)
        {
            LogModel lg = new LogModel();
            lg.SystemTime = DateTools.TimeToDouble(DateTime.Now);
            lg.SatelliteTime = 0;
            lg.Source = "webSystem";
            lg.Message = mlog;
            msgSend.send<LogModel>("Log", lg);
            hubContext.Clients.All.SendAsync("RecivMessage", new { MsgType = "sys", MsgInfo = mlog, dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") });
        }
        public void stopMsg()
        {
            cts.Cancel();
        }
    }
}
