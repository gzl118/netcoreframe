
const connection = new signalR.HubConnectionBuilder()
    .withUrl("/messagehub")
    .configureLogging(signalR.LogLevel.Information)
    .build();

//connectionState 0正在连接，1连接成功，2断开连接
$(function () {
    StartHub();
    ListingHub();
});
function CloseHub() {
    if (connection.connection.connectionState !== 2)
        connection.stop();
    else
        console.log("不是连接状态，无法断开！");
}
function StartHub() {
    if (connection.connection.connectionState === 2) {
        connection.start().then(() => {
            console.log("SignalR已开启！");
        }).catch(err => console.error("SignalR开启异常:" + err.toString()));
    }
    else
        console.log("不是断开状态，请不要重复连接！");
}
async function ReconnectHub() {
    if (connection.connection.connectionState === 2)
        await rstart();
}
function ListingHub() {
    connection.on("RecivMessage", (Msg) => {
        console.log(JSON.stringify(Msg));
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
    });
    //connection.on("ReceiveMessage", (usr, Msg) => {
    //    $("#txtreceivemsg").val(Msg);
    //});
    //connection.on("redisSubcrite", (Msg) => {
    //    $("#txtreceivemsg").val(Msg);
    //});
    connection.onclose(() => {
        console.log("SignalR已关闭！");
    });
}
async function rstart() {
    try {
        await connection.start();
        console.log('connected');
    } catch (err) {
        console.log(err);
        setTimeout(() => start(), 5000);
    }
};

function ClientSendMsg() {
    if (connection.connection.connectionState === 2) {
        alert("未连接服务端，无法发送消息！");
        return;
    }
    connection.invoke("SendMessage", "gzl1", "test clinet send msg for server!").catch(err => console.error(err.toString()));
}