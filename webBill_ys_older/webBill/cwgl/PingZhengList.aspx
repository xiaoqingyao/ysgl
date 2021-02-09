<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PingZhengList.aspx.cs" Inherits="webBill_cwgl_PingZhengList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script type="text/javascript">
        var status = "none";
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            $("#txtLoanDateFrm").datepicker();
            $("#txtLoanDateTo").datepicker();

            initMainTableClass("<%=myGrid.ClientID%>");
            //刷新
            $("#btn_refresh").click(function () {
                location.replace(location.href);
            });
            //导入凭证
            $("#btn_Add").click(function () {
                var varGridView = document.getElementById("<%=myGrid.ClientID %>");
                var iGridViewRow = varGridView.rows.length;
                var endCode = "";
                var flg = "0";
                for (var i = 0; i < iGridViewRow; i++) {
                    if (varGridView.rows[i].cells[0].getElementsByTagName("input")[0].checked) {
                        var code = varGridView.rows[i].cells[1].innerHTML;
                        var pzcode = varGridView.rows[i].cells[9].innerHTML;
                        if (pzcode != "" && pzcode != "&nbsp;") {
                            flg = "1";
                            //alert("选中记录中存在已经生成凭证的记录，如果需要重新生成凭证，请先删除目标系统中的凭证记录并在本系统中通过“修改凭证”将凭证号清空！"); return;
                        }
                        if (code != undefined && code != "") {
                            endCode += code + "|*|";
                        }
                    }
                }
                if (flg == "1" && !confirm("选中的记录中存在已经生成凭证的记录，您要重新生成凭证吗？")) {
                    //                    if () {
                    //                        return;
                    //                    }
                    return;
                }
                if (endCode != "") {
                    var url = "PingZhengDetail.aspx";
                    var strpeizhi = $("#hdpingzhengdetailurl").val(); //读取配置项
                    if (strpeizhi != null && strpeizhi != undefined && strpeizhi != "") {
                        url = strpeizhi;
                    }
                    openDetail(url + "?Code=" + endCode + "&djlx=" + '<%=Request["djlx"]%>');
                } else {
                    alert("请先勾选需要操作的记录！");
                }
            });
            $("#btn_Edit").click(function () {
                var iLen = document.getElementById("myGrid").getElementsByTagName("input").length;
                var vBillCode = "";
                var vbillname = "";
                var iSelectCount = 0;
                for (var i = 0; i < iLen; i++) {
                    var obj = document.getElementById("myGrid").getElementsByTagName("input")[i];
                    if (obj.checked) {
                        vBillCode = document.getElementById("myGrid").rows[i].cells[1].innerHTML;
                        vbillname = document.getElementById("myGrid").rows[i].cells[2].innerHTML;
                        iSelectCount++;
                    }
                }
                if (iSelectCount <= 0) {
                    alert("请选中一条记录！");
                } else if (iSelectCount > 1) {
                    alert("最多选择一条记录进行编辑！");
                } else {
                    var bxdtype = $("#hdbxdtype").val();
                    var djlx = '02';
                    var vdjlx='<%=Request["djlx"]%>';
                    if ( vdjlx!= '') {
                        djlx = vdjlx;
                    }
                    if (bxdtype == "" || bxdtype == undefined || bxdtype == null) {
                        openDetail("../bxgl/bxDetailFinal.aspx?type=edit&billCode=" + vBillCode + "&dydj=" + djlx);
                    } else if (bxdtype == "gkbxd") {
                        openDetail("../bxgl/bxDetailForGK.aspx?dydj=02&type=edit&billCode=" + vbillname + "&dydj=" + djlx);
                    } else { }

                }
            });
            //查询
            $("#btnSelect").click(function () {
                $("#trSelect").toggle();
            });
            //取消
            $("#btn_cancle").click(function () {
                document.getElementById("trSelect").style.display = "none";
            });
            //部门选择
            $("#txtLoanDeptCode").autocomplete({
                source: availableTags
            });
            //人员选择
            $("#txtloannamecode").autocomplete({
                source: avaiusertb
            });
            //            $("#clearpingzheng").click(function() {
            //                if (confirm("您确定清空该记录的凭证信息吗？")) {
            //                    var row = $(".highlight");
            //                    var billcode = row.children().eq(1).text();
            //                    if (billcode == '' || billcode == undefined) {
            //                        alert("请先选中要清空的记录。");
            //                    }
            //                    $.post("PingZhengList.aspx?type=ajax", {}, function(data, status) {
            //                        alert('保存成功。');
            //                    });
            //                }
            //            });
        });
        function openDetail(openUrl) {
            var ret = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:1100px;status:no;scroll:yes');
            $("#btn_Select").click();
        }
        function openDetail2(openUrl) {
            var ret = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:350px;status:no;scroll:yes');
            location.replace(location.href);
        }

        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
        $(function () {
            initWindowHW();

            $("#btn_toExcel").click(function () {
                window.open("PingZhengToExcel.aspx");
            });
        });
        function SelectAll(aControl) {
            var chk = document.getElementById("myGrid").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
            }
        }


    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
        <div class="baseDiv">
            <table cellpadding="0" cellspacing="0" width="100%">
                <tr>
                    <td style="height: 30px">
                        <input class="baseButton" id="btn_refresh" value="刷 新" type="button" />
                        <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                        <input class="baseButton" id="btn_Edit" value="编 辑" type="button" />
                        <asp:Button ID="btn_GuaZhang" CssClass="baseButton" runat="server" Text="挂账/出纳支付"
                            OnClientClick="return confirm('挂账后将无法在报销给付菜单给付，是否继续？');" OnClick="btn_GuaZhang_Click" />
                        <input class="baseButton" id="btn_Add" value="生成凭证" type="button" />
                        <asp:Button CssClass="baseButton" ID="btn_BuLu" runat="server" Text="补录凭证" OnClick="btn_BuLu_Click" />
                        <asp:Button CssClass="baseButton" ID="btn_Mian" runat="server" Text="免做凭证" OnClientClick="return confirm('您确定要对您选择的报销单做免凭证处理吗？');"
                            OnClick="btn_Mian_Click" />
                        <input class="baseButton" value="导出EXCEL" type="button" id="btn_toExcel" style="display: none;" />
                        <asp:Button ID="Button1" runat="server" Text="导出EXCEL" CssClass="baseButton" OnClick="Button1_Click" />
                        <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                        <%--<input type="button" class="baseButton" id="clearpingzheng" value="清空凭证信息" />--%>
                    </td>
                </tr>
                <tr id="trSelect" style="display: none;">
                    <td colspan="3">
                        <div style="float: left">
                            <table class="baseTable" border="0">
                                <tr>
                                    <td style="text-align: right">单据日期从：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoanDateFrm" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">到：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoanDateTo" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">报销人：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtloannamecode" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">单位：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoanDeptCode" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">单据编号：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">凭证号：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtPingZhengCode" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td style="text-align: right">单据类型：
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlBillType" runat="server" Width="122px">
                                            <asp:ListItem Value="">--全部--</asp:ListItem>
                                            <asp:ListItem Value="1" Selected="True">一般报销单</asp:ListItem>
                                            <asp:ListItem Value="0">其它报销单</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">支付方式：
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlPayType" runat="server" Width="122px">
                                            <asp:ListItem Value="">--全部--</asp:ListItem>
                                            <asp:ListItem Value="1">挂账</asp:ListItem>
                                            <asp:ListItem Value="0">现金给付</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td style="text-align: right">凭证状态：
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStatus" runat="server" Width="122px">
                                            <asp:ListItem Value="">--全部--</asp:ListItem>
                                            <asp:ListItem Value="1">已完成</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True">未完成</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">审批状态：
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlStep" runat="server" Width="122px">
                                            <asp:ListItem Value="">--全部--</asp:ListItem>
                                            <asp:ListItem Value="1" Selected="True">审批完毕</asp:ListItem>
                                            <asp:ListItem Value="0">未完毕</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td style="text-align: right">单据金额：
                                    </td>
                                    <td>
                                        <asp:TextBox runat="server" ID="txtje" Width="120px"></asp:TextBox>
                                    </td>
                                    <td colspan="6">
                                        <asp:Button ID="btn_Select" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_Select_click" />
                                        <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                                    </td>
                                </tr>
                            </table>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td>
                        <div id="divgrid" style="overflow-x: auto; position: relative; word-warp: break-word; word-break: break-all">
                            <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                CssClass="myGrid" Width="1300px" OnItemDataBound="myGrid_ItemDataBound">
                                <Columns>
                                    <asp:TemplateColumn HeaderText="选择" HeaderStyle-Width="40" ItemStyle-Width="40">
                                        <HeaderTemplate>
                                            <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                                Text="全选" />
                                        </HeaderTemplate>
                                        <ItemTemplate>
                                            &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="True" CssClass="myGridItem" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="billCode" HeaderText="billCode">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="True" CssClass=" myGridItem hiddenbill" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="billName" HeaderText="单据编号" HeaderStyle-Width="110" ItemStyle-Width="110">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="True" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="billUser" HeaderText="报销人" HeaderStyle-Width="100" ItemStyle-Width="100">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="true" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="tbilldate" HeaderText="申请日期" HeaderStyle-Width="100"
                                        ItemStyle-Width="100">
                                        <%-- DataFormatString="{0:D}"--%>
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="true" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="billDept" HeaderText="所属部门" DataFormatString="{0:D}"
                                        HeaderStyle-Width="100" ItemStyle-Width="100">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="true" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="billJe" HeaderText="报销总额" DataFormatString="{0:N2}" HeaderStyle-Width="100"
                                        ItemStyle-Width="100">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="True" CssClass="myGridItemRight" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="bxzy" HeaderText="摘要" HeaderStyle-Width="350" ItemStyle-Width="350">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="true" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="stepid" HeaderText="状态" HeaderStyle-Width="100" ItemStyle-Width="100">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="True" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="pzcode" HeaderText="凭证号" HeaderStyle-Width="100" ItemStyle-Width="100">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="true" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="pzdate" HeaderText="凭证日期" HeaderStyle-Width="100" ItemStyle-Width="100">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="true" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="zhangtao" HeaderText="帐套" HeaderStyle-Width="100" ItemStyle-Width="100">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="true" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="guazhang" HeaderText="支付方式" HeaderStyle-Width="100" ItemStyle-Width="100">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="true" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="sfgf" HeaderText="是否给付" HeaderStyle-Width="100" ItemStyle-Width="100">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                            Wrap="true" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                </Columns>
                                <PagerStyle Visible="False" />
                            </asp:DataGrid>
                        </div>
                    </td>
                </tr>
                <tr>
                    <td style="height: 30px">
                        <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
                        </pager:UcfarPager>
                        <input type="hidden" runat="server" id="hdwindowheight" />
                    </td>
                </tr>
            </table>
        </div>
        <asp:HiddenField runat="server" ID="hdpingzhengdetailurl" Value="" />
        <asp:HiddenField runat="server" ID="hdbxdtype" Value="" />
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
