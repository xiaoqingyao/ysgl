<%@ Page Language="C#" AutoEventWireup="true" CodeFile="deptyskmcwList.aspx.cs" Inherits="webBill_yskm_deptyskmcwList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <style type="text/css">
        .cwtb
        {
            color: Red;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script language="javascript" type="Text/javascript">
        function selectcwkm(url, obj) {
            var str = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:850px;status:no;scroll:yes');
            if (str != undefined) {
                obj.parentNode.getElementsByTagName('input')[0].value = str;
            }
        }
        function openSelectCwkm(obj) {
            var deptCode = document.getElementById("lblDeptCode").innerHTML;
            var yskm = $(obj).parent().parent().find("td:eq(0)").text();

            var returnValue = window.showModalDialog('selectCwkm.aspx?deptCode=' + deptCode + '&yskmCode=' + yskm + '', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:800px;status:no;scroll:no');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                $(obj).parent().parent().find("td:eq(3)").text(returnValue);
            }
        }
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
    </script>

</head>
<body onload="gudingbiaotou();">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%" style="margin-left: 5px;">
        <tr>
            <td style="height: 27px">
                &nbsp;<asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="保 存" 
                    OnClick="Button1_Click" />操作说明：选中部门需要启用的预算科目; 选择完毕后,点击保存。
            </td>
        </tr>
        <tr>
            <td align="left">
                <div id="header" style="overflow: hidden;">
                </div>
                <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 1200px; height: 420px;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" PageSize="17" Style="table-layout: fixed" Width="100%" OnItemDataBound="myGrid_ItemDataBound">
                        <Columns>
                            <asp:TemplateColumn HeaderText="启用" ItemStyle-Width="38">
                                <ItemTemplate>
                                    &nbsp;&nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="hiddenbill"
                                    Width="38px" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="hiddenbill" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="yskmcode" HeaderText="科目编码">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="yskmbm" HeaderText="科目代码">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="yskmmc" HeaderText="科目名称">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Width="200" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Width="200" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem"
                                HeaderText="借方科目">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Width="200" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Width="200" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                <FooterStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_jfkmcode1" runat="server" Text='' CssClass="txtright"></asp:TextBox>
                                    <input type="button" class="baseButton" id="btn_jf1" onclick="selectcwkm('../select/selectcwkmfram.aspx',this);"
                                        value="选择" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem"
                                HeaderText="贷方科目">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Width="200" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Width="200" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                <FooterStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_dfkmcode1" runat="server" Text='' CssClass="txtright"></asp:TextBox>
                                    <input type="button" id="btn_df1" class="baseButton" onclick="selectcwkm('../select/selectcwkmfram.aspx',this);"
                                        value="选择" />
                                </ItemTemplate>
                                <FooterTemplate>
                                    <asp:Label ID="lbeTotalAmount" runat="server" Text="0"></asp:Label>
                                </FooterTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItem"
                                HeaderText="借方科目2">
                                <HeaderStyle CssClass="hiddenbill" Font-Bold="True" Width="200" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="hiddenbill" Width="200" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                <FooterStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_jfkmcode2" runat="server" Text='' CssClass="txtright"></asp:TextBox><input
                                        type="button" class="baseButton" id="btn_jf2" value="选择" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:TemplateColumn HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="myGridItem"
                                HeaderText="贷方科目2">
                                <HeaderStyle CssClass="hiddenbill" Font-Bold="True" Width="200" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                    Wrap="False" />
                                <ItemStyle CssClass="hiddenbill" Width="200" Font-Bold="False" Font-Italic="False"
                                    Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                <FooterStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txt_dfkmcode2" runat="server" Text='' CssClass="txtright"></asp:TextBox>
                                    <input type="button" id="btn_df2" class="baseButton" value="选择" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="tbsm" HeaderText="填报说明" HeaderStyle-Width="300" ItemStyle-Width="300">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="tblx" HeaderText="填报类型">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="isend" Visible="False"></asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr style="display: none;">
            <td style="height: 30px">
                <asp:Label ID="lblDeptCode" runat="server"></asp:Label>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
