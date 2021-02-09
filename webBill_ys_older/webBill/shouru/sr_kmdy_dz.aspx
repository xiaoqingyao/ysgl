<%@ Page Language="C#" AutoEventWireup="true" CodeFile="sr_kmdy_dz.aspx.cs" Inherits="webBill_shouru_sr_kmdy_dz" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>科目对照表</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <style type="text/css">
        .highlight {
            background: #EBF2F5;
        }

        .hiddenbill {
            display: none;
        }
    </style>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script type="text/javascript">


        function AddCheck() {
            var ddlType = $("#ddlType").val();

            var txtOutName = $("#txtOutName").val();
            var txtYskm = $("#txtYskm").val();

            if (ddlType.length == 0) {
                alert("请选择对应类型");
                return false;
            }

            else if (txtOutName.length == 0) {
                alert("校管家科目名称不能为空");
                return false;
            }
            else if (txtYskm.length == 0) {
                alert("请选择对应本系统的预算科目");
                return false;
            }
            else {
                return true;
            }
        }


        function selectyskm(obj) {
            var str = window.showModalDialog("../bxgl/YskmSelectNew_dz.aspx?flag=s", 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined) {
                var json = $.parseJSON(str);
                obj.parentNode.getElementsByTagName('input')[0].value = json[0].Yscode;
            }
        }

        function selectyskm1() {
            var str = window.showModalDialog("../bxgl/YskmSelectNew_dz.aspx?flag=s", 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined) {
                var json = $.parseJSON(str);
                $("#txtYskm").val(json[0].Yscode);
            }
        }

    </script>

</head>
<body>
    <form id="form1" runat="server">

        <div class="baseDiv">
            <input type="button" class="baseButton" value="返回制单页面" onclick="window.location.href = 'srd_dz.aspx'" />
            收入类型：
        <asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
            AutoPostBack="true">
            <asp:ListItem Value="">全部</asp:ListItem>
            <asp:ListItem Text="课程" Value="课程">课程</asp:ListItem>
            <asp:ListItem Text="物品" Value="物品">物品</asp:ListItem>
        </asp:DropDownList >
            设置状态：
            <asp:DropDownList ID="ddlzt" runat="server"  OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
            AutoPostBack="true">
                <asp:ListItem Value="未设置"></asp:ListItem>
                <asp:ListItem Value="">全部</asp:ListItem>
                <asp:ListItem Value="已设置"></asp:ListItem>

            </asp:DropDownList>
            <div id="topAdd" style="margin-top: 5px;">
                <asp:Panel runat="server" GroupingText="添加">
                    <table>
                        <tr>

                            <td>校管家科目名称：
                            </td>
                            <td>
                                <asp:TextBox ID="txtOutName" runat="server"></asp:TextBox>
                            </td>
                            <td>预算系统科目：
                            </td>
                            <td>
                                <asp:TextBox ID="txtYskm" runat="server" CssClass="baseTextReadOnly" onkeyup="javascript:this.value='';"></asp:TextBox>
                                <input type="button" class="baseButton" onclick="selectyskm1();" value="选" />
                            </td>
                            <td>
                                <span style="width: 30px;"></span>
                            </td>
                            <td>
                                <asp:Button ID="btnSave" runat="server" CssClass="baseButton" Text="添加" OnClientClick="return  AddCheck();"
                                    OnClick="btnSave_Click" />
                            </td>
                        </tr>
                    </table>
                </asp:Panel>
            </div>
            <div id="divgrid" style="margin-top: 5px;">
                <asp:Panel ID="Panel1" runat="server" GroupingText="修改">
                    <div style="margin: 5px;">
                        <asp:Button ID="btnSaveAll" runat="server" Text="保存修改" OnClick="btnSaveAll_Click"
                            CssClass="baseButton" />
                    </div>
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" Style="table-layout: fixed; word-wrap: break-word;" Width="750"
                        ShowFooter="false">
                        <Columns>
                            <asp:BoundColumn DataField="atype" HeaderText="对应类型">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader hiddenbill" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem hiddenbill" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem hiddenbill" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="outname" HeaderText="校管家科目名称" ItemStyle-Width="150" HeaderStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader " />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem " />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem " />
                            </asp:BoundColumn>

                            <asp:TemplateColumn ItemStyle-Width="200" HeaderStyle-Width="200">
                                <HeaderTemplate>
                                    预算系统科目
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:TextBox ID="txtYskm" runat="server" Text='<%#Eval("yskmcode") %>' Width="70%"
                                        CssClass="baseTextReadOnly"></asp:TextBox>
                                    <input type="button" class="baseButton" id="btnChange" onclick="selectyskm(this);"
                                        value="选" />
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:TemplateColumn>
                              <asp:BoundColumn DataField="atype" HeaderText="收入类型" ItemStyle-Width="50" HeaderStyle-Width="50">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader " />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem " />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem " />
                            </asp:BoundColumn>

                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </asp:Panel>
            </div>
        </div>
    </form>
</body>
</html>
