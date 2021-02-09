<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RemitTancePrint.aspx.cs"
    Inherits="SaleBill_RemitTance_RemitTancePrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
     <script language="javascript" type="text/javascript">
        function Print() {
            window.print();
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" class="baseTable">
      
        <tr>
             <td style="text-align: center; height: 26px;" colspan="10">
                <strong><span style="font-size: 12pt">车款上缴明细表</span></strong>
                 <div style="float: right; width: auto;">
                    <a href="#" onclick="Print();">[打印]</a></div>
            </td>
        </tr>
        <tr>
            <td style="text-align: center" class="tableBg2">
                缴款单位
            </td>
            <td style="text-align: center" class="tableBg2">
                订单号
            </td>
            <td style="text-align: center" class="tableBg2">
                底盘号
            </td>
            <td style="text-align: center" class="tableBg2">
                经销商
            </td>
            <td style="text-align: center" class="tableBg2">
                回款辆数
            </td>
            <td style="text-align: center" class="tableBg2">
                汇款日期
            </td>
            <td style="text-align: center" class="tableBg2">
                回款形式
            </td>
            <td style="text-align: center" class="tableBg2">
                回款金额（万元）
            </td>
            <td style="text-align: center" class="tableBg2">
                回款用途
            </td>
            <td style="text-align: center" class="tableBg2">
                附件
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <asp:Label ID="lbldept" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblodercode" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lbltruckcode" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lbljxs" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblremitnumber" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblremdata" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblremtype" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblremitmoney" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblremituser" runat="server" Text="Label"></asp:Label>
            </td>
            <td style="text-align: center" class="tableBg2">
                <asp:Label ID="lblremitfj" runat="server" Text=""></asp:Label>
            </td>
        </tr>
    </table>
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td colspan="9">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="9">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="9">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align: right; font-size: 14px" colspan="10">
                缴款单位：（盖章）
                <asp:Label ID="lbljidw" runat="server" Text=" "></asp:Label>
            </td>
        </tr>
        <tr>
            <td colspan="9">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align: right; font-size: 14px" colspan="10">
               代缴人：（签字）
                <asp:Label ID="lbldjr" runat="server" Text=" "></asp:Label>
                &nbsp; &nbsp; &nbsp; &nbsp;&nbsp;
            </td>
        </tr>
        <tr>
            <td colspan="9">
                &nbsp;
            </td>
        </tr>
        <tr>
            <td style="text-align: right; font-size: 14px" colspan="10">
                日 &nbsp;期：
                <asp:Label ID="lblordertime" runat="server" Text=" "></asp:Label>
                 &nbsp; &nbsp; &nbsp;
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
