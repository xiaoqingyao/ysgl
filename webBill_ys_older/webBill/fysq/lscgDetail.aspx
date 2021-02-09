<%@ Page Language="C#" AutoEventWireup="true" CodeFile="lscgDetail.aspx.cs" Inherits="webBill_fysq_lscgDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>报告申请单</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script language="javascript" type="text/javascript" src="js/Jscript.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script type="text/javascript">
        function edit(){
            $("#txtCgrq").datepicker();
            }
   
        $(function() {
            //审核
            
        
            $("#btn_ok").click(function() {
                var billcode = '<%=Request["cgbh"] %>';
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
                var billcode = '<%=Request["cgbh"] %>';
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
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" class="style1" width="100%">
            <tr>
                <td style="text-align: center; height: 26px;">
                    <strong><span style="font-size: 12pt">报告申请单</span></strong>
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable">
                        <tr>
                            <td class="tableBg" colspan="2">
                                申请日期
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtCgrq" runat="server"></asp:TextBox>
                            </td>
                            <td class="tableBg">
                                申请单号
                            </td>
                            <td colspan="3">
                                <asp:Label ID="lblCgbh" runat="server" Text="No.201109120001"></asp:Label>
                                <asp:Button ID="Button1" runat="server" Text="生成编号" CssClass="baseButton" OnClick="Button1_Click"
                                    Visible="False" />
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg" colspan="2">
                                <asp:Label ID="Label1" runat="server" Text="部门" Width="79px"></asp:Label>
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblDept" runat="server" Text="[000001]采购蛇皮但单位" Width="100%" Style="display: none"></asp:Label>
                                <asp:Label ID="lblxsDept" runat="server" Text="[000001]采购蛇皮但单位" Width="100%"></asp:Label>
                            </td>
                            <td class="tableBg">
                                申请类别
                            </td>
                            <td colspan="3">
                                <asp:DropDownList ID="ddl_cglb" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                报销单据
                            </td>
                            <td colspan="7">
                                <%-- <td>--%>
                                <asp:FileUpload ID="upLoadFiles" runat="server" Width="240px" />
                                <asp:Label ID="Lafilename" runat="server" Text="" Width="340px"></asp:Label>
                                <asp:HiddenField ID="hiddFileDz" runat="server" />
                                <asp:Button ID="btn_sc" runat="server" Text="上 传" CssClass="baseButton" OnClick="btnScdj_Click" />
                                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                <asp:Label ID="laFilexx" runat="server" Text="" ForeColor="Red"></asp:Label>
                                <%-- <input type="File" runat="server" id="upLoadFiles" style="width: 401px" class="baseButton" />
                                    <asp:Button ID="btnScdj" runat="server" CausesValidation="False" Text="上传单据" CssClass="baseButton"
                                        OnClick="btnScdj_Click" /><br />--%>
                                <div id="divBxdj" runat="server">
                                </div>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg" colspan="2">
                                &nbsp;&nbsp;
                                <asp:Label ID="Label2" runat="server" Text="申请内容"></asp:Label>
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtZynr" runat="server" Rows="2" TextMode="MultiLine" Width="681px"
                                    Height="82px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg" colspan="2">
                                申请说明
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtSm" runat="server" Rows="2" TextMode="MultiLine" Width="681px"
                                    Height="60px"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg" colspan="2">
                                承办人
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lblCbr" runat="server" Text="[000001]系统挂丽媛"></asp:Label>
                            </td>
                            <td class="tableBg">
                                预计费用
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtYjfy" runat="server" onblur="clearNoNum(this,'Y');"></asp:TextBox>元
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="tableBg" colspan="2">
                            </td>
                            <td colspan="2">
                            </td>
                            <td class="tableBg">
                                部门主管意见
                            </td>
                            <td>
                                <asp:TextBox ID="txtbmzgyj" runat="server" TextMode="MultiLine" Width="150px" ReadOnly="True"></asp:TextBox>
                            </td>
                            <td class="tableBg">
                                部门经理意见
                            </td>
                            <td>
                                <asp:TextBox ID="txtbmjlyj" runat="server" TextMode="MultiLine" Width="150px" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="tableBg" colspan="2">
                                综合管理部<br />
                                审查意见
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtzhglbyj" runat="server" Width="681px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="tableBg" colspan="2">
                                财务部门<br />
                                审查意见
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtcwbmyj" runat="server" Width="681px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="tableBg" colspan="2">
                                监审部门<br />
                                审查意见
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtjsbmyj" runat="server" Width="681px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="tableBg" colspan="2">
                                分管领导审批
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtfgldsp" runat="server" Width="681px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="tableBg" colspan="2">
                                总会计师审批
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtzkjssp" runat="server" Width="681px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                        <tr style="display: none;">
                            <td class="tableBg" colspan="2">
                                总经理审批
                            </td>
                            <td colspan="6">
                                <asp:TextBox ID="txtzjlsp" runat="server" Width="681px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: center; height: 37px;">
                    <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click" />
                    &nbsp;
                    <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />&nbsp;
                    <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />&nbsp;
                    <asp:Button ID="btn_fh" runat="server" Text="取 消" CssClass="baseButton" OnClick="btn_fh_Click" />
                </td>
            </tr>
            <tr style="display: none">
                <td colspan="6" style="height: 37px; text-align: center">
                    <asp:Label ID="lbl_BillCode" runat="server"></asp:Label>
                    <asp:Button ID="btn_print" runat="server" Text="打印" CssClass="baseButton" OnClick="btn_print_Click" />
                </td>
            </tr>
        </table>
        </form>
    </center>
</body>
</html>
