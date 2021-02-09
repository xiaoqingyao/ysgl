<%@ Page Language="C#" AutoEventWireup="true" CodeFile="chkm_dy.aspx.cs" Inherits="webBill_bxgl_chkm_dy" %>

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
        function selectyskm(obj) {
            var str = window.showModalDialog("../bxgl/YskmSelectNew.aspx?deptCode=" + $("#hfDept").val() + "&dydj=01&flag=s", 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined) {
                var json = $.parseJSON(str);
                obj.parentNode.getElementsByTagName('input')[0].value = json[0].Yscode;
            }
        }

        $(function () {
            $(".myGrid input:text").autocomplete({
                source: availablekm
            });
            $("#btn_zd").click(function () {
                location.href = "chbxd_ImportExcel.aspx";
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <asp:HiddenField ID="hfDept" runat="server" />
        <div class="baseDiv">
            对应状态：
        <asp:DropDownList ID="ddlType" runat="server" OnSelectedIndexChanged="ddlType_SelectedIndexChanged"
            AutoPostBack="true">
            <asp:ListItem Text="全部" Value="">全部</asp:ListItem>
            <asp:ListItem Text="已对应" Value="1">已对应</asp:ListItem>
            <asp:ListItem Text="未对应" Selected="True" Value="0">未对应</asp:ListItem>
        </asp:DropDownList>

<%--            部门：
            <asp:DropDownList ID="ddl_dept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddl_dept_SelectedIndexChanged">
            </asp:DropDownList>--%>
            帐套:
                <asp:DropDownList ID="ddlZhangTao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="OnddlZhangTao_SelectedIndexChanged">
                </asp:DropDownList>
            <input type="button" value="返回制单页" id="btn_zd" class="baseButton" />
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
                            <asp:BoundColumn DataField="chcode" HeaderText="存货编号">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Width="100" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader " />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Width="100" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem " />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="chname" HeaderText="存货名称" ItemStyle-Width="150" HeaderStyle-Width="150">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader " />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem " />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem " />
                            </asp:BoundColumn>
                                 <asp:BoundColumn DataField="note1" HeaderText="部门" ItemStyle-Width="150" HeaderStyle-Width="150">
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
                                        CssClass="baseTableInput"></asp:TextBox>
                                </ItemTemplate>
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridHeader" />
                                <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                    Wrap="true" CssClass="myGridItem" />
                                <FooterStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="true" CssClass="myGridItem" />
                            </asp:TemplateColumn>
                             

                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </asp:Panel>
            </div>
        </div>
        <script type="text/javascript">
            parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
