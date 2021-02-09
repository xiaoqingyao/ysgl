<%@ Page Language="C#" AutoEventWireup="true" CodeFile="userList.aspx.cs" Inherits="user_userList" %>

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
        function openinportexecl(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:730px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("btn_sele").click();
            }
        }
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:530px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("btn_sel").click();
            }
        }
        $(function() {

            initMainTableClass("<%=myGrid.ClientID%>");
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });

            $("#btn_importExcel").click(function() {
                openinportexecl("userList_ImportExcel.aspx");
            });

            $("#btn_toExcel").click(function() {
                window.open("UserToExcel.aspx");
            });
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#hf_user").val($(".highlight td:eq(0)").html());
            });
            $("#btn_new").click(function() {
                var deptCode = $("#hf_dept").val();
                openDetail("userDetails.aspx?deptCode=" + deptCode + "&type=add");
            });
            $("#btn_edit").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var deptCode = $("#hf_dept").val();
                var usercode = checkrow.find("td")[0].innerHTML;
                openDetail("userDetails.aspx?userCode=" + usercode + "&deptCode=" + deptCode + "&type=edit");
            });
        });

        function gudingbiaotou() {
            var t = document.getElementById("<%=myGrid.ClientID%>");
            if (t == null || t.rows.length < 1) {
                return;
            }
            var t2 = t.cloneNode(true);
            t2.id = "cloneGridView";
            for (i = t2.rows.length - 1; i > 0; i--) {
                t2.deleteRow(i);
            }
            t.deleteRow(0);
            header.appendChild(t2);
            var mainwidth = document.getElementById("main").style.width;
            mainwidth = mainwidth.substring(0, mainwidth.length - 2);
            mainwidth = mainwidth - 16;
            document.getElementById("header").style.width = mainwidth;
        }

        function Delete() {
            var Rows = $(".highlight");
            if (Rows.html() == undefined) {
                alert("请先选中要操作的条目！");
                return false;
            }
            else if (confirm('使用中的角色禁止删除,是否继续？')) {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return false; ;
                }
                else {
                    return true;
                }

            }
            else {
                return false;
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
        $(function() {
            initWindowHW();
        });

        function ResetCheck()
        {
         var Rows = $(".highlight");
         if (Rows.html() == undefined) {
             alert("请先选中要操作的条目！");
             return false;
         }
         else
         {
          return confirm("初始化后用户密码为用户编号，您确定要初始化吗?");
         }
        }

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
                &nbsp;<asp:Button ID="btn_del" runat="server" Text="删 除" CssClass="baseButton" OnClientClick="return Delete();"
                    OnClick="btn_del_Click" />
                <asp:Button ID="btn_resetPwd" runat="server" Text="初始化密码" CssClass="baseButton" OnClientClick="return ResetCheck()"
                    OnClick="btn_resetPwd_click" />
                <input class="baseButton" value="导入EXCEL" type="button" id="btn_importExcel" />
                <input class="baseButton" value="导出EXCEL" type="button" id="btn_toExcel" />
                <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
                <asp:HiddenField ID="hf_dept" runat="server" />
                <asp:HiddenField ID="hf_user" runat="server" />
            </td>
        </tr>
        <tr>
            <td style="height: 30px">
                &nbsp;编号/姓名：<asp:TextBox ID="txb_where" runat="server"></asp:TextBox>&nbsp;
                <asp:Button ID="btn_sel" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_sel_Click" />
                <asp:Label ID="Label1" runat="server" Text="当前部门：未选择，请在左侧选择" ForeColor="Red"></asp:Label>
            </td>
        </tr>
        <tr>
            <td align="left">
                <div id="divgrid" style="overflow-x: auto;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" Width="850px">
                        <Columns>
                            <asp:BoundColumn DataField="usercode" HeaderText="人员编号" HeaderStyle-Width="120px"
                                ItemStyle-Width="120px">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="username" HeaderText="人员姓名" HeaderStyle-Width="120px"
                                ItemStyle-Width="120px">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="usergroup" HeaderText="角色" HeaderStyle-Width="120px"
                                ItemStyle-Width="120px">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="userstatus" HeaderText="人员状态" HeaderStyle-Width="120px"
                                ItemStyle-Width="120px">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="userdept" HeaderText="所在部门" ItemStyle-Width="180" HeaderStyle-Width="180">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="userPosition" HeaderText="职务" HeaderStyle-Width="120px"
                                ItemStyle-Width="120px">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" />
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
    </table>  <script type="text/javascript">
                  parent.parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
