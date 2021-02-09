<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NewsForLook.aspx.cs" Inherits="webBill_DeskMessage_NewsForLook" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head id="Head1" runat="server">
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<body>
    <form id="form1" runat="server">
       <table style="width:600px; margin:0 auto;" >
       <tr>
        <td id="title" runat="server" class="tableBg"></td>
       </tr>
       <tr>
        <td id="context" runat="server" ></td>
       </tr>
       <tr>
        <td>
            发布人:<asp:Label ID="lb_fbr" runat="server" Text=""></asp:Label>&nbsp;
            类型:<asp:Label ID="lb_type" runat="server" Text=""></asp:Label>
        </td>
       </tr>
       <tr>
        <td class="tableBg">
            <asp:Button ID="btn_return" runat="server" Text="返回" CssClass="baseButton" />
        </td>
       </tr>
       </table> 
   
    </form>
</body>
</html>