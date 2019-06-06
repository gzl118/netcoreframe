var socket;
if (typeof (WebSocket) == "undefined") {
    alert("您的浏览器不支持WebSocket");
}
$(function () {
    setTimeout(function () {
        StartSocket();
        window.addEventListener("beforeunload", function (event) {
            CloseHub();
            console.log("手动关闭socket!");
        });
    }, 500);
});
function StartSocket() {
    var host = location.host;
    console.log(host);
    var uid = parent.curUserID;
    //host = "192.168.31.224:43970";
    //host = "wxremote:6000";
    var token = parent.tokenUuid;
    var url = "ws://" + host + "/ws?u=" + uid + "&token=" + token;
    console.log(url);
    socket = new WebSocket(url);
    //打开事件
    socket.onopen = function (e) {
        console.log("连接成功");
        heartCheck.reset().start();
    };
    //获得消息事件
    socket.onmessage = function (evt) {
        heartCheck.reset().start();
        var result = evt.data;
        if (result === "ping") {
            console.log("心跳监测:" + new Date());
            return;
        }
        //console.log(result);
        var Msg = JSON.parse(result);
        var ntype = Msg.msgType;
        switch (ntype) {
            case "approve"://审批
                TipApprove(Msg);
                break;
            case "seluser": //选择审批用户
                TipSelUser(Msg);
                break;
            case "approveresult": //选择审批用户
                TipApproveResult(Msg);
                break;
            case "alarm":
                TipAlarm(Msg);
                break;
        }
    };
    //关闭事件
    socket.onclose = function (event) {
        console.log("Socket已关闭" + JSON.stringify(event));
        reconnect();
    };
    //发生了错误事件
    socket.onerror = function (event) {
        console.log("发生了错误" + event.error);
        reconnect();
    };
}
function CloseHub() {
    socket.close();
}
function ClientSendMsg() {
    socket.send("这是来自客户端的消息" + location.href + new Date());
}
var ntime = 5 * 60 * 1000; //5分钟发一次心跳
//心跳检测
var heartCheck = {
    timeout: ntime, //心跳时间
    timeoutObj: null,
    serverTimeoutObj: null,
    reset: function () {
        clearTimeout(this.timeoutObj);
        clearTimeout(this.serverTimeoutObj);
        return this;
    },
    start: function () {
        var self = this;
        this.timeoutObj = setTimeout(function () {
            //这里发送一个心跳，后端收到后，返回一个心跳消息，
            //onmessage拿到返回的心跳就说明连接正常
            socket.send("ping");
            //console.log("ping!");
            self.serverTimeoutObj = setTimeout(function () {
                //如果超过一定时间还没重置，说明后端主动断开了
                //如果onclose会执行reconnect，我们执行ws.close()就行了.如果直接执行reconnect 会触发onclose导致重连两次
                CloseHub();
            }, self.timeout);
        }, this.timeout);
    }
};
var lockReconnect = false;  //避免ws重复连接
var retime = 2 * 1000; //断开后2分钟重连一次
function reconnect() {
    if (lockReconnect) return;
    lockReconnect = true;
    setTimeout(function () {     //没连接上会一直重连，设置延迟避免请求过多
        StartSocket();
        lockReconnect = false;
    }, retime);
}