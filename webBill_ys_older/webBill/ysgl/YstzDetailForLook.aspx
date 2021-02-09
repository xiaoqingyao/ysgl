<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YstzDetailForLook.aspx.cs" Inherits="webBill_ysgl_YstzDetailForLook" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div style="text-align: center">
        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
            CssClass="myGrid" onrowdatabound="GridView1_RowDataBound">
            <Columns>
                <asp:BoundField HeaderText="类型" ItemStyle-CssClass="myGridItem" 
                    HeaderStyle-CssClass="myGridHeader" >
                <HeaderStyle CssClass="myGridHeader"></HeaderStyle>
                <ItemStyle CssClass="myGridItem"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="gcbh" HeaderText="预算过程" 
                    ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader" >
                    <HeaderStyle CssClass="myGridHeader"></HeaderStyle>
                    <ItemStyle CssClass="myGridItem"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="ysDept" HeaderText="预算部门" 
                    ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader" >
                    <HeaderStyle CssClass="myGridHeader"></HeaderStyle>
                    <ItemStyle CssClass="myGridItem"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField DataField="yskm" HeaderText="预算科目" 
                    ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader" >
                    <HeaderStyle CssClass="myGridHeader"></HeaderStyle>

                    <ItemStyle CssClass="myGridItem"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="预算金额" >
                    <HeaderStyle CssClass="myGridHeader"></HeaderStyle>
                    <ItemStyle CssClass="myGridItem"></ItemStyle>
                </asp:BoundField>
                <asp:BoundField HeaderText="剩余金额" >
                    <HeaderStyle CssClass="myGridHeader"></HeaderStyle>
                    <ItemStyle CssClass="myGridItem"></ItemStyle>
                </asp:BoundField>
                
                <asp:BoundField DataField="ysje" HeaderText="调整金额" >
                    <HeaderStyle CssClass="myGridHeader"></HeaderStyle>
                    <ItemStyle CssClass="myGridItem"></ItemStyle>
                </asp:BoundField>
            </Columns>
        </asp:GridView>
    </div>
        <div style="text-align: center">
        <input type="button" value="关闭" class="baseButton" onclick="javascript:window.close();" />
    </div>
    </form>
</body>
</html>
