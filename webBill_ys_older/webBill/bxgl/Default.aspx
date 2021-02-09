<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="webBill_bxgl_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script type="text/javascript">
        //function xiugai() {
        //    var a = $("#abc").val();
        //    alert($.parseJSON(a));
        //}
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <%-- decodeURI(unescape(data.filename))--%>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        </div>
    </form>
</body>
</html>
