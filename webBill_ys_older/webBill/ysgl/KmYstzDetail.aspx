<%@ Page Language="C#" AutoEventWireup="true" CodeFile="KmYstzDetail.aspx.cs" Inherits="webBill_ysgl_KmYstzDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.multiselect.min.js" type="text/javascript"></script>

    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <style type="text/css">
        .style1
        {
            background-color: #EDEDED;
        }
    </style>

    <script type="text/javascript"> 
     //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div style="width: 800px; margin: 0 auto;">
        <table class="myTable" cellpadding="0" width="100%">
            <tr>
                <td colspan="4" class="billtitle">
                    科目预算调整单
                </td>
            </tr>
            <tr>
                <td class="tableBg2">
                    制单人:
                </td>
                <td>
                    <asp:TextBox ID="txt_zdr" runat="server" ReadOnly="true" Width="90%"></asp:TextBox>
                </td>
                <td class="tableBg2">
                    目标预算过程:
                </td>
                <td>
                    <asp:TextBox ID="txt_source" runat="server" ReadOnly="true" Width="90%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td class="tableBg2">
                    调整部门:
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="LaDept" AutoPostBack="True" OnSelectedIndexChanged="LaDept_SelectedIndexChanged" Width="92%">
                    </asp:DropDownList>
                </td>
                <td class="tableBg2">
                    目标预算科目:
                </td>
                <td>
                    <asp:DropDownList ID="drp_yskm" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drp_yskm_SelectedIndexChanged" Width="92%">
                    </asp:DropDownList>
                </td>
            </tr>
            <tr>
                <td class="tableBg2">
                    摘要:
                </td>
                <td colspan="3">
                    <asp:TextBox ID="txt_zy" runat="server" Width="97%"></asp:TextBox>
                </td>
            </tr>
            <tr>
                <td colspan="4">
                    <div style=" width:100%; height:460px; overflow:auto;">
                    <asp:GridView ID="MyGridView" runat="server" CssClass="myGrid" AutoGenerateColumns="false"
                        OnRowDataBound="MyGridView_RowDataBound" Width="100%">
                        <Columns>
                            <asp:BoundField DataField="gcbh" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"
                                HeaderStyle-CssClass="hiddenbill" />
                            <asp:BoundField DataField="yskm" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"
                                HeaderStyle-CssClass="hiddenbill" />
                            <asp:BoundField HeaderText="科目" DataField="yskmShowName">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="源预算过程" DataField="gcbhShowName">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="预算金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="花费金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="占用金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:BoundField HeaderText="可调整金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundField>
                            <asp:TemplateField HeaderText="调整金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                                <ItemTemplate>
                                    <asp:TextBox ID="txtTzAmount" Text="0" runat="server"   onkeyup="replaceNaN(this);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ysje" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill"
                                HeaderStyle-CssClass="hiddenbill" /><%--预算调整金额--%>
                        </Columns>
                    </asp:GridView>
                    </div>
                </td>
            </tr>
            <tr>
                <td colspan="4" style="text-align: center">
                    <asp:Button ID="btn_save" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_Save_Click"/>
                    &nbsp;&nbsp;&nbsp;&nbsp;
                    <input id="Button2" type="button" value="取 消" class="baseButton" onclick="javascript:window.close();" />
                    <asp:HiddenField ID="hf_billcode" runat="server" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
