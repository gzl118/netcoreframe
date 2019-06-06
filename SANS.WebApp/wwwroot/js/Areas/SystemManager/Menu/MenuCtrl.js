var form = null;
layui.use(['form', 'layer'], function () {
    form = layui.form,
        $ = layui.jquery,
        layer = layui.layer; //获取form模块
    zhouliMenu.loadMenu();//加载菜单
    //监听提交
    form.on('submit(saveMenu)', function (data) {
        //layer.msg(JSON.stringify(data.field));
        $.post("/system/menu/addoreditmenu", data.field, function (res) {
            if (res.StateCode === 200) {
                layer.msg("保存成功！");
                zhouliMenu.loadMenu();
            }
            else {
                layer.msg(res.Messages);
            }
        }, "json");
        return false;
    });
    $("#MenuIcon").on("click", function () {
        var index = layer.open({
            type: 2,
            title: "选择图标",
            content: '/system/menu/selectmenuicon', //这里content是一个URL，如果你不想让iframe出现滚动条，你还可以content: ['http://sentsin.com', 'no']
            area: ['90%', '90%'],
            success: function (layero, index) {
                var body = layui.layer.getChildFrame('body', index);
                setTimeout(function () {
                    layer.msg("双击选择图标", { time: 500 });
                }, 500);
            }
        });
        layui.layer.full(index);
    });
});
//ztree配置
var setting = {
    view: {
        addHoverDom: addHoverDom,
        removeHoverDom: removeHoverDom,
        selectedMulti: false,
        showTitle: true
    },
    check: {
        enable: false
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
        enable: true
    },
    callback: {
        // 用于捕获节点编辑名称结束（Input 失去焦点 或 按下 Enter 键）之后，更新节点名称数据之前的事件回调函数，并且根据返回值确定是否允许更改名称的操作
        beforeRename: editBeforeName,
        //用于捕获单击节点之前的事件回调函数，并且根据返回值确定是否允许单击操作
        beforeClick: zTreeBeforeClick,
        //用于捕获节点被删除之前的事件回调函数，并且根据返回值确定是否允许删除操作
        beforeRemove: zTreeBeforeRemove,
        //用于捕获节点拖拽操作结束之前的事件回调函数，并且根据返回值确定是否允许此拖拽操作
        beforeDrop: zTreeBeforeDrop
    }
};
//添加菜单
function addHoverDom(treeId, treeNode) {
    var sObj = $("#" + treeNode.tId + "_span");
    if (treeNode.editNameFlag || $("#addBtn_" + treeNode.tId).length > 0) return;
    var addStr = "<span class='button add' id='addBtn_" + treeNode.tId
        + "' title='添加子菜单' onfocus='this.blur();'></span>";
    sObj.after(addStr);
    var btn = $("#addBtn_" + treeNode.tId);
    btn.next('.edit').attr('title', '编辑');
    btn.next('.edit').next('.remove').attr('title', '删除');
    if (btn) btn.bind("click", function () {
        var zTree = $.fn.zTree.getZTreeObj("treeMenu");
        var node = { MenuId: createGuid(), ParentMenuId: treeNode.MenuId, MenuName: "新建菜单1", MenuIcon: "layui-icon-file", MenuType: 0 };
        zTree.addNodes(treeNode, node);
        //添加之后启用编辑状态
        zTree.editName(zTree.getNodeByParam("MenuId", node.MenuId, null));
        return false;
    });
}
//移除菜单
function removeHoverDom(treeId, treeNode) {
    $("#addBtn_" + treeNode.tId).unbind().remove();
}
//编辑确定事件
function editBeforeName(treeId, treeNode, newName, isCancel) {
    console.log(treeNode);
    $("#MenuId").val(treeNode.MenuId);
    $("#MenuName").val(newName);
    $("#ParentMenuId").val(treeNode.ParentMenuId);
    $("#MenuUrl").val(treeNode.MenuUrl);
    $("#MenuSort").val(treeNode.MenuSort === undefined ? treeNode.getIndex() : treeNode.MenuSort);
    $("#MenuIcon").val(treeNode.MenuIcon);
    $("#Note").val(treeNode.Note);
    var postData = {
        MenuId: treeNode.MenuId,
        MenuName: newName,
        ParentMenuId: treeNode.ParentMenuId,
        MenuUrl: treeNode.MenuUrl,
        MenuSort: treeNode.MenuSort === undefined ? treeNode.getIndex() : treeNode.MenuSort,
        MenuIcon: treeNode.MenuIcon,
        Note: treeNode.Note
    };
    $.post("/system/menu/addoreditmenu", postData, function (res) {
        console.log(res);
    }, "json");
    return true;
}
//节点单击事件
function zTreeBeforeClick(treeId, treeNode, clickFlag) {
    $("#MenuId").val(treeNode.MenuId);
    $("#MenuName").val(treeNode.MenuName);
    $("#ParentMenuId").val(treeNode.ParentMenuId);
    $("#MenuUrl").val(treeNode.MenuUrl);
    $("#MenuSort").val(treeNode.MenuSort);
    $("#MenuIcon").val(treeNode.MenuIcon);
    $("#Note").val(treeNode.Note);
    var temp = treeNode.MenuType;
    if (temp === null)
        temp = 0;
    $(".menuType input[value=" + temp + "]").prop("checked", "checked");
    form.render();
    return true;
}
//节点删除之前的事件
function zTreeBeforeRemove(treeId, treeNode) {
    console.log(treeNode);
    if (treeNode.children.length > 0) {
        layer.msg("请先删除子节点！");
        return false;
    }
    layer.confirm('确定删除该菜单节点？', { icon: 3, title: '提示信息' }, function (index) {
        $.ajax({
            url: "/system/menu/delmenu",
            type: "post",
            data: { MenuId: treeNode.MenuId },
            dataType: "json",
            async: false,
            success: function (res) {
                if (res.StateCode !== 200) {
                    layer.msg(res.Messages);
                    return false;
                }
                else {
                    layer.msg("删除成功！");
                    //zTree.removeChildNodes(treeNode);//删除当前节点子节点
                    var zTree = $.fn.zTree.getZTreeObj("treeMenu");
                    zTree.removeNode(treeNode);//删除当前节点
                    return true;
                }
            },
            error: function (err) {
                layer.close(index);
                layer.msg("服务器出错");
                return false;
            }
        });
    });
    return false;
}
//拖拽后的事件
function zTreeBeforeDrop(treeId, treeNodes, targetNode, moveType) {
    if (moveType === "inner" || treeNodes[0].ParentMenuId != targetNode.ParentMenuId) {
        layer.msg("不允许跨级拖放", { time: 1000 });
        return false;
    }
    console.log(treeNodes);
    console.log(targetNode);
    console.log(moveType);
    var menuNode = treeNodes[0];
    if (menuNode) {
        menuNode.MenuSort = targetNode.getIndex();
        if (moveType === "prev") {
            menuNode.MenuSort = parseInt(targetNode.MenuSort) - 1;
        } else {
            menuNode.MenuSort = parseInt(targetNode.MenuSort) + 1;
        }
        $.ajax({
            url: "/system/menu/addoreditmenu",
            type: "post",
            data: menuNode,
            dataType: "json",
            async: false,
            success: function (res) {
                if (res.StateCode != 200) {
                    layer.msg(res.Messages);
                    zhouliMenu.loadMenu();
                }
            },
            error: function (err) {
                layer.msg("服务器出错");
            }
        });
    }
    return true;
};
//添加根节点
function addRootNode() {
    var ztree = zhouliMenu.getZtreeObj();
    var nodes = ztree.getSelectedNodes();
    if (nodes && nodes.length > 0) {
        var node = { MenuId: createGuid(), ParentMenuId: nodes[0].ParentMenuId, MenuName: "新建菜单1", MenuIcon: "layui-icon-file", MenuSort: nodes.length };
        ztree.addNodes(nodes[0].getParentNode(), node);
        //添加之后启用编辑状态
        ztree.editName(ztree.getNodeByParam("MenuId", node.MenuId, null));

    } else {
        layer.msg("请先选择一个节点");
    }

}
//js生成guid
function createGuid() {
    var s = [];
    var hexDigits = "0123456789abcdef";
    for (var i = 0; i < 36; i++) {
        s[i] = hexDigits.substr(Math.floor(Math.random() * 0x10), 1);
    }
    s[14] = "4";  // bits 12-15 of the time_hi_and_version field to 0010
    s[19] = hexDigits.substr((s[19] & 0x3) | 0x8, 1);  // bits 6-7 of the clock_seq_hi_and_reserved to 01
    s[8] = s[13] = s[18] = s[23] = "-";

    var uuid = s.join("");
    return uuid;
}
var zhouliMenu = {
    //加载菜单
    loadMenu: function () {
        $.get("/System/Menu/getmenulist", { name: "John", time: "2pm" },
            function (data) {
                console.log(data);
                //绑定zTree
                $.fn.zTree.init($("#treeMenu"), setting, data.JsonData);
                //展开所有节点
                $.fn.zTree.getZTreeObj("treeMenu").expandAll(true);
            }, "json");
    },
    getZtreeObj: function () {
        return $.fn.zTree.getZTreeObj("treeMenu");
    }
};

