<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SelectBxDeptForGk.aspx.cs"
    Inherits="webBill_bxgl_SelectBxDeptForGk" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择报销部门</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div style="float: left; width: 260px; overflow: auto; height: 600px">
        <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" ExpandDepth="2" OnSelectedNodeChanged="TreeView1_OnSelectedNodeChanged">
            <SelectedNodeStyle BackColor="#EBF2F5" />
        </asp:TreeView>
    </div>
    <div style="float: left; width: auto; overflow: auto; height: 360px">
        归口部门:<asp:Label runat="server" ID="lblDept"></asp:Label>
        费用科目：<asp:Label runat="server" ID="lblYskm"></asp:Label>
        报销时间：<asp:Label runat="server" ID="lblBxsj"></asp:Label>
        <asp:Button runat="server" ID="btn_save" OnClick="btn_save_Click" Text="保 存" CssClass="baseButton" />
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
            ShowHeader="true" Width="90%" EmptyDataText="暂无数据" OnRowDataBound="GridView1_OnRowDataBound">
            <Columns>
                <asp:BoundField DataField="deptcode" HeaderText="部门编号" ItemStyle-CssClass="myGridItem"
                    HeaderStyle-CssClass="myGridHeader" />
                <asp:BoundField DataField="deptname" HeaderText="部门名称" ItemStyle-CssClass="myGridItem"
                    HeaderStyle-CssClass="myGridHeader" />
                <asp:BoundField HeaderText="预算金额" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader" />
                <asp:BoundField HeaderText="剩余金额" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader" />
                <asp:BoundField HeaderText="已执行" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader" />
                <asp:TemplateField HeaderText="报销金额" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtbxje" Text="0" Width="60"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:TemplateField HeaderText="税额" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill">
                    <ItemTemplate>
                        <asp:TextBox runat="server" ID="txtse" Text="0"></asp:TextBox>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField HeaderText="报销金额" DataField="bxje" ItemStyle-CssClass="hiddenbill"
                    HeaderStyle-CssClass="hiddenbill" />
                <asp:BoundField HeaderText="税额" DataField="shuie" ItemStyle-CssClass="hiddenbill"
                    HeaderStyle-CssClass="hiddenbill" />
            </Columns>
        </asp:GridView>
    </div>
    </form>
</body>
</html>
