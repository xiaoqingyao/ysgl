<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpecialRebatesAppDetails.aspx.cs"
    Inherits="SaleBill_Salepreass_SpecialRebatesAppDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特殊返利申请单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

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
                    alert("请先选择单据!");
                }
                else {
                    if (confirm("确定要审批该单据吗?")) {
                        $.post("../../webBill/MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "approve" }, OnApproveSuccess);
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
                        $.post("../../webBill/MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "disagree" }, OnApproveSuccess);
                    }
                }
            });
            $("#ddlTravelType").autocomplete({
                source: deptAll
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
        //        function opAddDetail() {
        //            var returnValue = window.showModalDialog('../select/userFrame.aspx?', 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:yes');
        //            if (returnValue == undefined || returnValue == "") { } else {
        //                $("#hdUerCode").val('');
        //                $("#hdUerCode").val(returnValue);
        //                document.getElementById("btnAdd_Server").click();
        //            }
        //        }
        //替换非数字
        function replaceNaNf(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }
        function replaceNaN(obj) {
            var objval = obj.value;
            if (objval.indexOf("-") == 0) {
                objval = objval.substr(1);
            }
            if (isNaN(objval)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            }
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

            if (type == "file") {
                document.getElementById("<%=txtFileUrl.ClientID %>").value = file;
                alert("上传成功！");
                $("#HiddenField1").val(file);

                alert($("#HiddenField1").val(fileName));
            }
            if (type == "file2") {
                document.getElementById("<%=TextBox3.ClientID %>").innerText = file;

                alert("上传成功！");
                $("#HiddenField2").val(file);
                $("Hidfileurlfj").val(fileName);

            }
        }
        function checkEdit() {



            var caramount = $("#txtWorkPlan").val();

            if (caramount.length < 1) {
                alert("车辆数不能为空！");
                $("#txtWorkPlan").focus();
                return false;
            }
            //            var varTransport = $("#txtTransport").val();

            //            if (varTransport.length < 1) {
            //                alert("正常返利不能为空！");
            //                $("#txtTransport").focus();
            //                return false;
            //            }
            //            var varflyy = $("#TextBox1").val();
            //            if (varflyy.length < 1) {
            //                alert("返利原因不能为空！");
            //                $("#TextBox1").focus();
            //                return false;
            //            }
            //            var varFeePlan = $("#txtFeePlan").val();
            //            if (varFeePlan.length < 1) {
            //                alert("底盘号不能为空！");
            //                $("#txtFeePlan").focus();
            //                return false;
            //            }
            //            var varendflyy = $("#TextBox2").val();
            //            if (varendflyy.length < 1) {
            //                alert("申请超出正常返利点数不能为空！");
            //                $("#TextBox2").focus();
            //                return false;
            //            }
            var varendtime = $("#txtendtime").val();
            if (varendtime.length < 1) {
                alert("有效时间截止日期不能为空！");
                $("#txtendtime").focus();
                return false;
            }
            else {
                return true;
            }
        }
        //选择资产明细
        function opAddDetail() {
            var openUrl = "AddSpecialRebatesBySaleBill.aspx?par=" + Math.random();
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:yes');
            if (returnValue != undefined && returnValue != "") {
                $("#hdSelectBySaleBillVal").val(returnValue);
                $("#btnAdd_Server").click();
            }
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="90%" border="0">
            <tr>
                <td style="text-align: center; height: 26px;" colspan="4">
                    <strong><span style="font-size: 12pt">特殊返利申请单</span></strong>
                </td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center" colspan="4">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="myTable">
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                申请日期：
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtAppDate" runat="server" Style="width: 96%"></asp:TextBox>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                报告单号：
                            </td>
                            <td colspan="2">
                                <asp:Label ID="lbeBillCode" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                申请部门：
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="ddlTravelType" runat="server" Style="width: 96%"></asp:TextBox>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                辆数：
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtWorkPlan" runat="server" Width="96%" onblur="checkisNaN(this);"
                                    onkeyup="replaceNaNf(this);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                此报告执行有效时间：
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtbgtime" runat="server" Style="width: 96%"></asp:TextBox>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                截止日期：
                            </td>
                            <td colspan="2">
                                <asp:TextBox ID="txtendtime" runat="server" Style="width: 96%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right" rowspan="4">
                                车辆明细：
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                底盘号：
                            </td>
                            <td>
                                <asp:TextBox ID="txtFeePlan" runat="server" Width="94%"></asp:TextBox>
                            </td>
                            <td style="text-align: right">
                                <asp:Button ID="Button1" runat="server" Text="显示订单号" CssClass="baseButton" OnClick="Button1_Click" />
                            </td>
                            <td>
                                <asp:Label ID="txtReasion" runat="server" Text=""></asp:Label>
                            </td>
                        </tr>
                        <tr id="tridmoney" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                正常返利：
                            </td>
                            <td>
                                <asp:TextBox ID="txtTransport" runat="server" Width="94%" onblur="checkisNaN(this);"
                                    onkeyup="replaceNaN(this);"></asp:TextBox>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                超出正常标准点数：
                            </td>
                            <td>
                                <asp:TextBox ID="TextBox2" runat="server" Width="96%" onblur="checkisNaN(this);"
                                    onkeyup="replaceNaN(this);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr id="tryy" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                申请返利原因：
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="TextBox1" runat="server" TextMode="MultiLine" Width="99%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td style="text-align: left" colspan="5">
                                <asp:Button ID="Button2" runat="server" Text="添 加" CssClass="baseButton" OnClick="Button2_Click" />
                                <asp:Button ID="btn_Del" runat="server" CssClass="baseButton" Text="删 除" OnClick="btn_Del_Click" />
                                <input type="button" runat="server" id="btn_AddBySaleBill" class="baseButton" value="通过销售单添加.."
                                    onclick="opAddDetail();" />
                                <div style="display: none;"><asp:Button ID="btnAdd_Server" runat="server" CssClass="baseButton" OnClick="ReBindItem" /></div>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                                    Width="100%">
                                    <Columns>
                                        <asp:TemplateField HeaderText="选择" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                                            <ItemTemplate>
                                                <asp:CheckBox ID="CheckBox2" runat="server" />
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="Note1" HeaderText="订单号" HeaderStyle-CssClass="myGridHeader" />
                                        <asp:BoundField DataField="TruckCode" HeaderText="车架号" HeaderStyle-CssClass="myGridHeader" />
                                        <asp:TemplateField HeaderText="正常返利" HeaderStyle-CssClass="myGridHeader">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtStandardSaleAmount" runat="server" onblur="checkisNaN(this);"
                                                    onkeyup="replaceNaN(this);" Text='<%#DataBinder.Eval(Container.DataItem,"StandardSaleAmount")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="超出返利" HeaderStyle-CssClass="myGridHeader">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtExceedStandardPoint" runat="server" onblur="checkisNaN(this);"
                                                    onkeyup="replaceNaN(this);" Text='<%#DataBinder.Eval(Container.DataItem,"ExceedStandardPoint")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="原因" HeaderStyle-CssClass="myGridHeader">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txtExplain" runat="server" onblur="checkisNaN(this);" onkeyup="replaceNaN(this);"
                                                    Text='<%#DataBinder.Eval(Container.DataItem,"Explain")%>'></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:BoundField DataField="StandardSaleAmount" HeaderText="正常返利" HeaderStyle-CssClass="hiddenbill"
                                            ItemStyle-CssClass="hiddenbill" />
                                        <asp:BoundField DataField="ExceedStandardPoint" HeaderText="超出返利" HeaderStyle-CssClass="hiddenbill"
                                            ItemStyle-CssClass="hiddenbill" />
                                        <asp:BoundField DataField="Explain" HeaderText="申请原因" HeaderStyle-CssClass="hiddenbill"
                                            ItemStyle-CssClass="hiddenbill" />
                                    </Columns>
                                </asp:GridView>
                            </td>
                        </tr>
                        <tr id="tr4" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                附件：
                            </td>
                            <td colspan="3">
                                <iframe id="Iframe2" name="addPicture" src="../Upload/Upload.aspx?Type=file2&UseName=false"
                                    runat="server" scrolling="no" height="30px" frameborder="0" width="340" style="border: 0px solid #f0f0f0;">
                                </iframe>
                            </td>
                            <td nowrap="nowrap" runat="server" id="idfj" colspan="2">
                                <asp:Label ID="TextBox3" runat="server" Text="" Width="100%"></asp:Label>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr id="tr1" runat="server" style="display: none">
                <td class="tableBg2" colspan="4" style="text-align: center">
                    审批意见：
                </td>
            </tr>
            <tr id="trrepeat" runat="server" style="display: none">
                <td colspan="4" class="tableBg2" style="width: 100%">
                    <asp:Repeater ID="Repeater1" runat="server">
                        <HeaderTemplate>
                            <table style="width: 100%">
                        </HeaderTemplate>
                        <ItemTemplate>
                            <tr>
                                <td style="text-align: right">
                                    审批人:
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="spr" runat="server" Text='<%# Bind("shenpiren")%>'></asp:Label>
                                </td>
                                <td style="text-align: right">
                                    审批意见:
                                </td>
                                <td style="text-align: left">
                                    <asp:Label ID="spyj" runat="server" Text='<%# Bind("shenpimind")%>'></asp:Label>
                                </td>
                            </tr>
                        </ItemTemplate>
                        <FooterTemplate>
                            </table>
                        </FooterTemplate>
                    </asp:Repeater>
                </td>
            </tr>
            <tr id="tr10" runat="server" style="display: none">
                <td class="tableBg2" style="text-align: right">
                    审批附件：
                </td>
                <td>
                    <iframe id="Iframe1" name="addPicture" src="../Upload/Upload.aspx?Type=file&UseName=false"
                        scrolling="no" height="30px" frameborder="0" width="90%" style="border: 0px solid #f0f0f0;">
                    </iframe>
                </td>
                <td>
                    <asp:Label ID="txtFileUrl" runat="server" Text=""></asp:Label>
                </td>
                <td nowrap="nowrap" runat="server" id="idspfj">
                    <asp:Label runat="server" ID="lblspfj" />
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: center; height: 37px;">
                    <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="return checkEdit();"
                        OnClick="btn_bc_Click" />&nbsp;
                    <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />&nbsp;
                    <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />&nbsp;
                    <asp:Button ID="btn_fh" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close();" />
                    <%--<asp:HiddenField ID="hdUerCode" runat="server" />--%>
                    <asp:HiddenField ID="HiddenField1" runat="server" />
                    <asp:HiddenField ID="HiddenField2" runat="server" />
                    <asp:HiddenField ID="Hidfileurlfj" runat="server" />
                    <%--<asp:HiddenField ID="hidcjh" runat="server" />--%>
                    <asp:HiddenField ID="hdSelectBySaleBillVal" runat="server" />
                </td>
            </tr>
        </table>
        </form>
    </center>
</body>
</html>
