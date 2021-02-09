﻿<%@ Page Language="C#" AutoEventWireup="true" CodeFile="selectcwkmlist.aspx.cs" Inherits="webBill_select_selectcwkmlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <title></title>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            //人员选择
            $("#txtcwkm").autocomplete({
                source: availableTags
            });
        });
        function enterstr() {
            var str = document.getElementById("txt_se").value;
            if (str == "") {
                var ss = document.getElementById("txt_se").value;
                if (ss == "") {
                    alert("没有选择数据！");
                }
                else {
                    window.returnValue = ss;
                    window.close();
                }
            }
            else {
                window.returnValue = str;
                window.close();
            }
        }

        function setsel(str) {
            document.getElementById("txt_se").value = str;
        }

        function selected(obj) {
            window.returnValue = obj;
            window.close();
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 27px">
                    <asp:CheckBox ID="chkNextLevel" runat="server" AutoPostBack="True" OnCheckedChanged="chkNextLevel_CheckedChanged"
                        Text="包含下级" />
                    &nbsp;
                <asp:TextBox ID="txtcwkm" runat="server"></asp:TextBox>
                    &nbsp;
                <asp:Button ID="btn_select" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_select_Click" />
                    &nbsp;
                <asp:Button ID="btn_cancel" runat="server" Text="关 闭" CssClass="baseButton" OnClick="btn_cancel_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid ID="myGrid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="3" CssClass="myGrid" PageSize="8" Width="612px">
                        <Columns>
                            <asp:TemplateColumn HeaderText="选择">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Width="35px"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="cwkmcode" HeaderText="科目编码">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cwkmbm" HeaderText="科目代码">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cwkmmc" HeaderText="科目名称">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="hsxm1" HeaderText="核算项目一">
                                <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem hiddenbill" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="hsxm2" HeaderText="核算项目二">
                                <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem hiddenbill" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="hsxm3" HeaderText="核算项目三">
                                <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem hiddenbill" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="hsxm4" HeaderText="核算项目四">
                                <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem hiddenbill" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="hsxm5" HeaderText="核算项目五">
                                <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem hiddenbill" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="XianShiMc" HeaderText="显示名称">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Width="230" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Width="230" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Type" HeaderText="科目类型">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Fangxiang" HeaderText="方向">
                                <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem hiddenbill" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="JiCi" HeaderText="级次">
                                <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem hiddenbill" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="FuZhuHeSuan" HeaderText="辅助核算">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="sffc" HeaderText="是否封存">
                                <HeaderStyle CssClass="myGridHeader hiddenbill" Font-Bold="True" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem hiddenbill" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>&nbsp;
                </td>
            </tr>
            <tr>
                <td style="height: 30px">&nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                    第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                    </asp:DropDownList>
                    页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                    runat="server"></asp:Label>条
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
