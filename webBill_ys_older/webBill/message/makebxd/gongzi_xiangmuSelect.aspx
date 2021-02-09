<%@ Page Language="C#" AutoEventWireup="true" CodeFile="gongzi_xiangmuSelect.aspx.cs" Inherits="webBill_makebxd_gongzi_xiangmuSelect" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>工资</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript">
    function closeW()
    {
    self.close();
    }
    function closeParent(names)
    {
    window.returnValue=names; 
      window.close();  
    }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                &nbsp; &nbsp;<asp:Button ID="btn_edit" runat="server" Text="保存设置" CssClass="baseButton"
                    OnClick="btn_edit_Click" />
                &nbsp;<input id="Button1" type="button" value="关 闭" class="baseButton" onclick="closeW()" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                    CssClass="myGrid" Width="610px" PageSize="4" 
                    onitemdatabound="myGrid_ItemDataBound">
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
                        <asp:BoundColumn DataField="gzmxname" HeaderText="工资明细项目">
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

