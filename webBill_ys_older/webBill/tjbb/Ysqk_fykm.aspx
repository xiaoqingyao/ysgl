<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ysqk_fykm.aspx.cs" Inherits="webBill_tjbb_Ysqk_fykm" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>费用科目预算情况</title> <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="Text/javascript">
        function openDetail(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:Button ID="Button2" runat="server" Text="导出Excel" CssClass="baseButton" OnClick="Button2_Click" />
    </div>
    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
        CssClass="myGrid" PageSize="17" Width="100%" OnItemDataBound="myGrid_ItemDataBound">
        <Columns>
            <asp:BoundColumn DataField="ystype" HeaderText="预算类型">
                <HeaderStyle CssClass="hiddenbill" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="hiddenbill" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="billcode" HeaderText="单据编号">
                <HeaderStyle CssClass="hiddenbill" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="hiddenbill" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="bmbh" HeaderText="部门编号">
                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="bmmc" HeaderText="部门名称">
                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="fykm" HeaderText="费用科目">
                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="kmmc" HeaderText="科目名称">
                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="czr" HeaderText="操作人员">
                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="zcrq" HeaderText="操作日期" DataFormatString="{0:D}">
                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="gcbh" HeaderText="单据编号[查]">
                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="gcmc" HeaderText="预算过程">
                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="je" HeaderText="金额" DataFormatString="{0:F2}">
                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
            <asp:BoundColumn DataField="ystypename" HeaderText="预算类型">
                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
            </asp:BoundColumn>
        </Columns>
        <PagerStyle Visible="False" />
    </asp:DataGrid>
    </form>
</body>
</html>
