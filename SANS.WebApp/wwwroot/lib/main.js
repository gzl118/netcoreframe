//配置公共的js
var require = {
    baseUrl: "../",
    shim: {
        //'zTree': {
        //    deps: ['jquery'],
        //    exports: 'zTree'
        //},
        "ztree_excheck": ['jquery'],
        "ztree_exedit": ['jquery']
    },
    paths: {
        "jquery": ["https://apps.bdimg.com/libs/jquery/2.1.4/jquery.min", "/lib/jquery/dist/jquery.min"],
        "layui": "/lib/layui/layui",
        "pinyin": "/lib/pinyin/pinyin",//汉字转拼音插件
        "ztree": "/lib/ztree/js/jquery.ztree.core.min",
        "ztree_excheck": "/lib/ztree/js/jquery.ztree.excheck.min",
        "ztree_exedit": "/lib/ztree/js/jquery.ztree.exedit.min"
    }
};

