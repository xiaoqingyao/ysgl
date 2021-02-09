<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PingZhengDetail.aspx.cs"
    Inherits="webBill_cwgl_PingZhengDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>制作凭证</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            //冲借款
            $("#btn_Return").click(function () {
                var cheks = $("#GridView input:checkbox ");
                var dept = '';
                var amount = 0;

                for (var s = 1; s < cheks.length; s++) {
                    var evecheck = $(cheks).eq(s);
                    if (evecheck.attr('checked')) {
                        var evedep = evecheck.parent().parent().parent().children().eq(6).html();
                        if (evedep != null && evedep.indexOf("]") > 0) {
                            //获取部门
                            dept = evedep.substring(1, evedep.indexOf("]"));
                            //获取借金额
                            var jie = evecheck.parent().parent().parent().children().eq(8).children().eq(0).val();

                            if (jie != null && jie != undefined && jie != '') {
                                amount = parseFloat(amount) + parseFloat(jie);
                            }
                        }
                    }
                }
                if (dept == '') {
                    alert("请选择借方金额大于零的凭证明细，然后冲借款"); return;
                }
                var billcode = '<%=Request["Code"] %>';
                var openUrl = "../../SaleBill/BorrowMoney/FundBorrowList.aspx?type=hk&all=1&dept=" + dept + "&amount=" + amount + "&billcode=" + billcode;
                window.showModalDialog(openUrl, 'newwindow2', 'center:yes;dialogHeight:600px;dialogWidth:800px;status:no;scroll:yes');
            });
            //删除行
            $("#btn_Del").click(function () {
                $("#GridView tr").each(function (e, s) {
                    var check = $(this).find("input:checkbox");
                    if (check != null && check != undefined && check.attr('checked')) {
                        var hiddenf = $(this).find("input:hidden");
                        if (hiddenf != null && hiddenf != undefined) {
                            $(this).find("td").eq(14).children().eq(0).val('1');
                            $(this).addClass("hiddenbill");
                        }
                    }
                });
                calculateJie();
                calculateDai();
            });
        });
        function addSource(obj) {
            $(obj).autocomplete({
                source: availableTags
            });

        }
        function selectcwkm(url, obj) {
            var str = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:850px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                obj.parentNode.getElementsByTagName('input')[0].value = str;
                $("#reLoad").click();
            }
        }
        function jieBlur(obj) {
            var dai = $(obj).closest("tr").children().eq(9).children().eq(0).val();
            var jie = $(obj).val();
            if (isNaN(jie)) {
                $(obj).val("0");
                alert("请输入阿拉伯数字");
            } else if (dai - 0 != 0 && jie - 0 != 0) {//检查贷方必须为0
                $(obj).val("0");
                alert("借贷必须有一个为0");
            }
            calculateJie();
        }
        function daiBlur(obj) {
            var jie = $(obj).closest("tr").children().eq(8).children().eq(0).val();
            var dai = $(obj).val();
            if (isNaN(dai)) {
                $(obj).val("0");
                alert("请输入阿拉伯数字");
            } else if (dai - 0 != 0 && jie - 0 != 0) {
                $(obj).val("0");
                alert("借贷必须有一个为0");
            }
            calculateDai();
        }
        function SelectAll(aControl) {
            var chk = document.getElementById("GridView").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
            }
        }
        function calculateJie() {
            var trs = $("#GridView tr");
            var amount = 0;
            for (var s = 1; s < trs.length - 1; s++) {
                //检查删除标记  如果删除标记等于1  则不加
                var flg = trs.eq(s).find("td").eq(14).children().eq(0).val();
                if (flg != null && flg != undefined && flg != '1') {
                    var eve = trs.eq(s).find("td").eq(8).children().eq(0).val();
                    amount += parseFloat(eve);
                }
            }
            trs.eq(trs.length - 1).find("td").eq(8).html(amount);
        }
        function calculateDai() {
            var trs = $("#GridView tr");
            var amount = 0;
            for (var s = 1; s < trs.length - 1; s++) {
                //检查删除标记  如果删除标记等于1  则不加
                var flg = trs.eq(s).find("td").eq(14).children().eq(0).val();
                if (flg != null && flg != undefined && flg != '1') {
                    var eve = trs.eq(s).find("td").eq(9).children().eq(0).val();
                    amount += parseFloat(eve);
                }
            }
            trs.eq(trs.length - 1).find("td").eq(9).html(amount);
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <div class="baseDiv" style="margin-left: 5px; margin-top: 5px; width: 99%">
            <table>
                <tr>
                    <td style="text-align: right">
                        <span style="display: none">
                            <asp:Button ID="reLoad" runat="server" Width="0px" OnClick="reLoad_OnClick" /></span>
                        帐套：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlZhangTao" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right">制单日期：
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" onfocus="setday(this);" CssClass="baseText"></asp:TextBox>
                    </td>
                    <td style="text-align: right">附单据数：
                    </td>
                    <td>
                        <asp:TextBox ID="txtBillCount" runat="server" CssClass="baseText" Width="30"></asp:TextBox>
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlZhaiYao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlZhaiYao_SelectIndexChanged">
                        </asp:DropDownList>
                        <asp:TextBox ID="txtZhaiYao" runat="server" CssClass="baseText"></asp:TextBox><asp:CheckBox
                            ID="cbAddToUsually" runat="server" Text="添加到常用" />
                        <asp:Button ID="btnZhaiYaoToItem" runat="server" Text="统一摘要" CssClass="baseButton"
                            OnClick="btnZhaiYaoToItem_Click" />
                        <asp:Button ID="btn_addrow" runat="server" Text="添加行" CssClass="baseButton" OnClick="btn_addrow_Click" />
                        <asp:Button ID="btn_cfh" runat="server" Text="拆分行" CssClass="baseButton" OnClick="btn_cfh_Click" />
                        <input type="button" class="baseButton" value="删除行" id="btn_Del" />
                        <%--<asp:Button ID="" runat="server" Text="还 款" CssClass="baseButton" OnClick="btn_Return_Click" />--%>
                        <input type="button" class="baseButton" value="冲借款" id="btn_Return" />
                    </td>
                </tr>
            </table>
            <div class="baseDiv" style="color: Red">
                <asp:Label ID="lbeMsg" runat="server"></asp:Label>
            </div>
            <div style="text-align: center; width: 100%; height: 500px; overflow: auto;">
                <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                    OnRowDataBound="GridView_RowDataBound" Width="100%" ShowFooter="true">
                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                    <RowStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    <FooterStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    <Columns>
                        <asp:TemplateField>
                            <HeaderTemplate>
                                <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                    Text="全选" />
                            </HeaderTemplate>
                            <ItemTemplate>
                                &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="PingZhengType" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <asp:BoundField DataField="bxzy" HeaderText="摘要" HeaderStyle-CssClass="hiddenbill"
                            ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                        <asp:TemplateField HeaderText="摘要">
                            <ItemTemplate>
                                <asp:TextBox ID="txtZhaiYao" runat="server" Width="95%"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="fykmName" HeaderText="明细科目" FooterStyle-CssClass="myGridItem"
                        HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemCenter" />--%>
                        <asp:TemplateField HeaderText="明细科目" ControlStyle-Width="200">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMingXiKemu" runat="server" Text="" Width="90%"></asp:TextBox>
                                <input type="button" class="baseButton" id="btn_jf1" onclick="selectcwkm('../select/selectcwkmframe.aspx', this);"
                                    value="选择" />
                                <%--                 onkeydown="javascript:return false;"           <asp:Button ID="btnMxkmSelect" runat="server" Text="选择" OnCommand="btnMxkmSelect_OnCommand" CommandArgument="<%# Container.DataItemIndex %>"/>
                                --%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="辅助核算" ItemStyle-Width="200" FooterStyle-CssClass="myGridItem"
                            HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemCenter">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFuZhuHeSuan" runat="server" Text="" Width="90%"></asp:TextBox>
                                <asp:DropDownList ID="ddlFuZhuHeSuan" runat="server" Width="98%">
                                </asp:DropDownList>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="billDept" HeaderText="部门核算" FooterStyle-CssClass="myGridItem"
                            HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem" />
                        <asp:TemplateField HeaderText="部门核算">
                            <ItemTemplate>
                                <%--<asp:DropDownList ID="ddlForDept" runat="server">
                            </asp:DropDownList>--%>
                                <asp:TextBox ID="txtForDept" runat="server" onclick="addSource(this);"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--row 8--%>
                        <asp:TemplateField HeaderText="借方金额">
                            <ItemTemplate>
                                <asp:TextBox ID="txtjfje" runat="server" Width="95%" Text='<%# DataBinder.Eval(Container.DataItem, "jfje") %>'
                                    onkeyup="jieBlur(this);"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="贷方金额">
                            <ItemTemplate>
                                <asp:TextBox ID="txtdfje" runat="server" Width="95%" Text='<%# DataBinder.Eval(Container.DataItem, "dfje") %>'
                                    onkeyup="daiBlur(this);"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:BoundField DataField="jfje" HeaderText="借方金额" FooterStyle-CssClass="myGridItem"
                        HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />
                    <asp:BoundField DataField="dfje" HeaderText="贷方金额" FooterStyle-CssClass="myGridItem"
                        HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight" DataFormatString="{0:N2}" />--%>
                        <asp:BoundField DataField="fuzhuhesuan" HeaderText="fuzhuhesuan" HeaderStyle-CssClass="hiddenbill"
                            ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                        <asp:BoundField DataField="billUser" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <asp:BoundField DataField="OsDept" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <asp:BoundField DataField="fykmName" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <%--row 14   删除标记 1删除 0否--%>
                        <asp:TemplateField HeaderText="隐藏字段" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="delFlg" Text="2"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <asp:Literal ID="Literal1" runat="server"></asp:Literal>
            <div style="text-align: right; margin-right: 15px;">
                <asp:Button ID="btnSave" runat="server" Text="保 存" CssClass="baseButton" OnClick="btnSave_Click" />&nbsp;
            <input id="btnCancle" value="取 消" type="button" class="baseButton" onclick="javascript: window.close();" />
            </div>
        </div>
    </form>
</body>
</html>
