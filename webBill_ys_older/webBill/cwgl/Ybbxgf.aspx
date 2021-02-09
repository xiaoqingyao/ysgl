<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Ybbxgf.aspx.cs" Inherits="webBill_cwgl_Ybbxgf" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>一般报销给付</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .highlight {
            background: #EBF2F5;
        }

        .hiddenbill {
            display: none;
        }

        td {
            text-align: left;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            // gudingbiaotounew($("#myGrid"), 380);
            initWindowHW();
            $("#find_txt_time1").datepicker();
            $("#find_txt_time2").datepicker();
            initMainTableClass("<%=myGrid.ClientID%>");

            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {

                if ($(this).find("td")[0] != null) {
                    var billCode = $(this).find("td")[0].innerHTML;
                }

                $("#hd_billCode").val(billCode);
                $.post("../MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function (data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            $("#btn_look").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("../bxgl/bxDetailNew.aspx?type=look&billCode=" + billcode);
            });

            $("#btn_gf").click(function () {
                var isedit = $("#hd_isedit").val();
                var gfafterpingzheng = $("#hd_GfAfterPingzheng").val();
                var billCode = $("#hd_billCode").val();
                if (billCode == "") {
                    alert("请先选择单据");
                    return;
                }
                var checkrow = $(".highlight");
                if (gfafterpingzheng == '1') {
                    var status = checkrow.find("td")[8].innerHTML;
                    if (status == "&nbsp;") {
                        alert("该报销单未做凭证，不允许支付！");
                        return false;
                    }
                }
                var sfgf = checkrow.find("td")[9].innerHTML;
                if (sfgf != "未给付") {
                    alert("该报销单已经给付！");
                    return false;
                }
                if (isedit == "yes") {
                    openDetail("BxgfDetail.aspx?type=edit&billCode=" + billCode);
                    $("#btn_summit").click();
                }
                else {
                    if (confirm('确认给付？')) {
                        $("#Button1").click();
                    }
                }
            });

            //mxl
            //            $('#dialog').dialog({
            //                autoOpen: false,
            //                width: 500,
            //                buttons: {
            //                    "确定": function() {
            //                        $(this).dialog("close");
            //                        $("#btn_summit").click();
            //                    },
            //                    "取消": function() {
            //                        $(this).dialog("close");
            //                    }
            //                }
            //            });
            $("#btn_find").click(function () {
                document.getElementById("dialog").style.display = document.getElementById("dialog").style.display == "" ? "none" : "";
            });

            $("#find_txt_user").autocomplete({
                source: availableTags
            });
            $("#find_txt_dept").autocomplete({
                source: availableTags2
            });
            //刷新
            $("#btn_refresh").click(function () {
                location.replace(location.href);
            });
        });

        function openDetail(openUrl) {
            var ret = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
            if (ret == "1") {
                $("#Button4").click();
            }
        }
        function openLookSpStep(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }


        //替换非数字
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
            //给gridview表格外部的div设置宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 27px">
                    <input class="baseButton" id="btn_refresh" value="刷 新" type="button" />
                    <input id="btn_find" type="button" value="查 询" class="baseButton" />
                    <asp:Button ID="Button1" runat="server" Text="报销给付" CssClass="baseButton" OnClick="Button1_Click"
                        Style="display: none" />
                    <input class="baseButton" value="报销给付" type="button" id="btn_gf" />
                    <input type="button" value="详细信息" id="btn_look" class="baseButton" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                    <asp:HiddenField ID="hd_isedit" runat="server" />
                    <asp:HiddenField ID="hd_GfAfterPingzheng" runat="server" />
                    <asp:HiddenField ID="hd_billCode" runat="server" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="dialog" title="查询" style="display: none; float: left;">
                        <table class="baseTable">
                            <tr>
                                <td>所属部门:
                                </td>
                                <td>
                                    <asp:TextBox ID="find_txt_dept" runat="server"></asp:TextBox>
                                </td>
                                <td>报销人:
                                </td>
                                <td>
                                    <asp:TextBox ID="find_txt_user" runat="server"></asp:TextBox>
                                </td>
                                <td>起日期:
                                </td>
                                <td>
                                    <asp:TextBox ID="find_txt_time1" runat="server"></asp:TextBox>
                                </td>
                                <td>止日期:
                                </td>
                                <td>
                                    <asp:TextBox ID="find_txt_time2" runat="server"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td>起金额:
                                </td>
                                <td>
                                    <asp:TextBox ID="find_txt_money1" runat="server" onkeyup="replaceNaN(this);"></asp:TextBox>
                                </td>
                                <td>止金额:
                                </td>
                                <td>
                                    <asp:TextBox ID="find_txt_money2" runat="server" onkeyup="replaceNaN(this);"></asp:TextBox>
                                </td>
                                <td>单据编号：
                                </td>
                                <td>
                                    <asp:TextBox ID="find_txt_djcode" runat="server"></asp:TextBox>
                                </td>
                                <td>是否给付
                                </td>
                                <td>
                                    <asp:DropDownList runat="server" ID="isGF" Width="146">
                                        <asp:ListItem Value="">--全部--</asp:ListItem>
                                        <asp:ListItem Value="1">已给付</asp:ListItem>
                                        <asp:ListItem Value="0" Selected="True">未给付</asp:ListItem>
                                    </asp:DropDownList>
                                </td>
                            </tr>
                            <tr>
                                <td>开户银行:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_khh" runat="server"></asp:TextBox>
                                </td>
                                <td>账号:
                                </td>
                                <td>
                                    <asp:TextBox ID="txt_zh" runat="server"></asp:TextBox>
                                </td>
                                <td>
                                    <asp:Button ID="btn_summit" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_summit_Click" />
                                    <input id="Button2" type="button" value="取 消" class="baseButton" onclick="javascript: document.getElementById('dialog').style.display = 'none'" />
                                </td>
                            </tr>
                        </table>
                    </div>
                    <div id="divgrid" style="position: relative; overflow-x: auto; word-warp: break-word; word-break: break-all">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" Width="1200px" AllowPaging="false" OnItemDataBound="myGrid_ItemDataBound">
                            <Columns>
                                <asp:BoundColumn DataField="billCode" HeaderText="billCode">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader hiddenbill" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem hiddenbill" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billName" HeaderText="单据编号" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billUser" HeaderText="报销人" HeaderStyle-Width="60" ItemStyle-Width="60">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDate" HeaderText="报销申请日期" DataFormatString="{0:D}"
                                    HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billDept" HeaderText="所属部门" 
                                    HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="billJe" HeaderText="报销总额" DataFormatString="{0:F2}" HeaderStyle-Width="100"
                                    ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItemRight" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="stepid" HeaderText="状态" HeaderStyle-Width="60" ItemStyle-Width="60">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="pzcode" HeaderText="凭证编号" HeaderStyle-Width="60" ItemStyle-Width="60">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="pzdate" HeaderText="凭证日期" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="sfgf" HeaderText="是否给付" HeaderStyle-Width="60" ItemStyle-Width="60">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <%--  <asp:BoundColumn DataField="khh" HeaderText="开户行" HeaderStyle-Width="120" ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="zh" HeaderText="账号" HeaderStyle-Width="120" ItemStyle-Width="120">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>--%>

                                <asp:BoundColumn DataField="flowID" HeaderText="单据类型" HeaderStyle-Width="100" ItemStyle-Width="100">
                                      <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
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
            <tr>
                <td style="height: 10px">
                    <table>
                        <tr>
                            <td>审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
                            </td>
                            <td id="wf"></td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
