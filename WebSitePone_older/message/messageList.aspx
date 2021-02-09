<%@ Page Language="C#" AutoEventWireup="true" CodeFile="messageList.aspx.cs" Inherits="message_messageList" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>系统消息编辑页面</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="../js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <link href="../Css/CommenCss.css" rel="stylesheet" type="text/css" />

    <script src="../js/Common.js" type="text/javascript"></script>


    <script type="text/javascript">
    
    $(function(){
        $(".title-r").click(function(event){
            event.stopPropagation();
          }); 
    });
     function Delete(id,obj)
     {
         if(confirm("您确定要删除吗"))
         {
             $("#hfid").val(id);
             $("#btndelete").click();
         }
     }
     
     function ViewMsg(id)
     {
       $.post("Handler.ashx",function(data,status){
           if(status=="success")
           {
               $("#openMsg").attr("href","messageView.aspx?id="+id);
               $("#openMsg").click();
           }
           });
     }
     
    </script>

</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false">
    <div>
        <div data-role="header">
            <a data-icon="home" data-ajax="false" href="../Index.aspx">主页</a>
            <h1>
                发件箱</h1>
                <a href="messageEdit.aspx?type=add" data-role="button" class="ui-btn-right" data-icon="plus"
                data-ajax="false">发布消息</a>
        </div>
        <div data-role="content">
            <a href="messageView.aspx" id="openMsg" data-role="button" data-inline="true" data-rel="dialog"
                data-transition="pop" style="display: none;">Open dialog</a>
            <div style="display: none">
                <asp:Button ID="btndelete" runat="server" Text="dopostbadk" OnClick="DeleteMsg" /></div>
            <asp:HiddenField ID="hfid" runat="server" />
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <div data-role="collapsible" data-collapsed="false" data-theme="b" class="c-main">
                        <h1>
                            <div class="title">
                                <div class="title-l">
                                    <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("title").ToString().Length>9?Eval("title").ToString().Substring(0,9)+"...":Eval("title") %>'></asp:Literal>
                                </div>
                                <div class="title-r">
                                    <a data-role="button" data-inline="true" onclick="ViewMsg('<%#Eval("id") %>')" data-ajax="false">
                                        <img src="../images/metro/View-Small-Icons.png" />详细信息</a>
                                </div>
                            </div>
                        </h1>
                        <div class="c-row">
                            <div class="c-row-item">
                                <div>
                                    发布日期：</div>
                                <%#Eval("date") %></div>
                            <div class="c-row-item">
                                <div>
                                    有效期限：</div>
                                <%#Eval("endtime") %></div>
                            <div class="c-row-item">
                                <div>
                                    内容：</div>
                                </br>
                                <%#Eval("contents") %></div>
                        </div>
                        <div class="c-option" id="optionDiv" runat="server" data-role="controlgroup" data-type="horizontal"
                            data-inline="true">
                            <a data-role="button" data-inline="true" data-transition="pop" href='messageEdit.aspx?id=<%#Eval("id")%>&type=edit'
                                data-theme="d" data-ajax="false">
                                <img src='../images/metro/Road-Right.png' />修 改</a> <a data-role='button' data-inline='true'
                                    data-theme='d' onclick="Delete('<%#Eval("id") %>',this)">
                                    <img src='../images/metro/Delete.png' />删 除</a>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
            <div style="clear: both; height: 19px;">
            </div>
            <div id="lbPageNav" runat="server">
            
            </div>
        </div>
        <div data-role="footer" data-position="fixed">
             <footer data-role="footer" id="footer"><h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1></footer>
        </div>
    </div>
    </form>
</body>
</html>
