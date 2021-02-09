<%@ Page Language="C#" AutoEventWireup="true" CodeFile="messageGetList.aspx.cs" Inherits="message_messageGetList"  EnableViewState="false"%>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>收件箱</title>
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
    
     function ViewMsgD(id)
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
                收件箱</h1>
        </div>
        <div data-role="content">
        <a href="messageView.aspx" id="openMsg" data-role="button" data-inline="true" data-rel="dialog"
                data-transition="pop" style="display: none;">Open dialog</a>
            <asp:Repeater ID="Repeater1" runat="server">
                <ItemTemplate>
                    <div data-role="collapsible" data-collapsed="false" data-theme="b" class="c-main">
                        <h1>
                            <div class="title">
                                <div class="title-l">
                                    <asp:Literal ID="Literal1" runat="server" Text='<%# Eval("title").ToString().Length>9?Eval("title").ToString().Substring(0,9)+"...":Eval("title") %>' ></asp:Literal>
                                </div>
                                <div class="title-r">
                                <a data-role="button" data-inline="true" onclick="ViewMsgD('<%#Eval("id") %>')" data-ajax="false">
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
                                    内容：</div></br>
                                <%#Eval("contents") %></div>
                        </div>
                    </div>
                </ItemTemplate>
            </asp:Repeater>
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
