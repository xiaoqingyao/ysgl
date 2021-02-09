<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gkdept_zt_list.aspx.cs" Inherits="webBill_ysglnew_gkdept_zt_list" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <asp:Label runat="server" ID="lbl_masge" ForeColor="red"></asp:Label>
            <asp:GridView ID="Gridzt" runat="server" AutoGenerateColumns="false" CssClass="myGrid" Width="500" OnRowDataBound="Gridzt_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="code" HeaderStyle-Width="100" ItemStyle-Width="100" HeaderText="部门编号" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader" />
                    <asp:BoundField DataField="name" HeaderText="部门名称" HeaderStyle-Width="100" ItemStyle-Width="100" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader" />
                    <asp:BoundField DataField="zt" HeaderText="生效状态" HeaderStyle-Width="100" ItemStyle-Width="100" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
