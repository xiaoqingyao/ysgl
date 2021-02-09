<%@ Page Language="C#" AutoEventWireup="true" CodeFile="FundBorrowDetail.aspx.cs"
    Inherits="SaleBill_BorrowMoney_FundBorrowDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>借款单据</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8"/>
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js"
        type="text/javascript" charset="UTF-8"></script>

    <script language="javascript" type="text/javascript">
        $(function () {
            $("#txtaddtime").datepicker();
            $("#txtjksj").datepicker();

            //审核
            $("#btn_ok").click(function () {
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
                select: function (event, ui) {
                    var deptCode = ui.item.value;

                }
            });

            //人员
            $("#txtloanName").autocomplete({
                source: avaiusernamedt,
                select: function (event, ui) {
                    var rybh = ui.item.value;
                    $.post("../../webBill/MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function (data, status) {
                        if (status == "success") {
                            $("#HiddenField1").val(data);
                            $("#txtdeptname").val(data);
                        }
                        else {
                            alert("获取部门失败");
                        }
                    });
                }
            });
            //经办人员
            $("#txtResponsibleName").autocomplete({
                source: avaiusernamedt,
                select: function (event, ui) {
                    var deptCode = ui.item.value;

                }
            });
            $("#btnAddFykm").click(function () {
                var bmbh = "";
                bmbh = $("#txtdeptname").val();
                if (bmbh == "") {
                    alert("请先选择部门");
                    return;
                }
                openKm(bmbh);
            });
            //否决单据
            $("#btn_cancel").click(function () {
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
            $("body").keypress(function (event) {
                var key = event.keyCode;
                if (key == null || key == undefined || key == "") {
                    return;
                }
                if (key == "27") {
                    if (confirm("单据尚未保存，确定要关闭吗？")) {
                        window.close();
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


        function openKm(deptCode) {
            var str = window.showModalDialog("../../webBill/bxgl/YskmSelectNew.aspx?deptCode=" + deptCode, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var json = $.parseJSON(str);
                var strRel = "";
                for (var i = 0; i < json.length; i++) {
                    if (json[i].Yscode != '' && json[i].Yscode != undefined) {
                        strRel += json[i].Yscode;
                        strRel += ",";
                    }
                }
                if (strRel != "") {
                    $("#txtloankm").val(strRel.substr(0, strRel.length - 1));
                }
            }
        }
        function checkEdit() {
            var varordercodedate = $("#txtloanName").val();
            var lb = $("#ddljklb").val();
            var bm = $("#txtdeptname").val();
            var je = $("#txtmoney").val();
            var sy = $("#txtjksy").val();
            var ts = $("#txtjkts").val();
            var jkr = $("#txtResponsibleName").val();
            if (varordercodedate.length < 1) {
                alert("经办人不能为空！");
                $("#txtloanName").focus();
                return false;
            }
            else if (jkr.length < 1) {
                alert("借款人不能为空！");
                $("#txtResponsibleName").focus();
                return false;
            }
            else if (lb.length == 0) {
                alert("借款类别不能为空！");
                $("#ddljklb").focus();
                return false;
            }
            else if (bm.length == 0) {
                alert("部门不能为空！");
                bm.focus();
                return false;
            }
            else if (je.length == 0) {
                alert("金额不能为空！");
                $("#txtmoney").focus();
                return false;
            }
            else if (ts.length == 0) {
                alert("天数不能为空！");
                $("#txtjkts").focus();
                return false;
            }
            else if (sy.length == 0) {
                alert("借款原因不能为空！");
                $("#txtjksy").focus();
                return false;
            }
            else {

                var fjdj = "";
                $("#cgdj tr:gt(0)").each(function () {
                    var temp = $(this).find("td:eq(1)").text();
                    fjdj += temp + ",";
                });
                if (fjdj.length > 1)
                    fjdj = fjdj.substring(0, fjdj.length - 1);

                if (fjdj.length > 50)
                    alert("单据过多,请联系开发商");
                $("#hffjdj").val(fjdj);
                return true;
            }
        }
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:1050px;status:no;scroll:yes');
            if (returnValue == "1") {
                location.replace(location.href);
            }
        }
        function openDetailbx(deptCode) {
            window.showModalDialog('../../webBill/bxgl/bxDetailFinal.aspx?type=look&billCode=' + deptCode, 'newwindow', 'center:yes;dialogHeight:570px;dialogWidth:970px;status:no;scroll:yes');
        }
        var selectCode = ''; //单据撤销和单据详细信息用到的选择单据的编号

        //edit by lvcc 20120827 
        function onSelectBillChanged(strBillVal) {
            if (strBillVal == null || strBillVal == "") {
                return;
            } else if (strBillVal == "cc") {//出差
                openTravelApplication();
            } else if (strBillVal == "bg") {//报告单
                openCgsp2();
            } else if (strBillVal == "cg") {//采购单
                openCgsp();
            } else { }
            $("#selectBill").val("");
        }

        //mxl 2012.04.09 添加报告单
        function openCgsp2() {//选择附加的单据，打开单据选择
            var tempInner = window.showModalDialog('../../webBill/bxgl/selectlscg.aspx?from=jkd', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');
            //alert(tempInner);
            if (tempInner == undefined || tempInner == "")
            { }
            else {
                //给返回的结果添加上一个单选框 
                if (getFlag(tempInner)) {
                    var strTemp = tempInner.substring(4, tempInner.length);
                    var num = $("#tb_fysq").find('tr').length;
                    var endStr = "<tr><td><input id='radio" + num + "' type='radio' name='myrad' onclick='radCheck(this);;'/></td>" + strTemp;
                    $("#cgdj tbody").append(endStr);
                }
            }
        }

        //添加出差申请单 edit by Lvcc 2012.08.25 
        function openTravelApplication() {
            var url = '../../webBill/bxgl/selectTravelAppBill.aspx?from=jkd';
            if ($("#hdHsCCBG").val() == "1") {
                url += "?Status=HasRepBill";
            }
            var tempInner = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');

            if (tempInner == undefined || tempInner == "")
            { }
            else {
                //给返回的结果添加上一个单选框 
                if (getFlag(tempInner)) {
                    var strTemp = tempInner.substring(4, tempInner.length);
                    var num = $("#tb_fysq").find('tr').length;
                    var endStr = "<tr><td><input id='radio" + num + "' type='radio' name='myrad' onclick='radCheck(this);'/></td>" + strTemp;
                    $("#tb_fysq").append(endStr);
                }
            }
        }

        //附加采购单
        function openCgsp() {//选择附加的单据，打开单据选择
            var tempInner = window.showModalDialog('../../webBill/bxgl/selectCgsp.aspx?from=jkd', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');

            if (tempInner == undefined || tempInner == "")
            { }
            else {


                if (getFlag(tempInner)) {
                    var strTemp = tempInner.substring(4, tempInner.length);
                    var num = $("#tb_fysq").find('tr').length;
                    var endStr = "<tr><td><input id='radio" + num + "' type='radio' name='myrad' onclick='radCheck(this);;'/></td>" + strTemp;
                    $("#cgdj tbody").append(endStr);
                }
            }
        }


        function cancelAttachment() {
            if (selectCode == '' || selectCode == undefined) {
                alert("请选中要操作的记录！");
                return false;
            }
            var tbody = $("#tb_fysq");
            var trLength = tbody.children().length;
            var flg = false;
            var index = -1;
            for (var i = 0; i < trLength; i++) {
                var iRel = tbody.children()[i].children[1].innerHTML.indexOf(selectCode);
                if (iRel != '-1') {
                    flg = true;
                    index = i;
                    break;
                }
            }
            if (!flg) {
                alert("请选中要操作的记录");
                return false;
            }
            var tableMain = $("#cgdj");
            var nowTr = tbody.children()[index];
            nowTr.parentNode.removeChild(nowTr);
            selectCode == '';
        }

        $(function () {

            //查看信息 edit by lvcc
            $("#btnFysqXX").click(function () {
                if (selectCode == '' || selectCode == undefined) {
                    alert("请选中要操作的记录！");
                    return false;
                }
                var codeType = selectCode.substring(0, 4);
                if (codeType == 'ccsq') {
                    //出差申请
                    window.showModalDialog("../../webBill/fysq/travelApplicationDetails2.aspx?Ctrl=View&Code=" + selectCode, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:940px;status:no;scroll:yes');
                } else if (codeType == 'lscg') {
                    //报告申请
                    window.showModalDialog("../../webBill/fysq/lscgDetail.aspx?type=look&cgbh=" + selectCode, 'newwindow', 'center:yes;dialogHeight:560px;dialogWidth:940px;status:no;scroll:yes');
                } else if (codeType == 'cgsp') {
                    //采购审批
                    window.showModalDialog("../../webBill/fysq/cgspDetail.aspx?type=look&cgbh=" + selectCode, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:940px;status:no;scroll:yes');
                } else {
                }
                selectCode == '';

            });
        });

        function radCheck(self) {
            selectCode = self.parentNode.parentNode.childNodes[1].innerHTML;
        }

        function getFlag(code) {
            var flag = true;
            //给返回的结果添加上一个单选框 
            $("#cgdj tr:gt(0)").each(function (index) {
                var temp = $(this).find("td:eq(1)").text();
                if (code.indexOf(temp) != -1) {
                    alert("该单据已选择");
                    flag = false;
                    return false;
                }
            });
            return flag;
        }
    </script>

    <style type="text/css">
        .title {
            font-size: 18px;
            font-family: 微软雅黑;
            font-weight: 500;
            text-align: center;
            border: none;
            width: 100%;
        }
    </style>
</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
            <table cellpadding="0" cellspacing="0" width="100%" border="0">
                <tr>
                    <td class="title" colspan="4">借款单
                    <asp:Label ID="lbjkcode" runat="server"></asp:Label>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="text-align: center; width: 100%; padding: 0px; margin: 0px;">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="myTable">
                            <tr>
                                <td class="tableBg2" style="text-align: right">经办人：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtloanName" runat="server" Width="96%"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right">借款人：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtResponsibleName" runat="server" Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right">填报时间：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtaddtime" runat="server" Width="96%"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right">借款类别：
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddljklb" runat="server" Width="96%">
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right">部门：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtdeptname" runat="server" Width="96%"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right">借款天数：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtjkts" runat="server" Width="96%" onblur="checkisNaN(this);" onkeyup="replaceNaN(this);"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right">借款日期：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtjksj" runat="server" Width="96%"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right">借款金额：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtmoney" runat="server" onblur="checkisNaN(this);" onkeyup="replaceNaN(this);"
                                        Width="96%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr1" runat="server">
                                <td class="tableBg2" style="text-align: right">借款事由：
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtjksy" Height="50" TextMode="MultiLine" runat="server" Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="tr4" runat="server">
                                <td class="tableBg2" style="text-align: right">备注：
                                </td>
                                <td colspan="3">
                                    <asp:TextBox ID="txtbz" Height="50" TextMode="MultiLine" runat="server" Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>附件：
                                </td>
                                <td colspan="3">
                                    <asp:FileUpload ID="uploadFiles" runat="server" />
                                    <asp:Label ID="Lafilename" runat="server" Text=""></asp:Label>
                                    <asp:HiddenField ID="hiddFileDz" runat="server" />
                                    <asp:Button ID="btn_sc" runat="server" Text="上 传" CssClass="baseButton" OnClick="btn_sc_Click" />
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                    <asp:Label ID="laFilexx" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr runat="server" id="trcjmx">
                    <td colspan="4" class="tableBg2" style="text-align: center">冲减明细
                    </td>
                </tr>
                <tr runat="server" id="trgride">
                    <td colspan="4" style="text-align: center; width: 100%">
                        <div style="overflow: auto; width: 100%; max-height: 250px">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="baseTable"
                                ShowHeader="true" Width="100%" EmptyDataText="没有冲减/还款记录" OnItemDataBound="myGrid_ItemDataBound"
                                OnRowDataBound="GridView1_RowDataBound" ShowFooter="true">
                                <RowStyle CssClass="myGridItem" />
                                <HeaderStyle CssClass="myGridHeader" />
                                <Columns>
                                    <asp:BoundField DataField="loancode" HeaderText="单号" ItemStyle-HorizontalAlign="Center"
                                        HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                                    <asp:BoundField DataField="je" HeaderText="金额" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ltype" HeaderText="还款类型" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="ldate" HeaderText="还款日期" />
                                    <asp:BoundField DataField="billCode" HeaderText="报销单号" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="note1" HeaderText="审批状态" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="billName" HeaderText="billName" HeaderStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"
                                        ItemStyle-CssClass="hiddenbill" />
                                    <asp:BoundField DataField="flowid" HeaderText="flowid" HeaderStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"
                                        ItemStyle-CssClass="hiddenbill" />
                                    <asp:BoundField DataField="listid" HeaderText="listid" HeaderStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"
                                        ItemStyle-CssClass="hiddenbill" />
                                    <asp:BoundField DataField="note3" HeaderText="凭证号" ItemStyle-HorizontalAlign="Center" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td colspan="4" style="background-color: red; height: 4px"></td>
                </tr>
                <tr>
                    <td class="tableBg2">附加单据
                    </td>
                    <td colspan="3" style="text-align: right">
                        <asp:DropDownList ID="selectBill" runat="server" CssClass="baseSelect" onchange="onSelectBillChanged(this.options[this.selectedIndex].value);">
                            <asp:ListItem Value="">--选择附加单据--</asp:ListItem>
                        </asp:DropDownList>
                        <input id="btnFyspCx" value="附件撤销" type="button" class="baseButton" onclick="cancelAttachment();" />
                        <input id="btnFysqXX" value="单据信息" type="button" class="baseButton" />
                    </td>
                </tr>
                <tr class="cgspInfo" runat="server" id="cgspInfo">
                    <td colspan="4">
                        <table id="cgdj" width="100%">
                            <thead>
                                <tr class="myGridHeader">
                                    <td>选择
                                    </td>
                                    <td>单据编号
                                    </td>
                                    <td>采购单位
                                    </td>
                                    <td>承办人
                                    </td>
                                    <td>申请日期
                                    </td>
                                    <td>申请类别
                                    </td>
                                    <td>采购总额
                                    </td>
                                    <td>原因说明
                                    </td>
                                    <td>单据状态
                                    </td>
                                </tr>
                            </thead>
                            <tbody runat="server" id="tb_fysq">
                            </tbody>
                        </table>
                        <asp:HiddenField ID="HiddenField3" runat="server" />
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 37px;" colspan="4">
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
                        <asp:HiddenField ID="hffjdj" runat="server" Value="" />
                    </td>
                </tr>
            </table>
        </form>
    </center>
</body>
</html>
