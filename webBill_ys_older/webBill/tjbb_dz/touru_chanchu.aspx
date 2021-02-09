<%@ Page Language="C#" AutoEventWireup="true" CodeFile="touru_chanchu.aspx.cs" Inherits="webBill_tjbb_dz_touru_chanchu" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/fixHc/superTables.js"></script>
    <link href="../Resources/jScript/fixHc/superTables_Default.css" rel="stylesheet" />
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="baseDiv">
            <div>
                <asp:Button ID="btn_fh" runat="server" Text="返回" CssClass="baseButton" OnClick="btn_fh_Click" />
                <asp:Button ID="btn_export_main" runat="server" Text="导出当前报表Excel" OnClick="btn_export_main_Click" CssClass="baseButton" />
                <asp:Button ID="btn_export" runat="server" Text="导出报表决算明细Excel" OnClick="btn_export_Click" CssClass="baseButton" />
            </div>
            <div style="text-align: center; font-family: 微软雅黑; font-size:large; font-weight: 800"><%=bbTime %>投入产出表</div>
            <%--<div  style="text-align:  right; font-family: 微软雅黑; font-size:medium; font-weight: 800; margin-right:20px;">统计时间：<%=bbTime %></div>--%>
            <div style="margin-top: 5px; margin-left: 5px;">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false">
                    <Columns>
                        <asp:BoundField HeaderText="部门名称" DataField="deptname" />
                        <asp:BoundField HeaderText="收入预算" DataField="srje_ys" DataFormatString="{0:N}" />
                        <asp:BoundField HeaderText="费用预算" DataField="fyje_ys" DataFormatString="{0:N}" />
                        <asp:BoundField HeaderText="预算投入产出比" DataField="trccb_ys" DataFormatString="{0:N}" />
                        <asp:BoundField HeaderText="收入决算" DataField="srje_js" DataFormatString="{0:N}" />
                        <asp:BoundField HeaderText="费用决算" DataField="fyje_js" DataFormatString="{0:N}" />
                        <asp:BoundField HeaderText="决算投入产出比" DataField="trccb_js" DataFormatString="{0:N}" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </form>
    <script type="text/javascript">
        fix("GridView1", $(window).height()-130, $(window).width(),0);
    </script>
</body>
</html>
