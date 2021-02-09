<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cgspDetail.aspx.cs" Inherits="webBill_fysq_cgspDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>费用申请详细信息</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script type="text/javascript">
        $(function() {

        });
    </script>

    <style type="Text/css">
        .right
        {
            text-align: right;
        }
    </style>

    <script language="javascript" type="Text/javascript">
        function edit() {
            $("#txtCgrq").datepicker();
        }
        $(function() {
            var sumje = 0;
            $("#DataGrid1 tr td:nth-child(6)").not(":first,:last").each(function() { sumje += parseInt(this.innerHTML); });
            $("#DataGrid1 tr:last td:eq(5)").html(sumje);

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
            
             //报销人或单位
            $("#txtGys").autocomplete({
                source: allGys,
                select: function(event, ui) {
                 var gys = ui.item.value;
                    $("#txtGys").val(gys);
                    var dept=$("#lblDept").text();
                    
                    $.post("../MyAjax/GetDept.ashx", { "action": "gys", "code": gys ,"dept":dept}, function(data, status) {
                        if (status == "success") {
                             data= JSON.parse(data);
                            $("#txtKhh").val(data.khh);
                            $("#txtZh").val(data.zh);
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
        function cal(val) {
            var v1 = document.getElementById("txtDj" + val).value;
            var v2 = document.getElementById("txtSl" + val).value;

            if (v1 == "" || v2 == "")
            { }
            else {
                document.getElementById("txtZe" + val).value = webBill_fysq_cgspDetail.CalZj(v1, v2).value;
            }
        }
        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新 

            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("btnRefresh").click();
            }
        }

        function opAddDetail() {
            var billCode = $("#lbl_BillCode").html();
            var returnValue = window.showModalDialog('cgspDetailDetail.aspx?billCode=' + billCode, 'newwindow', 'center:yes;dialogHeight:350px;dialogWidth:457px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("btnRefresh").click();
            }
        }


        function deleteBxdj(djGuid) {
            var returnValue = webBill_fysq_cgspDetail.DeleteBxdj(djGuid).value;
            if (returnValue == true) {
                document.getElementById("btnRefreshFj").click();
            }
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
        <tr>
            <td style="text-align: center; height: 27px;">
                <strong><span style="font-size: 12pt">紧急、临时采购审批单</span><asp:Label ID="lblCgbh" runat="server" Text="No.201109120001"></asp:Label></strong>
            </td>
        </tr>
        <tr>
            <td style="height: 26px; text-align: center">
                <table border="0" cellpadding="1" cellspacing="0" class="myTable"width="100%">
                    <tr>
                        <td class="tableBg">
                            审批日期
                        </td>
                        <td>
                            <asp:TextBox ID="txtCgrq" runat="server"  style="width:90%"></asp:TextBox>
                        </td>
                      
                        <td class="tableBg" colspan="3"    >
                            <asp:Label ID="Label1" runat="server" Text="部门"  ></asp:Label>
                        </td>
                        <td>
                            <asp:Label ID="lblDept" runat="server" Text="[000001]采购蛇皮但单位" Style="display: none"></asp:Label>
                            <asp:Label ID="lblDeptShow" runat="server" Text="[000001]采购蛇皮但单位"></asp:Label>
                        </td>
                       
                    </tr>
                    <tr>
                        <td class="tableBg">
                            申请类别
                        </td>
                        <td colspan="2">
                            <asp:DropDownList ID="ddl_cglb" runat="server"   Width="120px">
                            </asp:DropDownList>
                        </td>
                        <td class="tableBg">
                            供应商
                        </td>
                        <td colspan="2">
                            <input type="text" runat="server" id="txtGys"  style="width:70%"/>
                              <asp:Button ID="Button4" runat="server" Text="添加到常用" CssClass="baseButton" OnClick="btnAddbxrTOChangyong_Click" />
                        </td>
                    </tr>
                    <tr>
                       
                        <td class="tableBg">
                            开户行
                        </td>
                        <td  colspan="2">
                            <input type="text" runat="server" id="txtKhh"  style="width:90%"/>
                        </td>
                        <td class="tableBg">
                            账号
                        </td>
                        <td colspan="2">
                            <input type="text" runat="server" id="txtZh"  style="width:90%"/>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg" rowspan="2">
                            采购主要内容
                        </td>
                        <td class="tableBg2" style="text-align: left" colspan="6">
                            <input type="button" runat="server" id="btn_insert" class="baseButton" value="增 加"
                                onclick="opAddDetail();" />
                            <asp:Button ID="Button5" runat="server" CssClass="baseButton" OnClick="Button5_Click"
                                Text="删 除" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6">
                            <asp:DataGrid ID="DataGrid1" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                BorderWidth="1px" CellPadding="3" CssClass="myGrid" ItemStyle-HorizontalAlign="Center"
                                PageSize="17" Width="100%" ShowFooter="True">
                                <PagerStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Mode="NumericPages"
                                    Position="Top" Visible="False" />
                                <ItemStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="选择">
                                        <ItemTemplate>
                                            &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Width="38px"
                                            Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="cgIndex" HeaderText="cgIndex" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="mc" HeaderText="名称">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="gg" HeaderText="规格型号">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="sl" HeaderText="数量">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" HorizontalAlign="Right" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="dj" HeaderText="单价" DataFormatString="{0:F2}">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" HorizontalAlign="Right" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="zj" HeaderText="总价" DataFormatString="{0:F2}">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" HorizontalAlign="Right" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="bz" HeaderText="备注">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg" rowspan="4">
                            市场询价情况
                        </td>
                        <td colspan="6" style="height: 22px">
                            <input type="File" runat="server" id="upLoadFiles" style="width: 401px" class="baseButton" />
                            <asp:Button ID="btnScdj" runat="server" CausesValidation="False" Text="上传单据" CssClass="baseButton"
                                OnClick="btnScdj_Click" />
                            <asp:Button ID="btnRefreshFj" runat="server" CausesValidation="False" Text="刷新" CssClass="baseButton"
                                OnClick="btnRefreshFj_Click" /><br />
                            <div id="divBxdj" runat="server">
                            </div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 50px">
                            单位一:<br />
                            <asp:TextBox ID="txtXjdw1" runat="server" TextMode="MultiLine" Width="333px"></asp:TextBox>
                        </td>
                        <td colspan="3" style="height: 50px">
                            询价一:<br />
                            <asp:TextBox ID="txtXj1" runat="server" TextMode="MultiLine" Width="333px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 50px">
                            单位二:<br />
                            <asp:TextBox ID="txtXjdw2" runat="server" TextMode="MultiLine" Width="333px"></asp:TextBox>
                        </td>
                        <td colspan="3" style="height: 50px">
                            询价二:<br />
                            <asp:TextBox ID="txtXj2" runat="server" TextMode="MultiLine" Width="333px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="3" style="height: 50px">
                            单位三:<br />
                            <asp:TextBox ID="txtXjdw3" runat="server" TextMode="MultiLine" Width="333px"></asp:TextBox>
                        </td>
                        <td colspan="3" style="height: 50px">
                            询价三:<br />
                            <asp:TextBox ID="txtXj3" runat="server" TextMode="MultiLine" Width="333px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            采购原因
                        </td>
                        <td colspan="6">
                            <asp:TextBox ID="txtSm" runat="server" Height="60px" Rows="2" TextMode="MultiLine"
                                Width="681px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg">
                            承办人
                        </td>
                        <td colspan="6">
                            <asp:Label ID="lblCbr" runat="server" Text="[000001]系统挂丽媛"></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg" colspan="1" style="height: 30px">
                            报告申请单
                        </td>
                        <td colspan="6" style="height: 30px">
                            <asp:Button ID="Button2" runat="server" CssClass="baseButton" OnClick="Button2_Click"
                                Text="增加报告申请" />
                            <asp:Button ID="Button3" runat="server" CssClass="baseButton" Text="删除报告申请" OnClick="Button3_Click" />
                            <asp:Button ID="btnRefresh" runat="server" CssClass="baseButton" Text="刷新数据" OnClick="btnRefresh_Click" />
                            <asp:Button ID="btn_Details" runat="server" CssClass="baseButton" Text="详细信息" OnClick="btn_Details_Click" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="7">
                            <asp:DataGrid ID="myGrid" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                                BorderWidth="1px" CellPadding="3" CssClass="myGrid" ItemStyle-HorizontalAlign="Center"
                                PageSize="17" Width="100%" OnItemDataBound="myGrid_ItemDataBound">
                                <PagerStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Mode="NumericPages"
                                    Position="Top" Visible="False" />
                                <ItemStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:TemplateColumn HeaderText="选择">
                                        <ItemTemplate>
                                            &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Width="38px"
                                            Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="billCode" HeaderText="单据编号">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="cgDept" HeaderText="采购单位">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="cbr" HeaderText="承办人">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="sj" DataFormatString="{0:D}" HeaderText="申请日期">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="cglb" HeaderText="申请类别">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="sm" HeaderText="原因说明">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="True" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="stepID_ID" HeaderText="stepID_ID" Visible="False">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="stepID" HeaderText="单据状态">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                        <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                            Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    </asp:BoundColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                </table>
                <table style="display: none;">
                    <tr style="display: none;">
                        <td class="tableBg">
                        </td>
                        <td colspan="2">
                        </td>
                        <td class="tableBg">
                            部门主管意见
                        </td>
                        <td>
                            <asp:TextBox ID="txtbmzgyj" runat="server" TextMode="MultiLine" Width="100px" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="tableBg">
                            部门经理意见
                        </td>
                        <td>
                            <asp:TextBox ID="txtbmjlyj" runat="server" TextMode="MultiLine" Width="100px" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="tableBg" colspan="1">
                            综合管理部<br />
                            审查意见
                        </td>
                        <td colspan="6">
                            <asp:TextBox ID="txtzhglbyj" runat="server" Width="681px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="tableBg" colspan="1">
                            财务部门<br />
                            审查意见
                        </td>
                        <td colspan="6">
                            <asp:TextBox ID="txtcwbmyj" runat="server" Width="681px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="tableBg" colspan="1">
                            监审部门<br />
                            审查意见
                        </td>
                        <td colspan="6">
                            <asp:TextBox ID="txtjsbmyj" runat="server" Width="681px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="tableBg" colspan="1">
                            分管领导审批
                        </td>
                        <td colspan="6">
                            <asp:TextBox ID="txtfgldsp" runat="server" Width="681px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="tableBg" colspan="1">
                            总会计师审批
                        </td>
                        <td colspan="6">
                            <asp:TextBox ID="txtzkjssp" runat="server" Width="681px" TextMode="MultiLine" ReadOnly="True"></asp:TextBox>
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td class="tableBg" colspan="1">
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
                &nbsp;<input id="btn_ok" type="button" value="审核通过" class="baseButton" runat="server" />&nbsp;
                <input id="btn_cancel" type="button" value="审核驳回" class="baseButton" runat="server" />&nbsp;
                <asp:Button ID="btn_fh" runat="server" Text="取 消" CssClass="baseButton" OnClick="btn_fh_Click" />
            </td>
        </tr>
        <tr style="display: none;">
            <td colspan="6" style="height: 37px; text-align: center">
                <asp:Label ID="lbl_BillCode" runat="server"></asp:Label><asp:Label ID="Label2" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
