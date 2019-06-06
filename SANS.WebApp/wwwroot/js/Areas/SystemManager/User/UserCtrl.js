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
                document.getElementById('userListBar').innerHTML = getTpl;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
        //用户列表
        var tableIns = table.render({
            elem: '#userList',
            url: '/System/User/GetUserList',
            cellMinWidth: 95,
            page: true,
            height: "full-125",
            limits: [10, 15, 20, 25],
            limit: 20,
            id: "userListTable",
            toolbar: "#userListBar",
            done: function (res, curr, count) {
                //如果是异步请求数据方式，res即为你接口返回的信息。
                //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
                //console.log(res);
            },
            cols: [[
                { type: "checkbox", fixed: "left", width: 50 },
                { field: 'UserName', title: '用户名', minWidth: 100, align: "center" },
                { field: 'UserNikeName', title: '昵称', align: 'center', minWidth: 150 },
                {
                    field: 'UserEmail', title: '邮箱', minWidth: 200, align: 'center', templet: function (d) {
                        if (d.UserEmail === null)
                            return "";
                        return d.UserEmail;
                    }
                },
                {
                    field: 'UserSex', title: '性别', align: 'center', width: 100, templet: function (d) {
                        return d.UserSex === 1 ? '<span style="color: #009688;">男</span>' : '<span style="color: #FF5722;">女</span>';
                    }
                },
                {
                    field: 'UserStatus', title: '状态', align: 'center', minWidth: 50, templet: function (d) {
                        return d.UserStatus === 1 ? '<span class="layui-btn layui-btn-xs">正常</span>'
                            : '<span class="layui-btn layui-btn-xs layui-btn-warm">停用</span>';
                    }
                },
                {
                    field: 'CreateTime', title: '创建时间', align: 'center'
                }
                //,{ title: '操作', minWidth: 175, templet: '#userListBar', fixed: "right", align: "center" }
            ]]
        });
        //搜索【此功能需要后台配合，所以暂时没有动态效果演示】
        $(".search_btn").on("click", function () {
            refreshData();
        });

        //添加用户
        function addModel(edit) {
            var index = layer.open({
                title: "添加/编辑用户",
                type: 2,
                content: "/System/User/UserAdd",
                area: ["1000px", "100%"],
                success: function (layero, index) {
                    var body = layui.layer.getChildFrame('body', index);
                    if (edit) {
                        body.find(".userId").val(edit.UserId);  //用户Id
                        body.find(".userName").val(edit.UserName);  //登录名
                        body.find(".userNikeName").val(edit.UserNikeName);  //邮箱
                        body.find(".userBirthday").val(edit.UserBirthday === null ? "" : edit.UserBirthday.substr(0, 10));  //出生日期
                        body.find(".userQq").val(edit.UserQq);  //qq
                        body.find(".userWx").val(edit.UserWx);  //微信
                        body.find(".userPhone").val(edit.UserPhone);  //手机号
                        body.find(".userEmail").val(edit.UserEmail);  //昵称
                        body.find(".userGroupId").val(edit.UserGroupId);  //所属用户组
                        body.find("#userStatus").val(edit.UserStatus);
                        body.find(".userSex input[value=" + edit.userSex + "]").prop("checked", "checked");  //性别
                        body.find(".note").text(edit.Note);    //用户简介
                        form.render();
                    }
                    setTimeout(function () {
                        layui.layer.tips('点击此处返回用户列表', '.layui-layer-setwin .layui-layer-close', {
                            tips: 3
                        });
                    }, 500);
                }
            });
            layer.full(index);

        }
        function editData() {
            var checkStatus = table.checkStatus('userListTable'),
                data = checkStatus.data;
            if (data.length > 0 && data.length === 1) {
                addModel(data[0]);
            } else if (data.length > 1) {
                layer.msg("一次只能修改一条记录");
            }
            else {
                layer.msg("请选择需要修改的记录");
            }

        }

        //批量删除
        function delData() {
            var checkStatus = table.checkStatus('userListTable'),
                data = checkStatus.data,
                UserId = [];
            if (data.length > 0) {
                for (var i in data) {
                    UserId.push(data[i].UserId);
                }
                console.log(UserId);
                layer.confirm('确定删除选中的用户？', { icon: 3, title: '提示信息' }, function (index) {
                    $.post("/System/User/DelUser", {
                        UserId: UserId  //将需要删除的UserId作为参数传入
                    }, function (res) {
                        layer.msg(res.Messages);
                        layer.close(index);
                        if (res.StateCode === 200) {
                            tableIns.reload();
                        }
                    }, "json");
                });
            } else {
                layer.msg("请选择需要删除的用户");
            }
        }

        function disabledUser() {
            var checkStatus = table.checkStatus('userListTable'),
                data = checkStatus.data,
                UserId = [];
            if (data.length > 0) {
                for (var i in data) {
                    UserId.push(data[i].UserId);
                }
                layer.confirm('确定禁用选中的用户？', { icon: 3, title: '提示信息' }, function (index) {
                    $.post("/System/User/DisableUser", {
                        UserId: UserId, UserStatus: 0  //将需要删除的UserId作为参数传入
                    }, function (res) {
                        layer.msg(res.Messages);
                        layer.close(index);
                        if (res.StateCode === 200) {
                            tableIns.reload();
                        }
                    }, "json");
                });
            } else {
                layer.msg("请选择需要禁用的用户");
            }
        }
        function usableUser() {
            var checkStatus = table.checkStatus('userListTable'),
                data = checkStatus.data,
                UserId = [];
            if (data.length > 0) {
                for (var i in data) {
                    UserId.push(data[i].UserId);
                }
                layer.confirm('确定启用选中的用户？', { icon: 3, title: '提示信息' }, function (index) {
                    $.post("/System/User/DisableUser", {
                        UserId: UserId, UserStatus: 1  //将需要删除的UserId作为参数传入
                    }, function (res) {
                        layer.msg(res.Messages);
                        layer.close(index);
                        if (res.StateCode === 200) {
                            tableIns.reload();
                        }
                    }, "json");
                });
            } else {
                layer.msg("请选择需要启用的用户");
            }
        }
        function refreshData() {
            table.reload("userListTable", {
                page: {
                    curr: 1 //重新从第 1 页开始
                },
                where: {
                    searchstr: $(".searchVal").val()  //搜索的关键字
                }
            });
        }
        table.on("toolbar(userList)", function (obj) {
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
                case 'usable':
                    usableUser();
                    break;
                case 'disable':
                    disabledUser();
                    break;
                case 'refresh':
                    refreshData();
                    break;
            }
        });
    });
});