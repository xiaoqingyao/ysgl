<%@ Page Language="C#" AutoEventWireup="true" CodeFile="messageView.aspx.cs" Inherits="message_messageView" %>

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
      
    </script>

</head>
<body>
    <form id="form1" runat="server" data-role="dialog" data-theme="b" method="post" >
    <div>
        <div data-role="header">
            <h1>
                消息详细信息</h1>
        </div>
        <div data-role="content">
           <table style="color: Black; font-family: 微软雅黑;">
                <tr>
                    <td style="width:70px"  class='tdborder'>
                        标题:
                    </td>
                    <td  class='tdborder'>
                        <asp:Label ID="lb_tilte" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td  class='tdborder'>
                        阅读次数:
                    </td>
                    <td  class='tdborder'>
                     <asp:Label ID="lb_count" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td  class='tdborder'>
                        发布人:
                    </td>
                    <td  class='tdborder'>
                     <asp:Label ID="lb_writer" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td  class='tdborder'>
                        发布类型:
                    </td>
                    <td  class='tdborder'>
                     <asp:Label ID="lb_type" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td  class='tdborder'>
                        发布时间:
                    </td>
                    <td  class='tdborder'>
                     <asp:Label ID="lb_addTime" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td  class='tdborder'>
                        有效期限:
                    </td>
                    <td  class='tdborder'>
                     <asp:Label ID="lb_endTime" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="2"  class='tdborder'>
                        消息内容:
                    </td>
                </tr>
                <tr>
                    <td colspan="2"  class='tdborder'>
                    <asp:Label ID="lb_content" runat="server" Text="Label"></asp:Label>
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>

