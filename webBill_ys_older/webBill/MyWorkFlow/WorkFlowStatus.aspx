<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkFlowStatus.aspx.cs" Inherits="webBill_MyWorkFlow_WorkFlowStatus" EnableEventValidation="false" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <title></title>
    <script>
        $(function () {
            initMainTableClass("<%=GridView1.ClientID%>");
        });
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div class="baseDiv">
            审批人：<asp:DropDownList ID="ddlShenPiRen" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlShenPiRen_SelectedIndexChanged"></asp:DropDownList>
            <asp:Button ID="btn_excle" runat="server" CssClass="baseButton" Text="导出excel" OnClick="btn_excle_Click" />
        </div>
        <div class="baseDiv">
            <div style="width: 520px; float: left">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" EnableViewState="true" CssClass="myGrid" Width="500px"
                    OnSelectedIndexChanged="GridView1_SelectedIndexChanged"
                    OnRowDataBound="GridView1_RowDataBound" AllowPaging="false">
                    <HeaderStyle CssClass="myGridHeader" Height="30px" />
                    <RowStyle CssClass="myGridItem"  Height="25px"/>
                    <FooterStyle CssClass="myGridItem" />
                    <Columns>
                        <asp:BoundField DataField="billname" HeaderText="单据类型" />
                        <asp:BoundField DataField="checkuser" HeaderText="审批人" />
                        <asp:BoundField DataField="sl" HeaderText="待审批单据数" />
                        <asp:BoundField DataField="flowid" HeaderText="审批类型" HeaderStyle-CssClass="hiddenbill"
                            ItemStyle-CssClass="hiddenbill" />
                    </Columns>
                </asp:GridView>
            </div>
            <div style="  float: left;">
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" EnableViewState="false" CssClass="myGrid" Width="500px">
                    <HeaderStyle CssClass="myGridHeader" Height="30px" />
                    <RowStyle CssClass="myGridItem" Height="25px"/>
                    <FooterStyle CssClass="myGridItem" />
                    <Columns>
                        <asp:BoundField DataField="billname" HeaderText="单据编号" />
                        <asp:BoundField DataField="billtype" HeaderText="单据类型" />
                        <asp:BoundField DataField="billuser" HeaderText="申请人" />
                        <asp:BoundField DataField="billdept" HeaderText="部门" />
                        <asp:BoundField DataField="je" HeaderText="单据金额" DataFormatString="{0:F2}"/>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
</body>
</html>
