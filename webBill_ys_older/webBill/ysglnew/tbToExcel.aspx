<%@ Page Language="C#" AutoEventWireup="true" CodeFile="tbToExcel.aspx.cs" Inherits="webBill_ysglnew_tbToExcel" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head >
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
      <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
</head>
<body>
    <form id="form1" runat="server">
    <asp:Button ID="btn_ExportExcel" runat="server" Text="导出excel" CssClass="baseButton"
        OnClick="btn_ExportExcel_Click" />
    <div>
        <asp:GridView ID="GridView1_toExcel" runat="server" AutoGenerateColumns="false" CssClass="baseTable">
            <Columns>
                <asp:BoundField DataField="kmbh" HeaderText="科目编码" HtmlEncode="false" />
                <asp:BoundField DataField="km" HeaderText="月份\科目" HtmlEncode="false" />
                <asp:BoundField DataField="January" HeaderText="1月份" />
                <asp:BoundField DataField="February" HeaderText="2月份" />
                <asp:BoundField DataField="march" HeaderText="3月份" />
                <asp:BoundField DataField="April" HeaderText="4月份" />
                <asp:BoundField DataField="May" HeaderText="5月份" />
                <asp:BoundField DataField="June" HeaderText="6月份" />
                <asp:BoundField DataField="July" HeaderText="7月份" />
                <asp:BoundField DataField="August" HeaderText="8月份" />
                <asp:BoundField DataField="September" HeaderText="9月份" />
                <asp:BoundField DataField="October" HeaderText="10月份" />
                <asp:BoundField DataField="November" HeaderText="11月份" />
                <asp:BoundField DataField="December" HeaderText="12月份" />
                <asp:BoundField DataField="year" HeaderText="年度总预算" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
