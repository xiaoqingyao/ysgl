<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travelApplicationOnlyPrint2.aspx.cs" Inherits="webBill_fysq_travelApplicationOnlyPrint2" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>出差管理单打印页预览</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
    <div style="margin: 0 auto; width: 90%">
        <table cellpadding="0" cellspacing="0" class="style1" width="100%" border="0">
            <tr>
                <td style="text-align: center; height: 46px;" colspan="6">
                    
                    <div style="float: left; text-align: center; width: 93%">
                        <strong><span style="font-size: 15pt">出差管理单</span></strong></div>
                    <div style="float: right; width: auto;">
                        <a href="#" onclick="Print();">[打印]</a></div>
                </td>
            </tr>
            <tr>
                <td style="text-align: right; width: 10px;">
                </td>
                <td colspan="2" style="width: 55%">
                    出差派遣部门：<asp:Label ID="lbesendDept" runat="server" Text=""></asp:Label>
                </td>
                <td style="text-align: right; width: 50px;">
                </td>
                <td colspan="2">
                    日期：
                    <label id="txtAppDate" runat="server" />
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center" colspan="6">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="baseTable">
                        <tr>
                            <td class="tableBg2" style="text-align: right; width: 150px;">
                                申请人：
                            </td>
                            <td style="text-align: center">
                                <asp:Label ID="lbeAppPersion" runat="server" Width="90%"></asp:Label>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                所属部门：
                            </td>
                            <td>
                                <asp:Label ID="lbeDept" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                职务：
                            </td>
                            <td>
                                <asp:Label runat="server" ID="lbeZhiWu"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差人：
                            </td>
                            <td colspan="5">
                                <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    BorderWidth="1px" CellPadding="3" CssClass="myGrid" ItemStyle-HorizontalAlign="Center"
                                    PageSize="17" Width="100%" ShowFooter="false" ShowHeader="true">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="选择" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill">
                                            <ItemTemplate>
                                                &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                            </ItemTemplate>
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="UserCode" HeaderText="编号"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="UserName" HeaderText="姓名"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="UserDept" HeaderText="所在单位"></asp:BoundColumn>
                                        <asp:BoundColumn DataField="UserPosition" HeaderText="职务"></asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差地点：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtTravelAddress" runat="server" Width="98%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                预计出差时间：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtPlanDate" runat="server" Width="98%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差事由：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtReasion" runat="server" Width="98%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2">
                                出差日程及工作安排：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtWorkPlan" runat="server" Width="98%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="text-align: center; font-size: 13pt;">
                                出差费用预算
                            </td>
                        </tr>
                        <tr style="text-align: center">
                            <td class="tableBg2">
                                交通费
                            </td>
                            <td class="tableBg2">
                                住宿费
                            </td>
                            <td class="tableBg2">
                                业务招待费
                            </td>
                            <td class="tableBg2">
                                会议（培训）费
                            </td>
                            <td class="tableBg2">
                                印刷费
                            </td>
                            <td class="tableBg2">
                                其它等
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center">
                                <label id="jiaotongfei" runat="server" style="width: 90%" />
                            </td>
                            <td style="text-align: center">
                                <label id="zhusufei" runat="server" style="width: 90%" />
                            </td>
                            <td style="text-align: center">
                                <label id="yewuzhaodaifei" runat="server" style="width: 90%" />
                            </td>
                            <td style="text-align: center">
                                <label id="huiyifei" runat="server" style="width: 90%" />
                            </td>
                            <td style="text-align: center">
                                <label id="yinshuafei" runat="server" style="width: 90%" />
                            </td>
                            <td style="text-align: center">
                                <label id="qitafei" runat="server" style="width: 90%" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                申请乘坐交通工具：
                            </td>
                            <td colspan="3">
                                <asp:Label ID="txtTransport" runat="server" Width="100%"></asp:Label>
                            </td>
                            <td colspan="2" style="text-align: left">
                                &nbsp;&nbsp;&nbsp; 是否超过规定的标准：
                                <asp:RadioButton ID="rdbYes" runat="server" GroupName="a" Text="是" />
                                <asp:RadioButton ID="rdbNo" runat="server" Checked="true" GroupName="a" Text="否" />
                            </td>
                        </tr>
                        <tr id="tr1" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                派遣部门经理签字：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtPQBMManagerMind" runat="server" TextMode="MultiLine" Width="98%"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr2" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                部门经理签字：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtBMManagerMind" runat="server" TextMode="MultiLine" Width="98%"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr3" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                分管领导签字：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtFGManagerMind" runat="server" TextMode="MultiLine" Width="98%"></asp:Label>
                            </td>
                        </tr>
                        <tr id="tr4" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                总经理签字：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtGeneralManagerMind" runat="server" TextMode="MultiLine" Width="98%"></asp:Label>
                            </td>
                        </tr>
                         <tr runat="server" id="tr5">
                            <td class="tableBg2" style="text-align: right">
                                法审部审备案：
                            </td>
                            <td colspan="5">
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
