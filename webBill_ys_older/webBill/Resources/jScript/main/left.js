Ext.onReady(function() {
    //导航菜单
    var navMenu = main_left.getNavMenu().value;
    var navMenuJson = Ext.util.JSON.decode(navMenu);

    viewport = new Ext.Viewport({
        renderTo: "mainPanel",
        enableTabScroll: true,
        layout: "border",
        items: [
                {
                    id: "centerTab",
                    region: "center",
                    tabPosition: "bottom"
                },
             {
                 id: "west",
                 region: "west",
                 title: "用户操作导航区",
                 width: 170,
                 //                         autoScroll: true,
                 collapsible: false,
                 split: false,
                 layout: 'accordion',
                 defaults: {//设置默认属性
                     bodyStyle: 'background-color:#f2f9ff;padding:5px'//设置面板体的背景色 
                 },
                 items: navMenuJson
             }
        ]
    });
});