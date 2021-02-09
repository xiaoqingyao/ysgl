<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Dept_uf_dy.aspx.cs" Inherits="webBill_bxgl_Dept_uf_dy" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            $("#btn_zd").click(function () {
                location.href = "chbxd_ImportExcel.aspx";
            });
        });
    </script>
</head>
<body style="background-color: #E4F5FF;">
    <form id="form1" runat="server">
     <%--   <div style="float: left; width: 23%">
            <asp:TreeView ID="TreeView1" runat="server" ShowLines="True" OnSelectedNodeChanged="TreeView1_SelectedNodeChanged">
            </asp:TreeView>
        </div>--%>
        <div >
            <div style="margin-top: 5px;">
                <table cellpadding="0" cellspacing="0" width="100%">                  
                    <tr>
                        <td>帐套<asp:DropDownList ID="ddlZhangTao" runat="server"  AutoPostBack="true" OnSelectedIndexChanged="OnddlZhangTao_SelectedIndexChanged">
                          
                        </asp:DropDownList>
                            <asp:Button ID="btnSaveAll" runat="server" Text="保存" OnClick="btnSaveAll_Click"
                                CssClass="baseButton" />
                             <input type="button" value="返回制单页" id="btn_zd" class="baseButton" />
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <div id="divgrid" style="overflow-x: auto;margin-top: 5px;">
                                <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                                    BorderWidth="1px" CssClass="myGrid" Width="800px" OnItemDataBound="myGrid_ItemDataBound">
                                    <Columns>
                                        <asp:BoundColumn DataField="deptCode" HeaderText="部门编号" HeaderStyle-Width="100" ItemStyle-Width="100">
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                            <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" Wrap="False" />
                                        </asp:BoundColumn>
                                        <asp:BoundColumn DataField="deptName" HeaderText="部门名称" HeaderStyle-Width="250" ItemStyle-Width="250">
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                            <ItemStyle CssClass="myGridItem" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" Wrap="true" />
                                        </asp:BoundColumn>

                                        <asp:TemplateColumn HeaderText="用友部门编号" HeaderStyle-Width="100" ItemStyle-Width="100">
                                            <ItemTemplate>
                                                <asp:TextBox ID="txt_ufdeptcode" runat="server" Width="70%"
                                                    CssClass="baseTableInput"></asp:TextBox>
                                            </ItemTemplate>
                                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                            <ItemStyle Font-Italic="False" Font-Overline="False" Font-Strikeout="False" Font-Underline="False"
                                                Wrap="False" CssClass="myGridItem" Width="100px" HorizontalAlign="Center" />
                                        </asp:TemplateColumn>
                                    </Columns>
                                    <PagerStyle Visible="False" />
                                </asp:DataGrid>
                            </div>
                        </td>
                    </tr>
                 
                </table>
                <script type="text/javascript">
                    parent.parent.closeAlert('UploadChoose');
                </script>
            </div>

        </div>
    </form>
</body>
</html>
