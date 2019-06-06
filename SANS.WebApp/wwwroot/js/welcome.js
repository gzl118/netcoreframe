var layer = null;
var form = null;
//var $ = null;
layui.use(['form', 'layer', 'jquery', 'table', 'laytpl', 'element'], function () {
    form = layui.form;
    layer = parent.layer === undefined ? layui.layer : top.layer;
    var table = layui.table, element = layui.element;

});
