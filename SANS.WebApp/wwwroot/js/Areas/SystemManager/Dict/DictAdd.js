require.config({
    paths: {
    }
});
require(["jquery", 'layui'], function ($) {
    layui.use(['form', 'layer', 'laydate'], function () {
        var form = layui.form;
        var laydate = layui.laydate;
        layer = parent.layer === undefined ? layui.layer : top.layer,
            $ = layui.jquery;
        form.on("submit(addDict)", function (data) {
            //弹出loading
            var index = top.layer.msg('数据提交中，请稍候', { icon: 16, time: false, shade: 0.8 });
            // 实际使用时的提交信息
            $.post("/System/Dict/AddorEditDict", {
                dict_id: $(".dict_id").val(),
                type_code: $(".type_code").val(),  //
                type_name: $(".type_name").val(),  //
                code: $(".code").val(),  //
                name: $(".name").val(),  //
                code_value: $(".code_value").val(),  //qq
                code_sort: $(".code_sort").val(),  //
                remark: $(".remark").val(),  //
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
    });
});