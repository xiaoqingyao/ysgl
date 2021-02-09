<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ybbxEditMain.aspx.cs" Inherits="bxd_ybbxEditMain" %>

<!DOCTYPE >
<html>
<head id="Head1" runat="server">
    <title>一般报销表头信息编辑页</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="../js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <link href="../Css/CommenCss.css" rel="stylesheet" type="text/css" />

    <script src="../js/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            ddlIsGkChange();

        });

        function ddlIsGkChange() {
            var isgk = $("#ddlIsGk").val();
            if (isgk == "0") {
                $("#tdgk").hide();
            }
            else {
                $("#tdgk").show();
            }

        }


        function Check() {
            var date = $("#txtBillDate").val().length;
            var isgk = $("#ddlIsGk").val();
            var gkdept = $("#ddlGkDept").val().length;
            var bxzy = $("#txtBxzy").val().length;
            if (date == 0) {
                ShowMsg("请输入申请日期");
                $("#txtBillDate").focus();
                return false;
            }
            else if (isgk == "1" && gkdept == 0) {
                ShowMsg("请选择归口部门");
                $("#ddlGkDept").focus();
                return false;
            }
            else if (bxzy == 0) {
                ShowMsg("请输入报销摘要");
                $("#txtBxzy").focus();
                return false;
            }
            else {
                return true;
            }

        }

        function ShowMsg(msg) {
            $("#popdiv").text(msg);
            $('#popdiv').css({ width: $(window).width() }).show(300).delay(1000).hide(3000);
            $("#popup").click();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false">
        <div>
            <div data-role="header">
                <a data-icon="home" data-ajax="false" onclick="ConfirmReturn('../Index.aspx','单据未保存，确定要返回吗')">主页</a>
                <h1>
                    <asp:Label ID="lbdjmc" runat="server" Text="Label"></asp:Label></h1>
                <a href="ybbxList.aspx" data-role="button" class="ui-btn-right" data-icon="grid"
                    data-ajax="false">我的单据</a>
            </div>
            <div data-role="content">
                <a id="popup" data-role="button" href="#popdiv" data-rel="popup" data-position-to="origin"
                    style="display: none;">弹出层</a>
                <div id="popdiv" class="pupMsg" data-role="popup" data-overlay-theme="a" data-theme="c"
                    class="ui-content">
                    操作成功！
                </div>
                <a href="../select/selectDept.aspx" data-role="button" data-rel="dialog" data-transition="pop"
                    id="open" style="display: none;">弹出框 </a>
                <table>
                    <tr>
                        <td style="width: 80px;">单据号：
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillCode" runat="server" Text="" ReadOnly="true" BackColor="#FFD9D9D9"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>申请日期：
                        </td>
                        <td>

                            <asp:TextBox ID="txtBillDate" runat="server" type="date"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>制单人：
                        </td>
                        <td>

                            <asp:TextBox ID="txtZdr" runat="server" ReadOnly="true" BackColor="#FFD9D9D9"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>所在部门：
                        </td>
                        <td>
                            <asp:TextBox ID="txtBillDept" runat="server" ReadOnly="true" BackColor="#FFD9D9D9"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>是否归口：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlIsGk" runat="server" data-role="slider" onchange="ddlIsGkChange()">
                                <asp:ListItem Value="1">是</asp:ListItem>
                                <asp:ListItem Selected="true" Value="0">否</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr id="tdgk" runat="server">
                        <td>归口部门
                        </td>
                        <td style="position: relative">
                            <asp:DropDownList ID="ddlGkDept" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>报销类型：
                        </td>
                        <td>
                            <asp:DropDownList ID="ddlBxmxlx" runat="server">
                            </asp:DropDownList>
                        </td>
                    </tr>
                    <tr>
                        <td>报销摘要：
                        </td>
                        <td>
                            <asp:TextBox ID="txtBxzy" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>报销说明：
                        </td>
                        <td>
                            <asp:TextBox ID="txtBxsm" runat="server" TextMode="MultiLine"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" align="center">
                            <asp:Button ID="btnSave" runat="server" Text="添加报销明细" data-inline="true" OnClientClick="return Check()"
                                OnClick="btnSave_Click" />
                        </td>
                    </tr>

                    <%--                    测试后台获取值和赋值
                    <tr style="display: none">
                        <td>
                            <input id="Text1" name="a" type="date" value="<%=strdate %>" />
                            <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />

                        </td>
                    </tr>--%>
                </table>
                <asp:HiddenField ID="hd_djtype" runat="server" />
                <asp:HiddenField ID="hfdydj" runat="server" />
            </div>
            <div data-role="footer" data-position="fixed">
                <footer data-role="footer" id="footer">
                    <h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1>
                </footer>
            </div>
        </div>
    </form>
</body>
</html>
