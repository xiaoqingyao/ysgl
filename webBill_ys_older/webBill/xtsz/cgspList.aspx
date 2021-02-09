<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cgspList.aspx.cs" Inherits="webBill_xtsz_cgspList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../Resources/jScript/treeView/treeView.js"></script>
    <script type="text/javascript">
   $(function(){
    $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
   });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 30px"> 
                  
                    <asp:Label ID="lblUserInfo" runat="server" Text="显示当前待设置权限人员信息" ForeColor="Red"></asp:Label>
                    &nbsp;<asp:Button ID="Button1" runat="server" Text="保 存" CssClass="baseButton" OnClick="Button1_Click" />
                    &nbsp;<input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
                    
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:TreeView ID="TreeView1" runat="server" ShowLines="True">
                    </asp:TreeView>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
