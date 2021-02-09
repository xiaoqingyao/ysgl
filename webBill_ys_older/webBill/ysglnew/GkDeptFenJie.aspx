<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GkDeptFenJie.aspx.cs" Inherits="webBill_ysglnew_GkDeptFenJie" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>归口部门金额分解</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script type="text/javascript">
        function pingjunrow(selfrow) {
            var rowtotalamount = $(selfrow).parent().parent().children().eq(3).html(); //年度预算
            var pingjunshu = rowtotalamount / 12;
            pingjunshu = pingjunshu.toFixed(2);
            var lastamount = rowtotalamount - pingjunshu * 11;
            var allinput = $(selfrow).parent().parent().find("input:text");

            for (var i = 0; i < allinput.length - 1; i++) {
                allinput.eq(i).val(pingjunshu);
            }
            allinput.eq(11).val(lastamount.toFixed(2));
        }
        function tconfirm() {
            if (confirm("保存后，将直接生成各部门可报销的预算数，是否继续？")) {
                document.getElementById("divOver").style.visibility = "visible";
                return true;
            } else { return false; }
        }
        function changefangshi(self) {
            if (self.value == '0') {
                $(self).parent().children().eq(1).addClass("hiddenbill");
                $(self).parent().children().eq(2).addClass("hiddenbill");
            } else if (self.value == '1') {
                $(self).parent().children().eq(1).removeClass();
                $(self).parent().children().eq(1).addClass("baseButton");
                $(self).parent().children().eq(2).addClass("hiddenbill");

            } else if (self.value == '2') {
                $(self).parent().children().eq(2).removeClass();
                $(self).parent().children().eq(2).addClass("baseButton");
                $(self).parent().children().eq(1).addClass("hiddenbill");
            }
        }
        //        function inputexcelForRow(self) {

        //        }
        function changefangshiForTab(self) {
            if (self.value == '0') {
                $("#pingjunForTab").addClass("hiddenbill");
                $("#importExcelForTab").addClass("hiddenbill");
            } else if (self.value == '1') {
                $("#pingjunForTab").removeClass();
                $("#pingjunForTab").addClass("baseButton");
                $("#btn_export").addClass("hiddenbill");
                $("#importExcelForTab").addClass("hiddenbill");
            } else if (self.value == '2') {
                $("#pingjunForTab").addClass("hiddenbill");
                $("#btn_export").removeClass();
                $("#importExcelForTab").removeClass();
                $("#btn_export").addClass("baseButton");
                $("#importExcelForTab").addClass("baseButton");
            }
        }
        function pingjunAlltab() {
            var gridview = $("#<%=GridView1.ClientID %> tbody tr");
            for (var i = 1; i < gridview.length; i++) {
                var rowtotalamount = gridview.eq(i).children().eq(3).html();
                var allinput = gridview.eq(i).find("input:text");
                var pingjunshu = rowtotalamount / 12;
                pingjunshu = pingjunshu.toFixed(2);
                var lastamount = rowtotalamount - pingjunshu * 11;
                for (var j = 0; j < allinput.length - 1; j++) {
                    allinput.eq(j).val(pingjunshu);
                }
                allinput.eq(11).val(lastamount.toFixed(2));
            }
        }
        function importExcel() {
            if ($("#ddlNd").val() == '' || $("#ddlKm").val() == '') {
                alert('请先选择年度和预算科目。');
                return;
            }
            var returnValue = window.showModalDialog('GkDeptFenJie_InExcel.aspx' + '', 'newwindow', 'center:yes;dialogHeight:400px;dialogWidth:800px;status:no;scroll:no');
            if (returnValue == undefined || returnValue == "")
            { }
            else {
                $("#btn_reload").click();
            }
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <div class="baseDiv">
            归口部门：<asp:DropDownList ID="LaDept" runat="server" AutoPostBack="true" OnSelectedIndexChanged="LaDept_SelectedIndexChanged">
            </asp:DropDownList>
            年度：<asp:DropDownList ID="ddlNd" runat="server" AutoPostBack="True" CssClass="baseSelect"
                OnSelectedIndexChanged="drpSelectNd_SelectedIndexChanged">
            </asp:DropDownList>
            <%--预算科目：<asp:DropDownList runat="server" ID="ddlKm" AutoPostBack="True" CssClass="baseSelect"
            OnSelectedIndexChanged="ddlKm_OnSelectedIndexChanged">
        </asp:DropDownList>--%>
            <%-- 归口预算：<asp:Label runat="server" ID="lblgkdeptamount"></asp:Label>--%>
            <asp:Button runat="server" ID="btn_save" Text="保 存" OnClick="btn_save_click" CssClass="baseButton"
                OnClientClick="return tconfirm();" />
            分解方式：<select id="seFjfs" onchange="changefangshiForTab(this);">
                <option value="0">手动填写</option>
                <option value="1">平均分配</option>
                <option value="2">导入EXCEL</option>
            </select>
            <input type="button" id="pingjunForTab" value="平均分配所有" class="hiddenbill" onclick="pingjunAlltab();" />
            <asp:Button runat="server" ID="btn_export" OnClick="btn_export_Click" CssClass="hiddenbill"
                Text="导出Excel模板" />
            <input type="button" id="importExcelForTab" value="导入Excel" class="hiddenbill" onclick="importExcel();" />
            <asp:Button ID="btn_reload" runat="server" OnClick="btn_reload_Click" CssClass="hiddenbill" />
            <div style="overflow: auto; height: 460px">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="myGrid"
                    ShowHeader="true" ShowFooter="true" Width="125%" EmptyDataText="暂无数据" OnRowDataBound="GridView1_OnRowDataBound">
                    <Columns>
                        <asp:TemplateField HeaderText="部门" ItemStyle-CssClass="myGridItem"
                            HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                [<%#DataBinder.Eval(Container.DataItem, "fjdeptcode")%>]<%#DataBinder.Eval(Container.DataItem, "deptname")%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="yskmmc" HeaderText="预算科目" ItemStyle-CssClass="myGridItem"
                            HeaderStyle-CssClass="myGridHeader" />
                        <asp:BoundField DataField="fjbl" HeaderText="分配比例" ItemStyle-CssClass="myGridItemRight"
                            HeaderStyle-CssClass="myGridHeader" />
                        <asp:BoundField DataField="nysje" HeaderText="年预算总额" ItemStyle-CssClass="myGridItemRight"
                            HeaderStyle-CssClass="myGridHeader" DataFormatString="{0:F2}" />
                        <asp:TemplateField HeaderText="一月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="one" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="二月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="two" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="三月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="three" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="四月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="four" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="五月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="five" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="六月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="six" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="七月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="seven" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="八月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="eight" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="九月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="nine" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="十月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="teen" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="十一月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="eleven" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="十二月份预算" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <asp:TextBox runat="server" ID="twelve" Width="60"></asp:TextBox>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="分配方式" ItemStyle-CssClass="myGridItem" HeaderStyle-CssClass="myGridHeader">
                            <ItemTemplate>
                                <select id="fpfs" onchange="changefangshi(this);">
                                    <option value="0">手动填写</option>
                                    <option value="1">平均分配</option>
                                </select>
                                <input type="button" id="pingjun" value="平均分配" class="hiddenbill" onclick="pingjunrow(this);" />
                                <%--<input type="button" id="inexcel" value="导入excel" class="hiddenbill" onclick="inputexcelForRow(this);" />--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
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
