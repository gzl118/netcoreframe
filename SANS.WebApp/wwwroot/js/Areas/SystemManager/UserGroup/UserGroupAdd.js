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
                if (res.StateCode === 200) {
                    layer.closeAll("iframe");
                    //刷新父页面
                    parent.location.reload();
                }

            }, 'json');
            return false;
        });
    });
});