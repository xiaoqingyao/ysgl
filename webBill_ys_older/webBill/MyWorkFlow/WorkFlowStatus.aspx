<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkFlowStatus.aspx.cs" Inherits="webBill_MyWorkFlow_WorkFlowStatus" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="baseDiv">
            审批人：<asp:DropDownList ID="ddlShenPiRen" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShenPiRen_SelectedIndexChanged"></asp:DropDownList>
            <asp:Button ID="btn_excle" runat="server"  CssClass="baseButton" Text="导出excel" OnClick="btn_excle_Click" />
        </div>
        <div class="baseDiv">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" EnableViewState="false" CssClass="myGrid" Width="500px">
                <HeaderStyle CssClass="myGridHeader" />
                <RowStyle CssClass="myGridItem" />
                <FooterStyle CssClass="myGridItem" />
                <Columns>
                    <asp:BoundField DataField="billname" HeaderText="单据类型" />
                    <asp:BoundField DataField="checkuser" HeaderText="审批人" />
                    <asp:BoundField DataField="sl" HeaderText="待审批单据数" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
