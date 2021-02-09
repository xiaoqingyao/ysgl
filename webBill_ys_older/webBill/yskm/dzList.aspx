<%@ Page Language="C#" AutoEventWireup="true" CodeFile="dzList.aspx.cs" Inherits="webBill_yskm_dzList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script language="javascript" type="Text/javascript">
        function openSelectCwkm(obj) {
            var deptCode = document.getElementById("lblDeptCode").innerHTML;
            var yskm = $(obj).parent().parent().find("td:eq(0)").text();

            var returnValue = window.showModalDialog('selectCwkm.aspx?deptCode=' + deptCode + '&yskmCode=' + yskm + '', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:800px;status:no;scroll:no');

            if (returnValue == undefined || returnValue == "")
            { }
            else
            {
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
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 27px">&nbsp;操作说明：点击对应预算科目行的"选"按钮,可选择对应的财务科目。</td>
            </tr>
            <tr>
                <td align="left">
                    <div id="header" style="overflow: hidden;">
                    </div>
                    <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 1100px; height: 400px;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False"
                            CellPadding="3" CssClass="myGrid" Style="table-layout: fixed" Width="100%" PageSize="17">
                            <Columns>
                                <asp:BoundColumn DataField="yskmcode" HeaderText="科目编码">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yskmbm" HeaderText="科目代码">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yskmmc" HeaderText="科目名称" HeaderStyle-Width="220" ItemStyle-Width="220">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="cwkm" HeaderText="对应财务科目" HeaderStyle-Width="150" ItemStyle-Width="150">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="选" HeaderStyle-Width="25" ItemStyle-Width="25">
                                    <ItemTemplate>
                                        <input id="btnSelectCwkm" class="baseButton" type="button" value="选" onclick="openSelectCwkm(this);" />
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="tbsm" HeaderText="填报说明" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:TemplateColumn HeaderText="填报类型">
                                    <ItemTemplate>
                                        单位填报
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:TemplateColumn>
                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 30px; display: none;">&nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton><asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                    第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged"></asp:DropDownList>页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                    <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                        runat="server"></asp:Label>条</td>
            </tr>
            <tr style="display: none;">
                <td style="height: 30px">
                    <asp:Label ID="lblDeptCode" runat="server"></asp:Label></td>
            </tr>
        </table>
        <script type="text/javascript">
            parent.parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
