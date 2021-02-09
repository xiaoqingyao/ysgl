<%@ Page Language="C#" AutoEventWireup="true" CodeFile="LrbXmfj.aspx.cs" Inherits="webBill_ysglnew_LrbXmfj" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript">
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            gudingbiaotounew($("#myGrid"), $(window).height() - 100);
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                if ($(this).find("td")[0] != null && $(this).find("td")[0].innerHTML != "") {
                    $("#procode").val($(this).find("td")[0].innerHTML);

                }
            });

        });
        function Istx(txt) {
            var tds = txt.parentNode;
            $("#" + tds.childNodes[6].id).val("1");
        }
        //替换非数字
        function replaceNaN(obj) {
            var objval = obj.value;
            if (objval.indexOf("-") == 0) {
                objval = objval.substr(1);
            }
            if (isNaN(objval)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
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

        function ImportExcel() {
            if (confirm('导入将会将原来做的对应年度的分解金额冲掉，确定继续吗？')) {
                window.showModalDialog("LrbXmfj_ImportExcel.aspx", 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:730px;status:no;scroll:no');
            }
        }
    </script>

    <style type="text/css">
        .txtright {
            text-align: right;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="90%" style="margin-left: 5px">
            <tr>
                <td>
                    <div style="" style="margin-top: 3px; margin-left: 3px">
                        财年：
                <asp:DropDownList ID="drpNd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpNd_SelectedIndexChanged">
                </asp:DropDownList>
                        <%-- <asp:TextBox ID="TextBox1" runat="server" Width="79px"></asp:TextBox>
                <asp:Button ID="btn_Add" runat="server" Text="增 加" CssClass="baseButton" OnClick="btn_Add_Click" />--%>
                预算项目：
                <asp:DropDownList ID="txtcx" runat="server" AutoPostBack="True" OnSelectedIndexChanged="btcx_Click">
                </asp:DropDownList>
                        &nbsp;
                <asp:Button ID="Button2" runat="server" Text="导出EXCEL" CssClass="baseButton" OnClick="Button2_Click" />
                        <input type="button" id="import" value="导入EXCEL" class="baseButton" onclick="ImportExcel();" />
                        &nbsp;<asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="保 存" OnClick="Button1_Click" />
                        <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
                        <div style="margin-top: 3px; margin-bottom: 3px">
                            <asp:Label ID="txt_mas" runat="server" ForeColor="Red"></asp:Label>
                        </div>

                    </div>
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div style="position: relative; word-warp: break-word; word-break: break-all">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" PageSize="99999" Width="850" OnItemDataBound="myGrid_ItemDataBound"
                            ShowFooter="true">
                            <Columns>
                                <asp:TemplateColumn HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem"
                                    HeaderText="序号" HeaderStyle-Width="40" ItemStyle-Width="40">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemCenter" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" />
                                    <ItemTemplate>
                                        <asp:BoundColumn DataField="" HeaderText="序号"></asp:BoundColumn>
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbeHeji" runat="server" Text=""></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="annual" HeaderText="年度" HeaderStyle-Width="50" ItemStyle-Width="50">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                        Width="40" />
                                    <ItemStyle CssClass="myGridItemCenter" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" Width="40" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" Width="40" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="proname" HeaderText="预算项目" HeaderStyle-Width="180" ItemStyle-Width="180">
                                    <HeaderStyle CssClass="myGridHeader" Width="180" Font-Bold="True" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yskmmc" HeaderText="预算科目" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem"
                                    HeaderText="预算控制金额" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                    <FooterStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                    <ItemTemplate>
                                        <asp:TextBox ID="txt_je" runat="server" Text='' Width="90%" onfocus="Istx(this)" onkeyup="replaceNaN(this);"
                                            CssClass="txtright"></asp:TextBox>
                                        <asp:HiddenField ID="hiddprocode" runat="server" Value='<%#Eval("procode") %>' />
                                        <asp:HiddenField ID="hiddkmcode" runat="server" Value='<%#Eval("kmcode") %>' />
                                        <asp:HiddenField ID="hiddistx" runat="server" Value="0" />
                                    </ItemTemplate>
                                    <FooterTemplate>
                                        <asp:Label ID="lbeTotalAmount" runat="server" Text="0"></asp:Label>
                                    </FooterTemplate>
                                </asp:TemplateColumn>
                                <%-- <asp:BoundColumn DataField="" HeaderText="状态" HeaderStyle-Width="100" ItemStyle-Width="100" >
                                <HeaderStyle CssClass="hidden" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="hidden" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                                <FooterStyle CssClass="hidden" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>--%>
                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid><asp:HiddenField ID="hf_km" runat="server" />
                    </div>
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            parent.parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
