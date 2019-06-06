require(["jquery", 'layui'], function ($) {
    layui.use(['form', 'layer'], function () {
        var form = layui.form;
        var layer = layui.layer;
        //监听提交
        form.on('submit(login)', function (data) {
            var index = layer.load(3); //风格1的加载
            $.ajax({
                url: "/User/UserLogin",
                dataType: "json",
                data: data.field,
                type: "post",
                success: function (res) {
                    layer.close(index);
                    if (res.StateCode === 200) {
                        var userID = res.JsonData.UserId;
                        var tokenUuid = res.JsonData.tokenUuid;
                        var storage = window.sessionStorage;
                        if (storage) {
                            storage.setItem("userID", userID);
                            storage.setItem("tokenUuid", tokenUuid);
                        }
                        location.href = res.JsonData.baseUrl;
                    }
                    else {
                        layer.msg(res.Messages);
                    }
                },
                error: function (err) {
                    layer.close(index);
                    console.log(err);
                }
            });
            return false;
        });
    });
})
