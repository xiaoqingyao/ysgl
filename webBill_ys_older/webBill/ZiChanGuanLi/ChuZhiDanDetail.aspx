<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChuZhiDanDetail.aspx.cs"
    Inherits="webBill_ZiChanGuanLi_ChuZhiDanDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>资产处置编辑</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function() {
            $("#btn_Shtg").click(function() {
                var billcode = '<%=Request["bh"] %>';
                if (billcode == undefined || billcode == "") {
                    alert("请先选择单据!");
                }
                else {
                    if (confirm("确定要审批该单据吗?")) {
                        $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "approve" }, OnApproveSuccess);
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
                        $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": " ", "action": "disagree" }, OnApproveSuccess);
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
            //变动前
            $("#txtchangeq").autocomplete({
                source: availableTagsBefore
            });
            //变动后
            $("#txtchangeh").autocomplete({
                source: availableTagsAfter
            });
            //资产编号
            $("#txtzcCode").autocomplete({
                source: avzcTags,
                select: function(event, ui) {
                    var code = ui.item.value;
                    $("#txtzcCode").val(code);
                    $("#hdZiChanCodeForShowMsg").val(code);
                    if (code != null && code != undefined && code != "") {
                        $("#btnShowmsg").click();
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
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:yes');
            if (returnValue == "1") {
                location.replace(location.href);
            }
        }
        function specialAppCode(code) {
            openDetail("../Salepreass/SpecialRebatesAppDetails.aspx?Ctrl=look&Code=" + code);
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
        function showZiChanMsg() {
            var zichancode = $("#txtzcCode").val();
            if (zichancode == "") {
                alert("请先输入资产名称然后查看详细信息！");return;
            }
            var left=zichancode.indexOf("[");
            var right = zichancode.indexOf("]");
            if (left <= -1 || right <= -1) {
                alert("请输入标准的格式'['编号']'名称");return;
            }
            zichancode = zichancode.substring(1, right);
            openDetail("ZiChan_JiluDetail.aspx?Ctrl=look&Code=" + zichancode);
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div style="margin-top: 10px;">
        <center>
            <table cellpadding="0" cellspacing="0" width="95%" border="0">
                <tr>
                    <td style="text-align: center; height: 26px;">
                        <strong><span style="font-size: 12pt">资产处置单</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="myTable">
                            <tr>
                                <td class="tableBg2" style="text-align: right;width: 150px;">
                                    处置单号：
                                </td>
                                <td>
                                    <asp:Label ID="laSqbh" runat="server" Text="" Width="90%"></asp:Label>
                                </td>
                                <td class="tableBg2" style="text-align: right;">
                                    申请日期：
                                </td>
                                <td style="text-align: left;">
                                    <asp:TextBox ID="txtsqrq" runat="server" Width="90%" onfocus="javascript:setday(this);"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right;">
                                    变动日期：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtchangetime" runat="server" onfocus="javascript:setday(this);"
                                        Width="90%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right;">
                                    资产：
                                </td>
                                <td>
                                    <span style="width: 90%;">
                                        <asp:TextBox ID="txtzcCode" runat="server" Width="70%"></asp:TextBox>
                                        <input type="button" id="zichanxiangxi" class="baseButton" value="详" style="width:15%" title="资产详细信息" onclick="showZiChanMsg();";/>
                                    </span>
                                </td>
                                <td class="tableBg2" style="text-align: right;">
                                    申请人：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtsqname" runat="server" Width="90%"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right;">
                                    经办人：
                                </td>
                                <td>
                                    <asp:TextBox ID="txtjbname" runat="server" Width="90%" ReadOnly="true"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td style="text-align: right" class="tableBg2">
                                    变动类别：
                                </td>
                                <td>
                                    <asp:DropDownList ID="ddlSalewxlb" runat="server" AutoPostBack="true" Width="93%"
                                        OnSelectedIndexChanged="ddlSalewxlb_SelectedIndexChanged">
                                        <asp:ListItem Value="04" Selected="True">类别变动</asp:ListItem>
                                        <asp:ListItem Value="01" >位置变动</asp:ListItem>
                                        <asp:ListItem Value="02">部门变动</asp:ListItem>
                                        <asp:ListItem Value="03">使用状况变动</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                                <td colspan="4">
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right;">
                                    <asp:Label ID="biandongbefore" runat="server" Text="类别变动前："></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtchangeq" runat="server" Width="98%" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right;">
                                    <asp:Label ID="biandongafter" runat="server" Text="类别变动后："></asp:Label>
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtchangeh" runat="server" Width="98%" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trAddressBefore" runat="server" visible="false">
                                <td class="tableBg2" style="text-align: right;">
                                    位置变动前：
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtAddressBefore" runat="server" Width="98%" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr id="trAddressAfter" runat="server" visible="false">
                                <td class="tableBg2" style="text-align: right;">
                                    位置变动后：
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtAddressAfter" runat="server" Width="98%" TextMode="MultiLine"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right">
                                    变动说明：
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtExplina" runat="server" TextMode="MultiLine" Height="123px" Width="98%"></asp:TextBox>
                                    <asp:TextBox ID="lasqbm" Visible="false" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 37px;">
                        <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click" />&nbsp;
                        <input id="btn_Shtg" type="button" value="审核通过" runat="server" class="baseButton" />
                        &nbsp;
                        <input id="btn_Shbh" type="button" value="审核驳回" class="baseButton" runat="server" />
                        &nbsp;
                        <asp:Button ID="btn_gb" runat="server" Text="关 闭" CssClass="baseButton" OnClientClick="javascript:window.close()" />
                        <span style="display: none">
                            <asp:Button ID="btnShowmsg" runat="server" OnClick="btnShowmsg_Click" /></span>
                            <asp:HiddenField ID="hdZiChanCodeForShowMsg" runat="server" Value=""/>
                    </td>
                </tr>
            </table>
        </center>
    </div>
    </form>
</body>
</html>
