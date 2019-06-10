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
                document.getElementById('roleListBar').innerHTML = getTpl;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
        //用户组列表
        var tableIns = table.render({
            elem: '#roleList',
            url: '/System/Role/GetRoleList',
            cellMinWidth: 95,
            page: true,
            height: "full-105",
            limits: [10, 15, 20, 25],
            limit: -1,
            id: "roleListTable",
            toolbar: "#roleListBar",
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
                { field: 'Note', title: '备注', align: 'center', minWidth: 150 }
                //{ title: '操作', minWidth: 175, templet: '#roleListBar', fixed: "right", align: "center" }
            ]]
        });
        //搜索【此功能需要后台配合，所以暂时没有动态效果演示】
        $(".search_btn").on("click", function () {
            refreshData();
        });
        //添加角色
        function addModel(edit) {
            console.log(edit);
            var index = layer.open({
                title: "添加/编辑角色",
                type: 2,
                content: "/System/Role/RoleAdd?RoleId=" + (edit !== undefined ? edit.RoleId : ""),
                success: function (layero, index) {
                    var body = layer.getChildFrame('body', index);
                    if (edit) {
                        body.find(".roleId").val(edit.RoleId);  //用户Id
                        body.find(".roleName").val(edit.RoleName);  //角色名称
                        body.find(".note").text(edit.Note);    //备注
                        form.render();
                    }
                }
            });
            layer.full(index);
        }
        function editData() {
            var checkStatus = table.checkStatus('roleListTable'),
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
        //批量删除
        function delData() {
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
                        if (res.StateCode === 200) {
                            tableIns.reload();
                        }
                    }, "json");
                });
            } else {
                layer.msg("请选择需要删除的角色");
            }
        }
        function refreshData() {
            table.reload("roleListTable", {
                page: {
                    curr: 1 //重新从第 1 页开始
                },
                where: {
                    searchstr: $(".searchVal").val()  //搜索的关键字
                }
            });
        }
        function roleAssignment(oid) {
            var checkStatus = table.checkStatus('roleListTable'),
                data = checkStatus.data;
            if (data.length === 0) {
                layer.msg("请选择需要操作的记录");
                return;
            }
            if (data.length > 1) {
                layer.msg("一次只能操作一条记录");
                return;
            }
            var index = layer.open({
                title: "分配用户(" + data[0].RoleName + ")",
                type: 2,
                content: "/System/Role/RoleAssignmentUser?RoleId=" + (data[0] !== undefined ? data[0].RoleId : "") + "&oid=" + oid,
                success: function (layero, index) {
                }
            });
            layer.full(index);
        }
        function usergroupAssignment(oid) {
            var checkStatus = table.checkStatus('roleListTable'),
                data = checkStatus.data;
            if (data.length === 0) {
                layer.msg("请选择需要操作的记录");
                return;
            }
            if (data.length > 1) {
                layer.msg("一次只能操作一条记录");
                return;
            }
            var index = layer.open({
                title: "分配用户组(" + data[0].RoleName + ")",
                type: 2,
                content: "/System/Role/RoleAssignmentUserGroup?RoleId=" + (data[0] !== undefined ? data[0].RoleId : "") + "&oid=" + oid,
                success: function (layero, index) {
                }
            });
            layer.full(index);
        }
        table.on("toolbar(roleList)", function (obj) {
            var oid = $(this).data("oid");
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
                case 'roleassignment':
                    roleAssignment(oid);
                    break;
                case 'usergroupassignment':
                    usergroupAssignment(oid);
                    break;
                case 'refresh':
                    refreshData();
                    break;
            }
        });
    });
});