<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xmDeptList.aspx.cs" Inherits="webBill_xmsz_xmDeptList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            initMainTableClass("<%=myGrid.ClientID%>");

        });
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:480px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("btn_sele").click();
            }
        }

        function openSelectZg(deptCode) {
            var returnValue = window.showModalDialog('selctZgUser.aspx?deptCode=' + deptCode + '', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:630px;status:no;scroll:yes');
        }

        function openSelectLd(deptCode) {
            var returnValue = window.showModalDialog('selctLdUser.aspx?deptCode=' + deptCode + '', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:630px;status:no;scroll:yes');
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

        function Istx(txt) {
            var tds = txt.parentNode;
            $("#" + tds.childNodes[4].id).val("1");
        }

        function SelectAll(aControl) {
            var chk = document.getElementById("myGrid").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
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
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
        $(function () {
            initWindowHW();
        });

    </script>

    <style type="text/css">
        .txtright {
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 30px">&nbsp;年度：
                <asp:DropDownList ID="drpNd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpNd_SelectedIndexChanged">
                </asp:DropDownList>
                    &nbsp;
                <asp:Button ID="Button2" runat="server" Text="导出EXCEL" CssClass="baseButton" OnClick="Button2_Click" />
                    &nbsp;<asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="保 存" OnClick="Button1_Click" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
                    <input id="btn_Copy" type="button" value="复制其它部门项目" class="baseButton" onclick="selectDept();"
                        runat="server" style="display: none;" />
                    &nbsp;编号/名称：<asp:TextBox ID="txb_where" runat="server"></asp:TextBox>
                    <asp:Button ID="btn_sele" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_sele_Click" />
                </td>
            </tr>
            <tr>
                <td style="height: 30px">&nbsp;操作说明：填写完毕后,点击保存。
                <asp:Label ID="Label1" runat="server" Text="当前部门：未选择，请在左侧选择" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" Width="910px">
                            <Columns>
                                <asp:TemplateColumn HeaderText="选择" HeaderStyle-Width="50" ItemStyle-Width="50">
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
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="nd" HeaderText="年度" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="xmCode" HeaderText="项目" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="xmDept" HeaderText="所属部门" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="是否控制" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <ItemTemplate>
                                        <asp:DropDownList ID="ddlIsCtrl" runat="server" Width="45">
                                            <asp:ListItem Value="0" Text="否">否</asp:ListItem>
                                            <asp:ListItem Value="1" Text="是">是</asp:ListItem>
                                        </asp:DropDownList>
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:TemplateColumn>
                                <asp:TemplateColumn HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem"
                                    HeaderText="预算控制金额">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Width="200" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Width="200" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_je" runat="server" Text='0.00' onfocus="Istx(this)" onkeyup="replaceNaN(this);"
                                            CssClass="txtright"></asp:TextBox>
                                        <asp:HiddenField ID="hfxmCode" runat="server" Value='<%#Eval("xmCode") %>' />
                                        <asp:HiddenField ID="hfxmDept" runat="server" Value='<%#Eval("xmDept") %>' />
                                        <asp:HiddenField ID="hiddistx" runat="server" Value='<%#Eval("isCtrl") %>' />
                                        <asp:HiddenField ID="hfStatus" runat="server" Value='<%#Eval("status") %>' />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbeTotalAmount" runat="server" Text="0"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
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
        <script type="text/javascript">
            parent.parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
