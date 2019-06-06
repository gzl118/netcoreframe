require.config({
    paths: {
    }
});
require(["jquery", 'layui'], function ($) {
    layui.use('table', function () {
        var table = layui.table;
        var $ = layui.$, active = {
            parseTable: function () {
                table.init('parse-table-demo', { //转化静态表格
                    //height: 'full-500'
                });
            }
        };
        table.init('parse-table-demo', { //转化静态表格
            //height: 'full-500'
            even: true
        });

        $('#btntest').on('click', function () {
            var type = $(this).data('type');
            active[type] ? active[type].call(this) : '';
        });
        //active['parseTable'].call(this);
        table.on('tool(parse-table-demo)', function (obj) {
            var layEvent = obj.event,
                data = obj.data;
            if (layEvent === 'excute') {
                layer.open({
                    type: 1,
                    title: '审批选择',
                    content: $("#ApprovePerson"),
                    btn: ['确定', '取消'],
                    yes: function () {
                        layer.msg("审批请求已发送！");
                    },
                    btn2: function () {
                        layer.msg("审批请求已取消！");
                    },
                    closeBtn: 0
                });
            }
            else if (layEvent === 'stop') {
                layer.confirm("【ZK11.B4:SSE-A电源状态(AOCE)】指令请求执行，是否同意？", {
                    btn: ['确定', '取消'],
                    closeBtn: 0
                },
                    function (index, layero) {
                        layer.msg("指令执行请求已通过！");
                    },
                    function (index) {
                        layer.msg("指令执行请求未通过！");
                    }
                );
            }
            else if (layEvent === 'pause') {
                layer.confirm("【ZK11.B4:SSE-A电源状态(AOCE)】指令请求执行审批已通过，是否继续？", {
                    btn: ['确定', '取消'],
                    closeBtn: 0
                },
                    function (index, layero) {
                        layer.msg("继续执行指令！");
                    },
                    function (index) {
                        layer.msg("取消执行指令！");
                    }
                );
            }
            else if (layEvent === 'continue') {
                layer.confirm("【ZK11.B4:SSE-A电源状态(AOCE)】指令请求执行审批未通过，是否重新发送请求！", {
                    btn: ['确定', '取消'],
                    closeBtn: 0
                },
                    function (index, layero) {
                        layer.msg("审批请求已发送！");
                    },
                    function (index) {
                        layer.msg("审批请求已取消！");
                    }
                );
            }
        });
    });
});