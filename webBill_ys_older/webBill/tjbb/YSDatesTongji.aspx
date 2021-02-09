<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YSDatesTongji.aspx.cs" Inherits="webBill_tjbb_YSDatesTongji" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>日期期间查询（查询2）</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <style type="text/css">
        .NoBreak
        {
            white-space: pre;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script type="text/javascript" language="javascript" charset="utf-8">
        $(function() {
        $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                parent.helptoggle();
                }
            });
            gudingbiaotounew($("#GridView1"), 380);
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=GridView1.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
            $("#txt_beg").datepicker();
            $("#txt_end").datepicker();
        });
        function gudingbiaotou() {
            var t = document.getElementById("<%=GridView1.ClientID%>");
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
        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "-1px 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
    </script>

</head>
<body>
    <form id="form2" runat="server">
    <table>
        <tr>
            <td>
                <div>
                    <asp:Button ID="btn_excel" runat="server" Text="导出excel" CssClass="baseButton" OnClick="btn_excel_Click" />
                    开始时间:
                    <asp:TextBox ID="txt_beg" runat="server"></asp:TextBox>
                    结束时间:
                    <asp:TextBox ID="txt_end" runat="server"></asp:TextBox>
                    <asp:Button ID="btn_find" runat="server" Text="查询" CssClass="baseButton" OnClick="btn_find_Click" />
                      <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                </div>
            </td>
        </tr>
        <tr>
            <td>
                <div style="position: relative; word-warp: break-word; word-break: break-all">
                    <asp:GridView ID="GridView1" runat="server" CssClass="myGrid" AutoGenerateColumns="False"
                        OnPreRender="GridView1_PreRender" ShowFooter="False">
                        <HeaderStyle CssClass="myGridHeader" />
                        <RowStyle CssClass="myGridItem" />
                    </asp:GridView>
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>