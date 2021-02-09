<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ysbh.aspx.cs" Inherits="webBill_xtsz_ysbh" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        function confirms() {
            var r = window.confirm("是否确定操作该部门的吗？");
            if (r != true) {
                return;
            }
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <div style="margin-top:5px; margin-left:5px;"></div>
        <asp:Panel ID="Panel1" runat="server" GroupingText="预算状态修改">
            <table style="width: 700px;">
                <tr>
                    <td style="text-align: right">部  门
                    </td>
                    <td>
                        <asp:DropDownList ID="drp_dept" runat="server" Width="200">
                            <asp:ListItem Value="">----全部----</asp:ListItem>
                        </asp:DropDownList>
                    </td>

                    <td style="text-align: right">预算类型
                    </td>
                    <td>
                        <asp:DropDownList ID="drp_yslx" runat="server" Width="200">
                            <asp:ListItem Value="ys">现金预算</asp:ListItem>
                            <asp:ListItem Value="xmys">项目预算</asp:ListItem>
                        </asp:DropDownList>
                    </td>

                    <%-- <td>
                    <asp:DropDownList ID="drp_xm" runat="server" Width="200">

                    </asp:DropDownList>
                </td>--%>
                    <td>
                        <asp:Button ID="btn_bh" runat="server" Text="审批驳回" OnClick="btn_bh_Click" CssClass="baseButton" />
                        <asp:Button ID="btn_tg" runat="server" Text="审批通过" OnClick="btn_tg_Click" CssClass="baseButton" />
                    </td>
                </tr>
            </table>
        </asp:Panel>
        <div style="height:20px"></div>
        <asp:Panel ID="Panel2" runat="server" GroupingText="决算单据状态修改">
              单据编号：<asp:TextBox ID="txtBillName" runat="server"></asp:TextBox>
             <asp:Button ID="btn_js_bohui" runat="server" Text="审批驳回" OnClick="btn_js_bohui_Click" CssClass="baseButton" />
            <asp:Button ID="btn_js_tongguo" runat="server" Text="审批通过" OnClick="btn_js_tongguo_Click" CssClass="baseButton" />
                       
        </asp:Panel>
    </form>
</body>
</html>
