<%@ Page Language="C#" AutoEventWireup="true" CodeFile="testapi.aspx.cs" Inherits="testapi" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <script src="webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js"></script>
    <script type="text/javascript">
        //$.getJSON("http://api-data.xiaogj.com/dazhi/GetFeeList?sign=2584239397f66bbf5306a9b56fb4ce49&timestamp=1442560189&sdate=2015-09-01&edate=2015-09-30", function (json) {
        //    alert("jsonData:" + json.ErrorCode);
        //    alert(json.Data.length);
        //    for (var i = 0; i < 2; i++) {
        //        alert(json.Data[0].Type);
        //        alert(json.Data[0].CampusName);
        //        alert(json.Data[0].ReceiptNo);
        //        alert(json.Data[0].UserName);
        //        alert(json.Data[0].Date);
        //        alert(json.Data[0].ItemName);
        //        alert(json.Data[0].TotalMoney);
        //    }
        //});
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:TextBox ID="TextBox1" runat="server" Height="203px" Width="346px" TextMode="MultiLine"></asp:TextBox>
            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
        </div>
        <div>
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="true"></asp:GridView>
        </div>
    </form>
</body>
</html>
