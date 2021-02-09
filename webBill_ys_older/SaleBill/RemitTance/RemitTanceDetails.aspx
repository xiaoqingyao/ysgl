<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RemitTanceDetails.aspx.cs"
    Inherits="SaleBill_RemitTance_RemitTanceDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>车款上缴明细</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery1.32.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/fysq/js/JScript.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function() {
            //审核
            $("#btn_ok").click(function() {
                var billcode = '<%=Request["Code"] %>';
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据！");
                }
                else {
                    if (confirm("确定要审批该单据吗?")) {
                        $.post("../../webBill/MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "approve" }, OnApproveSuccess);
                    }
                }
            });
             //部门
               $("#txtdeptname").autocomplete({
                source: availableTagsdt,
                select: function(event, ui) {
                    var deptCode = ui.item.value;
                 
                }
            });
            //否决单据
            $("#btn_cancel").click(function() {
                var billcode = '<%=Request["Code"] %>';
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据！");
                }
                else {
                    if (confirm("确定要否决该单据吗?")) {
                        $.post("../../webBill/MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "disagree" }, OnApproveSuccess);
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
        function opAddDetail() {
            var returnValue = window.showModalDialog('../select/userFrame.aspx?', 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:yes');
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
        //非空验证
        function checkisNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("不能为空！");
                obj.focus();
                return false;
            };
        }
        function CloseWithParam(file, fileName, type) {

          
            if (type == "file2") {
                document.getElementById("<%=TextBox3.ClientID %>").innerText = file;

                alert("上传成功！");
                $("#HiddenField2").val(file);
                $("Hidfileurlfj").val(fileName);

            }
        }
        function checkEdit() {
         
            var varorderCode = $("#txtorderCode").val();

//            if (varorderCode.length < 1) {
//                alert("必须填写订单号！");
//                $("#txtorderCode").focus();
//                return false;
//            }

            var caramount = $("#txtremitnumber").val();

            if (caramount.length < 1) {
                alert("汇款辆数！");
                $("#txtremitnumber").focus();
                return false;
            }
        
//            var varFeePlan = $("#txtTruckCode").val();
//            if (varFeePlan.length < 1) {
//                alert("底盘号不能为空！");
//                $("#txtTruckCode").focus();
//                return false;
//            }
            var varmoney=$("#txtmoney").val();
            if(varmoney.length<1){
                alert("回款金额不能空！");
                $("#txtmoney").focus();
                return false;
            }
            var varremitancetype=$("#txtremitancetype").val();
            if(varremitancetype.length<1){
                alert("请填写回款形式！");
                $("#txtremitancetype").focus();
                return false;
            }
            var varordercodedate=$("#txtordercodedate").val();
            if(varordercodedate.length<1){
                alert("制单日期不能为空！");
                $("#txtordercodedate").focus();
                return false;
            }
            else {
                return true;
            }
        }
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:1050px;status:no;scroll:yes');
            if (returnValue == "1") {
                location.replace(location.href);
            }
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="90%" border="0">
            <tr>
                <td style="text-align: center; height: 26px;">
                    <strong><span style="font-size: 12pt">车款上缴明细</span></strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="myTable">
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                回款日期：
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppDate" runat="server" Width="96%" onfocus="setday(this);"></asp:TextBox>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                制单日期：
                            </td>
                            <td>
                                <asp:TextBox ID="txtordercodedate" runat="server" Width="96%" onfocus="setday(this);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                申请部门：
                            </td>
                            <td>
                                <asp:TextBox ID="txtdeptname" runat="server" Width="96%"></asp:TextBox>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                回款金额（万元）：
                            </td>
                            <td>
                                <asp:TextBox ID="txtmoney" runat="server" onblur="checkisNaN(this);" onkeyup="replaceNaN(this);"
                                    Width="96%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                回款辆数：
                            </td>
                            <td>
                                <asp:TextBox ID="txtremitnumber" runat="server" onblur="checkisNaN(this);" onkeyup="replaceNaN(this);"
                                    Width="96%"></asp:TextBox>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                汇款形式：
                            </td>
                            <td>
                                <asp:DropDownList ID="txtremitancetype" runat="server" Width="96%">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr id="tr4" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                回款用途：
                            </td>
                            <td>
                                <asp:DropDownList ID="txtremitanceuse" runat="server" Width="98.5%">
                                    <asp:ListItem Value="订金">订金</asp:ListItem>
                                    <asp:ListItem Value="车款">车款</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td colspan="2"></td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                底盘号：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtTruckCode" runat="server" Width="78%"></asp:TextBox>
                                <asp:Button ID="Button1" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button1_Click" />
                            </td>
                            <td>
                                订单号：
                                <asp:TextBox ID="txtorderCode" runat="server" Width="72%"></asp:TextBox>
                            </td>
                            <td>
                                经销商：
                                <asp:TextBox ID="txtjxs" runat="server" Width="72%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                车辆明细：
                            </td>
                            <td colspan="3">
                                <table width="100%">
                                    <tr>
                                        <td style="text-align: left">
                                            <asp:Button ID="Button2" runat="server" Text="添 加" CssClass="baseButton" OnClick="Button2_Click" />
                                            <asp:Button ID="btn_Del" runat="server" CssClass="baseButton" Text="删 除" OnClick="btn_Del_Click" />
                                            <asp:Button ID="btn_DuiYing" runat="server" CssClass="baseButton" Text="上缴明细" OnClick="btn_DuiYing_Click" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                                                Width="100%">
                                                <Columns>
                                                    <asp:TemplateField HeaderText="选择" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                                                        <ItemTemplate>
                                                            <asp:CheckBox ID="CheckBox2" runat="server" />
                                                        </ItemTemplate>
                                                    </asp:TemplateField>
                                                    <asp:BoundField DataField="OrderCode" HeaderText="订单号" HeaderStyle-CssClass="myGridHeader" />
                                                    <asp:BoundField DataField="TruckCode" HeaderText="车架号" HeaderStyle-CssClass="myGridHeader" />
                                                    <asp:BoundField DataField="NOTE1" HeaderText="经销商" HeaderStyle-CssClass="myGridHeader" />
                                                </Columns>
                                            </asp:GridView>
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right" runat="server" id="tdfj">
                                附件：
                            </td>
                            <td runat="server" id="tdiframe" colspan="3">
                                <iframe id="Iframe2" name="addPicture" src="../Upload/Upload.aspx?Type=file2&UseName=false"
                                    width="98%" runat="server" scrolling="no" height="30px" frameborder="0" style="border: 0px solid #f0f0f0;">
                                </iframe>
                                <asp:Label ID="TextBox3" runat="server" Text="" Width="100%"></asp:Label>
                                <asp:Label runat="server" ID="lbfj" />
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td style="text-align: center; height: 37px;">
                    <div style="width: 50px;">
                    </div>
                    <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="return checkEdit();"
                        OnClick="btn_bc_Click" />&nbsp;
                    <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />&nbsp;
                    <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />&nbsp;
                    <asp:Button ID="btn_fh" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close();" />
                    <asp:HiddenField ID="hdUerCode" runat="server" />
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:HiddenField ID="Hidfileurlfj" runat="server" />
                    <asp:HiddenField ID="hidcjh" runat="server" />
                </td>
            </tr>
        </table>
        </form>
    </center>
</body>
</html>
