layui.use(['form', 'layer', 'element'], function () {
    var form = layui.form;
    element = layui.element;
    layer = parent.layer === undefined ? layui.layer : top.layer,
        $ = layui.jquery;
    //选项卡切换事件
    element.on('tab(RoleAuthTab)', function (data) {
        console.log(data);
    });
    var roleModel = {
        RoleId: null,
        RoleName: null,
        Note: null
    };
    var roleVO = {
        roleDto: null,
        objCheckMenus: null
    };
    form.on("submit(addRole)", function (data) {
        //弹出loading
        var index = top.layer.msg('数据提交中，请稍候', { icon: 16, time: false, shade: 0.8 });
        var objCheckMenus = zhouliMenu.getZtreeObj().getCheckedNodes(true);
        console.log(objCheckMenus);

        roleModel.RoleId = $(".roleId").val();
        roleModel.RoleName = $(".roleName").val();
        roleModel.Note = $(".note").val();

        roleVO.roleDto = roleModel;
        roleVO.objCheckMenus = objCheckMenus;
        var strjson = JSON.stringify(roleVO);

        // 实际使用时的提交信息
        $.post("/System/Role/AddorEditRole", {
            sjson: strjson
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
    $(function () {
        setTimeout(function () {
            console.log($(".roleId").val());
            zhouliMenu.loadMenu($(".roleId").val());
        }, 200);

    })
});
//ztree配置
var setting = {
    view: {
        addHoverDom: false,
        removeHoverDom: false,
        selectedMulti: false,
        showTitle: true
    },
    check: {
        enable: true
    },
    data: {
        simpleData: {
            enable: true
        },
        key: {
            name: "MenuName",
            idKey: "MenuId",
            pIdKey: "ParentMenuId"
        }
    },
    edit: {
        enable: false
    }
};
var zhouliMenu = {
    //加载菜单
    loadMenu: function (RoleId) {
        $.get("/System/Role/GetRoleMenuList", { RoleId: RoleId },
            function (data) {
                console.log(data);
                //绑定zTree
                $.fn.zTree.init($("#treeMenu"), setting, data.JsonData.MenuList);
                //展开所有节点
                $.fn.zTree.getZTreeObj("treeMenu").expandAll(true);
                if (data.JsonData.RoleMenuList.length > 0) {
                    for (var i = 0; i < data.JsonData.RoleMenuList.length; i++) {
                        var node = $.fn.zTree.getZTreeObj("treeMenu").getNodeByParam("MenuId", data.JsonData.RoleMenuList[i].MenuId);
                        $.fn.zTree.getZTreeObj("treeMenu").checkNode(node, true, false, true);
                    }
                }

            }, "json");
    },
    getZtreeObj: function () {
        return $.fn.zTree.getZTreeObj("treeMenu");
    }
};
