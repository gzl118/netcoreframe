layui.use(['form', 'layer', 'table', 'laytpl'], function () {
    var form = layui.form,
        layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    //用户列表
    var tableIns = table.render({
        elem: '#userGroupList',
        url: '/System/Role/GetRoleNotUserGroupList',
        where: { RoleId: getQueryString("RoleId") },
        cellMinWidth: 95,
        page: true,
        height: "full-225",
        limits: [10, 15, 20, 25],
        limit: 20,
        id: "userGroupListTable",
        done: function (res, curr, count) {
            //如果是异步请求数据方式，res即为你接口返回的信息。
            //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
            //console.log(res);
        },
        cols: [[
            { type: "checkbox", fixed: "left", width: 50 },
            { field: 'UserGroupName', title: '用户组名称', minWidth: 100, align: "center" },
            { field: 'ParentUserGroupName', title: '父用户组名称', align: 'center', minWidth: 150 },
            {
                field: 'CreateTime', title: '创建时间', align: 'center'
            },
            { field: 'Note', title: '备注', align: 'center', minWidth: 150 }
        ]]
    });
    //搜索【此功能需要后台配合，所以暂时没有动态效果演示】
    $(".search_btn").on("click", function () {
        table.reload("userGroupListTable", {
            page: {
                curr: 1 //重新从第 1 页开始
            },
            where: {
                searchstr: $(".searchVal").val()  //搜索的关键字
            }
        });
    });
    $("#SaveRoleUserGroup").on('click', function () {
        var checkStatus = table.checkStatus('userGroupListTable'); //test即为基础参数id对应的值
        console.log(checkStatus);
        if (checkStatus.data.length > 0) {
            var arrUserGroupId = [];
            for (var i = 0; i < checkStatus.data.length; i++) {
                arrUserGroupId.push(checkStatus.data[i].UserGroupId);
            }
            var postData = {
                RoleId: getQueryString("RoleId"),
                UserGroupIds: arrUserGroupId
            };
            console.log(postData);
            //刷新父页面
            $.post("/System/Role/AssignmentRoleUserGroup", postData, function (res) {
                layer.msg(res.Messages);
                if (res.StateCode == 200) {
                    parent.location.reload();
                }
            }, "json");
        }
        else {
            layer.msg("请选择一个及以上的用户组");
        }
    });
});
//获取url中的参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return r[2]; return '';
}
