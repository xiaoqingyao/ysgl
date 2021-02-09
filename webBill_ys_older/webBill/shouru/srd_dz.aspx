<%@ Page Language="C#" AutoEventWireup="true" CodeFile="srd_dz.aspx.cs" Inherits="webBill_shouru_srd_dz" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <title>大智教育集团收入制单</title>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top: 5px;">
             <input type="button" class="baseButton" value="返回报告单列表页" onclick="window.location.href = '../bxgl/bxglFrame.aspx?dydj=01'" />
            收费日期从：<asp:TextBox ID="txtDateF" runat="server"></asp:TextBox>
            到：<asp:TextBox ID="txtDateT" runat="server"></asp:TextBox>
            校区名称：<asp:DropDownList ID="drp_dept" runat="server"></asp:DropDownList>

            <%--<asp:TextBox ID="txtXiaoqu" runat="server"></asp:TextBox>--%>
            <asp:Button ID="btn_select" runat="server" Text="查询" OnClick="btn_select_Click" CssClass="baseButton" />
           
        </div>
        <div  style="margin-top: 5px;">
             制单日期：<asp:TextBox ID="txtzdrq" runat="server"></asp:TextBox>
             <asp:Button ID="btn_makeBill" runat="server" Text="生成收入报告单" OnClick="btn_makeBill_Click" CssClass="baseButton" />
        </div>
        <div style="margin-top: 5px;">
            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" OnRowDataBound="GridView1_RowDataBound" EnableViewState="false" CssClass="myGrid" ShowFooter="true">
                <HeaderStyle CssClass="myGridHeader" />
                <RowStyle CssClass="myGridItem" />
                <FooterStyle CssClass="myGridItem" />
                <Columns>
                    <asp:BoundField DataField="Date" HeaderText="收费日期" />
                    <asp:BoundField DataField="ReceiptNo" HeaderText="收据号" />
                    <asp:BoundField DataField="CampusName" HeaderText="校区名称" />
                    <asp:BoundField DataField="UserName" HeaderText="操作员姓名" />
                    <asp:BoundField DataField="type" HeaderText="项目类型" />
                    <asp:BoundField DataField="ItemName" HeaderText="项目名称" />
                    <asp:BoundField DataField="TotalMoney" HeaderText="金额" ItemStyle-CssClass="myGridItemRight" HTMLEncode="false" DataFormatString="{0:N}"  />
                    <asp:BoundField DataField="EmployeeNames" HeaderText="业绩归属人姓名" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"/>
                    <asp:BoundField DataField="ConfirmUserName" HeaderText="确认人" />
                    <asp:BoundField DataField="ConfirmTime" HeaderText="收费确认的时间" />

                </Columns>
            </asp:GridView>
        </div>
        <script type="text/javascript">
            $(function () {
                $("#txtDateF").datepicker();
                $("#txtDateT").datepicker();
                $("#txtzdrq").datepicker();
            });
        </script>
    </form>
</body>
</html>
