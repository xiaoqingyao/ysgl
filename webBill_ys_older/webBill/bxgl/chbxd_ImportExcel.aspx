<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chbxd_ImportExcel.aspx.cs" Inherits="webBill_bxgl_chbxd_ImportExcel" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
        var status = "none";
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            $("#txt_zdrq").datepicker();
            $("#txtLoanDateTo").datepicker();
            $("#txt_billDate").datepicker();
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[0] != null) {
                    var billCode = $(this).find("td")[1].innerHTML;
                }
                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function (data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            $("#txt_dept").autocomplete({
                source: availabledept
            });
            $("#txt_user").autocomplete({
                source: availableTags,
                select: function (event, ui) {
                    var rybh = ui.item.value;
                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function (data, status) {
                        if (status == "success") {
                            $("#txt_dept").val(data);
                        }
                        else {
                            alert("获取部门失败");
                        }
                    });
                }
            });
        });
        function openDetail(openUrl) {
            //            var returnValue = window.showModelessDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            //            if (returnValue == undefined || returnValue == "") {
            //                return false;
            //            }
            //            //            else {
            //            //                document.getElementById("Button2").click();
            //            //            }

            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("Button2").click();
            }

        }
        function openLookSpStep(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }

        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
        $(function () {
            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");
        });
        function fh() {
            window.location.href = "bxglFrame_Dz.aspx?dydj=06&isdz=1";
        }
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
        <table cellpadding="0" cellspacing="0" width="100%" style="margin-top: 5px">
            <tr>
                <td align="left" colspan="3">
                    <input type="button" id="btn_fh" value="返回费用报销单列表页" class="baseButton" onclick="fh()" />
                </td>
            </tr>
            <tr>
                <td align="left" colspan="3">
                    <asp:Panel ID="Panel1" runat="server" GroupingText="查询条件">
                        <div style="float: left">
                            <table class="baseTable" style="text-align: left;">

                                <tr>

                                    <td class="style1">日期：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_billDate" runat="server"></asp:TextBox>
                                    </td>
                                    <td>到：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtLoanDateTo" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                     <td>单据编号：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_pocode" runat="server" Width="120px"></asp:TextBox>
                                    </td>
                                    <td>存货：
                                    </td>
                                    <td>

                                        <asp:TextBox ID="txt_ch" runat="server" Width="120px"></asp:TextBox>

                                    </td>
                                    <td>帐套:
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="ddlZhangTao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="OnddlZhangTao_SelectedIndexChanged">
                                        </asp:DropDownList>
                                    </td>
                                     <td>
                                        <asp:DropDownList ID="ddl_status" runat="server">
                                            <asp:ListItem Value="">全部</asp:ListItem>
                                            <asp:ListItem Value="0" Selected="True">未导入</asp:ListItem>
                                            <asp:ListItem Value="1">已导入</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                    <td>
                                        <asp:Button ID="Button2" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button4_Click" />
                                    </td>

                                </tr>


                            </table>
                        </div>
                    </asp:Panel>
                </td>
            </tr>

            <tr>
                <td align="left" colspan="3">
                    <asp:Panel ID="Panel2" runat="server" GroupingText="导入填写信息">
                        <div style="float: left">
                            <table class="baseTable" style="text-align: left;">
                                <tr>
                                    <td class="style1">制单人：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_user" runat="server"></asp:TextBox>
                                    </td>
                                    <td class="style1">制单部门：
                                    </td>
                                    <td >
                                        <asp:TextBox ID="txt_dept" runat="server">
                                        </asp:TextBox>
                                    </td>

                                      <td class="style1" >制单日期：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txt_zdrq" runat="server"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="style1">说明：
                                    </td>
                                    <td colspan="3">
                                        <asp:TextBox ID="txt_sm" Width="90%" runat="server" TextMode="MultiLine"></asp:TextBox>
                                    </td>


                                    <td colspan="2">
                                        <asp:Button runat="server" ID="btn_dr" Text="导入" CssClass="baseButton" OnClick="btn_dr_Click" />

                                    </td>
                                </tr>
                            </table>
                        </div>
                    </asp:Panel>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" Width="1200px"
                            CssClass="myGrid">
                            <Columns>
                                <asp:TemplateColumn ItemStyle-Width="32" HeaderStyle-Width="30" HeaderText="选择">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                            Text="全选" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="false" />
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem " />
                                </asp:TemplateColumn>

                                <asp:BoundColumn DataField="code" HeaderText="单据编号">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem " />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="maker" HeaderText="制单人" HeaderStyle-Width="100"
                                    ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="zdDate" HeaderText="制单日期" DataFormatString="{0:D}"
                                    HeaderStyle-Width="150" ItemStyle-Width="150">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="showstatus" HeaderText="状态" HeaderStyle-Width="120" ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem hiddenbill" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="dept" HeaderText="部门" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="chcode" HeaderText="存货编号" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="false" CssClass="hiddenbill" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="chname" HeaderText="存货名称" HeaderStyle-Width="120"
                                    ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItemRight " />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="quantity" HeaderText="数量" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn HeaderText="单价" DataField="price" HeaderStyle-Width="120" ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="amount_row" HeaderText="金额" HeaderStyle-Width="120" ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="amount_total" HeaderText="单据总金额" HeaderStyle-Width="120" ItemStyle-Width="120" DataFormatString="{0:N2}">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="showstatus" HeaderText="导入状态" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>

                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid>
                        <asp:HiddenField ID="hidflowid" runat="server" />
                    </div>
                </td>
            </tr>
            <%-- <tr>
                <td style="height: 30px">
                    <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
                    </pager:UcfarPager>
                    <input type="hidden" runat="server" id="hdwindowheight" />
                </td>
            </tr>--%>
        </table>

    </form>
</body>
</html>
