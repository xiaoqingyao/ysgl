<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ystbHzDetail.aspx.cs" Inherits="webBill_ysglnew_ystbHzDetail" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/fixHc/superTables.js"></script>
    <link href="../Resources/jScript/fixHc/superTables_Default.css" rel="stylesheet" />
    <script src="../Resources/jScript/Common.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(function () {
            initMainTableClass("<%=GridView1.ClientID%>");
           <%-- $("#btn_zt").click(function () {
                var deptcode = '<%=Request["deptCode"]%>';
                var nd = $("#drpSelectNd").val();
                window.showModalDialog("gkdept_zt_list.aspx?deptcode=" + deptcode + "&nian=" + nd, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:500px;status:no;scroll:yes');
                //openDetail("gkdept_zt_list.aspx?deptcode=" + deptcode + "&nian=" + nd);
            });--%>
        });
        function selectdept() {
            var returnval = window.showModalDialog("../select/dbSelectDepts.aspx?showflg=all&type=m", 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:500px;status:no;scroll:yes');
            var obj = $.parseJSON(returnval);
            var show = '';
            var val = '';
            if (obj == null) {
                return;
            }
            for (var i = 0; i < obj.length; i++) {
                show += obj[i].name + ","
                val += obj[i].code + ','
            }
            $("#lblDepts").text(show);
            $("#hdshowdepts").val(show);
            $("#hddepts").val(val);
        }
        //通过
        function tongguo() {
            if (confirm("是否确定审核？")) {
                var billcode = '<%=Request["billcode"]%>';
                billcode = billcode + "*,";
                var mind = $("#mind").val();
                mind = escape(mind);
                $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": "", "action": "approve" }, function (data, status) {
                    if (data < 0 && status != "success") { alert("审核失败！"); }
                });
                alert("操作成功！");
                window.location.href = "../MyWorkFlow/BillMainToApprove.aspx?flowid=yshz";
            }

        }
        //驳回
        function bohui() {
            if (confirm("是否确定驳回？")) {
                var billcode = '<%=Request["billcode"]%>';
                var mind = $("#mind").val();
                mind = escape(mind);
                billcode = billcode + "*" + mind + ",";
                $.post("../MyWorkFlow/WorkFlowApprove.ashx", { "billcode": billcode, "mind": "", "action": "disagree" }, function (data, status) {
                    if (data < 0 && status != "success") { alert("审核失败！"); }
                });
                alert("操作成功！");
                window.location.href = "../MyWorkFlow/BillMainToApprove.aspx?flowid=yshz";
            }

        }
        function fanhui() {
            var url = '<%= Request["fromurl"]%>';
            var xmcode = '<%=Request["xmcode"]%>';//项目编号

            if (xmcode.length > 1) {
                url = "ystbHzList.aspx?xmys=1";
            } else {
                if (url == null || url == undefined || url == "") {
                    url = "ystbHzList.aspx";
                } else if (url == "-1") {
                    window.history.go(-1);
                    return;
                }
            }

            window.location.href = url;
        }
    </script>
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div class="baseDiv">
            <div style="margin-top: 5px; margin-left: 5px; margin-bottom: 5px;">
                <asp:Panel ID="pShenHe" runat="server" Visible="false">
                    <div style="margin-top: 5px;">
                        审批意见：
                    <input type="text" id="mind" style="width: 200px;" />
                        <input type="button" value="审批通过" onclick="tongguo();" class="baseButton" id="btntongguo" />
                        <input type="button" value="审批驳回" onclick="bohui();" class="baseButton" id="btnbohui" />
                        <input type="button" value="返回审核页面" onclick="window.history.go(-1);" class="baseButton" id="fanhui" />
                    </div>
                </asp:Panel>
            </div>
            <input type="button" class="baseButton" value="返  回" id="returnlist" runat="server" onclick="fanhui();" />
            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;财年：&nbsp;<asp:DropDownList ID="drpSelectNd" runat="server" Width="202" />
            <asp:RadioButton ID="yskm" runat="server" GroupName="a" Text="按预算科目汇总" Checked="true" OnCheckedChanged="yskm_CheckedChanged" AutoPostBack="true" />
            <asp:RadioButton ID="dept" runat="server" GroupName="a" Text="按部门汇总" OnCheckedChanged="yskm_CheckedChanged" AutoPostBack="true" />
            <asp:Button ID="btn_excel" runat="server" Text="导出excel表格" CssClass="baseButton" OnClick="btn_excel_Click" />
            <asp:Label ID="lbl_masge" runat="server" ForeColor="Red"></asp:Label>
            <div style="margin: 5px;" runat="server" id="divHzbm">
                汇总部门：
            <asp:Label ID="lblDepts" runat="server" Text=""></asp:Label>
            </div>
            <asp:HiddenField ID="hdshowdepts" runat="server" />
            <asp:HiddenField ID="hddepts" runat="server" />

            <input type="button" value="选  择" onclick="selectdept();" class="baseButton" id="select" runat="server" />
            <asp:Button ID="Button1" runat="server" Text="生成汇总表" CssClass="baseButton" OnClick="Button1_Click1" />
            <asp:Button ID="btn_save" runat="server" Text="保  存" CssClass="baseButton" OnClick="btn_save_Click" />
            <%--<input type="button" id="btn_zt" value="查看分校预算状态" class="baseButton"     AutoGenerateColumns="false"/>--%>
            <div style="margin-top: 5px; margin-left: 5px;">
                <asp:GridView ID="GridView1" runat="server" CssClass="myGrid" OnRowDataBound="GridView1_RowDataBound"
                    ShowFooter="true">
                    <HeaderStyle CssClass="myGridHeader" />
                    <RowStyle CssClass="myGridItemRight" HorizontalAlign="Right" />
                    <FooterStyle CssClass="myGridItem" />

                </asp:GridView>
            </div>
            <div style="margin-top: 5px; margin-left: 5px;">
                <asp:GridView ID="GridView2" runat="server" CssClass="myGrid" OnRowDataBound="GridView1_RowDataBound"
                    ShowFooter="true">
                    <HeaderStyle CssClass="myGridHeader" />
                    <RowStyle CssClass="myGridItemRight" HorizontalAlign="Right" />
                    <FooterStyle CssClass="myGridItem" />

                </asp:GridView>
            </div>

        </div>
    </form>
    <script type="text/javascript">
        fix("GridView1", $(window).height() - 70, $(window).width() - 40, 3);
        fix("GridView2", $(window).height() - 70, $(window).width() - 40, 3);
    </script>
</body>
</html>
