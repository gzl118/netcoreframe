var curUserID = null;
var tokenUuid = null;
$(function () {
    var storage = window.sessionStorage;
    if (storage) {
        curUserID = storage.getItem("userID");
        tokenUuid = storage.getItem("tokenUuid");
    }
    $("#iframeHome").attr("src", "/Home/Welcome");
});
//获取url中的参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r !== null) return r[2]; return '';
}
