<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LoanListDetails.aspx.cs"
    Inherits="SaleBill_BorrowMoney_LoanListDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>预支单据</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

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
            
             //科目
               $("#txtloankm").autocomplete({
                source: availablekmdt,
                select: function(event, ui) {
                    var deptCode = ui.item.value;
                 
                }
            });
            
              //人员
               $("#txtloanName").autocomplete({
                source: avaiusernamedt,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                    $.post("../../webBill/MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function(data, status) {
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
                select: function(event, ui) {
                    var deptCode = ui.item.value;
                 
                }
            });
              $("#btnAddFykm").click(function() {
                var bmbh = "";
                   bmbh = $("#txtdeptname").val();
                if (bmbh == "") {
                    alert("请先选择部门");
                    return;
                }
                openKm(bmbh);
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
            $("body").keypress(function(event) {
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
            var str = window.showModalDialog("../../webBill/bxgl/YskmSelectNew.aspx?deptCode=" + deptCode , 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
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
         
               var varloankm=$("#txtloankm").val();
            if(varloankm.length<1){
                alert("借款科目不能为空！");
                $("#txtloankm").focus();
                return false;
            }
          

            var varmoney=$("#txtmoney").val();
            if(varmoney.length<1){
                alert("借款金额不能空！");
                $("#txtmoney").focus();
                return false;
            }
          
            var varordercodedate=$("#txtloanName").val();
            if(varordercodedate.length<1){
                alert("借款人不能为空！");
                $("#txtloanName").focus();
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
          function openDetailbx(deptCode) {
            window.showModalDialog('../../webBill/bxgl/bxDetailFinal.aspx?type=look&billCode='+ deptCode , 'newwindow', 'center:yes;dialogHeight:570px;dialogWidth:970px;status:no;scroll:yes');
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="90%" border="0">
            <tr>
                <td style="text-align: center; height: 26px;">
                    <strong><span style="font-size: 12pt">借款单</span></strong>
                </td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" width="100%" class="myTable">
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                借款单号：
                            </td>
                            <td style="text-align: left" colspan="3">
                                <asp:Label ID="lbjkcode" runat="server" Width="96%"></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                借款人：
                            </td>
                            <td>
                                <asp:TextBox ID="txtloanName" runat="server" Width="96%"></asp:TextBox>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                经办人：
                            </td>
                            <td>
                                <asp:TextBox ID="txtResponsibleName" runat="server" Width="96%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr runat="server" id="trjs">
                            <td class="tableBg2" style="text-align: right">
                                冲减单据号：
                            </td>
                            <td>
                                <asp:TextBox ID="txtcjCode" runat="server" Width="96%"></asp:TextBox>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                结算方式：
                            </td>
                            <td>
                                <asp:DropDownList ID="txtremitanceuse" runat="server" Width="98.5%">
                                    <asp:ListItem Value="0">现金</asp:ListItem>
                                    <asp:ListItem Value="1">单据冲减</asp:ListItem>
                                </asp:DropDownList>
                            </td>
                            <td class="tableBg2" style="text-align: right; display: none">
                                还款日期：
                            </td>
                            <td>
                                <asp:TextBox ID="txtAppDate" runat="server" Width="96%" onfocus="setday(this);"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                借款部门：
                            </td>
                            <td>
                                <asp:TextBox ID="txtdeptname" runat="server" Width="96%"></asp:TextBox>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                借款金额：
                            </td>
                            <td>
                                <asp:TextBox ID="txtmoney" runat="server" onblur="checkisNaN(this);" onkeyup="replaceNaN(this);"
                                    Width="96%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" style="text-align: right">
                                借款日期：
                            </td>
                            <td>
                                <asp:TextBox ID="txtordercodedate" runat="server" Width="96%" onfocus="setday(this);"></asp:TextBox>
                            </td>
                            <td class="tableBg2" style="text-align: right">
                                可报销科目：
                            </td>
                            <td style="text-align: left">
                                <asp:TextBox ID="txtloankm" runat="server" Width="76%"></asp:TextBox>
                                <input type="button" value="选择费用" id="btnAddFykm" runat="server" class="baseButton" />
                            </td>
                        </tr>
                        <tr id="tr4" runat="server">
                            <td class="tableBg2" style="text-align: right">
                                借款说明：
                            </td>
                            <td colspan="3">
                                <asp:TextBox ID="txtloanexplain" Height="200" TextMode="MultiLine" runat="server"
                                    Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr runat="server" id="trcjmx">
                <th>
                    冲减明细
                </th>
            </tr>
            <tr runat="server" id="trgride">
                <td style="text-align: center">
                    <div style="width: 100%; height: 200px">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                            ShowHeader="true" Width="100%" EmptyDataText="没有冲减的单据">
                            <Columns>
                                <asp:TemplateField HeaderText="选 择" ItemStyle-CssClass="hiddenbill" HeaderStyle-CssClass="hiddenbill">
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox2" runat="server" />
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="billCode" HeaderText="编号" HeaderStyle-CssClass="hiddenbill"
                                    ItemStyle-CssClass="hiddenbill" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="billname" HeaderText="报销单号" HeaderStyle-CssClass="myGridHeader"
                                    ItemStyle-CssClass="myGridItem" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="je" HeaderText="金额" HeaderStyle-CssClass="myGridHeader"
                                    ItemStyle-CssClass="myGridItem" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="fykmname" HeaderText="费用科目" HeaderStyle-CssClass="myGridHeader"
                                    ItemStyle-CssClass="myGridItem" ItemStyle-HorizontalAlign="Center" />
                                <asp:BoundField DataField="billname" HeaderText="报销单号" HeaderStyle-CssClass="hiddenbill"
                                    ItemStyle-CssClass="hiddenbill" ItemStyle-HorizontalAlign="Center" />
                            </Columns>
                        </asp:GridView>
                    </div>
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
