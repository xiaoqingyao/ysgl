<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travelReportDetail.aspx.cs"
    Inherits="webBill_fysq_travelReportDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
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


   
    <script language="javascript" type="text/javascript">
        $(function() {
            //审核
            $("#btn_ok").click(function() {
                var billcode = '<%=Request["Code"] %>';
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                }
                else {
                    if (confirm("确定要审批该单据吗?")) {
                        $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "approve" }, OnApproveSuccess);
                    }
                }
            });
            
            $("#lbeRepPersion").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function(data, status) {
                        if (status == "success") {
                            $("#lbeDept").text(data);
                        }
                        else {
                            alert("获取部门失败");
                        }
                    });
                }
            });
            //否决单据
            $("#btn_cancel").click(function() {
                var billcode = '<%=Request["Code"] %>';
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                }
                else {
                    if (confirm("确定要否决该单据吗?")) {
                        $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "disagree" }, OnApproveSuccess);
                    }
                }
            });
        });
        function OnApproveSuccess(data, status) {
            if (data > 0 && status == "success") {
                alert("操作成功！");
                self.close();
            } else {
                alert("审批失败！");
            }
        }
        function checkEdit() {
            if ($("#txtTravelProcess").val() == "") {
                alert("请填写出差过程！");
                return false;
            } else if ($("#txtWorkProcess").val() == "") {
                alert("请填写工作过程！");
                return false;
            } else if ($("#txtRel").val() == "") {
                alert("请填写工作结果！");
                return false;
            } else if ($("#txtRepDate").val() == "") {
                alert("请填写报告时间！");
                return false;
            } else {
                return true;
            }
        }
        function closeWindow() {
            window.returnValue = "";
            self.close();
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" class="style1" width="90%" border="0">
            <tr>
                <td style="text-align: center; height: 26px;" colspan="6">
                    <strong><span style="font-size: 12pt">出差申请单</span></strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: right">
                    出差派遣部门：
                </td>
                <td colspan="2" style="width: 55%; text-align:left">
                    <asp:Label ID="lbeAppDept" runat="server" Text=""></asp:Label>
                </td>
                <td style="text-align: right">
                    申请单号：
                </td>
                <td colspan="2" style="text-align:left">
                    <asp:Label ID="lbeAppCode" runat="server" Text=""></asp:Label>
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
                                <input id="txtAppDate" runat="server" type="text" />
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                出差类型：
                            </td>
                            <td>
                                <asp:DropDownList ID="ddlTravelType" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差人：
                            </td>
                            <td colspan="5">
                                <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    BorderWidth="1px" CellPadding="3" CssClass="myGrid" ItemStyle-HorizontalAlign="Center"
                                    PageSize="17" Width="100%"  ShowHeader="true">
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="选择">
                                            <ItemTemplate>
                                                &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Width="38px" Height="10px"
                                                Wrap="False" />
                                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"  Height="10px"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                        </asp:TemplateColumn>
                                        <asp:BoundColumn DataField="UserCode" HeaderText="编号">
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Height="10px"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"  Height="10px"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="UserName" HeaderText="姓名">
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Height="10px"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False" Height="10px"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="UserDept" HeaderText="所在单位">
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Height="10px"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"  Height="10px"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="UserPosition" HeaderText="职务">
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Height="10px"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                            <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"  Height="10px"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" HorizontalAlign="Right" />
                                        </asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right;">
                                出差地址：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtTravelAddress" runat="server" Text="" Width="99%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                预计时间：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtPlanDate" runat="server" Text="" Width="99%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差事由：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtReasion" runat="server" Text="" Width="99%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                工作计划：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtWorkPlan" runat="server" Text="" Width="99%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                费用预算：
                            </td>
                            <td colspan="5">
                                <asp:Label ID="txtFeePlan" runat="server" Text="" Width="99%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                申请交通工具：
                            </td>
                            <td colspan="2">
                                <asp:Label ID="txtTransport" runat="server" Text="" Width="99%"></asp:Label>
                            </td>
                            <td colspan="3" style="text-align: left">
                                &nbsp;是否超过规定的标准：
                                <asp:RadioButton ID="rdbYes" runat="server" GroupName="a" Text="是" />
                                <asp:RadioButton ID="rdbNo" runat="server" Checked="true" GroupName="a" Text="否" />
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: center; height: 26px;" colspan="6">
                                <strong><span style="font-size: 12pt">出差报告单</span></strong>
                            </td>
                        </tr>
                        <tr>
                        <td class="tableBg2" style="text-align: right">
                                报告人：
                            </td>
                            <td colspan="3" style="text-align:left">
                        
                                <asp:TextBox ID="lbeRepPersion" runat="server"></asp:TextBox>
                             
                            </td>
                           
                            <td style="text-align: right" class="tableBg2">
                                报告单号：
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lbeBillCode" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                             <td style="text-align: right" class="tableBg2">
                                部门：
                            </td>
                            <td colspan="3" style="width: 55%; text-align:left">
                                <asp:Label ID="lbeDept" runat="server" Text=""></asp:Label>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                报告日期：
                            </td>
                            <td colspan="3">
                                <input id="txtRepDate" runat="server" type="text" onfocus="setday(this);" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差过程：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtTravelProcess" runat="server" TextMode="MultiLine" Height="60px"
                                    Width="99%"></asp:TextBox> 
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                工作过程：
                            </td>
                            <td colspan="5">
                                 <asp:TextBox ID="txtWorkProcess" runat="server" TextMode="MultiLine" Height="100px"
                                    Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                完成效果（结果）：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtRel" runat="server" TextMode="MultiLine" Height="60px"
                                    Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="tr1" runat="server" style="display:none">
                            <td class="tableBg2" style="text-align: right">
                                部门经理审批：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtBMManagerMind" runat="server" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="tr2" runat="server" style="display:none">
                            <td class="tableBg2" style="text-align: right">
                                分管领导审批：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtFGManagerMind" runat="server" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="tr3" runat="server" style="display:none">
                            <td class="tableBg2" style="text-align: right">
                                总经理审批：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtGeneralManagerMind" runat="server" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="tr4" runat="server" style="display:none">
                            <td class="tableBg2" style="text-align: right">
                                法审部审备案：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtFSMind" runat="server" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" style="text-align: center; height: 37px;">
                                <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click"
                                    OnClientClick="return checkEdit();" />&nbsp;
                                <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />&nbsp;
                                <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />&nbsp;
                                <input id="btn_fh" type="button" value="关 闭" class="baseButton" runat="server" onclick="closeWindow();" />
                                <asp:HiddenField ID="hdUerCode" runat="server" />
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
