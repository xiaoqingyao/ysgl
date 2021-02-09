<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DeptsFykmdb.aspx.cs" Inherits="webBill_tjbb_graph_DeptsFykmdb" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>各部门同费用科目对比</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../FusionCharts/FusionCharts.js" type="text/javascript"></script>

    <style type="text/css">
        #div_left
        {
            display: inline;
            float: left;
            width: 20%;
            height: auto;
        }
        #div_right
        {
            display: inline;
            float: left;
            width: 78%;
        }
        .div_left_items
        {
            float: none;
        }
        .div_left_item
        {
            float: none;
        }
        .left_dataContainer
        {
            min-height: 50px;
            max-height: 200px;
            margin-left: 20px;
            overflow-y: scroll;
            border: 1px solid gray;
        }
        .letf_title
        {
            font-size: 16px;
            margin-top: 5px;
            margin-left: 20px;
            width: 300px;
            height: 30px;
            line-height: 30px;
        }
    </style>

    <script type="text/javascript">

        $(function() {
            var w = $(this).width();
            //$("div_left").attr("width",w*0.25);
            $("div_right").attr("max-width", w * 0.78);
            // $("div_right").attr("width",w*0.75);
        });

        function SingleSelect(aControl) {
            var chk = $("#div_rptYskm input[type='checkbox']:checked");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = false;
            }
            aControl.checked = true;
        }


        function Reset(type) {
            var chk = $("#div_" + type + " input[type='checkbox']:checked");
            for (var s = 0; s < chk.length; s++) {
                chk[s].checked = false;
            }
        }


        function Check() {

            var chk = $("#div_rptYsgc input[type='checkbox']:checked");
            var chk1 = $("#div_rptDept input[type='checkbox']:checked");
            var chk2 = $("#div_rptYskm input[type='checkbox']:checked");
            if (chk.length == 0) {
                alert("请选择预算过程");
                return false;
            }
            else if (chk1.length == 0) {
                alert("请选择部门");
                return false;
            }
            else if (chk2.length == 0) {
                alert("请选择预算科目");
                return false;
            }
            else {
                return true;
            }
        }

        $(function() {
            $("#txt_findYskm").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                    var chk = $("#div_rptYskm input[type='checkbox']:checked");
                    for (var s = 0; s < chk.length; s++) {
                        chk[s].checked = false;
                    }

                    var lbs = $("#div_rptYskm label");
                    for (var s = 0; s < lbs.length; s++) {
                        if (lbs[s].innerText == rybh) {
                            $(lbs[s]).prev("input[type='checkbox']").attr("checked", "true").focus().select();
                            //               var $lbs=$(lbs[s]);
                            //               alert($lbs.attr('name'));
                            //               $lbs.prev("input[type='checkbox']").checked = true;

                        }
                    }
                    //                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function(data, status) {
                    //                        if (status == "success") {
                    //                            $("#txtDept").val(data);
                    //                        }
                    //                        else {
                    //                            alert("获取部门失败");
                    //                        }
                    //                    });
                }
            });
        });
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <div id="div_main">
        <div id="div_left">
            <div style="float: none; border-right: 1px solid black; border-bottom: 1px solid black;">
                <div class="div_left_items">
                    <div class="letf_title">
                        预算过程(多选)<input id="Button2" type="button" value="重置" onclick="Reset('rptYsgc');" />
                    </div>
                    <div class="left_dataContainer">
                        <div id="div_rptYsgc">
                            <asp:Repeater ID="rptYsgc" runat="server">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckYsgc" runat="server" /><%#Eval("xmmc")%>
                                    <asp:HiddenField ID="hfYsgc" runat="server" Value='<%#Eval("gcbh") %>' />
                                    <br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <div class="div_left_items">
                    <div class="letf_title">
                        部门(多选)<input id="Button3" type="button" value="重置" onclick="Reset('rptDept');" />
                    </div>
                    <div class="left_dataContainer">
                        <div id="div_rptDept">
                            <asp:Repeater ID="rptDept" runat="server">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckDept" runat="server" /><%#Eval("deptName")%>
                                    <asp:HiddenField ID="hfDept" runat="server" Value='<%#Eval("deptCode") %>' />
                                    <br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
                <div class="div_left_item">
                    <div class="letf_title">
                        科目(单选)
                    </div>
                    <div class="letf_title" style="font-size: 13px; margin-top: 0px;">
                        快速检索<asp:TextBox ID="txt_findYskm" runat="server"></asp:TextBox>
                    </div>
                    <div class="left_dataContainer">
                        <div id="div_rptYskm">
                            <asp:Repeater ID="rptYskm" runat="server">
                                <ItemTemplate>
                                    <asp:CheckBox ID="ckYskm" runat="server" onclick="javascript:SingleSelect(this);" />
                                    <label>
                                        <%#Eval("yskmMc")%></label>
                                    <asp:HiddenField ID="hfyskm" runat="server" Value='<%#Eval("yskmCode") %>' />
                                    <br />
                                </ItemTemplate>
                            </asp:Repeater>
                        </div>
                    </div>
                </div>
            </div>
        </div>
        <div id="div_right">
            <div style="margin: 20px; text-align: left;">
                <asp:Button ID="Button1" runat="server" Text="生成图表" OnClick="Button1_Click" OnClientClick="return Check()" />
            </div>
                <%--<script type="text/javascript">
                var chart = new FusionCharts("../FusionCharts/MSColumn3D.swf", "ChartId", "600", "350", "0", "0");
                //chart.setDataURL("../FusionCharts/MSColumn3D.xml");
                chart.setDataXML("");
                chart.render("chartdiv");
            </script>--%>
                <%--<script type="text/javascript">
                var chart_FactorySum = new FusionCharts("../FusionCharts/MSColumn3D.swf", "FactorySum", "600", "300", "0", "0");
                chart_FactorySum.setDataXML("<chart caption='Country Comparison' shownames='1' showvalues='0' decimals='0' numberPrefix='$'><categories><category label='Austria' /><category label='Brazil' /><category label='France' /><category label='Germany' /><category label='USA' /></categories><dataset seriesName='1996' color='AFD8F8' showValues='0'><set value='25601.34' /><set value='20148.82' /><set value='17372.76' /><set value='35407.15' /><set value='38105.68' /></dataset><dataset seriesName='1997' color='F6BD0F' showValues='0'><set value='57401.85' /><set value='41941.19' /><set value='45263.37' /><set value='117320.16' /><set value='114845.27' /></dataset><dataset seriesName='1998' color='8BBA00' showValues='0'><set value='45000.65' /><set value='44835.76' /><set value='18722.18' /><set value='77557.31' /><set value='92633.68' /></dataset></chart>");
                chart_FactorySum.render("chartdiv");
            </script>--%>
                <%=FusionHTML %>
        </div>
    </div>
    </form>
</body>
</html>
