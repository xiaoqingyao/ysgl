<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travelApplicationPrint.aspx.cs"
    Inherits="travelApplicationPrint" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>出差管理单打印预览</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script language="javascript" type="text/javascript" src="js/Jscript.js"></script>
    <script language="javascript" type="text/javascript">
        function Print() {
            window.print();
        }
    </script>
</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" class="style1" width="90%" border="0">
            <tr>
                <td style="text-align: center; height: 26px;" colspan="6">
                <div>
                    <div style="  float:left; text-align:center; width:93%"><strong><span style="font-size: 12pt">出差管理单</span></strong></div>
                    <div style=" float:right; width:auto;"><a href="#" onclick="Print();">[打印]</a></div>
                </div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    出差派遣部门：
                </td>
                <td colspan="2" style="width: 55%">
                    <asp:Label ID="lbeDept" runat="server" Text=""></asp:Label>
                </td>
                <td style="text-align: right">
                    申请单号：
                </td>
                <td colspan="2">
                    <asp:Label ID="lbeBillCode" runat="server" Text=""></asp:Label>
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center" colspan="6">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="myTable">
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                申请人：
                            </td>
                            <td>
                                <asp:Label ID="lbeAppPersion" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                申请日期：
                            </td>
                            <td>
                                <asp:Label ID="lbeAppDate" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                出差类型：
                            </td>
                            <td>
                                <asp:Label ID="lbeTravelType" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td  class="tableBg2" style="text-align: right">出差人：</td>
                            <td colspan="5">
                                <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    BorderWidth="1px" CellPadding="3" CssClass="myGrid" ItemStyle-HorizontalAlign="Center"
                                    PageSize="17" Width="100%" ShowFooter="True" ShowHeader="true">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:BoundColumn DataField="UserCode" HeaderText="编号">
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="UserName" HeaderText="姓名">
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="UserDept" HeaderText="所在单位">
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="UserPosition" HeaderText="职务">
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                            <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" HorizontalAlign="Right" />
                                        </asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差地址：
                            </td>
                            <td colspan="5" style="word-break:break-all;">
                                <asp:Label ID="lbeTravelAddress" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                预计时间：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="lbePlanDate" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差事由：
                            </td>
                            <td colspan="5"  style="word-break:break-all;">
                                <asp:Label ID="lbeReasion" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差日程安排及工作计划：
                            </td>
                            <td colspan="5"  style="word-break:break-all;">
                                <asp:Label ID="lbeWorkPlan" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right; word-break:break-all;">
                                出差费用预算：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="lbeFeePlan" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="width: 19%;text-align: right; word-break:break-all;">
                                申请乘坐交通工具：
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lbeTransport" runat="server" Text=""></asp:Label>
                            </td>
                            <td colspan="2" style="text-align: left">
                                &nbsp;&nbsp;&nbsp; 是否超过规定的标准：
                                 <asp:Label ID="IsOutStradard" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr1" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                派遣部门经理签字：
                            </td>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr2" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                部门经理签字：
                            </td>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr3" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                分管领导签字：
                            </td>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr4" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                总经理签字：
                            </td>
                            <td colspan="5">
                                &nbsp;
                            </td>
                        </tr>
                        <tr id="tr21" runat="server">
                            <td style="text-align: center; height: 26px;" colspan="6">
                                <strong><span style="font-size: 12pt">出差报告单</span></strong>
                            </td>
                        </tr>
                        <tr  id="tr22" runat="server">
                            <td style="text-align: right" class="tableBg2">
                                部门：
                            </td>
                            <td colspan="3" style="width: 55%">
                                <asp:Label ID="lbeDeptForReport" runat="server" Text=""></asp:Label>
                            </td>
                            <td style="text-align: right" class="tableBg2">
                                报告单号：
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lbeReportBillCode" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr  id="tr23" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                报告人：
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lbeRepPersion" runat="server" Text="" />
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                报告日期：
                            </td>
                            <td colspan="3">
                                <asp:Label ID="txtRepDate" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr  id="tr24" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                出差过程：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtTravelProcess" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr  id="tr25" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                工作过程：
                            </td>
                            <td colspan="5">
                                    <asp:Label ID="txtWorkProcess" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr26" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                完成效果：
                            </td>
                            <td colspan="5">
                             <asp:Label ID="txtRel" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr5" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                部门经理审批：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtBMManagerMind" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr6" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                分管领导审批：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtFGManagerMind" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr7" runat="server" >
                            <td class="tableBg2" style="text-align: right">
                                总经理审批：
                            </td>
                            <td colspan="5">
                                 <asp:Label ID="txtGeneralManagerMind" runat="server"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr8" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                法审部审备案：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtFSMind" runat="server"></asp:Label>
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
