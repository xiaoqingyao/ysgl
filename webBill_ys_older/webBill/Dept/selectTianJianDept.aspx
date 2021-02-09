<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectTianJianDept.aspx.cs"
    Inherits="webBill_Dept_selectTianJianDept" %>
  

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>选择对应天健系统部门</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 27px">
                    部门名称：<asp:TextBox ID="txtDept" runat="server"></asp:TextBox>
                    <asp:Button ID="btn_sel" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_sel_Click" />
                    &nbsp; &nbsp; &nbsp;<asp:Button ID="btn_edit" runat="server" Text="确定" CssClass="baseButton"
                        OnClick="btn_edit_Click" />
                    <input type="button" class="baseButton" value="关 闭" onclick="javascript:window.close();" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" Width="610px" PageSize="4">
                        <Columns>
                            <asp:TemplateColumn HeaderText="选择">
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                    Width="38px" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="DeptCode" HeaderText="部门编号">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="DeptName" HeaderText="部门姓名">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
