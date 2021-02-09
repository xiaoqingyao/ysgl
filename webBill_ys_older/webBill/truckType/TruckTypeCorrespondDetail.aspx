<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TruckTypeCorrespondDetail.aspx.cs"
    Inherits="webBill_truckType_TruckTypeCorrespondDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>车辆类型对应编辑</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/calender.js" type="text/javascript"></script>

    <script type="text/javascript" language="javascript">

        $(function() {

            $("#<%=GridView1.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=GridView1.ClientID%> tr").removeClass("highlight");
                try {
                    var billCode = $(this).find("td")[1].innerHTML;
                    if (billCode != null && billCode != "编号") {
                        $(this).addClass("highlight");
                        var nbCxh = $(this).find("td")[3].innerHTML;
                        $("#txtcartype").val(nbCxh);
                    }
                } catch (e) { e.exception; $("#txtcartype").val(''); }
            });
            $("#txtcartype").autocomplete({
                source: availableTags
            });
        });
    

        function verification() {
            var varcar = document.getElementById("lblCarMsg").innerText;
            if (varcar == "车辆类型") {
                alert("请选择车辆类型！");
                return false;
            }
            return true;
        }

    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <table border="1" style="display: none">
        <tr>
            <td>
                当前选中的内部编号：
                <asp:Label ID="lblCarMsg" runat="server" Text="车辆类型" />
            </td>
        </tr>
    </table>
    <div style="height: 90%; width: 98%; margin: 0 auto;">
        <div style="float: left; width: 30%; overflow: auto; height: 460px">
            <asp:TreeView ID="treeViewTruckType" runat="server" ShowLines="True" ExpandDepth="2"
                ShowCheckBoxes="None" OnSelectedNodeChanged="treeViewTruckType_SelectedNodeChanged">
                <SelectedNodeStyle BackColor="#CCE8CF" />
                <Nodes>
                    <asp:TreeNode ImageUrl="../Resources/Images/treeView/treeHome.gif" SelectAction="None"
                        Text="车辆类型" Value="0"></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </div>
        <div style="float: left; overflow: auto; margin-left: 5px; width: 69%;">
            <table class="baseTable" border="0" width="100%" style="height: 100%">
                <tr>
                    <td style="text-align: right; width: 100px;">
                        内部车型号：
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtcartype" runat="server" Width="96%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left;" colspan="4">
                        <asp:Button ID="btnAdd" runat="server" CssClass="baseButton" Text="添 加" OnClientClick=" return verification() "
                            OnClick="btnAdd_Click" />
                        <asp:Button ID="btn_Del" runat="server" CssClass="baseButton" Text="删 除" OnClick="btn_Del_Click" />
                    </td>
                </tr>
                <tr runat="server" id="trgrid">
                    <td colspan="4" style="width: auto;">
                        <div style="overflow: scroll; width: 100%; height: 383px;">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                                ShowHeader="true" Width="100%" EmptyDataText="暂无数据">
                                <Columns>
                                    <asp:TemplateField HeaderText="选 择" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox2" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="list_id" HeaderText="编号" HeaderStyle-CssClass="hiddenbill"
                                        ItemStyle-HorizontalAlign="Center" ControlStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill"
                                        FooterStyle-CssClass="hiddenbill" />
                                    <asp:BoundField DataField="truckTypeName" HeaderText="车辆类型" HeaderStyle-CssClass="myGridHeader"
                                        ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="factTruckType" HeaderText="内部车型号" HeaderStyle-CssClass="myGridHeader"
                                        ItemStyle-HorizontalAlign="Left" />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr style="display: none">
                    <td colspan="4" style="text-align: right">
                        <asp:Button ID="btn_Save" class="baseButton" runat="server" Text="保 存" OnClientClick=" return isnull()" />
                        <asp:HiddenField ID="hidfieldcartype" runat="server" />
                    </td>
                </tr>
            </table>
            <asp:HiddenField ID="HiddenBh" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
