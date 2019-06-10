require.config({
    paths: {
    }
});
require(["jquery", 'layui'], function ($) {
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
        //用户组列表
        var tableIns = table.render({
            elem: '#userGroupList',
            url: '/System/UserGroup/GetUserGroupList',
            cellMinWidth: 95,
            page: true,
            height: "full-125",
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
                { type: "checkbox", fixed: "left", width: 50 },
                { field: 'UserGroupName', title: '用户组名称', minWidth: 100, align: "center" },
                { field: 'ParentUserGroupName', title: '父用户组名称', align: 'center', minWidth: 150 },
                {
                    field: 'CreateTime', title: '创建时间', align: 'center'
                },
                { field: 'Note', title: '备注', align: 'center', minWidth: 150 }
                //{ title: '操作', minWidth: 175, templet: '#userGroupListBar', fixed: "right", align: "center" }
            ]]
        });
        //搜索【此功能需要后台配合，所以暂时没有动态效果演示】
        $(".search_btn").on("click", function () {
            refreshData();
        });
        //添加用户
        function addModel(edit) {
            var index = layer.open({
                title: "添加/编辑用户组",
                type: 2,
                content: "/System/UserGroup/UserGroupAdd?UserGroupId=" + (edit != undefined ? edit.UserGroupId : ""),
                success: function (layero, index) {
                    var body = layer.getChildFrame('body', index);
                    if (edit) {
                        body.find(".userGroupId").val(edit.UserGroupId);  //用户Id
                        body.find(".userGroupName").val(edit.UserGroupName);  //登录名
                        body.find(".parentUserGroupId").val(edit.ParentUserGroupId);  //父用户组
                        body.find(".note").val(edit.Note);    //备注
                        form.render();
                    }
                    setTimeout(function () {
                        layui.layer.tips('点击此处返回用户组列表', '.layui-layer-setwin .layui-layer-close', {
                            tips: 3
                        });
                    }, 500);
                }
            });
            layer.full(index);
        }
        //批量删除
        function delData() {
            var checkStatus = table.checkStatus('userGroupListTable'),
                data = checkStatus.data,
                UserGroupId = [];
            if (data.length > 0) {
                for (var i in data) {
                    UserGroupId.push(data[i].UserGroupId);
                }
                console.log(UserGroupId);
                layer.confirm('确定删除选中的用户组？', { icon: 3, title: '提示信息' }, function (index) {
                    $.post("/System/UserGroup/DelUserGroup", {
                        UserGroupId: UserGroupId  //将需要删除的UserId作为参数传入
                    }, function (res) {
                        layer.msg(res.Messages);
                        layer.close(index);
                        if (res.StateCode === 200) {
                            tableIns.reload();
                        }
                    }, "json");
                });
            } else {
                layer.msg("请选择需要删除的用户组");
            }
        }
        function editData() {
            var checkStatus = table.checkStatus('userGroupListTable'),
                data = checkStatus.data;
            if (data.length === 0) {
                layer.msg("请选择需要修改的记录");
                return;
            }
            if (data.length > 1) {
                layer.msg("一次只能修改一条记录");
                return;
            }
            addModel(data[0]);
        }
        function refreshData() {
            table.reload("userGroupListTable", {
                page: {
                    curr: 1 //重新从第 1 页开始
                },
                where: {
                    searchstr: $(".searchVal").val()  //搜索的关键字
                }
            });
        }
        table.on("toolbar(userGroupList)", function (obj) {
            switch (obj.event) {
                case 'add':
                    addModel();
                    break;
                case 'del':
                    delData();
                    break;
                case 'edit':
                    editData();
                    break;
                case 'refresh':
                    refreshData();
                    break;
            }
        });
    });
});