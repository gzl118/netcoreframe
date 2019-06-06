var arr = new Array();
jqmain(function () {
    arr["add"] = '<a class="layui-btn" lay-event="add"><i style="font-style:normal;font-size:14px !important;position:relative;top:-2px;">添加</i><i class="layui-icon layui-icon-add-1"></i></a>';
    arr["edit"] = '<a class="layui-btn layui-btn-normal" lay-event="edit"><i style="font-style:normal;font-size:14px !important;position:relative;top:-2px;">修改</i><i class="layui-icon layui-icon-edit"></i></a>';
    arr["del"] = '<a class="layui-btn layui-btn-danger layui-btn-normal delAll_btn" lay-event="del"><i style="font-style:normal;font-size:14px !important;position:relative;top:-2px;">删除</i><i class="layui-icon layui-icon-delete"></i></a>';
    arr["refresh"] = '<a class="layui-btn layui-btn-primary" data-type="reload" lay-event="refresh"><i style="font-style:normal;font-size:14px !important;">刷新</i><i class="layui-icon layui-icon-refresh-1" style="font-size:13px !important;"></i></a>';
    arr["usable"] = '<a class="layui-btn" lay-event="usable"><i style="font-style:normal;font-size:14px !important;position:relative;top:-2px;">启用</i><i class="layui-icon layui-icon-play"></i></a>';
    arr["disable"] = '<a class="layui-btn layui-btn-warm" lay-event="disable"><i style="font-style:normal;font-size:14px !important;position:relative;top:-2px;">禁用</i><i class="layui-icon layui-icon-pause"></i></a>';
    arr["roleassignment"] = ' <a class="layui-btn layui-btn-warm roleassignment" lay-event="roleassignment">分配用户</a>';
    arr["usergroupassignment"] = '<a class="layui-btn layui-btn-warm roleassignment" lay-event="usergroupassignment">分配用户组</a>';
});
function getTempleteHTML(ndata) {
    var str = "";
    jqmain.each(ndata, function (i, item) {
        str += arr[item];
    });
    return str;
}