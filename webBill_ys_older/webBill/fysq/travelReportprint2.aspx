<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travelReportprint2.aspx.cs"
    Inherits="webBill_fysq_travelReportprint2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>出差报告单打印预览</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script type="text/javascript">
        function Print() {
            window.print();
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div>
        <table class="baseTable" width="90%">
            <tr>
                <td style="text-align: center; height: 26px;" colspan="6">
                    <div style="float: left; text-align: center; width: 93%">
                        <strong><span style="font-size: 12pt">出差报告单</span></strong></div>
                    <div style="float: right; width: auto;">
                        <a href="#" onclick="Print();">[打印]</a></div>
                </td>
            </tr>
            <tr>
                <td class="tableBg2" style="text-align: right; width: 150px;">
                    报告类别：
                </td>
                <td colspan="2">
                    <asp:Label runat="server" ID="ddlReportType"></asp:Label>
                </td>
                <td class="tableBg2" style="text-align: right">
                    报告编号：
                </td>
                <td colspan="2">
                    <asp:Label runat="server" ID="txtReportCode"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableBg2" style="text-align: right">
                    报告人：
                </td>
                <td colspan="2">
                    <asp:Label ID="lbeRepPersion" runat="server"></asp:Label>
                </td>
                <td class="tableBg2" style="text-align: right">
                    报告时间：
                </td>
                <td colspan="2">
                    <asp:Label ID="txtRepDate" runat="server"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableBg2" style="text-align: right">
                    所在部门负责人：
                </td>
                <td colspan="2">
                    <asp:Label runat="server" ID="txtdeptManager"></asp:Label>
                </td>
                <td class="tableBg2" style="text-align: right">
                    主管派遣单位（人）：
                </td>
                <td colspan="2">
                    <asp:Label runat="server" ID="txtsendDeptManager"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableBg2" style="text-align: right; width: 100px;">
                    出差报告特殊审批：
                </td>
                <td colspan="5">
                    <asp:Label runat="server" ID="txtspecialCheck"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td colspan="5">
                    <asp:Label runat="server" ID="txtContent" TextMode="MultiLine"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableBg2" style="text-align: right">
                    派遣单位意见：
                </td>
                <td colspan="5">
                    <asp:Label runat="server" ID="txtTitle"></asp:Label>
                </td>
            </tr>
            <tr id="tr1" runat="server">
                <td class="tableBg2" style="text-align: right">
                    部门经理审批：
                </td>
                <td colspan="5">
                </td>
            </tr>
            <tr id="tr2" runat="server">
                <td class="tableBg2" style="text-align: right">
                    分管领导审批：
                </td>
                <td colspan="5">
                </td>
            </tr>
            <tr id="tr3" runat="server">
                <td class="tableBg2" style="text-align: right">
                    总经理审批：
                </td>
                <td colspan="5">
                </td>
            </tr>
            <tr id="tr4" runat="server">
                <td class="tableBg2" style="text-align: right">
                    法审部审备案：
                </td>
                <td colspan="5">
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
