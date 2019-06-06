require.config({
    paths: {
    }
});
require(["jquery", 'layui'], function ($) {
    layui.use(['form', 'layer'], function () {
        var form = layui.form;
        layer = parent.layer === undefined ? layui.layer : top.layer,
            $ = layui.jquery;
        form.on("submit(addUserGroup)", function (data) {
            //弹出loading
            var index = top.layer.msg('数据提交中，请稍候', { icon: 16, time: false, shade: 0.8 });
            // 实际使用时的提交信息
            $.post("/System/UserGroup/AddorEditUserGroup", {
                UserGroupId: $(".userGroupId").val(),//用户组Id
                UserGroupName: $(".userGroupName").val(),  //用户组名称
                ParentUserGroupId: $(".parentUserGroupId").val(),  //父用户组
                Note: $(".note").val()    //备注
            }, function (res) {
                console.log(res);
                top.layer.close(index);
                top.layer.msg(res.Messages);
                if (res.StateCode == 200) {
                    layer.closeAll("iframe");
                    //刷新父页面
                    parent.location.reload();
                }

            }, 'json');
            return false;
        });
        //自定义验证规则
        form.verify({
            username: function (value, item) { //value：表单的值、item：表单的DOM对象
                if (/[\u4e00-\u9fa5]+/.test(value)) {
                    return '用户名不能为汉字';
                }
                if (/\s+/.test(value)) {
                    return '用户名不能包含空格';
                }
                if (!new RegExp("^[a-zA-Z0-9_\u4e00-\u9fa5\\s·]+$").test(value)) {
                    return '用户名不能有特殊字符';
                }
            },
            email: function (value, item) { //value：表单的值、item：表单的DOM对象
                if (value != '') {
                    if (!new RegExp("^[a-z0-9]+([._\\-]*[a-z0-9])*@([a-z0-9]+[-a-z0-9]*[a-z0-9]+.){1,63}[a-z0-9]+$").test(value)) {
                        return '邮箱格式错误';
                    }
                }
            },
            phone: function (value, item) { //value：表单的值、item：表单的DOM对象
                if (value != '') {
                    if (!new RegExp("^[1][3,4,5,7,8,9][0-9]{9}$").test(value)) {
                        return '手机号格式 wd 错误';
                    }
                }
            },
            qq: function (value, item) { //value：表单的值、item：表单的DOM对象
                if (value != '') {
                    if (!new RegExp("^[0-9][0-9]{4,}$").test(value)) {
                        return 'QQ格式错误';
                    }
                }
            }
        });
    });
});