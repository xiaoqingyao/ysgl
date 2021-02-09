<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Lrbmx.aspx.cs" Inherits="webBill_ysglnew_Lrbmx" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.helptoggle();
                }
            });
            initMainTableClass("<%=GridView1.ClientID%>");
            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[0] != null && $(this).find("td")[0].innerHTML != "") {
                    $("#procode").val($(this).find("td")[0].innerHTML);

                }
            });
            gudingbiaotounew($("#GridView1"), 380);
        });
        function SetMoney(obj) {

            var MainTable = document.getElementById("GridView1");

            for (var s = 1; s < MainTable.rows.length; s++) {

                if (MainTable.rows[s].cells[5].innerHTML == "本表计算") {
                    var firstMoney = 0;

                    if (MainTable.rows[1].cells[5].innerHTML == "直接录入") {
                        var x = MainTable.rows[1].cells[6].childNodes[0].value;
                        firstMoney = parseFloat(x == "" ? "0" : x);
                    }
                    else {
                        var x1 = MainTable.rows[1].cells[6].innerHTML;
                        firstMoney = parseFloat(x1 == "" ? "0" : x1);
                    }
                    for (var i = 1; i < s; i++) {
                        if (MainTable.rows[i].cells[5].innerHTML == "直接录入") {
                            var m = MainTable.rows[i].cells[6].childNodes[0].value;
                            if (MainTable.rows[i].cells[4].innerHTML == "加") {
                                firstMoney += parseFloat(m == "" ? "0" : m);
                            }
                            if (MainTable.rows[i].cells[4].innerHTML == "减") {
                                firstMoney -= parseFloat(m == "" ? "0" : m);
                            }
                        }
                        else {
                            var v = MainTable.rows[i].cells[6].innerHTML;
                            if (MainTable.rows[i].cells[4].innerHTML == "加") {
                                firstMoney += parseFloat(v == "" ? "0" : v);
                            }
                            if (MainTable.rows[i].cells[4].innerHTML == "减") {
                                firstMoney -= parseFloat(v == "" ? "0" : v);
                            }
                        }
                    }
                    if (isNaN(firstMoney)) {
                        alert("输入错误");
                        $("#" + obj.id).val("");
                        return;
                    }
                    else {
                        MainTable.rows[s].cells[6].innerHTML = firstMoney.toFixed(2);
                    }
                }
            }
        }
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
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
    </script>

    <style type="text/css">
        .Hidden {
            display: none;
        }

        .righttxt {
            text-align: right;
            background-color: Transparent;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td>
                    <div class="baseDiv" style="margin-top: 3px;">
                        年度：
                    <asp:DropDownList ID="drpNd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpNd_SelectedIndexChanged">
                    </asp:DropDownList>
                        <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click" />
                        <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                    </div>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="position: relative; word-warp: break-word; word-break: break-all">
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" Style="table-layout: fixed" Width="800px" AllowPaging="True"
                            PageSize="9999">
                            <Columns>
                                <asp:BoundField DataField="" HeaderText="序号">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Width="30" Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Width="30" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:BoundField>
                                <asp:BoundField DataField="annual" HeaderText="年度">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" Width="80" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" Width="80" />
                                </asp:BoundField>
                                <asp:BoundField DataField="procode" HeaderText="编号">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Width="100" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                        CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Width="100" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                                </asp:BoundField>
                                <asp:BoundField DataField="proname" HeaderText="名称">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" Width="200" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" Width="200" />
                                </asp:BoundField>
                                <asp:BoundField DataField="calculatype" HeaderText="计算方式" ItemStyle-CssClass="Hidden"
                                    HeaderStyle-CssClass="Hidden">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" />
                                </asp:BoundField>
                                <asp:BoundField DataField="fillintype" HeaderText="录入方式" ItemStyle-CssClass="Hidden"
                                    HeaderStyle-CssClass="Hidden">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" />
                                </asp:BoundField>
                                <asp:TemplateField HeaderText="金额" ItemStyle-CssClass=" righttxt" ItemStyle-Width="150">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Width="150" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                        CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItemRight" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_je" runat="server" Width="140" onblur="SetMoney(this)" CssClass="righttxt"
                                            Text='<%#Eval("je","{0:N2}") %>'></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
