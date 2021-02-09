<%@ Page Language="C#" AutoEventWireup="true" CodeFile="weicaiyaopin_ckd.aspx.cs"
    Inherits="webBill_makebxd_weicaiyaopin_ckd" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>卫材、药品出库单生成报销单</title>
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
</head>
<body>
    <form id="form1" runat="server">
    <div class="baseDiv">
        <table>
            <tr>
                <td>
                    单据日期从：
                </td>
                <td>
                    <asp:TextBox ID="txtDateF" runat="server"></asp:TextBox>
                </td>
                <td>
                    到：
                </td>
                <td>
                    <asp:TextBox ID="txtDateT" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnSelect" runat="server" Text="查询" OnClick="btnSelect_OnClick" CssClass="baseButton" />
                    <asp:Button ID="btnMakBxd" runat="server" Text="制单" OnClick="btnMakBxd_OnClick" CssClass="baseButton" />
                </td>
                <td>
                    <input type="button" class="baseButton" value="设置对应关系" onclick="javascript:window.location.href='../yskm/gdzc_yskm_yongyou.aspx';" />
                </td>
            </tr>
            <tr>
                <td>
                </td>
            </tr>
        </table>
        <asp:GridView ID="GridView1" runat="server" CssClass="myGrid" EnableViewState="false">
        </asp:GridView>
    </div>
    </form>
</body>

<script type="text/javascript">
    $(function() {
        $("#txtDateF").datepicker();
        $("#txtDateT").datepicker();
    });
</script>

</html>
