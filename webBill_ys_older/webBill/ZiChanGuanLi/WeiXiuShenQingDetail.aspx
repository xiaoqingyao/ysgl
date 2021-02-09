<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WeiXiuShenQingDetail.aspx.cs"
    Inherits="webBill_ZiChanGuanLi_WeiXiuShenQingDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>维修申请单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />


    <script type="text/javascript" language="javascript">
        $(function() {
            //申请人输入时
            $("#txtAppPersion").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function(data, status) {
                        if (status == "success") {
                            $("#lblDept").text(data);
                        }
                        else {
                            alert("获取部门失败！");
                        }
                    });
                }
            });
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
        });
        //选择资产明细
        function opAddDetail() {
            var openUrl = "ZiChan_Select.aspx?par=" + Math.random();
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:yes');
            if (returnValue != undefined && returnValue != "") {
                $("#hdZiChanCodes").val(returnValue);
                $("#btnAdd_Server").click();
            }
        }
        //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '0.00';
                alert("必须用阿拉伯数字表示！");
            };
        }
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
    <form id="form1" runat="server">
    <div>
        <table class="baseTable" width="90%" style="height: 80%;background-color: #EBF2F5;">
            <tr>
                <td colspan="4" style="text-align: center; height: 26px; font-family: @华文宋体; font-size: medium;
                    font-weight: 800; background-color:#EDEDED;">
                    维修申请单
                </td>
            </tr>
            <tr>
                <td class="tableBg2">
                    申请人：
                </td>
                <td>
                    <asp:TextBox ID="txtAppPersion" runat="server"></asp:TextBox>
                </td>
                <td class="tableBg2">
                    申请部门：
                </td>
                <td>
                    <asp:Label runat="server" ID="lblDept"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="tableBg2">
                    申请日期：
                </td>
                <td>
                    <asp:TextBox ID="txtAppDate" runat="server" onfocus="setday(this);"></asp:TextBox>
                </td>
                <td class="tableBg2">
                    经办人：
                </td>
                <td>
                    <asp:TextBox ID="txtJingBanPersion" runat="server" ReadOnly="true"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableBg2">
                    资产：
                </td>
                <td colspan="3">
                    <input type="button" runat="server" id="btn_insert" class="baseButton" value="添 加"
                        onclick="opAddDetail();" />
                    <div style="display: none;">
                        <asp:Button ID="btnAdd_Server" runat="server" Text="" OnClick="btnAdd_Server_Click" /></div>
                    <asp:Button ID="btn_Del" runat="server" CssClass="baseButton" OnClick="btn_Del_Click"
                        Text="删 除" />
                    <span style="color: Red">【友情提示】：单击增加添加资产维修明细。</span>
                    <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        BorderWidth="1px" CellPadding="3" ItemStyle-HorizontalAlign="Center" PageSize="17"
                        Width="100%" ShowFooter="false" ShowHeader="true" OnItemDataBound="DataGrid1_OnItemCommand">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="选择">
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="cbSelect" runat="server" class="chkDataList" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="ZiChanCode" HeaderText="编号"></asp:BoundColumn>
                            <asp:BoundColumn DataField="ZiChanName" HeaderText="名称"></asp:BoundColumn>
                            <asp:BoundColumn DataField="LeiBieCode" HeaderText="类别"></asp:BoundColumn>
                            <asp:BoundColumn DataField="GuiGeXingHao" HeaderText="规格"></asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="维修类别">
                                <ItemTemplate>
                                    <asp:DropDownList runat="server" ID="ddlWeiXiuLeiBie">
                                    </asp:DropDownList>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="预计金额">
                                <ItemTemplate>
                                    <asp:TextBox runat="server" ID="txtNeedJe" Text="0" onkeyup="replaceNaN(this);" onblur="replaceNaN(this);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="Note1" HeaderText="维修类别" HeaderStyle-CssClass="hiddenbill"
                                ItemStyle-CssClass="hiddenbill"></asp:BoundColumn>
                            <asp:BoundColumn DataField="Note2" HeaderText="预计金额" HeaderStyle-CssClass="hiddenbill"
                                ItemStyle-CssClass="hiddenbill"></asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
            <tr>
                <td class="tableBg2" style="text-align: right">
                    申请说明：
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txtShuoMing" runat="server" TextMode="MultiLine" Height="250px"
                        Width="99%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: center; height: 37px;">
                    <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click"/>
                    <input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />&nbsp;
                    <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />&nbsp;
                    <input id="btn_fh" type="button" value="关 闭" class="baseButton" runat="server" onclick="javascript:self.close();" />&nbsp;
                    <asp:HiddenField ID="hdZiChanCodes" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
