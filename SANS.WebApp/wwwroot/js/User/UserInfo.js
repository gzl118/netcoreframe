require(["jquery", 'layui'], function ($) {
    layui.use(['form', 'layer', 'upload', 'laydate'], function () {
        form = layui.form;
        $ = layui.jquery;
        var layer = parent.layer === undefined ? layui.layer : top.layer,
            upload = layui.upload,
            laydate = layui.laydate;
        //上传头像
        upload.render({
            elem: '.userFaceBtn',
            url: '/user/UploadImg',
            method: "post",  //此处是为了演示之用，实际使用中请将此删除，默认用post方式提交
            done: function (res, index, upload) {
                var num = parseInt(4 * Math.random());  //生成0-4的随机数，随机显示一个头像信息
                $('#userFace').attr('src', res.data[num].src);
                window.sessionStorage.setItem('userFace', res.data[num].src);
            }
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
        //选择出生日期
        laydate.render({
            elem: '.userBirthday',
            trigger: 'click',
            max: 0
        });
        //提交个人资料
        form.on("submit(changeUser)", function (data) {
            var index = layer.msg('提交中，请稍候', { icon: 16, time: false, shade: 0.8 });
            var postdata = {
                UserId: $(".userId").val(),  //用户id
                UserName: $(".userName").val(),  //用户名
                UserNikeName: $(".userNikeName").val(),  //昵称
                UserBirthday: $(".userBirthday").val(),  //出生日期
                UserQq: $(".userQq").val(),  //qq
                UserAvatar: $(".userAvatar").val(),  //头像
                UserEmail: $(".userEmail").val(),  //邮箱
                UserWx: $(".userWx").val(),  //微信
                UserPhone: $(".userPhone").val(),  //手机号
                UserGroupId: $(".userGroupId").val(),  //所属用户组
                UserSex: data.field.sex,  //性别
                Note: $(".note").val()    //备注
            };
            console.log(postdata);
            $.post("/system/user/addoredituser", postdata, function (res) {
                console.log(res);
                layer.close(index);
                layer.msg(res.Messages);
                if (res.Statecode == 200) {
                   location.reload();
                }

            }, 'json');
            return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
        });

        //修改密码
        form.on("submit(changePwd)", function (data) {
            var index = layer.msg('提交中，请稍候', { icon: 16, time: false, shade: 0.8 });
            setTimeout(function () {
                layer.close(index);
                layer.msg("密码修改成功！");
                $(".pwd").val('');
            }, 2000);
            return false; //阻止表单跳转。如果需要表单跳转，去掉这段即可。
        });
    });
});
