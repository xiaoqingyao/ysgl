<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cwkmList.aspx.cs" Inherits="cwkm_cwkmList" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script language="javascript" type="Text/javascript">
        $(function () {
            $(this).keypress(function (event) {
                var key = event.keyCode;
                if (key == '104') {
                    parent.parent.helptoggle();
                }
            });

            initMainTableClass("<%=myGrid.ClientID%>");
            //导入excel
        $("#btn_inportExcel").click(function () {
            window.showModalDialog("ExcelInport.aspx", 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:580px;status:no;scroll:no');
        });
        });
    function openDetail(openUrl) {
        var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:480px;status:no;scroll:no');
        if (returnValue == undefined || returnValue == "") {
            return false;
        }
        else {
            document.getElementById("Button4").click();
        }
    }

    function initWindowHW() {
        //给隐藏域设置窗口的高度
        $("#hdwindowheight").val($(window).height());
        //给gridview表格外部的div设置宽度  宽度为页面宽度
        $("#divgrid").css("width", ($(window).width() - 5));
    }
    $(function () {
        initWindowHW();
    });



    </script>

</head>
<body>
    <form id="form1" runat="server" onsubmit="initWindowHW();">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 30px">
                    <asp:CheckBox ID="chkNextLevel" runat="server" AutoPostBack="True" OnCheckedChanged="chkNextLevel_CheckedChanged"
                        Text="包含下级" Checked="True" />
                    &nbsp;<asp:Button ID="Button1" runat="server" Text="增 加" CssClass="baseButton" OnClick="Button1_Click" />
                    &nbsp;<asp:Button ID="Button2" runat="server" Text="修 改" CssClass="baseButton" OnClick="Button2_Click" />
                    &nbsp;<asp:Button ID="Button3" runat="server" Text="删 除" CssClass="baseButton" OnClientClick="return confirm('删除财务科目,会关联删除相关的预算科目对照关系及下级财务科目,是否继续？');"
                        OnClick="Button3_Click" />
                    &nbsp;<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    <asp:Button ID="Button4" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button4_Click" />
                    <input class="baseButton" id="btn_inportExcel" value="导入Excel" type="button" />
                    <input type="button" class="baseButton" value="帮助" onclick="javascript: parent.parent.helptoggle();" />
                </td>
            </tr>
            <tr>
                <td align="left">
                    <div id="divgrid" style="overflow-x: auto;">
                        <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                            CssClass="myGrid">
                            <Columns>
                                <asp:TemplateColumn HeaderText="选择" HeaderStyle-Width="50" ItemStyle-Width="50">
                                    <ItemTemplate>
                                        &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                    </ItemTemplate>
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                        Font-Underline="False" Wrap="False" />
                                </asp:TemplateColumn>
                                <asp:BoundColumn DataField="cwkmcode" HeaderText="科目编码" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="cwkmbm" HeaderText="科目代码" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="cwkmmc" HeaderText="科目名称" HeaderStyle-Width="150" ItemStyle-Width="150">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center"
                                        Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="XianShiMc" HeaderText="显示名称" HeaderStyle-Width="110" ItemStyle-Width="110">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Type" HeaderText="科目类型" HeaderStyle-Width="110" ItemStyle-Width="110">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="Fangxiang" HeaderText="方向" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="JiCi" HeaderText="级次" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="FuZhuHeSuan" HeaderText="辅助核算" HeaderStyle-Width="150" ItemStyle-Width="150">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="false" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="ShiFouFengCun" HeaderText="是否封存" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="hsxm1" HeaderText="核算项目一" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="hsxm2" HeaderText="核算项目二" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="hsxm3" HeaderText="核算项目三" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="hsxm4" HeaderText="核算项目四" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                                <asp:BoundColumn DataField="hsxm5" HeaderText="核算项目五" HeaderStyle-Width="70" ItemStyle-Width="70">
                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                    <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False"
                                        Font-Strikeout="False" Font-Underline="False" Wrap="false" />
                                </asp:BoundColumn>
                            </Columns>
                            <PagerStyle Visible="False" />
                        </asp:DataGrid>
                    </div>
                </td>
            </tr>
            <tr>
                <td style="height: 30px">
                    <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
                    </pager:UcfarPager>
                    <input type="hidden" runat="server" id="hdwindowheight" />
                </td>
            </tr>
        </table>
        <script type="text/javascript">
            parent.parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
