/*
var arr = new Array();
jqmain(function () {
    arr["add"] = '<a class="layui-btn" lay-event="add"><i style="font-style:normal;font-size:14px !important;position:relative;top:-2px;">添加</i><i class="layui-icon layui-icon-add-1"></i></a>';
    arr["edit"] = '<a class="layui-btn layui-btn-normal" lay-event="edit"><i style="font-style:normal;font-size:14px !important;position:relative;top:-2px;">修改</i><i class="layui-icon layui-icon-edit"></i></a>';
    arr["del"] = '<a class="layui-btn layui-btn-danger delAll_btn" lay-event="del"><i style="font-style:normal;font-size:14px !important;position:relative;top:-2px;">删除</i><i class="layui-icon layui-icon-delete"></i></a>';
    arr["refresh"] = '<a class="layui-btn layui-btn-primary" data-type="reload" lay-event="refresh"><i style="font-style:normal;font-size:14px !important;">刷新</i><i class="layui-icon layui-icon-refresh-1" style="font-size:13px !important;"></i></a>';
    arr["usable"] = '<a class="layui-btn" lay-event="usable"><i style="font-style:normal;font-size:14px !important;position:relative;top:-2px;">启用</i><i class="layui-icon layui-icon-play"></i></a>';
    arr["disable"] = '<a class="layui-btn layui-btn-warm" lay-event="disable"><i style="font-style:normal;font-size:14px !important;position:relative;top:-2px;">禁用</i><i class="layui-icon layui-icon-pause"></i></a>';
    arr["roleassignment"] = ' <a class="layui-btn layui-btn-warm roleassignment" lay-event="roleassignment">分配用户</a>';
    arr["usergroupassignment"] = '<a class="layui-btn layui-btn-warm roleassignment" lay-event="usergroupassignment">分配用户组</a>';
});
*/
var temp = '<a class="layui-btn {3}" lay-event="{0}" data-oid="{4}"><i class="layui-icon {2}"></i><i style="font-style:normal;font-size:14px !important;position:relative;top:-2px;">{1}</i></a>';
function getTempleteHTML(ndata) {
    var str = "";
    jqmain.each(ndata, function (i, item) {
        str += temp.format(item.MenuUrl, item.MenuName, item.MenuIcon, item.Note, item.MenuId);
    });
    return str;
}

/**
 * 替换所有匹配exp的字符串为指定字符串
 * @param exp 被替换部分的正则
 * @param newStr 替换成的字符串
 */
String.prototype.replaceAll = function (exp, newStr) {
    return this.replace(new RegExp(exp, "gm"), newStr);
};

/**
 * 原型：字符串格式化
 * @param args 格式化参数值
 */
String.prototype.format = function (args) {
    var result = this;
    if (arguments.length < 1) {
        return result;
    }

    var data = arguments; // 如果模板参数是数组
    if (arguments.length === 1 && typeof (args) === "object") {
        // 如果模板参数是对象
        data = args;
    }
    for (var key in data) {
        var value = data[key];
        if (undefined !== value) {
            result = result.replaceAll("\\{" + key + "\\}", value);
        }
    }
    return result;
}