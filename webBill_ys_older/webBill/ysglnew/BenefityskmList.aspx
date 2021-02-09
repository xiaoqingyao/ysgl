<%@ Page Language="C#" AutoEventWireup="true" CodeFile="BenefityskmList.aspx.cs"
    Inherits="webBill_ysglnew_BenefityskmList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>无标题页</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <style type="text/css">
        .cwtb {
            color: Red;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
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

        function SelectAll(aControl) {
            var chk = document.getElementById("myGrid").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
            }
        }
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });
            gudingbiaotounew($("#myGrid"), $(window).height() - 100);
            initMainTableClass("<%=myGrid.ClientID%>");

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
                <td style="height: 27px">&nbsp;<asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="保 存" OnClick="Button1_Click" />&nbsp;
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />&nbsp;<span style="color: Red"><asp:Label ID="lb_msg" runat="server" ForeColor="Red"> &nbsp;&nbsp;操作说明：先选择左侧的利润项目,然后在右侧选择对应科目，点击保存。</asp:Label></span>
                </td>
            </tr>
            <tr>
                <td>
                    <div style="position: relative; word-warp: break-word; word-break: break-all">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid" PageSize="17" Width="750" OnRowDataBound="myGrid_rowDataBound" OnItemDataBound="myGrid_ItemDataBound">
                            <Columns>
                                <asp:TemplateColumn HeaderText="启用" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                            Text="全选" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        &nbsp;&nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="dept" HeaderText="部门">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yskmcode" HeaderText="科目编码" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yskmmc" HeaderText="科目名称" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False"
                                        Font-Overline="False" Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="tbsm" HeaderText="填报说明" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="tblx" HeaderText="填报类型" HeaderStyle-Width="200" ItemStyle-Width="200">
                                    <HeaderStyle CssClass="hiddenbill" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="hiddenbill" Font-Italic="False" Font-Overline="False"
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
        <script type="text/javascript">
            parent.parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
