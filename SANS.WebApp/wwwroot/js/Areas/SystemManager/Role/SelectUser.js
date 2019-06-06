layui.use(['form', 'layer', 'table', 'laytpl'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    //用户列表
    var tableIns = table.render({
        elem: '#userList',
        url: '/System/Role/GetRoleNotUserList',
        where: { RoleId: getQueryString("RoleId") },
        cellMinWidth: 95,
        page: true,
        height: "full-225",
        limits: [10, 15, 20, 25],
        limit: 10,
        id: "userListTable",
        done: function (res, curr, count) {
            //如果是异步请求数据方式，res即为你接口返回的信息。
            //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
            //console.log(res);
        },
        cols: [[
            { type: "checkbox", fixed: "left", width: 50 },
            { field: 'UserName', title: '用户名', minWidth: 100, align: "center" },
            { field: 'UserNikeName', title: '用户昵称', align: 'center', minWidth: 150 },
            {
                field: 'UserEmail', title: '用户邮箱', minWidth: 200, align: 'center', templet: function (d) {
                    //return '<a class="layui-blue" href="mailto:' + d.UserEmail + '">' + d.UserEmail + '</a>';
                    if (d.UserEmail === null)
                        return "";
                    return d.UserEmail;
                }
            },
            {
                field: 'UserSex', title: '用户性别', align: 'center', width: 100, templet: function (d) {
                    return d.UserSex == "1" ? "男" : "女";
                }
            },
            {
                field: 'UserStatus', title: '用户状态', align: 'center', minWidth: 50, templet: function (d) {
                    return d.UserStatus == "1" ? "正常" : "停用";
                }
            },
            {
                field: 'CreateTime', title: '创建时间', align: 'center', minWidth: 150
            }
        ]]
    });
    //搜索【此功能需要后台配合，所以暂时没有动态效果演示】
    $(".search_btn").on("click", function () {
        table.reload("userListTable", {
            page: {
                curr: 1 //重新从第 1 页开始
            },
            where: {
                searchstr: $(".searchVal").val()  //搜索的关键字
            }
        });
    });
    $("#SaveRoleUser").on('click', function () {
        var checkStatus = table.checkStatus('userListTable'); //test即为基础参数id对应的值
        if (checkStatus.data.length > 0) {
            var arrUserId = [];
            for (var i = 0; i < checkStatus.data.length; i++) {
                arrUserId.push(checkStatus.data[i].UserId);
            }
            var postData = {
                RoleId: getQueryString("RoleId"),
                UserIds: arrUserId
            };
            console.log(postData);
            //刷新父页面
            $.post("/System/Role/AssignmentRoleUser", postData, function (res) {
                layer.msg(res.Messages);
                if (res.StateCode == 200) {
                    parent.location.reload();
                }
            }, "json");
        }
        else {
            layer.msg("请选择一个及以上的用户");
        }
    });
});
//获取url中的参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return r[2]; return '';
}
