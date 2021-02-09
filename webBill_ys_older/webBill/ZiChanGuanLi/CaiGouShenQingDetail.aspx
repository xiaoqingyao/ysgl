<%@ Page Language="C#" AutoEventWireup="true" CodeFile="CaiGouShenQingDetail.aspx.cs"
    Inherits="ZiChan_ZiChanGuanLi_CaiGouShenQingDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>资产采购申请</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function() {
            $("#btn_Shtg").click(function() {
                var billcode = '<%=Request["bh"] %>';
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                }
                else {
                    if (confirm("确定要审批该单据吗?")) {
                        $.post("../../webBill/MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "approve" }, OnApproveSuccess);
                    }
                }
            });
            $("#btn_Shbh").click(function() {
                var billcode = '<%=Request["bh"] %>';
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                }
                else {
                    if (confirm("确定要否决该单据吗?")) {
                        $.post("../../webBill/MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "disagree" }, OnApproveSuccess);
                    }
                }
            });
            //申请人
            $("#txtsqname").autocomplete({
                source: availableTags
            });
            //经办人
             $("#txtjbname").autocomplete({
                source: availableTags
            });
        })
          function check()
        {
               
            var reptname = $("#txtcgzcname").val();
            if (reptname.length < 1) {
                alert("资产名称不能为空！");
                $("#txtcgzcname").focus();
                return false;
            }
              var vartxtggxh = $("#txtggxh").val();
            if (vartxtggxh.length < 1) {
                alert("规格型号不能为空！");
                $("#txtggxh").focus();
                return false;
            }
              var vartxtcount = $("#txtcount").val();
            if (vartxtcount.length < 1) {
                alert("数量不能为空！");
                $("#txtcount").focus();
                return false;
            }
              var vartxtprice = $("#txtprice").val();
            if (vartxtprice.length < 1) {
                alert("价格不能为空！");
                $("#txtprice").focus();
                return false;
            }
              var vartxtExplina = $("#txtExplina").val();
            if (vartxtExplina.length < 1) {
                alert("采购说明不能为空！");
                $("#txtExplina").focus();
                return false;
            }
            else
            {
                return true;
            
            }

        }
        function OnApproveSuccess(data, status) {
            if (data > 0 && status == "success") {
                alert("操作成功！");
                self.close();
            } else {
                alert("审批失败！");
            }
        }
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:yes');
            if (returnValue == "1") {
                location.replace(location.href);
            }
        }
        function specialAppCode(code) {
            openDetail("../Salepreass/SpecialRebatesAppDetails.aspx?Ctrl=look&Code="+code);
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
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div>
        <center>
            <table cellpadding="0" cellspacing="0" width="95%" border="0">
                <tr>
                    <td style="text-align: center; height: 26px; font-family: @华文宋体; font-size: medium;
                        font-weight: 800; background-color: #EDEDED;" colspan="6">
                        <strong><span style="font-size: 12pt">资产采购申请单</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="height: 26px; text-align: center" colspan="6">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="myTable">
                            <tr>
                                <td class="tableBg2" style="text-align: right; width: 100px">
                                    申请日期：
                                </td>
                                <td style="text-align: left; width: 100px">
                                    <asp:TextBox ID="txtsqrq" runat="server" Width="200px" onfocus="javascript:setday(this);"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right;">
                                    申请编号：
                                </td>
                                <td style="width: 200px">
                                    <asp:Label ID="laSqbh" runat="server" Text=""></asp:Label>
                                </td>
                                <td class="tableBg2" style="text-align: right;">
                                    申请人：
                                </td>
                                <td style="width: 200px">
                                    <asp:TextBox ID="txtsqname" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right;">
                                    资产名称：
                                </td>
                                <td style="width: 200px">
                                    <asp:TextBox ID="txtcgzcname" runat="server" Width="200px"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right;">
                                    规格型号：
                                </td>
                                <td style="width: 200px">
                                    <asp:TextBox ID="txtggxh" runat="server" Width="200px"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right;">
                                    经办人：
                                </td>
                                <td style="width: 200px" colspan="3">
                                    <asp:TextBox ID="txtjbname" runat="server" ReadOnly="true" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right;">
                                    数量：
                                </td>
                                <td style="width: 200px">
                                    <asp:TextBox ID="txtcount" runat="server" onkeyup="replaceNaN(this);" Width="200px"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right;">
                                    价格：
                                </td>
                                <td style="width: 200px">
                                    <asp:TextBox ID="txtprice" runat="server" onkeyup="replaceNaN(this);" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right">
                                    采购说明：
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtExplina" runat="server" TextMode="MultiLine" Height="223px" Width="99%"></asp:TextBox>
                                    <asp:TextBox ID="lasqbm" runat="server" CssClass="hiddenbill" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: center; height: 37px;">
                        <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="return check();"
                            OnClick="btn_bc_Click" />&nbsp;
                        <input id="btn_Shtg" type="button" value="审核通过" runat="server" class="baseButton" />
                        &nbsp;
                        <input id="btn_Shbh" type="button" value="审核驳回" class="baseButton" runat="server" />
                        &nbsp;
                        <asp:Button ID="btn_gb" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close()" />
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
