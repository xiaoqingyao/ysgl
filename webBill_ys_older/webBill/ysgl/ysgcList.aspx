<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ysgcList.aspx.cs" Inherits="ysgl_ysgcList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:400px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "") {
                return false;
            }
            else {
                document.getElementById("Button4").click();
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
                    parent.helptoggle();
                }
            });
            initMainTableClass("<%=myGrid.ClientID%>");
            initWindowHW();
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
        //获取宽度
        function initWindowHW() {
            //给隐藏域设置窗口的高度
            $("#hdwindowheight").val($(window).height());
            //给gridview表格外部的div设置宽度  宽度为页面宽度
            $("#divgrid").css("width", ($(window).width() - 5));
        }
    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 30px">
                    <asp:Label ID="Label1" runat="server" Text="财年"></asp:Label>
                    <asp:TextBox ID="TextBox2" runat="server" Width="45px"></asp:TextBox>
                    <asp:Button ID="Button7" runat="server" CssClass="baseButton" OnClick="Button7_Click"
                        Text="生成预算过程" />&nbsp;
                    <asp:Button ID="btn_Delysgc" runat="server" Text="删除预算过程" CssClass="baseButton"
                            OnClick="btn_Delysgc_Click" />
                    <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" visible="False" />
                    <asp:Button ID="Button3" runat="server" Text="删 除" CssClass="baseButton" OnClick="Button3_Click"
                        Visible="False" /><asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    <asp:Button ID="Button4" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button4_Click" />
                    <asp:Button ID="Button2" runat="server" Text="修 改" CssClass="baseButton" OnClick="Button2_Click" />
                    <asp:Button ID="Button5" runat="server" Text="开始预算过程" CssClass="baseButton" OnClick="Button5_Click" />
                    <asp:Button ID="Button6" runat="server" Text="结束预算过程" CssClass="baseButton" OnClick="Button6_Click" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.helptoggle();" />
                </td>
            </tr>
            <tr>
                <td>
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CssClass="myGrid"
                            Width="1200">
                            <Columns>
                                <asp:TemplateColumn HeaderText="选择" HeaderStyle-Width="50" ItemStyle-Width="50">
                                    <HeaderTemplate>
                                        <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                            Text="全选" />
                                    </HeaderTemplate>
                                    <ItemTemplate>
                                        &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                    </ItemTemplate>
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="gcbh" HeaderText="过程编号" HeaderStyle-Width="120" ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="xmmc" HeaderText="过程名称" HeaderStyle-Width="150" ItemStyle-Width="150">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="kssj" HeaderText="预算开始日期" DataFormatString="{0:D}" HeaderStyle-Width="120"
                                    ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="jzsj" HeaderText="预算截止日期" DataFormatString="{0:D}" HeaderStyle-Width="120"
                                    ItemStyle-Width="120">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="fqr" HeaderText="过程发起人" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="true" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="fqsj" HeaderText="过程发起时间" DataFormatString="{0:D}" HeaderStyle-Width="100"
                                    ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="status" HeaderText="status" Visible="False"></asp:BoundColumn>
                                <asp:BoundColumn DataField="ysType" HeaderText="预算类型" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="nian" HeaderText="预算年份" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="yue" HeaderText="预算季度/月份" HeaderStyle-Width="100" ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="statusName" HeaderText="过程状态" HeaderStyle-Width="100"
                                    ItemStyle-Width="100">
                                    <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                        Wrap="False" CssClass="myGridItem" />
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <tr>
                <%-- 这个地方和上面的按钮组 都写死高度 30px--%>
                <td style="height: 30px">
                    <%-- &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                </asp:DropDownList>
                页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                    runat="server"></asp:Label>条--%>
                    <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
                    </pager:UcfarPager>
                    <input type="hidden" runat="server" id="hdwindowheight" />
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
