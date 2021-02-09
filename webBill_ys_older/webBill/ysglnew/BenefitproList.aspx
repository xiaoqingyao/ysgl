<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BenefitproList.aspx.cs" Inherits="webBill_ysglnew_BenefitproList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
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
                    parent.helptoggle();
                }
            });
            gudingbiaotounew($("#myGrid"), $(window).height() - 100);
            initMainTableClass("<%=myGrid.ClientID%>");
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function () {
                if ($(this).find("td")[0] != null && $(this).find("td")[0].innerHTML != "") {
                    $("#procode").val($(this).find("td")[0].innerHTML);

                }
            });
            $("#btn_new").click(function () {
                var deptCode = $("#hf_pro").val();
                var xmtype = '<%=Request["xmtype"] %>';
                openDetail("BenefitproDetails.aspx?type=add&pCode=" + deptCode + "&xmtype=" + xmtype);
            });
            $("#btn_edit").click(function () {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                var deptCode = $("#procode").val();
                openDetail("BenefitproDetails.aspx?type=edit&pCode=" + deptCode);
            });
        });

        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:440px;status:no;scroll:no');
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
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="90%" style="margin-left: 5px">
            <tr>
                <td style="height: 27px">&nbsp;<asp:CheckBox ID="CheckBox2" runat="server" Text="全部状态" AutoPostBack="True"
                    OnCheckedChanged="CheckBox2_CheckedChanged" />
                    &nbsp;财年：<asp:DropDownList ID="drpSelectNd" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList1_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:TextBox ID="txb_where" runat="server" Width="120"></asp:TextBox>
                    <asp:Button ID="btn_sele" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_sele_Click" />
                    &nbsp;<input class="baseButton" type="button" id="btn_new" value="新 增" />
                    &nbsp;<input class="baseButton" type="button" id="btn_edit" value="修 改" />
                    &nbsp;<asp:Button ID="btn_del" runat="server" Text="禁 用" CssClass="baseButton" OnClientClick="return confirm('项目已经填写,是否继续？');"
                        OnClick="btn_del_Click" />
                    <asp:Button ID="Button1" runat="server" Text="启 用" CssClass="baseButton" OnClientClick="return confirm('确定要启用吗？');"
                        OnClick="Button1_Click"/>
                    &nbsp;<input class="baseButton" value="导出EXCEL" type="button" id="btn_toExcel" />
                    &nbsp;<asp:Button ID="btn_copYear" runat="server" Text="复制上年" CssClass="baseButton"
                        OnClick="btn_copYear_Click" />



                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div style="position: relative; word-warp: break-word; word-break: break-all">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" Style="table-layout: fixed; word-wrap: break-word" Width="500px" AllowPaging="True" PageSize="99999">
                            <Columns>
                                <asp:BoundColumn DataField="procode" HeaderText="项目编号" HeaderStyle-Width="80" ItemStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemCenter" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="proname" HeaderText="项目名称" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="annual" HeaderText="年度" HeaderStyle-Width="80" ItemStyle-Width="80">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemCenter" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="calculatype" HeaderText="计算方式" HeaderStyle-Width="120" ItemStyle-Width="120" HeaderStyle-CssClass="hiddenbill " ItemStyle-CssClass="hiddenbill">
                                    <%--  <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem"    Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />--%>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="fillintype" HeaderText="填写方式" Visible="false" HeaderStyle-CssClass="hiddenbill " ItemStyle-CssClass="hiddenbill">
                                    <%--  <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem"    Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />--%>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="sortcode" HeaderText="行次" HeaderStyle-Width="120" ItemStyle-Width="120" HeaderStyle-CssClass="hiddenbill " ItemStyle-CssClass="hiddenbill">
                                    <%--  <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem"    Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />--%>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="status" HeaderText="状态" HeaderStyle-Width="120" ItemStyle-Width="120">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItemCenter" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="adduser" HeaderText="添加人" HeaderStyle-Width="120" ItemStyle-Width="120" HeaderStyle-CssClass="hiddenbill " ItemStyle-CssClass="hiddenbill">
                                    <%-- <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem"    Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />--%>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="adddate" HeaderText="添加时间" DataFormatString="{0:D}" HeaderStyle-Width="120" ItemStyle-Width="120" HeaderStyle-CssClass="hiddenbill " ItemStyle-CssClass="hiddenbill">
                                    <%--  <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem"    Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />--%>
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="fillintype" HeaderText="类型" HeaderStyle-Width="120" ItemStyle-Width="120" HeaderStyle-CssClass="hiddenbill " ItemStyle-CssClass="hiddenbill">
                                    <HeaderStyle CssClass="hiddenbill" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="hiddenbill" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid>&nbsp;<asp:HiddenField ID="hf_pro" runat="server" />
                        <asp:HiddenField ID="procode" runat="server" />
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
