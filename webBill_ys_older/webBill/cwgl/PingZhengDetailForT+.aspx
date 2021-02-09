<%@ Page Language="C#" AutoEventWireup="true" CodeFile="PingZhengDetailForT+.aspx.cs" Inherits="webBill_cwgl_PingZhengDetailForT_" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>制作凭证</title>
    <%--u810--%>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>

    <script type="text/javascript">
        $(function () {
            initMainTableClass("<%=GridView.ClientID%>");
        });
        function addSource(obj) {
            $(obj).autocomplete({
                source: availableTags
            });
        }
        function selectcwkm(url, obj) {
            var str = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:850px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                obj.parentNode.getElementsByTagName('input')[0].value = str;
                $("#reLoad").click();
            }
        }
        function disablebutton() {
            document.getElementById("divOver").style.visibility = "visible";
            return true;
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <div class="baseDiv" style="margin-left: 5px; margin-top: 5px; width: 99%;">
            <table class="baseTable" style="margin-left: 0px; background-color: #EBF2F5;">
                <tr>
                    <td style="text-align: right">
                        <span style="display: none">
                            <asp:Button ID="reLoad" runat="server" Width="0px" OnClick="reLoad_OnClick" />
                        </span>
                        帐套：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlZhangTao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="OnddlZhangTao_SelectedIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td>凭证类别：
                    </td>
                    <td>
                        <asp:DropDownList ID="ddlPingZhengType" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td>年度：</td>
                    <td>
                        <asp:DropDownList ID="ddlNd" runat="server" AutoPostBack="true" OnSelectedIndexChanged="OnNd_SelectedIndexChanged">
                        </asp:DropDownList></td>
                    <td style="text-align: right" colspan="2">附单据数：
                        <asp:TextBox ID="txtBillCount" runat="server" CssClass="baseText"></asp:TextBox>
                    </td>

                </tr>
                <tr>
                    <td style="text-align: right">制单日期：
                    </td>
                    <td>
                        <asp:TextBox ID="txtDate" runat="server" onfocus="setday(this);" CssClass="baseText"
                            Width="200"></asp:TextBox>
                    </td>

                    <td colspan="2">
                        <asp:DropDownList ID="ddlZhaiYao" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlZhaiYao_SelectIndexChanged">
                        </asp:DropDownList>
                    </td>
                    <td colspan="4">
                        <asp:TextBox ID="txtZhaiYao" runat="server" CssClass="baseText"></asp:TextBox><asp:CheckBox
                            ID="cbAddToUsually" runat="server" Text="添加到常用" />
                        <asp:Button ID="btnZhaiYaoToItem" runat="server" Text="统一摘要" CssClass="baseButton"
                            OnClick="btnZhaiYaoToItem_Click" />
                    </td>
                </tr>
            </table>
            <div class="baseDiv" style="color: Red">
                <asp:Label ID="lbeMsg" runat="server"></asp:Label>
            </div>
            <div style="text-align: center; width: 100%; height: 500px; overflow: auto;">
                <asp:GridView ID="GridView" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                    OnRowDataBound="GridView_RowDataBound" Width="100%" ShowFooter="true">
                    <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                    <RowStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    <FooterStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                        Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                    <Columns>
                        <asp:BoundField DataField="PingZhengType" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <asp:BoundField DataField="bxzy" HeaderText="摘要" HeaderStyle-CssClass="hiddenbill"
                            ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                        <asp:TemplateField HeaderText="摘要">
                            <ItemTemplate>
                                <asp:TextBox ID="txtZhaiYao" runat="server" Width="95%"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="明细科目" ControlStyle-Width="200">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMingXiKemu" runat="server" Text="" Width="90%"></asp:TextBox>
                                <input type="button" class="baseButton" id="btn_jf1" onclick="selectcwkm('../select/selectcwkmfram.aspx', this);"
                                    value="选择" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="辅助核算" ItemStyle-Width="200" FooterStyle-CssClass="hiddenbill"
                            HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill">
                            <ItemTemplate>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="billDept" HeaderText="核算部门" FooterStyle-CssClass="myGridItem"
                            HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemCenter" />
                        <asp:TemplateField HeaderText="部门核算">
                            <ItemTemplate>
                                <asp:TextBox ID="txtForDept" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="借方金额" FooterStyle-CssClass="myGridItem"
                            HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight">
                            <ItemTemplate>
                                <asp:TextBox ID="jfje" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "jfje", "{0:F2}")%>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="贷方金额" FooterStyle-CssClass="myGridItem"
                            HeaderStyle-CssClass="myGridHeader" ItemStyle-CssClass="myGridItemRight">
                            <ItemTemplate>
                                <asp:TextBox ID="dfje" runat="server" Text='<%#DataBinder.Eval(Container.DataItem, "dfje", "{0:F2}")%>'></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="fuzhuhesuan" HeaderText="fuzhuhesuan" HeaderStyle-CssClass="hiddenbill"
                            ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                        <asp:BoundField DataField="billUser" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <asp:BoundField DataField="OsDept" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <asp:BoundField DataField="fykmName" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <%-- 13是否现金科目 iscash--%>
                        <asp:BoundField DataField="OsDept" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <%--14是否部门核算 IsAuxAccDepartment--%>
                        <asp:BoundField DataField="OsDept" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <%--15是否个人往来核算IsAuxAccPerson --%>
                        <asp:BoundField DataField="OsDept" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <%--16是否客户往来核算IsAuxAccCustomer--%>
                        <asp:BoundField DataField="OsDept" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <%--17是否供应商往来核算IsAuxAccSupplier--%>
                        <asp:BoundField DataField="OsDept" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <%--18是否项目核算IsAuxAccProject--%>
                        <asp:BoundField DataField="OsDept" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <%--19财务科目对应项目大类编码  T+用不到 为了不改后台程序的序号 这里随便绑定一个 IsAuxAccProject--%>
                        <asp:BoundField DataField="OsDept" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <%--20大类 临时存储bigclasscode--%>
                        <asp:BoundField DataField="OsDept" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <%--21xiao类 临时存储smallclasscode--%>
                        <asp:BoundField DataField="OsDept" HeaderStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                            FooterStyle-CssClass="hiddenbill" />
                        <%--22 现金流量 临时存储xianjinliuxiangmucode--%>
                        <asp:BoundField DataField="xianjinliuxiangmucode" HeaderStyle-CssClass="hiddenbill"
                            ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                        <%--23 会计科目主键--%>
                        <asp:BoundField DataField="idcurrency" HeaderStyle-CssClass="hiddenbill"
                            ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                    </Columns>
                </asp:GridView>
            </div>
            <div style="text-align: right; margin-right: 15px;">
                <asp:Button ID="btnSave" runat="server" Text="保 存" CssClass="baseButton" OnClientClick="return disablebutton();"
                    OnClick="btnSave_Click" />&nbsp;
            <input id="btnCancle" value="取 消" type="button" class="baseButton" onclick="javascript: window.close();" />
            </div>
        </div>
        <div id="divOver" runat="server" style="z-index: 1200; left: 30%; width: 160; cursor: wait; position: absolute; top: 25%; height: 100; visibility: hidden;">
            <table style="width: 17%; height: 10%;">
                <tr>
                    <td>
                        <table style="width: 316px; height: 135px;">
                            <tr align="center" valign="middle">
                                <td>
                                    <img src="../../webBill/Resources/Images/Loading/pressbar2.gif" alt="" /><br />
                                    <b>正在处理中，请耐心等候....<br />
                                    </b>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
            </table>
        </div>
    </form>
</body>
</html>
