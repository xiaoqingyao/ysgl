<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selctLdUser.aspx.cs" Inherits="webBill_Dept_selctZgUser" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>分管领导设置</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                &nbsp; &nbsp; 角色
                <asp:DropDownList ID="ddl_group" runat="server" Width="157px" 
                    AutoPostBack="true" onselectedindexchanged="ddl_group_SelectedIndexChanged" >
                </asp:DropDownList>
                &nbsp;<asp:TextBox ID="txb_where" runat="server"></asp:TextBox>
                <asp:Button ID="btn_sel" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_sel_Click" />
                 &nbsp; &nbsp; &nbsp;<asp:Button ID="btn_edit" runat="server" Text="保存设置" CssClass="baseButton"
                    OnClick="btn_edit_Click" />
                <asp:Button ID="btn_del" runat="server" Text="关 闭" CssClass="baseButton" OnClick="btn_del_Click" />
               
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
                        <asp:BoundColumn DataField="usercode" HeaderText="人员编号">
                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="username" HeaderText="人员姓名">
                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="usergroup" HeaderText="角色">
                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="userstatus" HeaderText="人员状态">
                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                        </asp:BoundColumn>
                        <asp:BoundColumn DataField="userdept" HeaderText="所在部门">
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
    </form>
</body>
</html>
