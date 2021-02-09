<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KpsqDetails.aspx.cs" Inherits="SaleBill_kpsq_KpsqDetails" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>开票申请单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
              $("#lasqbm").autocomplete({
                source: availableTags
            });
        
        })
        
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
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div>
        <center>
            <table cellpadding="0" cellspacing="0"  width="95%" border="0">
                <tr>
                    <td style="text-align: center; height: 26px;" colspan="6">
                        <strong><span style="font-size: 12pt">开票申请单</span></strong>
                    </td>
                </tr>
                <tr>
                    <td style="height: 26px; text-align: center" colspan="6">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="myTable">
                            <tr>
                                <td class="tableBg2" style="text-align: right; width:100px">
                                    申请日期：
                                </td>
                                <td style="text-align:left; width:100px">
                                    <asp:TextBox ID="txtsqrq" runat="server" Width="200px"  onfocus="javascript:setday(this);"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right;">
                                    申请单号：
                                </td>
                                <td style="width: 200px" colspan="4">
                                    <asp:Label ID="laSqbh" runat="server" Text=""></asp:Label>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right">
                                    申请单位：
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="lasqbm" runat="server" Width="200px"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right; width:100px">
                                    申请车辆：
                                </td>
                                <td colspan="5" valign="top" style="vertical-align: top">
                                    <div style="width: 100%" runat="server" id="AddDiv">
                                      车架号：<asp:TextBox ID="txtcjh" runat="server" Width="152"></asp:TextBox>
                                        <asp:Button ID="btdr" runat="server" Text="确 定" CssClass="baseButton" 
                                            onclick="btdr_Click" />
                                     订单号：<asp:TextBox ID="txtxsdh" runat="server" Enabled="false"></asp:TextBox>
                                        经销商：<asp:TextBox ID="txtDealersName" runat="server" Enabled="false" ></asp:TextBox>
                                         <asp:CheckBox ID="checkjc" runat="server" Text="是否军车"/>
                                         <asp:CheckBox ID="checksp" runat="server" Enabled="false" Text="特殊返利" />
                                         <asp:CheckBox ID="checkdf" runat="server" Enabled="false" Text="已对付" />
                                    </div>
                                    <asp:Button ID="btn_Addcx" runat="server" Text="增 加" CssClass="baseButton" OnClick="btn_Addcx_Click" />
                                    <asp:Button ID="btn_Del" runat="server" CssClass="baseButton" Text="删 除" OnClick="btn_Del_Click" />
                                    <span style="color: Red" id="tsxx" runat="server">【友情提示】：编号不允许空。</span>
                                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                                        Width="100%" OnRowDataBound="GridView1_RowDataBound">
                                        <Columns>
                                            <asp:TemplateField HeaderText="选择" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                                                <ItemTemplate>
                                                    <asp:CheckBox ID="CheckBox2" runat="server" />
                                                </ItemTemplate>
                                            </asp:TemplateField>
                                               <asp:BoundField DataField="Note2" HeaderText="订单号" HeaderStyle-CssClass="myGridHeader" />
                                            <asp:BoundField DataField="TruckCode" HeaderText="车架号" HeaderStyle-CssClass="myGridHeader" />
                                          <asp:BoundField DataField="DealersName" HeaderText="经销商" HeaderStyle-CssClass="myGridHeader" />
                                            <asp:BoundField DataField="IsJC" HeaderText="是否军车" HeaderStyle-CssClass="myGridHeader" />
                                             <asp:BoundField DataField="IsSpApp" HeaderText="是否特殊返利" HeaderStyle-CssClass="myGridHeader" />
                                               <asp:BoundField DataField="Note3" HeaderText="是否对付" HeaderStyle-CssClass="myGridHeader" />
                                           
                                           
                                        </Columns>
                                    </asp:GridView>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right">
                                    申请说明：
                                </td>
                                <td colspan="5">
                                    <asp:TextBox ID="txtExplina" runat="server" TextMode="MultiLine" Height="123px" Width="99%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td class="tableBg2" style="text-align: right">
                                    附件：
                                </td>
                                <td colspan="3">
                                    <asp:FileUpload ID="FileUpload1" runat="server" Width="440px" />
                                    <asp:Label ID="Lafilename" runat="server" Text="" Width="440px"></asp:Label>
                                    <asp:HiddenField ID="hiddFileDz" runat="server" />
                                </td>
                                <td colspan="2" style="text-align: left; padding-left: 30px; width: 200px">
                                    <asp:Button ID="btn_sc" runat="server" Text="上 传" CssClass="baseButton" OnClick="btn_sc_Click" />
                                    <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                                    <asp:Label ID="laFilexx" runat="server" Text="" ForeColor="Red"></asp:Label>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: center; height: 37px;">
                        <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click" />&nbsp;
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
