<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Deptblsz.aspx.cs" Inherits="webBill_search_Deptblsz" %>

<%@ Register Assembly="UcfarPager" Namespace="UcfarPager" TagPrefix="pager" %>
<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">


<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <title></title>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>
    <script type="text/javascript">
        function sumje(obj) {
            var bl = obj.value;
            var zje = $(obj).parent().next().children().eq(0).text();
            var blje = parseFloat(zje) * parseFloat(bl);

            $(obj).parent().parent().children().eq(4).children().eq(0).val(blje.toFixed(4));
        }

        function jsbl(obj) {
            //1.获取总金额
            var zje = $(obj).parent().prev().children().eq(0).text();
            //2.获取比例金额
            var blje = obj.value;

            var bl = (parseFloat(blje) / parseFloat(zje)) ;
          
            //3为比例赋值
            $(obj).parent().parent().children().eq(2).children().eq(0).val(bl.toFixed(8));
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="90%" style="margin-left: 5px">
            <tr>
                <td style="height: 30px">预算年度：
                    <asp:DropDownList ID="drpSelectNd" runat="server"
                        AutoPostBack="True"
                        OnSelectedIndexChanged="drpSelectNd_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:CheckBox ID="chkNextLevel" runat="server" AutoPostBack="True" OnCheckedChanged="chkNextLevel_CheckedChanged"
                        Text="包含下级" Checked="false" Visible="false" />
                    <asp:Button ID="btn_excl" runat="server" Text="导出excel" CssClass="baseButton" OnClick="btn_excl_Click" />
                    <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="baseButton" OnClick="btnSave_Click" />
                    <asp:Button ID="btn_fzsn" runat="server" Text="复制上年" CssClass="baseButton" OnClick="btn_fzsn_Click" />
                    <asp:Label ID="Label1" runat="server" Text="当前部门：未选择，请在左侧选择" ForeColor="Red"></asp:Label>
                    <span style="color: Red">
                        <asp:Label ID="lblmsg" runat="server"></asp:Label></span>
                </td>
            </tr>
            <tr>
                <td>
                    <div>
                        <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid" OnRowDataBound="GridView1_RowDataBound"
                            ShowHeader="true" ShowFooter="true" Width="600" EmptyDataText="暂无数据">
                            <Columns>
                                <asp:BoundField DataField="deptcode" HeaderText="部门编号" ItemStyle-CssClass="myGridItem"
                                    HeaderStyle-CssClass="myGridHeader" FooterStyle-CssClass="myGridItem" />
                                <asp:BoundField DataField="deptname" HeaderText="部门名称" ItemStyle-CssClass="myGridItem"
                                    HeaderStyle-CssClass="myGridHeader" FooterStyle-CssClass="myGridItem" />
                                <asp:TemplateField HeaderText="分解比例" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader"
                                    FooterStyle-CssClass="myGridItem">
                                    <ItemTemplate>
                                        <asp:TextBox ID="bl" Text="0.000000" runat="server" onkeyup="sumje(this)"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="总金额" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader"
                                    FooterStyle-CssClass="myGridItem">
                                    <ItemTemplate>
                                        <asp:Label runat="server" ID="lbl_je"></asp:Label>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:TemplateField HeaderText="分解金额" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader"
                                    FooterStyle-CssClass="myGridItem">
                                    <ItemTemplate>
                                        <asp:TextBox ID="blje" Width="90%" Text="" runat="server" onkeyup="jsbl(this)"></asp:TextBox>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </div>
                    <asp:HiddenField ID="hidzje" runat="server" />
                </td>
            </tr>
            <%--  <tr>
                <td style="height: 30px">
                    <pager:UcfarPager ID="ucPager" runat="server" OnPageChanged="UcfarPager1_PageChanged">
                    </pager:UcfarPager>
                    <input type="hidden" runat="server" id="hdwindowheight" />
                </td>
            </tr>--%>
        </table>
        <script type="text/javascript">
            parent.parent.closeAlert('UploadChoose');
        </script>
    </form>
</body>
</html>
