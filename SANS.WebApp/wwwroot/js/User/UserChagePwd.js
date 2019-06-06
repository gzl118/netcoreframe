require(["jquery", 'layui'], function ($) {
    layui.use(['form', 'layer'], function () {
        var form = layui.form,
            layer = parent.layer === undefined ? layui.layer : top.layer;
        form.on("submit(changePwd)", function (data) {
            console.log(data.field);
            //弹出loading
            var index = top.layer.msg('正在修改，请稍候', { icon: 16, time: false, shade: 0.8 });
            console.log($(".userGroupId").val());
            $.post("/User/UserChagePwd", data.field,
                function (res) {
                    console.log(res);
                    layer.close(index);
                    layer.msg(res.Messages);
                    //if (res.StateCode == 200) {
                    //    layer.closeAll("iframe");
                    //}
                }, 'json');
            return false;
        });
        //添加验证规则
        form.verify({
            newPwd: function (value, item) {
                if (value.length < 6) {
                    return "密码长度不能小于6位";
                }
            },
            confirmPwd: function (value, item) {
                if (!new RegExp($("#oldPwd").val()).test(value)) {
                    return "两次输入密码不一致，请重新输入！";
                }
            }
        });
        //控制表格编辑时文本的位置【跟随渲染时的位置】
        $("body").on("click", ".layui-table-body.layui-table-main tbody tr td", function () {
            $(this).find(".layui-table-edit").addClass("layui-" + $(this).attr("align"));
        });

    });
});