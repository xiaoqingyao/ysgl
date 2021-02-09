<%@ Page Language="C#" AutoEventWireup="true" CodeFile="xmList.aspx.cs" Inherits="Dept_deptList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

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

        function selectDept() {
            var deptCode = '<%=Request["deptCode"] %>';
            if (deptCode == undefined || deptCode == "") {
                alert("请先选择部门"); return;
            }
            var returnValue = window.showModalDialog('CopyDeptXm.aspx?deptCode=' + deptCode + '', 'newwindow', 'center:yes;dialogHeight:260px;dialogWidth:630px;status:no;scroll:yes');
            if (returnValue != undefined && returnValue != "" && returnValue != "no") {
                location.replace(location.href);
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
        function fh() {
            window.history.go(-1);
            return;

            //返回列表页ysglnew/ystbFrame.aspx?yskmtype=02&limittotal=1&tbtype=zxes&isdz=1&xkfx=1
          <%--  var yskmtype = '<%=Request["yskmtype"]%>';
            var tbtype = '<%=Request["tbtype"]%>';
            var limittotal = '<%=Request["limittotal"]%>';
            var jecheckflg = '<%=Request["jecheckflg"]%>';
            var xmcode = '<%=Request["xmcode"]%>';
            var url = "ystbFrame.aspx?yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal + "&jecheckflg=" + jecheckflg;
            var isdz = '<%= Request["isdz"]%>';//是否是大智学校的标记
                 if (isdz != null || isdz != undefined || isdz == "") {
                     url = url + "&isdz=" + isdz;
                 }

                 var xkfx = '<%=Request["xkfx"]%>';//新开分校标记
            if (xkfx != null || xkfx != undefined || xkfx == "") {
                url = url + "&xkfx=" + xkfx;
            }
            if (xmcode != null || xmcode != undefined || xmcode == "") {
                url = url + "&xmcode=" + xmcode
            }
            window.location.href = "../ysglnew/ystbFrame.aspx?yskmtype=02&limittotal=1&tbtype=zxes&isdz=1&xkfx=1";--%>
        }
        $(function () {
            initWindowHW();
        });


    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 30px">
                    <asp:CheckBox ID="chkNextLevel" runat="server" AutoPostBack="True" OnCheckedChanged="chkNextLevel_CheckedChanged"
                        Text="包含下级" Checked="True"  Visible="false" />
                    <input type="button" value="返回列表页" class="baseButton" onclick="fh()" id="fanhui" runat="server" />
                    <asp:Button ID="btn_ystb" CssClass="baseButton" Text="预算填报" runat="server" OnClick="btn_ystb_Click" />
                    <asp:Button ID="btn_xmyshz" CssClass="baseButton" Text="项目预算汇总" runat="server" OnClick="btn_xmyshz_Click" />
                    &nbsp;<asp:Button ID="btn_add" runat="server" Text="增 加" CssClass="baseButton" OnClick="btn_add_Click" />
                    &nbsp;<asp:Button ID="btn_edit" runat="server" Text="修 改" CssClass="baseButton" OnClick="btn_edit_Click" />
                    &nbsp;<asp:Button ID="btn_dele" runat="server" Text="删 除" CssClass="baseButton" OnClick="btn_dele_Click" />
                    <%-- OnClientClick="return confirm('删除项目会关联删除相关的预算数据,是否继续？');"--%>
                    <input id="btn_Copy" type="button" value="复制其它部门项目" class="baseButton" onclick="selectDept();"
                        runat="server" />
                    <input type="button" class="baseButton" style="display: none" value="帮助" onclick="javascript: parent.parent.helptoggle();" />


                </td>
            </tr>
            <tr>
                <td style="height: 30px">&nbsp;编号/名称:<asp:TextBox ID="txb_where" runat="server"></asp:TextBox>
                    <asp:Button ID="btn_sele" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_sele_Click" />
                    <asp:Label ID="Label1" runat="server" Text="当前部门：未选择，请在左侧选择" ForeColor="Red"></asp:Label>
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" Width="910px">
                            <Columns>
                                <asp:TemplateColumn HeaderText="选择" ItemStyle-Width="38">
                                    <ItemTemplate>
                                        &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" />
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                        Width="38px" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" Width="38px" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="xmCode" HeaderText="项目编号" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="xmName" HeaderText="项目名称" HeaderStyle-Width="200px" ItemStyle-Width="200px">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="sjxm" HeaderText="上级项目编号" HeaderStyle-Width="100px" ItemStyle-Width="100px">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="sjxmname" HeaderText="上级项目名称" HeaderStyle-Width="120px"
                                    ItemStyle-Width="120px">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="xmDeptname" HeaderText="所属部门" HeaderStyle-Width="200px"
                                    ItemStyle-Width="200px">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="xmStatus" HeaderText="项目状态" HeaderStyle-Width="100px"
                                    ItemStyle-Width="100px">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
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
    </form>
</body>
</html>
