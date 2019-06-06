require.config({
    paths: {
    }
});
require(["jquery", 'layui'], function ($) {
    layui.use(['form', 'layer', 'table', 'laytpl'], function () {
        var form = layui.form,
            layer = parent.layer === undefined ? layui.layer : top.layer,
            $ = layui.jquery,
            laytpl = layui.laytpl,
            table = layui.table;
        //用户组列表
        var tableIns = table.render({
            elem: '#roleList',
            url: '/System/Role/GetRoleList',
            cellMinWidth: 95,
            page: true,
            height: "full-125",
            limits: [10, 15, 20, 25],
            limit: -1,
            id: "roleListTable",
            done: function (res, curr, count) {
                //如果是异步请求数据方式，res即为你接口返回的信息。
                //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
                //console.log(res);
            },
            cols: [[
                { type: "checkbox", fixed: "left", width: 50 },
                { field: 'RoleName', title: '角色名称', minWidth: 100, align: "center" },
                {
                    field: 'CreateTime', title: '创建时间', align: 'center'
                },
                { field: 'Note', title: '备注', align: 'center', minWidth: 150 },
                { title: '操作', minWidth: 175, templet: '#roleListBar', fixed: "right", align: "center" }
            ]]
        });
        //搜索【此功能需要后台配合，所以暂时没有动态效果演示】
        $(".search_btn").on("click", function () {
            table.reload("roleListTable", {
                page: {
                    curr: 1 //重新从第 1 页开始
                },
                where: {
                    searchstr: $(".searchVal").val()  //搜索的关键字
                }
            });
        });
        //添加角色
        function addUser(edit) {
            console.log(edit);
            var index = layui.layer.open({
                title: "添加/编辑角色",
                type: 2,
                content: "/System/Role/RoleAdd?RoleId=" + (edit != undefined ? edit.RoleId : ""),
                success: function (layero, index) {
                    var body = layui.layer.getChildFrame('body', index);
                    if (edit) {
                        body.find(".roleId").val(edit.RoleId);  //用户Id
                        body.find(".roleName").val(edit.RoleName);  //角色名称
                        body.find(".note").text(edit.Note);    //备注
                        form.render();
                    }
                    setTimeout(function () {
                        layui.layer.tips('点击此处返回角色列表', '.layui-layer-setwin .layui-layer-close', {
                            tips: 3
                        });
                    }, 500);
                }
            });
            layui.layer.full(index);
        }
        $(".addNews_btn").click(function () {
            addUser();
        });
        //批量删除
        $(".delAll_btn").click(function () {
            var checkStatus = table.checkStatus('roleListTable'),
                data = checkStatus.data,
                RoleId = [];
            if (data.length > 0) {
                for (var i in data) {
                    RoleId.push(data[i].RoleId);
                }
                layer.confirm('确定删除选中的角色？', { icon: 3, title: '提示信息' }, function (index) {
                    $.post("/System/role/DelRole", {
                        RoleId: RoleId  //将需要删除的UserId作为参数传入
                    }, function (res) {
                        layer.msg(res.Messages);
                        layer.close(index);
                        if (res.StateCode == 200) {
                            tableIns.reload();
                        }
                    }, "json");
                });
            } else {
                layer.msg("请选择需要删除的角色");
            }
        });
        //列表操作
        table.on('tool(roleList)', function (obj) {
            var layEvent = obj.event,
                data = obj.data;
            if (layEvent === 'edit') { //编辑
                addUser(data);
            } else if (layEvent === 'del') { //删除
                layer.confirm('确定删除此角色？', { icon: 3, title: '提示信息' }, function (index) {
                    $.post("/System/Role/DelRole", {
                        RoleId: data.RoleId  //将需要删除的UserId作为参数传入
                    }, function (res) {
                        layer.msg(res.Messages);
                        layer.close(index);
                        if (res.StateCode == 200) {
                            tableIns.reload();
                        }
                    }, "json");
                });
            }
            else if (layEvent === 'roleassignment') {//分配用户

                var index = layui.layer.open({
                    title: "分配用户(" + data.RoleName + ")",
                    type: 2,
                    content: "/System/Role/RoleAssignmentUser?RoleId=" + (data != undefined ? data.RoleId : ""),
                    success: function (layero, index) {
                        var body = layui.layer.getChildFrame('body', index);
                        setTimeout(function () {
                            layui.layer.tips('点击此处返回角色列表', '.layui-layer-setwin .layui-layer-close', {
                                tips: 3
                            });
                        }, 500);
                    }
                });
                layui.layer.full(index);
            }
            else if (layEvent === 'usergroupassignment') {//分配用户组

                var index1 = layui.layer.open({
                    title: "分配用户组(" + data.RoleName + ")",
                    type: 2,
                    content: "/System/Role/RoleAssignmentUserGroup?RoleId=" + (data != undefined ? data.RoleId : ""),
                    success: function (layero, index) {
                        var body = layui.layer.getChildFrame('body', index);
                        setTimeout(function () {
                            layui.layer.tips('点击此处返回角色列表', '.layui-layer-setwin .layui-layer-close', {
                                tips: 3
                            });
                        }, 500);
                    }
                });
                //var index1 = layui.layer.open({
                //    title: "分配用户组(" + data.RoleName + ")",
                //    type: 2,
                //    content: "/System/Role/SelectUserGroup",
                //    success: function (layero, index) {
                //        var body = layui.layer.getChildFrame('body', index);
                //        setTimeout(function () {
                //            layui.layer.tips('点击此处返回角色列表', '.layui-layer-setwin .layui-layer-close', {
                //                tips: 3
                //            });
                //        }, 500);
                //    }
                //});
                layui.layer.full(index1);
            }
        });
    });
});