<%@ Page Language="C#" AutoEventWireup="true" CodeFile="messageEdit.aspx.cs" Inherits="message_messageEdit" %>

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
      $("#txt_content").css({"minHeight":"200px"});
    });
    
    function Check()
    {
    
    var title=$("#txt_title").val().length;
    var content=$("#txt_content").val().length;
    if(title==0)
    {
        alert("标题不能为空");
        $("#txt_title").focus();
         return false;
    }
    else if(content==0)
    {
        alert("内容不能为空");
        $("#txt_content").focus();
        return false;
    }
    else
    {
       return true;
    }
    }
    </script>

</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false">
    <div>
        <div data-role="header">
            <a data-icon="home" data-ajax="false" onclick="ConfirmReturn('../Index.aspx','消息未保存，确定要返回吗')">
                返回</a>
            <h1>
               系统消息 发布</h1>
            <a href="messageList.aspx" data-role="button" class="ui-btn-right" data-icon="grid"
                data-ajax="false">发件箱</a>
        </div>
        <div data-role="content">
            <table>
                <tr>
                    <td style="width: 70px">
                        标题:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_title" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        发布人:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_writer" runat="server" ReadOnly="true"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        发布类型:
                    </td>
                    <td>
                        <asp:Label ID="lbType" runat="server" Text="新闻"></asp:Label>(所有人可见)
                       <%-- <asp:DropDownList ID="ddlType" runat="server">
                            <asp:ListItem Value="新闻">新闻</asp:ListItem>
                           <asp:ListItem Value="通知">通知</asp:ListItem>
                        </asp:DropDownList>--%>
                    </td>
                </tr>
                <tr>
                    <td>
                        发布时间:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_addTime" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td>
                        有效期限:
                    </td>
                    <td>
                        <asp:TextBox ID="txt_endTime" runat="server"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        消息内容:
                    </td>
                </tr>
                <tr>
                    <td colspan="2">
                        <asp:TextBox ID="txt_content" runat="server" TextMode="MultiLine" CssClass="mutiText"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td colspan="2" style="text-align: center;">
                        <asp:Button ID="btnSave" runat="server" Text="保存" data-inline="true" OnClick="btnSave_Click"
                            OnClientClick="return Check()" />
                        <asp:Button ID="btnDelete" runat="server" Text="删除" data-inline="true" OnClick="btnDelete_Click" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="hffj" runat="server" />
            <asp:HiddenField ID="hftzr" runat="server" />
        </div>
        <div data-role="footer" data-position="fixed">
             <footer data-role="footer" id="footer"><h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1></footer>
        </div>
    </div>
    </form>
</body>
</html>
