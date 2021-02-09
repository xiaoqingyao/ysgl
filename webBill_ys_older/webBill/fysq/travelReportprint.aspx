<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travelReportprint.aspx.cs"
    Inherits="webBill_fysq_travelReportprint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>出差报告单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
    </style>

    <script type="text/javascript">
      function closeWindow() {
            window.returnValue = "";
            self.close();
        }
          function Print() {
            window.print();
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="90%" border="0">
            <tr>
                <td style="height: 26px; text-align: center" colspan="6">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="myTable">
                        <tr>
                            <td style="text-align: center; height: 26px;" colspan="6">
                                <div style="float: left; text-align: center; width: 93%">
                                    <strong><span style="font-size: 12pt">出差报告单</span></strong></div>
                                <div style="float: right; width: auto;">
                                    <a href="#" onclick="Print();">[打印]</a></div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                报告人：
                            </td>
                            <td colspan="2" style="text-align: left">
                                <asp:Label ID="lbeRepPersion" runat="server" Text="Label"></asp:Label>
                            </td>
                            <td style="text-align: right" class="tableBg2">
                                报告单号：
                            </td>
                            <td colspan="2"  style="text-align:left">
                                <asp:Label ID="lbeBillCode" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: right" class="tableBg2">
                                部门：
                            </td>
                            <td colspan="2" style="width: 55%; text-align: left">
                                <asp:Label ID="lbeDept" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                报告日期：
                            </td>
                            <td colspan="2"  style="text-align:left">
                                <asp:Label ID="txtRepDate" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right; width:150px">
                                出差过程：
                            </td>
                            <td colspan="5" style="text-align:left;word-break:break-all;">
                            
                                <asp:Label ID="txtTravelProcess" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right;width:150px">
                                工作过程：
                            </td>
                            <td colspan="5"  style="text-align:left;word-break:break-all;" >
                                <asp:Label ID="txtWorkProcess" runat="server" Text=""  ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right;width:150px">
                                完成效果（结果）：
                            </td>
                            <td colspan="5"  style="text-align:left;word-break:break-all;">
                                <asp:Label ID="txtRel" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr1" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                部门经理审批：
                            </td>
                            <td colspan="5" >
                                <asp:Label ID="txtBMManagerMind" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr2" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                分管领导审批：
                            </td>
                            <td colspan="5"  style="text-align:left">
                                <asp:Label ID="txtFGManagerMind" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr3" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                总经理审批：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtGeneralManagerMind" runat="server" Text="" ></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr4" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                法审部审备案：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtFSMind" runat="server" Text="" ></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="text-align: center; height: 37px;">
                                <input id="btn_fh" type="button" value="关 闭" class="baseButton" runat="server" onclick="closeWindow();" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        </form>
    </center>
</body>
</html>
