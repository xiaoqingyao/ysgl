<%@ Page Language="C#" AutoEventWireup="true" CodeFile="yszxqk_jd_result.aspx.cs" Inherits="webBill_search_yszxqk_jd_result" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>部门月预算明细</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript" charset="utf-8">
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            gudingbiaotouzidingyi($("#GridView1"), $(window).height() - 160);
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                $("#<%=GridView1.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
        });

        function gudingbiaotouzidingyi(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tbody tr:gt(0)").css({ display: "none" });
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "-1px 0 0 0");
            obj.before(gvn);
            obj.find("thead").remove();
            obj.find("tbody tr:lt(1)").css({ display: "none" });
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table>
            <tr>
                <td>
                    <input type="button" class="baseButton" value="返回" onclick="window.location.href = 'yszxqk_jz_select.aspx'" />
                    <div style="margin-top: 5px">
                        <label style="color: Red">
                            报表说明：该报表的统计结果为选中年度的选中部门的各月份年初填报的预算金额(不包含追加和调整);年度累计执行率=累计决算/年度预算</label>
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="position: relative; word-warp: break-word; word-break: break-all">
                        <asp:GridView ID="GridView1" runat="server" CssClass="myGrid" AutoGenerateColumns="False"
                            EnableViewState="false" OnPreRender="GridView1_PreRender" ShowFooter="true" OnRowCreated="GridView1_OnRowCreated"
                            OnRowDataBound="GridView1_RowDataBound">
                            <HeaderStyle CssClass="myGridHeader" />
                            <RowStyle CssClass="myGridItem" />
                            <FooterStyle CssClass="myGridItem" />
                        </asp:GridView>

                    </div>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
