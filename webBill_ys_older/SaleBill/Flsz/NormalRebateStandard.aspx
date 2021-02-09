<%@ Page Language="C#" AutoEventWireup="true" CodeFile="NormalRebateStandard.aspx.cs"
    Inherits="SaleBill_Flsz_NormalRebateStandard" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>一般返利设置</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
        function isnull() {
            var varbgtime = document.getElementById("txtbgtime").value;
            var varedtime = document.getElementById("txtedtime").value;
            var varcar = document.getElementById("lblCarMsg").innerText;
            var vardept = document.getElementById("lblDeptMsg").innerText;
            var varfeetype = document.getElementById("lblFeeTypeMsg").innerText;

            if (varbgtime.length == 0) {
                alert("有效日期起不能为空！");
                return false;
            }
            if (varedtime.length == 0) {
                alert("有效日期止不能为空！");
                return false;
            }
            if (varcar == "车辆类型") {

                alert("请选择车辆类型！");
                return false;
            } if (vardept == "部门" || vardept == null) {
                alert("请选择部门！");
                return false;
            } if (varfeetype == "费用选择") {
                alert("请选择费用类别！");
                return false;
            }
            return true;

        }
        function verification() {
            var varnumberfrom = document.getElementById("txtnumberfrom").value;
            var varnumberto = document.getElementById("txtnumberto").value;
            var varmoney = document.getElementById("txtmoney").value;
            var varbgtime = document.getElementById("txtbgtime").value;
            var varedtime = document.getElementById("txtedtime").value;
            var varcar = document.getElementById("lblCarMsg").innerText;
            var vardept = document.getElementById("lblDeptMsg").innerText;
            var varfeetype = document.getElementById("lblFeeTypeMsg").innerText;
            var varobject=document.getElementById("drowpobject").value;
            if(varobject!="[Qcfp]期初分配")
            {
                     if (varbgtime.length == 0) {
                    alert("有效日期起不能为空！");
                    return false;
                }
                if (varedtime.length == 0) {
                    alert("有效日期止不能为空！");
                    return false;
                }
                if (varcar == "车辆类型") {

                    alert("请选择车辆类型！");
                    return false;
                } 
                 if (varnumberfrom.length == 0) {
                alert("车辆数起不能为空！");
                document.getElementById("txtnumberfrom").focus();
                return false;
                }
                if (varnumberto.length == 0) {
                    alert("车辆数止不能为空！");
                    document.getElementById("txtnumberto").focus();
                    return false;
                }
                if (varnumberto - varnumberfrom<=0) {
                    alert("辆数止大于辆数起数！");
                    return false;
                }
               
            }

             if (vardept == "部门" || vardept == null) {
                alert("请选择部门！");
                return false;
            } if (varfeetype == "费用选择") {
                alert("请选择费用类别！");
                return false;
            }
             if (varmoney.length == 0) {
                    alert("额度不能为空！");
                    document.getElementById("txtmoney").focus();
                    return false;
                }
           
            return true;
        }
     
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div style="width: 100%; height: 30%;">
        <table border="0">
            <tr>
                <td style="text-align: right; width: 80px;">
                    有效日期起：
                </td>
                <td align="left" style="text-align: left; width: 120px">
                    <asp:TextBox ID="txtbgtime" runat="server" onfocus="setday(this);"></asp:TextBox>
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
                <td style="display:none">
                    当前选中信息：
                    <asp:Label ID="lblCarMsg" runat="server" Text="车辆类型" />—
                    <asp:Label ID="lblDeptMsg" runat="server" Text="部门" />—
                    <asp:Label ID="lblFeeTypeMsg" runat="server" Text="费用选择" />
                </td>
            </tr>
        </table>
    </div>
    <div style="height: 70%;">
        <div style="float: left; width: 20%; overflow: auto; height: 460px">
            <asp:TreeView ID="treeViewTruckType" runat="server" ShowLines="True" ExpandDepth="2"
                ShowCheckBoxes="None" OnSelectedNodeChanged="treeViewTruckType_SelectedNodeChanged">
                <SelectedNodeStyle BackColor="#CCE8CF" />
                <Nodes>
                    <asp:TreeNode ImageUrl="../../webBill/Resources/Images/treeView/treeHome.gif" SelectAction="None"
                        Text="车辆类型" Value="0"></asp:TreeNode>
                </Nodes>
            </asp:TreeView>
        </div>
        <div style="float: left; width: 25%; overflow: auto; height: 460px">
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
            <table class="baseTable" border="0" width="100%">
                <tr>
                    <td style="text-align: right; width: 50px;">
                        辆数从：
                    </td>
                    <td>
                        <asp:TextBox ID="txtnumberfrom" runat="server" onkeyup="replaceNaN(this);" Width="90%"></asp:TextBox>
                    </td>
                    <td style="text-align: right; width: 50px;">
                        到：
                    </td>
                    <td>
                        <asp:TextBox ID="txtnumberto" runat="server" onkeyup="replaceNaN(this);" Width="90%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 50px;">
                        项目：
                    </td>
                    <td>
                        <asp:DropDownList ID="drowpobject" runat="server" Width="92%">
                        </asp:DropDownList>
                    </td>
                    <td style="text-align: right; width: 50px;">
                        额度：
                    </td>
                    <td>
                        <asp:TextBox ID="txtmoney" runat="server" Width="90%" onkeyup="replaceNaN(this);"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: right; width: 50px;">
                        备注：
                    </td>
                    <td colspan="3">
                        <asp:TextBox ID="txtbz" runat="server" Width="96%"></asp:TextBox>
                    </td>
                </tr>
                <tr>
                    <td style="text-align: left" colspan="4">
                        <asp:Button ID="btnAdd" runat="server" CssClass="baseButton" Text="添 加" OnClick="btnAdd_Click"
                            OnClientClick=" return verification() " />
                        <asp:Button ID="btn_Del" runat="server" CssClass="baseButton" Text="删 除" OnClick="btn_Del_Click" />
                    </td>
                </tr>
                <tr>
                    <td colspan="4">
                        <div style="overflow: scroll; width: 100%; height: 300px">
                            <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                                ShowHeader="true" Width="120%" EmptyDataText="暂无数据">
                                <Columns>
                                    <asp:TemplateField HeaderText="选 择" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                                        <ItemTemplate>
                                            <asp:CheckBox ID="CheckBox2" runat="server" />
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                    <asp:BoundField DataField="NID" HeaderText="编号" HeaderStyle-CssClass="hiddenbill" ItemStyle-HorizontalAlign="Center"
                                        ControlStyle-CssClass="hiddenbill" ItemStyle-CssClass="hiddenbill" FooterStyle-CssClass="hiddenbill" />
                                    <asp:BoundField DataField="SaleCountFrm" HeaderText="辆数从" HeaderStyle-CssClass="myGridHeader" ItemStyle-HorizontalAlign="Center" />
                                    <asp:BoundField DataField="SaleCountTo" HeaderText="到" HeaderStyle-CssClass="myGridHeader"  ItemStyle-HorizontalAlign="Center"  />
                                    <asp:BoundField DataField="ControlItemCode" HeaderText="项目" HeaderStyle-CssClass="myGridHeader"  ItemStyle-HorizontalAlign="Center"  />
                                    <asp:BoundField DataField="Fee" HeaderText="额度" HeaderStyle-CssClass="myGridHeader"  ItemStyle-HorizontalAlign="Right"  />
                                    <asp:BoundField DataField="Status" HeaderText="状态" HeaderStyle-CssClass="myGridHeader"  ItemStyle-HorizontalAlign="Center"  />
                                    <asp:BoundField DataField="DeptCode" HeaderText="销售公司" HeaderStyle-CssClass="myGridHeader"  ItemStyle-HorizontalAlign="Center"  />
                                    <asp:BoundField DataField="TruckTypeCode" HeaderText="车辆类型" HeaderStyle-CssClass="myGridHeader"  ItemStyle-HorizontalAlign="Center"  />
                                    <asp:BoundField DataField="SaleFeeTypeCode" HeaderText="费用类别" HeaderStyle-CssClass="myGridHeader" ItemStyle-HorizontalAlign="Center"  />
                                    <asp:BoundField DataField="Remark" HeaderText="备注" HeaderStyle-CssClass="myGridHeader" ItemStyle-HorizontalAlign="left"  />
                                </Columns>
                            </asp:GridView>
                        </div>
                    </td>
                </tr>
                <tr style="display: none">
                    <td colspan="4" style="text-align: right">
                        <asp:Button ID="btn_Save" class="baseButton" runat="server" Text="保 存" OnClientClick=" return isnull()" />
                    </td>
                </tr>
            </table>
        </div>
    </div>
    </form>
</body>
</html>
