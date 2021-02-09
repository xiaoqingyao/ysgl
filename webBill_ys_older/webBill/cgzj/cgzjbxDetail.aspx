<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cgzjbxDetail.aspx.cs" Inherits="webBill_cgzj_cgzjbxDetail" %>

<%@ Register Assembly="PaginationControl" Namespace="PaginationControl" TagPrefix="cc1" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>采购资金付款详细信息</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <style type="Text/css">
        .right
        {
            text-align: right;
        }
        #txtCgrq
        {
            width: 82px;
        }
    </style>

    <script language="javascript" type="Text/javascript">
        $(function() {
            $('#dwxz').dialog({
                autoOpen: false,
                width: 600,
                buttons: {
                    "确定": function() {
                        if (document.all.DGgys != undefined) {
                            for (i = 1; i < document.all.DGgys.rows.length; i++) {
                                var cb = document.all.DGgys.rows(i).cells(0).children(0);
                                if (cb.checked) {
                                    temp0 = document.all.DGgys.rows(i).cells(1).innerText;
                                    temp1 = document.all.DGgys.rows(i).cells(2).innerText;
                                    if (document.getElementById("TextBox4").value == "") {
                                        document.getElementById("TextBox4").value = "'" + temp0 + "'";
                                        document.getElementById("TextBox5").value = temp0;
                                    }
                                    else {
                                        document.getElementById("TextBox4").value = document.getElementById("TextBox4").value + ",'" + temp0 + "'";
                                        document.getElementById("TextBox5").value = document.getElementById("TextBox4").value + "," + temp0;
                                    }

                                    cb.checked = false;
                                }
                            }

                            $("#Button2").click();
                        }
                        $(this).dialog("close");
                    },
                    "取消": function() {
                        $(this).dialog("close");
                    }
                }
            });
            $("#btn_close").click(function() {
                window.close();
            });
        });
        function editClick() {
            $("#dwxz").parent().appendTo($("form:first"));
            $('#dwxz').dialog('open');
            return false;
        }

        function deleteBxdj(djGuid) {
            var returnValue = webBill_fysq_cgspDetail.DeleteBxdj(djGuid).value;
            if (returnValue == true) {
                document.getElementById("btnRefreshFj").click();
            }
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div id="dwxz" title="单位选择">
        <table>
            <tr>
                <td>
                    编号:
                </td>
                <td>
                    <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                </td>
                <td>
                    名称:
                </td>
                <td>
                    <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="Button3" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button3_Click" />
                </td>
            </tr>
        </table>
        <asp:GridView ID="DGgys" runat="server" AutoGenerateColumns="False" CssClass="ui-widget ui-widget-content"
            ItemStyle-HorizontalAlign="Center">
            <Columns>
                <asp:TemplateField HeaderText="选择">
                    <ItemTemplate>
                        <asp:CheckBox ID="CheckBox1" runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="gysbh" HeaderText="供应商编号" />
                <asp:BoundField DataField="mc" HeaderText="供应商名称" />
                <asp:BoundField DataField="je" HeaderText="金额" />
            </Columns>
            <HeaderStyle CssClass="ui-widget-header" />
        </asp:GridView>
        <cc1:PaginationToGV ID="PaginationToGV1" runat="server" OnGvBind="PaginationToGV1_GvBind" />
    </div>
    <table cellpadding="0" id="taball" cellspacing="0" width="100%">
        <tr>
            <td style=" width:3%"></td>
            <td align="center" style="text-align:center; width:94%">
                <table width="100%">
                    <tr>
                        <td style="text-align: center; height: 27px;">
                            <strong><span style="font-size: 12pt">采购付款单</span></strong>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 26px;">
                            <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="100%">
                                <tr>
                                    <td class="tableBg" style="text-align: right">
                                        付款日期：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtCgrq" runat="server" Width="97%"></asp:TextBox>
                                    </td>
                                    <td class="tableBg" style="text-align: right">
                                        <asp:Label ID="Label1" runat="server" Text="付款部门："></asp:Label>
                                    </td>
                                    <td>
                                        <asp:Label ID="lblDept" runat="server" Text="[000001]采购蛇皮但单位" Style="display: none"></asp:Label>
                                        <asp:Label ID="lblDeptShow" runat="server" Text="[000001]采购蛇皮但单位"></asp:Label>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableBg" style="text-align: right">
                                        付款人：
                                    </td>
                                    <td>
                                        <asp:Label ID="Label4" runat="server" Text="[000001]采购蛇皮但单位" Style="display: none"></asp:Label>
                                        <asp:Label ID="lblUser" runat="server" Text="[000001]采购蛇皮但单位"></asp:Label>
                                    </td>
                                    <td class="tableBg" style="text-align: right">
                                        备注：
                                    </td>
                                    <td>
                                        <asp:TextBox ID="txtBxzy" runat="server" Width="97%"></asp:TextBox>
                                    </td>
                                </tr>
                                <tr>
                                    <td class="tableBg" rowspan="2" style="text-align: right">
                                        付款单位：
                                    </td>
                                    <td class="tableBg2" style="text-align: left" colspan="5">
                                        <%--<input type="button" class="baseButton" value="增 加" onclick="opAddDetail();" />--%>
                                        <input type="button" class="baseButton" value="增 加" onclick="return editClick()" />
                                        <asp:Button ID="Button5" runat="server" CssClass="baseButton" OnClick="Button5_Click"
                                            Text="删 除" />
                                        <asp:FileUpload ID="FileUpload1" runat="server" />
                                        <asp:Button ID="Button4" runat="server" Text="导 入" CssClass="baseButton" OnClick="Button4_Click" />
                                        <asp:TextBox ID="TextBox4" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:TextBox ID="TextBox5" runat="server" Style="display: none"></asp:TextBox>
                                        <asp:Button ID="Button2" runat="server" Text="Button" Style="display: none" OnClick="Button2_Click" />
                                    </td>
                                </tr>
                                <tr>
                                    <td colspan="5">
                                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowSorting="True"
                                            BorderWidth="1px" CellPadding="3" CssClass="myGrid" ItemStyle-HorizontalAlign="Center"
                                            PageSize="17" Width="100%">
                                            <PagerStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" />
                                            <Columns>
                                                <asp:TemplateField HeaderText="选择">
                                                    <ItemTemplate>
                                                        &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" />
                                                    </ItemTemplate>
                                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Width="30px"
                                                        Wrap="False" />
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="编号">
                                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Width="40px"
                                                        Wrap="False" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="gysbh" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "gysbh") %>'
                                                            Enabled="False" Width="40px"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="供应商名称">
                                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                                        Width="150px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="gysmc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "gysmc") %>'
                                                            Enabled="False" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="计划金额">
                                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                                        Width="80px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="jhje" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "jhje") %>'
                                                            Enabled="False" Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="付款金额">
                                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                                        Width="80px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="fkje" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "fkje") %>'
                                                            Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                                <asp:TemplateField HeaderText="备注说明">
                                                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"
                                                        Width="80px" />
                                                    <ItemTemplate>
                                                        <asp:TextBox ID="bz" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "bz") %>'
                                                            Width="90%"></asp:TextBox>
                                                    </ItemTemplate>
                                                </asp:TemplateField>
                                            </Columns>
                                        </asp:GridView>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                    <tr>
                        <td style="text-align: center; height: 37px;">
                            <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click" />
                            &nbsp;
                            <input id="btn_close" type="button" value="取 消" class="baseButton" />
                        </td>
                    </tr>
                    <tr style="display: none;">
                        <td style="height: 37px; text-align: center">
                            <asp:Label ID="lbl_BillCode" runat="server"></asp:Label><asp:Label ID="Label2" runat="server"></asp:Label>
                        </td>
                    </tr>
                </table>
            </td>
             <td style=" width:3%"></td>
        </tr>
    </table>
    </form>
</body>
</html>
