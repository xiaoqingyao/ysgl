<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebatesStandardQuery.aspx.cs"
    Inherits="SaleBill_Flsz_RebatesStandardQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>标准返利查询</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script type="text/javascript">
        //替换非数字
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
        function bgtimechange() {
            //alert('onblur');
            //           var varbgtime=document.getElementById("txtbgtime").value;
            //           document.getElementById("hidbgtime").value=varbgtime;
            //           alert(varbgtime);
            //           alert(document.getElementById("hidbgtime").value);
            //           document.getElementById("btn_select").click();
        }
    </script>

</head>
<body style="background-color: #EBF2F5">
    <form id="form1" runat="server">
    <div style="width: 100%; height: 30px;">
        <table width="99%" border="0">
            <tr>
                <td style="text-align: right; width: 80px;">
                    有效日期起：
                </td>
                <td align="left" style="text-align: left; width: 120px">
                    <asp:TextBox ID="txtbgtime" runat="server" onfocus="setday(this);" onblur="bgtimechange()"></asp:TextBox>
                </td>
                <td style="text-align: right; width: 80px;">
                    有效日期止：
                </td>
                <td align="left" style="text-align: left; width: 120px">
                    <asp:TextBox ID="txtedtime" runat="server" onfocus="setday(this);"></asp:TextBox>
                </td>
                <td style="text-align: right; width: 80px;">
                    销售公司：
                </td>
                <td align="left" style="text-align: left; width: 120px">
                    <asp:DropDownList ID="ddlSaleDept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="ddlSaleDept_SelectedIndexChanged">
                    </asp:DropDownList>
                </td>
                <td style="display: none">
                    当前选中信息：
                    <asp:Label ID="lblCarMsg" runat="server" Text="车辆类型" />—
                    <asp:Label ID="lblDeptMsg" runat="server" Text="部门" />—
                    <asp:Label ID="lblFeeTypeMsg" runat="server" Text="费用选择" />
                </td>
                <td>
                    <asp:Button ID="btn_select" runat="server" Text="查询" Visible="false" OnClick="btn_select_Click" />
                </td>
            </tr>
        </table>
    </div>
    <div>
        <div style="float: left; width: 20%; overflow:auto; height: 462px;">
            <asp:TreeView ID="treeViewTruckType" runat="server" ShowLines="True" ExpandDepth="2"
                ShowCheckBoxes="None" OnSelectedNodeChanged="treeViewTruckType_SelectedNodeChanged">
                <SelectedNodeStyle BackColor="#CCE8CF" />
                <Nodes>
                    <asp:TreeNode ImageUrl="../../webBill/Resources/Images/treeView/treeHome.gif" SelectAction="None"
                        Text="车辆类型" Value="0"></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </div>
        <div style="float: left; width: 25%; overflow: auto; height: 462px;">
            <asp:TreeView ID="treeViewFeeType" runat="server" ShowLines="True" ExpandDepth="2"
                ShowCheckBoxes="None" OnSelectedNodeChanged="treeViewFeeType_SelectedNodeChanged">
                <SelectedNodeStyle BackColor="#CCE8CF" />
                <Nodes>
                    <asp:TreeNode ImageUrl="../../webBill/Resources/Images/treeView/treeHome.gif" SelectAction="None"
                        Text="预算科目" Value=""></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </div>
        <div style="float: left; width: 55%; overflow: auto; height: 462px;">
            <asp:DataGrid ID="GridView1" runat="server" AutoGenerateColumns="False" CellPadding="3"
                Width="100%" CssClass="myGrid" AllowPaging="True" PageSize="20" ShowHeader="true">
                <Columns>
                    <asp:BoundColumn DataField="SaleCountFrm" HeaderText="辆数从">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Left" Wrap="False" CssClass="myGridHeader " />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="SaleCountTo" HeaderText="到">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Left" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="feekz" HeaderText="项目" DataFormatString="{0:D}">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Left" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Fee" HeaderText="额度">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Right" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" HorizontalAlign="Right"  />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="deptname" HeaderText="部门">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Left" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="caname" HeaderText="车辆类型">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Left" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" HorizontalAlign="Left"/>
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="feename" HeaderText="费用类别">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Left" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" HorizontalAlign="Left" />
                    </asp:BoundColumn>
                    <asp:BoundColumn DataField="Remark" HeaderText="备注">
                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" HorizontalAlign="Left" Wrap="False" CssClass="myGridHeader" />
                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                            Font-Underline="False" Wrap="False" CssClass="myGridItem" HorizontalAlign="Left" />
                    </asp:BoundColumn>
                </Columns>
                <PagerStyle Visible="False" />
            </asp:DataGrid>
            <asp:HiddenField ID="hidbgtime" runat="server" />
            <asp:HiddenField ID="hidendtime" runat="server" />
        </div>
    </div>
    </form>
</body>
</html>
