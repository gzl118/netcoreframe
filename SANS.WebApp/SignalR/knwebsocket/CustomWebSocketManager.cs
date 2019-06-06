using SANS.Log;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SANS.WebApp.SignalR
{
    public class CustomWebSocketManager
    {
        private readonly RequestDelegate _next;

        public CustomWebSocketManager(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ICustomWebSocketFactory wsFactory, ICustomWebSocketMessageHandler wsmHandler)
        {
            if (context.Request.Path == "/ws")
            {
                if (context.Request.Headers.ContainsKey("Sec-WebSocket-Protocol"))
                {
                    context.Response.Headers.Add("Sec-WebSocket-Protocol", "protocol1");
                }
                if (context.WebSockets.IsWebSocketRequest)
                {
                    string username = context.Request.Query["u"];
                    string token = context.Request.Query["token"];
                    //Log4netHelper.Info(this, "websokct进入,用户为：" + username);
                    if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(token) && token.Length == 36)
                    {
                        WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                        CustomWebSocket userWebSocket = new CustomWebSocket()
                        {
                            WebSocket = webSocket,
                            Username = username,
                            token = token
                        };
                        wsFactory.Add(userWebSocket);
                        wsFactory.RemoveInvalid();
                        await wsmHandler.SendInitialMessages(userWebSocket);
                        await Listen(context, userWebSocket, wsFactory, wsmHandler);
                    }
                }
                else
                {
                    context.Response.StatusCode = 400;
                    //Log4netHelper.Info(this, "不是websocket请求！");
                }
            }
            else
            {
                await _next(context);
            }
        }

        private async Task Listen(HttpContext context, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory, ICustomWebSocketMessageHandler wsmHandler)
        {
            WebSocket webSocket = userWebSocket.WebSocket;
            var buffer = new byte[1024 * 4];
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            while (!result.CloseStatus.HasValue)
            {
                await wsmHandler.HandleMessage(result, buffer, userWebSocket, wsFactory);
                buffer = new byte[1024 * 4];
                result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            }
            wsFactory.Remove(userWebSocket);
            //Log4netHelper.Info(this, "websokct关闭,移除用户为：" + userWebSocket.Username);
            await webSocket.CloseAsync(result.CloseStatus.Value, result.CloseStatusDescription, CancellationToken.None);
        }
    }
    public static class WebSocketExtensions
    {
        public static IApplicationBuilder UseCustomWebSocketManager(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomWebSocketManager>();
        }
    }
    public class CustomWebSocket
    {
        public WebSocket WebSocket { get; set; }
        public string Username { get; set; }
        public string token { get; set; }
    }
    class CustomWebSocketMessage
    {
        public string Text { get; set; }
        public DateTime MessagDateTime { get; set; }
        public string Username { get; set; }
        public string Type { get; set; }
    }
    public interface ICustomWebSocketFactory
    {
        /// <summary>
        /// 添加客户端缓冲
        /// </summary>
        /// <param name="uws"></param>
        void Add(CustomWebSocket uws);
        /// <summary>
        /// 移动指定的客户端
        /// </summary>
        /// <param name="curclient"></param>
        void Remove(CustomWebSocket curclient);
        /// <summary>
        /// 移除无效的Socket连接
        /// </summary>
        void RemoveInvalid();
        /// <summary>
        /// 获取所有的客户端
        /// </summary>
        /// <returns></returns>
        List<CustomWebSocket> All();
        /// <summary>
        /// 获取除了指定客户端外的客户端
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        List<CustomWebSocket> Others(CustomWebSocket client);
        /// <summary>
        /// 获取指定的客户端
        /// </summary>
        /// <param name="curclient"></param>
        /// <returns></returns>
        CustomWebSocket Client(CustomWebSocket curclient);
        /// <summary>
        /// 获取用户名相同的所有客户端
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        List<CustomWebSocket> Clients(string username);
    }

    public class CustomWebSocketFactory : ICustomWebSocketFactory
    {
        List<CustomWebSocket> List;

        public CustomWebSocketFactory()
        {
            List = new List<CustomWebSocket>();
        }

        public void Add(CustomWebSocket uws)
        {
            List.Add(uws);
        }

        public List<CustomWebSocket> All()
        {
            return List;
        }

        public List<CustomWebSocket> Others(CustomWebSocket client)
        {
            return List.Where(c => c.Username != client.Username && c.token != client.token).ToList();
        }

        public CustomWebSocket Client(CustomWebSocket client)
        {
            return List.First(p => p.Username.Equals(client.Username) && p.token.Equals(client.token));
        }
        public List<CustomWebSocket> Clients(string username)
        {
            Log4netHelper.Info(this, username + ":" + List.Count);
            return List.Where(c => c.Username.Equals(username)).ToList();
        }

        public void Remove(CustomWebSocket curclient)
        {
            var client = List.First(p => p.Username.Equals(curclient.Username) && p.token.Equals(curclient.token));
            List.Remove(client);
        }
        public void RemoveInvalid()
        {
            List.RemoveAll(p =>
            {
                if (p.WebSocket.State == WebSocketState.Closed || p.WebSocket.State == WebSocketState.Aborted)
                {
                    p.WebSocket.CloseAsync(WebSocketCloseStatus.EndpointUnavailable, "关闭已经异常的连接", CancellationToken.None);
                    return true;
                }
                return false;
            });
        }
    }
    public interface ICustomWebSocketMessageHandler
    {
        Task SendInitialMessages(CustomWebSocket userWebSocket);
        Task HandleMessage(WebSocketReceiveResult result, byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory);
        Task BroadcastOthers(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory);
        Task BroadcastAll(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory);
        Task SendMessage(string msg, string username, ICustomWebSocketFactory wsFactory);
    }

    public class CustomWebSocketMessageHandler : ICustomWebSocketMessageHandler
    {
        /// <summary>
        /// 发送链接成功消息给客户端
        /// </summary>
        /// <param name="userWebSocket"></param>
        /// <returns></returns>
        public async Task SendInitialMessages(CustomWebSocket userWebSocket)
        {
            WebSocket webSocket = userWebSocket.WebSocket;
            var msg = new CustomWebSocketMessage
            {
                MessagDateTime = DateTime.Now,
                Type = userWebSocket.token,
                Text = "socket连接成功!",
                Username = userWebSocket.Username,

            };

            string serialisedMessage = JsonConvert.SerializeObject(msg);
            byte[] bytes = Encoding.UTF8.GetBytes(serialisedMessage);
            await webSocket.SendAsync(new ArraySegment<byte>(bytes, 0, bytes.Length), WebSocketMessageType.Text, true, CancellationToken.None);
        }
        /// <summary>
        /// 处理收到的客户端消息
        /// </summary>
        /// <param name="result"></param>
        /// <param name="buffer"></param>
        /// <param name="userWebSocket"></param>
        /// <param name="wsFactory"></param>
        /// <returns></returns>
        public async Task HandleMessage(WebSocketReceiveResult result, byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            string msg = Encoding.UTF8.GetString(buffer, 0, result.Count);
            try
            {
                if (msg.Equals("ping")) //心跳监测
                {
                    //Log4netHelper.Error(this, "HandleMessage:心跳监测：" + msg);
                    byte[] buffer1 = Encoding.UTF8.GetBytes(msg);
                    await userWebSocket.WebSocket.SendAsync(new ArraySegment<byte>(buffer1, 0, buffer1.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                }
                else
                {
                    //Log4netHelper.Error(this, "HandleMessage:" + msg);
                    //var message = JsonConvert.DeserializeObject<CustomWebSocketMessage>(msg);
                    //if (message.Type == "test")
                    //{
                    //    await BroadcastOthers(buffer, userWebSocket, wsFactory);
                    //}
                }
            }
            catch (Exception ex)
            {
                Log4netHelper.Error(this, ex);
                await userWebSocket.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), result.MessageType, result.EndOfMessage, CancellationToken.None);
            }
        }
        /// <summary>
        /// 发送消息给除了指定客户端外的其他的客户端
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="userWebSocket"></param>
        /// <param name="wsFactory"></param>
        /// <returns></returns>
        public async Task BroadcastOthers(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            var others = wsFactory.Others(userWebSocket);
            foreach (var uws in others)
            {
                if (uws.WebSocket.State != WebSocketState.Open)
                {
                    Log4netHelper.Info(this, "此websocket的状态为：" + uws.Username + "<>" + uws.WebSocket.State.ToString());
                    continue;
                }
                await uws.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        /// <summary>
        /// 广播消息给所有的客户端
        /// </summary>
        /// <param name="buffer"></param>
        /// <param name="userWebSocket"></param>
        /// <param name="wsFactory"></param>
        /// <returns></returns>
        public async Task BroadcastAll(byte[] buffer, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            var all = wsFactory.All();
            foreach (var uws in all)
            {
                if (uws.WebSocket.State != WebSocketState.Open)
                {
                    Log4netHelper.Info(this, "此websocket的状态为：" + uws.Username + "<>" + uws.WebSocket.State.ToString());
                    continue;
                }
                await uws.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
        /// <summary>
        /// 发送消息给指定的客户端（用户名相同的所有客户端）
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="username"></param>
        /// <param name="wsFactory"></param>
        /// <returns></returns>
        public async Task SendMessage(string msg, string username, ICustomWebSocketFactory wsFactory)
        {
            var clients = wsFactory.Clients(username);
            if (clients != null)
            {
                foreach (var client in clients)
                {
                    if (client.WebSocket.State != WebSocketState.Open)
                    {
                        Log4netHelper.Info(this, "此websocket的状态为：" + client.Username + "<>" + client.WebSocket.State.ToString());
                        continue;
                    }
                    byte[] buffer = Encoding.UTF8.GetBytes(msg);
                    await client.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
                    //Log4netHelper.Info(this, client.Username + "-->推送消息成功：" + msg);
                }
            }
            else
            {
                Log4netHelper.Info(this, username + "-->客户端不存在！");
            }
        }
        /// <summary>
        /// 发送消息给指定的客户端
        /// </summary>
        /// <param name="msg"></param>
        /// <param name="userWebSocket"></param>
        /// <param name="wsFactory"></param>
        /// <returns></returns>
        public async Task SendMessageToSingle(string msg, CustomWebSocket userWebSocket, ICustomWebSocketFactory wsFactory)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(msg);
            var client = wsFactory.Client(userWebSocket);
            if (client != null)
            {
                if (client.WebSocket.State != WebSocketState.Open)
                {
                    Log4netHelper.Info(this, "此websocket的状态为：" + client.Username + "<>" + client.WebSocket.State.ToString());
                    return;
                }
                await client.WebSocket.SendAsync(new ArraySegment<byte>(buffer, 0, buffer.Length), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
