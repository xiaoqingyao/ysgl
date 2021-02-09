<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GKDeptFenJieBiLi.aspx.cs"
    Inherits="webBill_yskm_GKDeptFenJieBiLi" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script type="text/javascript">
        $(function() {
            $("#btn_inExcel").click(function() {
                if ($("#lblNowDeptForBz").text() == '未选择' || $("#lblNowKmForBz").text() == '未选择') {
                    alert("请先选择归口部门和预算科目。"); return;
                }
                var returnValue = window.showModalDialog('GkDeptFenJieBili_InExcel.aspx', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:800px;status:no;scroll:no');
                if (returnValue == undefined || returnValue == "")
                { }
                else {
                    $("#btn_reload").click();
                }
            });

        });
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div style="width: 100%; height: 10%">
        年度：<asp:DropDownList runat="server" ID="ddlYear">
            <%--OnSelectedIndexChanged="ddlYear_SelectedIndexChanged"--%>
        </asp:DropDownList>
        <span style="color: Red">【友情提示】：请先选择年度，如果没有所需年度，请先开启对应预算过程。</span>
    </div>
    <div>
        <div style="float: left; width: 20%; overflow: auto; height: 460px">
            <asp:TreeView ID="treeViewGkDept" runat="server" ShowLines="True" ExpandDepth="2"
                ShowCheckBoxes="None" OnSelectedNodeChanged="treeViewGkDept_SelectedNodeChanged">
                <SelectedNodeStyle BackColor="#CCE8CF" />
                <Nodes>
                    <asp:TreeNode ImageUrl="../../webBill/Resources/Images/treeView/treeHome.gif" SelectAction="None"
                        Text="归口部门" Value=""></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </div>
        <div style="float: left; width: 25%; overflow: auto; height: 460px">
            <span style="color: Red">部门：<asp:Label runat="server" ID="lblNowDeptForKm">未选择</asp:Label></span>
            <asp:TreeView ID="treeViewFeeType" runat="server" ShowLines="True" ExpandDepth="2"
                ShowCheckBoxes="None" OnSelectedNodeChanged="treeViewFeeType_SelectedNodeChanged">
                <SelectedNodeStyle BackColor="#CCE8CF" />
                <Nodes>
                    <asp:TreeNode ImageUrl="../../webBill/Resources/Images/treeView/treeHome.gif" SelectAction="None"
                        Text="预算科目" Value=""></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </div>
        <div style="float: left; width: 55%; overflow: auto; height: 50%;">
            <span style="color: Red">部门：<asp:Label runat="server" ID="lblNowDeptForBz">未选择</asp:Label>
                科目：<asp:Label runat="server" ID="lblNowKmForBz">未选择</asp:Label></span>
            <asp:Button runat="server" ID="btn_outExcel" Text="导出Excel模板" CssClass="baseButton"
                OnClick="btn_outExcel_Click" />
            <input type="button" value="导入Excel" id="btn_inExcel" class="baseButton" />
            <asp:Button ID="btnSave" runat="server" Text="保存" CssClass="baseButton" OnClick="btnSave_OnClick" />
            <asp:Button ID="btn_reload" runat="server" OnClick="btn_reload_Click" CssClass="hiddenbill" />
            <span style="color: Red">
                <asp:Label ID="lblmsg" runat="server"></asp:Label></span>
            <div style="overflow: auto; height: 450px;">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                    ShowHeader="true" ShowFooter="true" Width="100%" EmptyDataText="暂无数据" OnRowDataBound="GridView1_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="deptcode" HeaderText="部门编号" ItemStyle-CssClass="myGridItem"
                            HeaderStyle-CssClass="myGridHeader" FooterStyle-CssClass="myGridItem" />
                        <asp:BoundField DataField="deptname" HeaderText="部门名称" ItemStyle-CssClass="myGridItem"
                            HeaderStyle-CssClass="myGridHeader" FooterStyle-CssClass="myGridItem" />
                        <asp:TemplateField HeaderText="分解比例" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader"
                            FooterStyle-CssClass="myGridItem">
                            <ItemTemplate>
                                <asp:TextBox ID="bl" Text="0.00" runat="server"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="fjbl" HeaderText="比例" HeaderStyle-CssClass="hiddenbill"
                            ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
    </form>
</body>
</html>
