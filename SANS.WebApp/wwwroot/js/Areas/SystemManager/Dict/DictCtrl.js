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
                document.getElementById('dictListBar').innerHTML = getTpl;
            },
            error: function (XMLHttpRequest, textStatus, errorThrown) {
            }
        });
        //字典列表
        var tableIns = table.render({
            elem: '#dictList',
            url: '/System/Dict/GetDictList',
            cellMinWidth: 95,
            page: true,
            height: "full-20",
            limits: [10, 15, 20, 25],
            limit: 20,
            id: "dictListTable",
            toolbar: "#dictListBar",
            done: function (res, curr, count) {
                //如果是异步请求数据方式，res即为你接口返回的信息。
                //如果是直接赋值的方式，res即为：{data: [], count: 99} data为当前页数据、count为数据总长度
                //$("[data-field='dict_id']").css('display', 'none');
            },
            cols: [[
                { type: "checkbox", fixed: "left", width: 50 },
                { field: 'dict_id', title: 'ID', minWidth: 100, align: "center", hide: true },
                { field: 'type_code', title: '分类编号', minWidth: 100, align: "center" },
                { field: 'type_name', title: '分类名称', align: 'center', minWidth: 150 },
                { field: 'code', title: '子类编号', align: 'center', minWidth: 150 },
                { field: 'name', title: '子类名称', align: 'center', minWidth: 150 },
                { field: 'code_value', title: '子类值', align: 'center', minWidth: 150 },
                { field: 'code_sort', title: '子类排序', align: 'center', minWidth: 15 },
                { field: 'creat_time', title: '创建时间', align: 'center', minWidth: 150 },
                { field: 'remark', title: '备注', align: 'center' }
                //{ title: '操作', minWidth: 175, templet: '#dictListBar', fixed: "right", align: "center" }
            ]]
        });

        //添加用户
        function addModel(edit) {
            var index = layer.open({
                title: "添加/编辑字典",
                type: 2,
                content: "/System/Dict/DictAdd",
                success: function (layero, index) {
                    var body = layer.getChildFrame('body', index);
                    if (edit) {
                        body.find(".dict_id").val(edit.dict_id);
                        body.find(".type_code").val(edit.type_code);  //
                        body.find(".type_name").val(edit.type_name);  //
                        body.find(".code").val(edit.code);  //
                        body.find(".name").val(edit.name);  //
                        body.find(".code_value").val(edit.code_value);  //
                        body.find(".code_sort").val(edit.code_sort);  //
                        body.find(".remark").val(edit.remark);  //
                        form.render();
                    }
                    setTimeout(function () {
                        layui.layer.tips('点击此处返回字典列表', '.layui-layer-setwin .layui-layer-close', {
                            tips: 3
                        });
                    }, 500);
                }
            });
            layer.full(index);

        }
        //批量删除
        function delData() {
            var checkStatus = table.checkStatus('dictListTable'),
                data = checkStatus.data,
                dict_id = [];
            if (data.length > 0) {
                for (var i in data) {
                    dict_id.push(data[i].dict_id);
                }
                console.log(dict_id);
                layer.confirm('确定删除选中的信息？', { icon: 3, title: '提示信息' }, function (index) {
                    $.post("/System/Dict/DelDicts", {
                        dict_id: dict_id
                    }, function (res) {
                        layer.msg(res.Messages);
                        layer.close(index);
                        if (res.StateCode === 200) {
                            tableIns.reload();
                        }
                    }, "json");
                });
            } else {
                layer.msg("请选择需要删除的信息");
            }
        }
        function editData() {
            var checkStatus = table.checkStatus('dictListTable'),
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
        function refreshData() {
            table.reload("dictListTable", {
                page: {
                    curr: 1 //重新从第 1 页开始
                }
            });
        }
        table.on("toolbar(dictList)", function (obj) {
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
                case 'refresh':
                    refreshData();
                    break;
            }
        });

    });
});