<%@ Page Language="C#" AutoEventWireup="true" CodeFile="YSBalance.aspx.cs" Inherits="webBill_tjbb_YSBalance" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算平衡表</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        $(function() {
            $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            initWindowHW();
            initMainTableClass("<%=GridView1.ClientID%>");

            $("#GridView1 tr").each(function(index, obj) {
                if (index > 1 && index < 4) {

                    $(this).find("td:gt(0)").each(function(i, item) {
                        var temp = $(this).html();
                        if (parseFloat(temp) == 0) {
                            return;
                        }
                        else {
                            var nd = $("#ddlNd").val();
                            var yf = parseInt((i / 2)) + 1;
                            var zjflag = index == 2 ? 1 : 0; //0支出 费用 1收入
                            var factflg = (i % 2) == 1 ? "1" : "0";
                            var str = "<a style='cursor:pointer;text-decoration:underline;' onclick='openDetail(" + nd + "," + yf + "," + zjflag + "," + factflg + ")'>" + temp + "</a>";
                            $(this).html(str);
                        }
                    });
                }
            });
        });

        function openDetail(nd, yf, flag, factflg) {
            window.location.href = 'YSBalance_mx.aspx?nd=' + nd + '&yf=' + yf + '&flag=' + flag + '&fcflg=' + factflg;
            //window.showModalDialog('YSBalance_mx.aspx?nd=' + nd + '&yf=' + yf + '&flag=' + flag + '&fcflg=' + factflg, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:400px;status:no;scroll:yes');
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
        function initWindowHW() {
            //给gridview表格外部的div设置宽度
            $("#divData").css("width", ($(window).width() - 5));
        }


    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div>
        <div id="divOption" style="margin: 5px">
            年度:
            <asp:DropDownList ID="ddlNd" runat="server">
            </asp:DropDownList>
            <asp:Button ID="btn_find" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_find_Click" />
            <asp:Button ID="btn_excel" runat="server" Text="导出excel" CssClass="baseButton" OnClick="btn_excel_Click" />
            <input type="button" class="baseButton" value="帮 助" onclick="javascript:parent.helptoggle();" />
            <label style="color: Red">
                【友情提示】：单击“收入”或“支出”行内的金额可查看对应的明细。
            </label>
        </div>
        <div id="divData" style="overflow-x: auto">
            <asp:GridView ID="GridView1" runat="server" CssClass="myGrid" AutoGenerateColumns="False"
                OnRowCreated="GridView1_Created" OnPreRender="GridView1_PreRender" ShowFooter="True"
                OnRowDataBound="GridView1_RowDataBound">
                <HeaderStyle CssClass="myGridHeader" />
                <RowStyle CssClass="myGridItem" />
                <FooterStyle CssClass="myGridItem" />
            </asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>
