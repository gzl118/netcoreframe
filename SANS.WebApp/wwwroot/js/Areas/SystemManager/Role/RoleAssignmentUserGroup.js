layui.use(['form', 'layer', 'table', 'laytpl'], function () {
    var form = layui.form,
        layer = layui.layer,
        $ = layui.jquery,
        laytpl = layui.laytpl,
        table = layui.table;
    $.ajax({
        url: "/System/Menu/GetMenuBtn",
        type: "get",
        async: false,
        dataType: "json",
        data: { oid: $("#menuoid").val() },
        success: function (data) {
            var ndata = data.data;
            var getTpl = getTempleteHTML(ndata);
            document.getElementById('userGroupListBar').innerHTML = getTpl;
        },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
        }
    });
    //用户列表
    var tableIns = table.render({
        elem: '#userGroupList',
        url: '/System/Role/GetRoleUserGroupList',
        where: { RoleId: getQueryString("RoleId") },
        cellMinWidth: 95,
        page: true,
        height: "full-120",
        limits: [10, 15, 20, 25],
        limit: 20,
        id: "userGroupListTable",
        toolbar: "#userGroupListBar",
        done: function (res, curr, count) {
            //如果是异步请求数据方式，res即为你接口返回的信息。
            //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
            //console.log(res);
        },
        cols: [[
            { type: "checkbox", fixed: "left", width: '5%' },
            { field: 'UserGroupName', title: '用户组名称', align: "center", width: '20%' },
            //{ field: 'ParentUserGroupName', title: '父用户组名称', align: 'center', minWidth: 100 },
            {
                field: 'CreateTime', title: '创建时间', align: 'center', Width: '20%'
            },
            { field: 'Note', title: '备注', align: 'center', Width: '20%' }
            //,{ title: '操作', minWidth: 100, templet: '#userGroupListBar', fixed: "right", align: "center" }
        ]]
    });
    //搜索【此功能需要后台配合，所以暂时没有动态效果演示】
    $(".search_btn").on("click", function () {
        refreshData();
    });
    function addModel() {
        var index = layer.open({
            title: "选择用户",
            type: 2,
            area: ["90%", "90%"],
            content: "/System/Role/SelectUserGroup?RoleId=" + getQueryString("RoleId"),
            success: function (layero, index) {
                var body = layui.layer.getChildFrame('body', index);
                setTimeout(function () {
                    layui.layer.tips('点击此处返回列表', '.layui-layer-setwin .layui-layer-close', {
                        tips: 3
                    });
                }, 500);
            }
        });
        layer.full(index);
    }
    function CancelUserAssignmentM() {
        var checkStatus = table.checkStatus('userGroupListTable'); //test即为基础参数id对应的值
        if (checkStatus.data.length === 0) {
            layer.msg("请选择一个或以上用户");
            return;
        }
        var UserGroupIds = [];
        for (var i = 0; i < checkStatus.data.length; i++) {
            UserGroupIds.push(checkStatus.data[i].UserGroupId);
        }
        var roleId = getQueryString("RoleId");
        if (roleId)
            cancelUserGroupAssignment(roleId, UserGroupIds);
        else
            layer.msg("页面参数有误,请刷新页面后重试!");

    }
    function cancelUserGroupAssignment(RoleId, UserGroupIds) {
        layer.confirm('确认取消授权吗?', { icon: 3, title: '温馨提示' }, function (index) {
            layer.close(index);
            $.post("/System/Role/CancelUserGroupAssignment", { RoleId: RoleId, UserGroupIds: UserGroupIds }, function (res) {
                layer.msg(res.Messages);
                if (res.StateCode === 200) {
                    refreshData();
                }
            }, "json");
        });
    }
    function refreshData() {
        table.reload("userGroupListTable", {
            page: {
                curr: 1 //重新从第 1 页开始
            },
            where: {
                searchstr: $(".searchVal").val(),//搜索的关键字
                RoleId: getQueryString("RoleId")
            }
        });
    }
    table.on("toolbar(userGroupList)", function (obj) {
        switch (obj.event) {
            case 'add':
                addModel();
                break;
            case 'cancelUserGroupAssignment':
                CancelUserAssignmentM();
                break;
            case 'refresh':
                refreshData();
                break;
        }
    });
});
//获取url中的参数
function getQueryString(name) {
    var reg = new RegExp("(^|&)" + name + "=([^&]*)(&|$)");
    var r = window.location.search.substr(1).match(reg);
    if (r != null) return r[2]; return '';
}
