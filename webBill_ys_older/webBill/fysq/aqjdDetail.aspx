<%@ Page Language="C#" AutoEventWireup="true" CodeFile="aqjdDetail.aspx.cs" Inherits="webBill_fysq_aqjdDetail" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>请假单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
    
    $(function(){
    
    $("#txtAppDate").focus(function(){
    setday(this);
    });
     
    $("#txtLoanDateFrm").focus(function(){
    setday(this);
    });
     
    $("#txtLoanDateTo").focus(function(){
    setday(this);
    });
    });
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
       
        //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }
        function checkEdit() {
            var txtDays = $("#txtDays").val();
            if (isNaN(txtDays)) {
                alert("请假天数必须用阿拉伯数字表示！");
                return false;
            } else if (txtDays.length < 1) {
                alert("必须填请假天数！");
                return false;
            } else {
                return true;
            }
        }
        
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form2" runat="server">
        <table cellpadding="0" cellspacing="0" class="style1" width="90%" border="0">
            <tr>
                <td style="text-align: center; height: 26px;" colspan="6">
                    <strong><span style="font-size: 12pt">请假单</span></strong>
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
                            <td  style="text-align: right" >
                                申请日期：
                            </td>
                            <td>
                            
                                <input id="txtAppDate" runat="server" type="text"  />
                            </td>
                             <td colspan="3">
                                    <asp:Label ID="lblCgbh" runat="server" Text="No.201109120001"></asp:Label>
                                    <asp:Button ID="Button1" runat="server" Text="生成编号" CssClass="baseButton" OnClick="Button1_Click"
                                        Visible="False" /></td>
                    </tr>
                        <tr>
                           
                            <td class="tableBg2" style="text-align: right">
                            请假时间：
                              </td>
                            <td  style="text-align: right" >
                                请假起始日期：
                             </td>
                            <td>
                                <asp:TextBox ID="txtLoanDateFrm" runat="server" Width="120px"></asp:TextBox>
                            </td>
                            <td>
                                结束日期：
                            </td>
                            <td>
                                <asp:TextBox ID="txtLoanDateTo" runat="server" Width="120px"></asp:TextBox>
                            </td>
                             <td>
                               
                                <asp:TextBox ID="txtDays" runat="server"></asp:TextBox>天
                            </td>
                        </tr>
                    
                        
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                               原因：
                            </td>
                            <td colspan="5">
                                <asp:TextBox  ID="txtReason" runat="server" TextMode="MultiLine" Height="150px"
                                    Width="99%"></asp:TextBox>
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
