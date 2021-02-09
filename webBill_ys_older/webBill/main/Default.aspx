<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="webBill_main_Default" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
<%--  --%>  <link href="../Resources/jScript/Ext1.4.2/resources/css/ext-all.css" rel="stylesheet"
        type="text/css" />

    <script src="../Resources/jScript/Ext1.4.2/ext-all.js" type="text/javascript"></script>

    <script type="text/javascript">
        Ext.onReady(function() {
            //        var mypanel = new Ext.Panel({
            //            renderTo: Ext.getBody(),
            //            title: "test面板",
            //            width: 400,
            //            height: 300,
            //            html: '<h1>主面板</h1>',
            //            tbar: [{ text: 'button1' }, { text: 'button2'}],
            //            bbar: [{ text: '底部工具栏'}],
            //            buttons: [{ text: "按钮位于footer"}],
            //            items: [{xtype:'htmleditor'}]
            //        });
            //        var panel = new Ext.TabPanel({
            //            renderTo: Ext.getBody(),
            //            width: 300,
            //            height: 200,
            //            activeTab: 0,
            //            items: [new Ext.Panel({
            //                title: "面板1",
            //                height: 30,
            //                html: '面板1'
            //            }), new Ext.Panel({
            //                title: "面板2",
            //                height: 30,
            //                html: '面板2'
            //            }),
            //            new Ext.Panel({
            //                title: "面板3",
            //                height: 30,
            //                html: '面板3'
            //            })]
            //        });
            //            var viewport = new Ext.Viewport({
            //                layout: 'border',
            //                renderTo: Ext.getBody(),
            //                items: [{ title: 'borth', region: 'north', split: true, border: true, collapsible: true, height: 100, mainsize: 100, maxSize: 120, html: '北方' }
            //                    , { title: 'south', region: 'south', split: true, border: true, collapsible: true, height: 100, mainsize: 100, maxSize: 120, html: '南方' }
            //                    , { title: 'east', region: 'east', split: true, border: true, collapsible: true, height: 100, mainsize: 100, maxSize: 120, html: '帮助信息' }
            //                    , { title: 'west', region: 'west', split: true, border: true, collapsible: true, height: 100, mainsize: 100, maxSize: 120,html:'菜单' }
            //                    , { title: 'center', region: 'center', split: true, border: true, collapsible: true, html: '内容' }
            //                ]
        //            });
            //定义列
            var cm = new Ext.grid.ColumnModel([
            { header: '编号', dataIndex: 'id' }
            , { header: '名称', dataIndex: 'name' }
            ]);
            //数据定义
            var data = [
                ['1', 'name1'],
                ['2', 'name2'],
                ['3', 'name2']
            ];
            //数据源定义
            var ds = new Ext.data.Store({
                proxy: new Ext.data.MemoryProxy(data),
                reader: new Ext.data.ArrayReader({}, [
                    { name: 'id' },
                    { name: 'name'}
                ])
            });
            ds.load();
            var grid = new Ext.grid.GridPanel({
                renderTo: Ext.getBody(),
                ds: ds,
                cm:cm,
                width: 300,
                autoHeight:true
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
    </div>
    </form>
</body>
</html>
