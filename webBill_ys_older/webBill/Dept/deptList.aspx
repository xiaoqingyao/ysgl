<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deptList.aspx.cs" Inherits="Dept_deptList" %>

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

        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#btn_importExcel").click(function() {
                openinportexecl("deptList_ImportExcel.aspx");
            });
            $("#btn_excel").click(function() {
                window.open("DeptToExcel.aspx");
            });
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#hf_user").val($(".highlight td:eq(0)").html());
            });
            $("#btn_new").click(function() {
                var deptCode = $("#hf_dept").val();
                openDetail("deptDetails.aspx?type=add&pCode=" + deptCode);
            });
            $("#btn_edit").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var usercode = checkrow.find("td")[0].innerHTML;
                openDetail("deptDetails.aspx?type=edit&deptCode=" + usercode);
            });
        });
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:530px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("btn_sele").click();
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
        function openSelectZg(deptCode) {
            var returnValue = window.showModalDialog('selctZgUser.aspx?deptCode=' + deptCode + '', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:630px;status:no;scroll:yes');
        }

        function openSelectLd(deptCode) {
            var returnValue = window.showModalDialog('selctLdUser.aspx?deptCode=' + deptCode + '', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:630px;status:no;scroll:yes');
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
        $(function() {
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
                    Text="包含下级" Checked="True" />
                &nbsp;<input class="baseButton" type="button" id="btn_new" value="新 增" />
                &nbsp;<input class="baseButton" type="button" id="btn_edit" value="修 改" />
                &nbsp;<asp:Button ID="btn_dele" runat="server" Text="删 除" CssClass="baseButton" OnClientClick="return confirm('使用中的部门禁止删除,是否继续？');"
                    OnClick="btn_dele_Click" />
                &nbsp;
                <input type="button" value="导入EXCEL" id="btn_importExcel" class="baseButton" />&nbsp;
                <input type="button" value="导出EXCEL" id="btn_excel" class="baseButton" />
                <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
                <asp:HiddenField ID="hf_dept" runat="server" />
                <asp:HiddenField ID="hf_user" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="height: 30px">
                 &nbsp;名称/编号：<asp:TextBox ID="txb_where" runat="server"></asp:TextBox>
                &nbsp;<asp:Button ID="btn_sele" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_sele_Click" />
                <asp:Label ID="Label1" runat="server" Text="当前部门：未选择，请在左侧选择" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td>
                <div id="divgrid" style="overflow-x: auto;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        BorderWidth="1px" CssClass="myGrid" Width="1200px" OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                            <asp:BoundColumn DataField="deptCode" HeaderText="部门编号" HeaderStyle-Width="100" ItemStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptName" HeaderText="部门名称" HeaderStyle-Width="250" ItemStyle-Width="250">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="sjDeptCode" HeaderText="上级部门" HeaderStyle-Width="100"
                                ItemStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="业务主管" HeaderStyle-Width="100" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <input id="btnSetZg" type="button" value="设置" class="baseButton" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="False" CssClass="myGridItem" Width="100px" HorizontalAlign="Center" />
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderText="分管领导" HeaderStyle-Width="100" ItemStyle-Width="100">
                                <ItemTemplate>
                                    <input id="btnSetLd" type="button" value="设置" class="baseButton" runat="server" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="False" CssClass="myGridItem" Width="100px" HorizontalAlign="Center" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="deptJianma" HeaderText="部门简码" HeaderStyle-Width="80"
                                ItemStyle-Width="80">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptStatus" HeaderText="状态" HeaderStyle-Width="80" ItemStyle-Width="80">
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
         <script type="text/javascript">
             parent.parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
