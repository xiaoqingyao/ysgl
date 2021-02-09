<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yskmList.aspx.cs" Inherits="yskm_yskmList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        //高亮显示
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            initWindowHW();
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#btn_importExcel").click(function () {
                openinportexecl("yskmList_ImportExcel.aspx");
            });

            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[0] != null) {
                    $("#kmcode").val($(this).find("td")[0].innerHTML);

                }
            });
            $("#btn_new").click(function () {
                var deptCode = $("#hf_km").val();
                openDetail("yskmDetails.aspx?type=add&pCode=" + deptCode);
            });
            $("#btn_edit").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var deptCode = $("#hf_km").val();
                var usercode = $("#kmcode").val(); //checkrow.find("td")[0].innerHTML;
                openDetail("yskmDetails.aspx?type=edit&kmCode=" + usercode + "&pCode=" + deptCode);
            });
        });

        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:530px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("Button4").click();
            }
        }

        function openinportexecl(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:730px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("btn_sele").click();
            }
        }

        $(function () {
            $("#btn_toExcel").click(function () {
                window.open("YskmToExcel.aspx");
            });
        });

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


    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 30px">
                    <asp:CheckBox ID="chkNextLevel" runat="server" AutoPostBack="True" OnCheckedChanged="chkNextLevel_CheckedChanged"
                        Text="包含下级" Checked="True" />&nbsp;
                <asp:CheckBox ID="CheckBox2" runat="server" Text="全部状态" AutoPostBack="True" OnCheckedChanged="CheckBox2_CheckedChanged" />
                    <input class="baseButton" type="button" id="btn_new" value="新 增" />
                    <input class="baseButton" type="button" id="btn_edit" value="修 改" />
                    <asp:Button ID="Button3" runat="server" Text="删 除" CssClass="baseButton" OnClientClick="return confirm('删除预算科目,会关联删除相关的财务科目对照关系及下级预算科目,是否继续？');"
                        OnClick="Button3_Click" />
                    <asp:Button ID="Button5" runat="server" CssClass="baseButton" OnClick="Button5_Click1"
                        Text="禁 用" />
                    <asp:Button ID="Button6" runat="server" CssClass="baseButton" OnClick="Button6_Click"
                        Text="启 用" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
                </td>
            </tr>
            <tr>
                <td style="height: 30px">&nbsp;编号/名称：<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    <asp:Button ID="Button4" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button4_Click" />
                    <input class="baseButton" value="导入EXCEL" type="button" id="btn_importExcel" />
                    <input class="baseButton" value="导出EXCEL" type="button" id="btn_toExcel" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" BorderWidth="1px"
                            CellPadding="3" CssClass="myGrid" Width="1400px">
                            <Columns>
                                <asp:BoundColumn DataField="yskmcode" HeaderText="科目编码" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yskmmc" HeaderText="科目名称" HeaderStyle-Width="280" ItemStyle-Width="280">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="tblx" HeaderText="填报类型" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="status" HeaderText="状态" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="kmlx" HeaderText="费用类型" HeaderStyle-Width="110" ItemStyle-Width="110">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="gkfy" HeaderText="归口费用" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="xmhs" HeaderText="项目核算" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="bmhs" HeaderText="部门核算" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ryhs" HeaderText="人员核算" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="tbsm" HeaderText="填报说明" HeaderStyle-Width="300" ItemStyle-Width="300">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid>&nbsp;<asp:HiddenField ID="hf_km" runat="server" />
                        <asp:HiddenField ID="kmcode" runat="server" />
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
