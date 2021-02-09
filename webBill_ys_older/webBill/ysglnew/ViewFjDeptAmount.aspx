<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ViewFjDeptAmount.aspx.cs"
    Inherits="webBill_ysglnew_ViewFjDeptAmount" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>各部门预算分解情况</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div>
        科目：<asp:Label runat="server" ID="lblKmmc"></asp:Label>
        总分配金额：<asp:TextBox runat="server" ID="lblTotalAmount"></asp:TextBox>
        <asp:Button ID="btn_Calculate" runat="server" Text="重新计算" OnClick="btn_Calculate_Click"
            CssClass="baseButton" />
        <input class="baseButton" type="button" id="closebutton" onclick="javascript:self.close();"
            value="关闭" />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
            ShowHeader="true" ShowFooter="true" Width="98%" EmptyDataText="暂无数据" OnRowDataBound="GridView_RowDataBound">
            <Columns>
                <asp:BoundField DataField="deptcode" HeaderText="部门编号" ItemStyle-CssClass="myGridItem"
                    HeaderStyle-CssClass="myGridHeader" FooterStyle-CssClass="myGridItem" />
                <asp:BoundField DataField="deptname" HeaderText="部门名称" ItemStyle-CssClass="myGridItem"
                    HeaderStyle-CssClass="myGridHeader" FooterStyle-CssClass="myGridItem" />
                <asp:BoundField DataField="fjje" HeaderText="预算控制金额" ItemStyle-CssClass="myGridItemRight"
                    HeaderStyle-CssClass="myGridHeader" FooterStyle-CssClass="myGridItem" DataFormatString="{0:F2}" />
                <asp:BoundField DataField="fjbl" HeaderText="分解比例" ItemStyle-CssClass="myGridItemRight"
                    HeaderStyle-CssClass="myGridHeader" FooterStyle-CssClass="myGridItem" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
