<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AddSpecialRebatesBySaleBill.aspx.cs"
    Inherits="SaleBill_Salepreass_AddSpecialRebatesBySaleBill" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>根据销售订单选择车辆</title>
     <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script type="text/javascript">
        function replaceNaN(obj) {
            var objval = obj.value;
            if (objval.indexOf("-") == 0) {
                objval = objval.substr(1);
            }
            if (isNaN(objval)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            }
        }
        //非空验证
        function checkisNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("不能为空！");
                obj.focus();
                return false;
            };
        }
        function SelectAll(aControl) {
            var chk = document.getElementById("GridView1").getElementsByTagName("input");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = aControl.checked;
            }
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div>
        <table class="baseTable" style="text-align: left;" width="90%">
            <tr>
                <td>
                订单号：
                    <asp:TextBox ID="txtBillCode" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSelect" runat="server" Text="查询" CssClass="baseButton" OnClick="btnSelect_Click" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                        Width="100%">
                        <Columns>
                            <asp:TemplateField HeaderText="选择" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <HeaderTemplate>
                                <asp:CheckBox ID="chk_SelectedHeaderAll" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                        Text="全选" />
                            </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField DataField="ddh" HeaderText="订单号" HeaderStyle-CssClass="myGridHeader" />
                            <asp:BoundField DataField="cjh" HeaderText="车架号" HeaderStyle-CssClass="myGridHeader" />
                            <asp:TemplateField HeaderText="正常返利" HeaderStyle-CssClass="myGridHeader">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtStandardSaleAmount" runat="server" onblur="checkisNaN(this);"
                                        onkeyup="replaceNaN(this);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="超出返利" HeaderStyle-CssClass="myGridHeader">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtExceedStandardPoint" runat="server" onblur="checkisNaN(this);"
                                        onkeyup="replaceNaN(this);"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="原因" HeaderStyle-CssClass="myGridHeader">
                                <ItemTemplate>
                                    <asp:TextBox ID="txtExplain" runat="server"></asp:TextBox>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Button runat="server" ID="btn_sure" OnClick="btn_sure_Click" Text="确 定" CssClass="baseButton"/>
                    <input type="button" id="cancle" onclick="javascript:window.close();" value="取 消" class="baseButton" />
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
