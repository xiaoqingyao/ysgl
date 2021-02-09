<%@ Page Language="C#" AutoEventWireup="true" CodeFile="模板.aspx.cs" Inherits="模板" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>无标题页</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <style>
        #main
        {
            width: 100px;
            height: 100px;
            position: relative;
            background-color: purple;
        }
        #son
        {
            width: 20px;
            height: 20px;
            position: absolute;
            right: 0px;
            top: 0px;
            background-color: Yellow;
        }
    </style>

    <script>
 function objfocus(obj) {
        obj.focus();
    }
    
    $(function(){
       $("#main").click(function(){
       alert("main");
       });
        
       $("#son").click(function(event){
       alert("son");
        event.stopPropagation();
       }); 
    });
    
    $(function(){

$("#green").click(function(event){
alert("green click");
//event.stopImmediatePropagation();
});
$("#green").click(function(){
alert("green click2");
});
$("#orange").click(function(){
alert("orange click");
});
});
    </script>

</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false">
    <div>
        <div data-role="header">
        <a  href="workflow/ybbxAuditList.aspx" data-icon="back" data-role="button">返回列表</a>
            <h1>
                主页</h1>
        </div>
        <div data-role="content">
            <div style="height: 200px; background-color: green;" id="green">
                <div style="height: 100px; background-color: orange;" id="orange">
                </div>
            </div>
            <div id="main" style="margin-top:10px;">
                <div id="son">
                </div>
            </div>
        </div>
        <div data-role="footer" data-position="fixed">
            <footer data-role="footer" id="footer"><h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1></footer>
        </div>
    </div>
    </form>
</body>
</html>
