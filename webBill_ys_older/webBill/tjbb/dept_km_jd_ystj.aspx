<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dept_km_jd_ystj.aspx.cs" Inherits="webBill_tjbb_dept_km_jd_ystj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
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
                    <div>
                        年度：
                    <asp:DropDownList runat="server" ID="ddlNd">
                    </asp:DropDownList>
                        部门：<asp:DropDownList ID="drpDept" runat="server">
                        </asp:DropDownList>
                        <asp:Button ID="btn_find" runat="server" Text="查询" CssClass="baseButton" OnClick="btn_find_Click" />
                        <asp:Button ID="btn_excel" OnClick="btn_excel_Click" Text="导出excel" runat="server"
                            CssClass="baseButton" />
                        <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                    </div>
                    <div style="margin-top: 5px">
                        <label style="color: Red">
                            报表说明：该报表的统计结果为选中年度的选中部门的各月份年初填报的预算金额（不包含追加和调整）；执行率=决算/预算。</label>
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
