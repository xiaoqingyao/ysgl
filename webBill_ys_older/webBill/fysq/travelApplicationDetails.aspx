<%@ Page Language="C#" AutoEventWireup="true" CodeFile="travelApplicationDetails.aspx.cs"
    Inherits="travelApplicationDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>出差管理单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="js/Jscript.js"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

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
            //弹出选择出差报告单
            //            $("#addBill").click(function() {
            //                var returnValue = window.showModalDialog('../select/selectTravelRepBill.aspx?', 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:yes');
            //                if (returnValue != undefined && returnValue != '') {
            //                    $("#txtTravelReport").val(returnValue);
            //                }
            //            });
            //报告单详细信息
            $("#BillView").click(function() {
                var billCode = $("#txtTravelReport").text();
                if (billCode == '' || billCode == undefined) {
                    return;
                }
                window.showModalDialog("travelReportDetail.aspx?Ctrl=View&Code=" + billCode, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:yes');
            });
            //申请人输入时
            $("#lbeAppPersion").autocomplete({
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
        });
        function OnApproveSuccess(data, status) {
            if (data > 0 && status == "success") {
                alert("操作成功！");
                self.close();
            } else {
                alert("审批失败！");
            }
        }
        function opAddDetail() {
            var returnValue = window.showModalDialog('../select/userFrame.aspx?Flg=All', 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:yes');
            if (returnValue == undefined || returnValue == "") { } else {
                $("#hdUerCode").val('');
                $("#hdUerCode").val(returnValue);
                document.getElementById("btnAdd_Server").click();
            }
        }
        //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }
        function checkEdit() {
            var needAmount = $("#txtFeePlan").val();
            if (isNaN(needAmount)) {
                alert("出差费用预算必须用阿拉伯数字表示！");
                return false;
            } else if (needAmount.length < 1) {
                alert("必须填写出差费用预算数量！");
                return false;
            } else {
                return true;
            }
        }
        
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" class="style1" width="90%" border="0">
            <tr>
                <td style="text-align: center; height: 26px;" colspan="6">
                    <strong><span style="font-size: 12pt">出差管理单</span></strong>
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
                                <asp:TextBox ID="lbeAppPersion" runat="server"></asp:TextBox>
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
                                <input type="button" runat="server" id="btn_insert" class="baseButton" value="增 加"
                                    onclick="opAddDetail();" />
                                <div style="display: none;">
                                    <asp:Button ID="btnAdd_Server" runat="server" Text="" OnClick="btnAdd_Server_Click" /></div>
                                <asp:Button ID="btn_Del" runat="server" CssClass="baseButton" OnClick="btn_Del_Click"
                                    Text="删 除" />
                                <span style="color: Red">【友情提示】：单击增加添加出差人员明细。</span>
                                <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                    BorderWidth="1px" CellPadding="3" CssClass="myGrid" ItemStyle-HorizontalAlign="Center"
                                    PageSize="17" Width="100%" ShowFooter="True" ShowHeader="true">
                                    <ItemStyle HorizontalAlign="Center" />
                                    <Columns>
                                        <asp:TemplateColumn HeaderText="选择">
                                            <ItemTemplate>
                                                &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Width="38px"
                                                Wrap="False" />
                                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                        </asp:TemplateColumn>
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
                                            <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                        </asp:BoundColumn>
                                    </Columns>
                                </asp:DataGrid>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差地址：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtTravelAddress" runat="server" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                预计时间：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtPlanDate" runat="server" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差事由：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtReasion" runat="server"  Height="150px" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差日程安排及工作计划：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtWorkPlan" runat="server" TextMode="MultiLine" Height="150px"
                                    Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                出差费用预算：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtFeePlan" runat="server" onblur="replaceNaN(this);" onkeyup="replaceNaN(this);"
                                    Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                申请乘坐交通工具：
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtTransport" runat="server" TextMode="MultiLine" Width="100%"></asp:TextBox>
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
                                <asp:TextBox ID="txtPQBMManagerMind" runat="server" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="tr2" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                部门经理签字：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtBMManagerMind" runat="server" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="tr3" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                分管领导签字：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtFGManagerMind" runat="server" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="tr4" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                总经理签字：
                            </td>
                            <td colspan="5">
                                <asp:TextBox ID="txtGeneralManagerMind" runat="server" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="trAddBill" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                出差报告单：
                            </td>
                            <td colspan="5">
                                <input type="button" id="addBill" value="…" style="display: none" />
                                <%-- <input type="button" id="BillView" value="报告单详细信息" />--%>
                                <a href="#" id="BillView" style="color: Blue; border-bottom-width: 1px;">
                                    <asp:Label ID="txtTravelReport" runat="server" Text="Label"></asp:Label>
                                </a>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: center; height: 37px;">
                    <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click"
                        OnClientClick="return checkEdit();" />&nbsp;
                    <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />&nbsp;
                    <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />&nbsp;
                    <asp:Button ID="btn_fh" runat="server" Text="关 闭" CssClass="baseButton" OnClick="btn_fh_Click" />
                    <asp:HiddenField ID="hdUerCode" runat="server" />
                </td>
            </tr>
        </table>
        </form>
    </center>
</body>
</html>
