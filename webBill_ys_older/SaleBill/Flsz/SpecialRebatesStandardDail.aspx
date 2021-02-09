<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SpecialRebatesStandardDail.aspx.cs"
    Inherits="SaleBill_Flsz_SpecialRebatesStandardDail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>返利设置(废弃页面)</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/bxgl/bxDetail.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
    </style>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js"
        type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        var availableTagsfy = new Array();
        $(function() {


            //车辆类型
            $("#txtCartype").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var carTypeCode = ui.item.value;
                    $("#lbmcar").text("车辆类型：" + carTypeCode);
                }

            });

            //部门选择

            $("#txtdept").autocomplete({
                source: availableTagsdt,
                select: function(event, ui) {
                    var deptCode = ui.item.value;
                    $("#lblbm").text("部门：" + deptCode);
                    $("#hdDeptCode").val(deptCode);


                    //$("#btnHF").click();

                    //根据部门改变费用类别的可选项
                    var fylbsource = '';
                    $.post("../../webBill/MyAjax/GetYSKMByDept.ashx", { "deptCode": deptCode }, function(data, status) {

                        if (status == "success") {
                            fylbsource = data;

                        }
                    });

                    if (fylbsource != '') {
                        availableTagsfy = fylbsource;
                    }

                }
            });

            //费用类别
            $("#txtfylb").autocomplete({

                source: availableTagsfy,
                select: function(event, ui) {

                    var rybh = ui.item.value;
                    $("#lblfy").text("费用：" + rybh);
                }
            });

            $("#btnAddFykm").click(function() {

                var gkbmbh = $("#txtdept").val();

                if (gkbmbh == "") {
                    alert("请先选择部门");
                    return;
                }
                openfy(gkbmbh);
            });

            $("#btcartype").click(function() {

                opencar();
            });

            $("#btn_choosedept").click(function() {
                openBm();
            });
        });


        //选择部门dvid


        function openBm() {
            var str = window.showModalDialog("../select/DeptSelct.aspx", 'newwindow', 'center:yes;dialogHeight:720px;dialogWidth:300px;status:no;scroll:yes');

            if (str != undefined && str != "") {
                var depts = str.split('|');
                var innerval = '';
                for (var i = 0; i < depts.length; i++) {
                    innerval += "<li><span>" + depts[i] + ":</span><input type='text' value='0.00' /></li>";
                }
                innerval += "";
                $("#txtdept").val(depts);
                $("#lblbm").text("部门：" + depts);
            }
        }


        //选择车辆类型dvid

        function opencar() {
            var deptCode = "";
            var str = window.showModalDialog("../select/CarType.aspx", 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');

            if (str != undefined && str != "") {
                var depts = str.split('|');

                var innerval = '';
                for (var i = 0; i < depts.length; i++) {
                    innerval += "<li><span>" + depts[i] + ":</span><input type='text' value='0.00' /></li>";
                }
                innerval += "";

                $("#txtCartype").val(depts);
                $("#lbmcar").text("车辆类型：" + depts);
            }
        }



        //费用类别

        function openfy(deptCode) {

            var str = window.showModalDialog("../select/YSKMSelct.aspx?deptCode=" + deptCode, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var json = $.parseJSON(str);

                var inner = "";
                var checkArray = new Array();
                for (var i = 0; i < json.length; i++) {
                    if (str.indexOf(json[i].Yscode) < 0) {
                        inner += '<tr id="tr_' + kmIndex + '">';
                        inner += "<td>" + json[i].Yscode + "</td>";
                        inner += "<td>" + json[i].Ysje + "</td>";
                        inner += "<td>" + json[i].Syje + "</td>";
                        inner += '<td><input type="text" class="baseText ysje" onblur="htjeChange();" value="0.00" /></td>';
                        inner += '<td><input type="text" class="baseText ysse" onblur="htjeChange();" value="0.00" /></td>';
                        inner += "</tr>";
                        $("#td_dept").append('<ul id="bm_' + kmIndex.toString() + '"></ul>');
                        $("#td_xm").append('<ul id="xm_' + kmIndex.toString() + '"></ul>');
                        kmIndex++;
                    }
                    checkArray[i] = json[i].Yscode;

                }

                $("#txtfylb").val(checkArray);
                $("#lblfy").text("费用：" + checkArray);
            }
        }
       
    </script>

    <style type="text/css">
        ul
        {
            list-style: none;
            margin-left: 0px;
            margin-top: 0px;
            padding-left: 5px;
        }
        ul li
        {
            margin: 5px 5px 5px 5px;
        }
        #td_dept input
        {
            width: 100px;
        }
    </style>
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%" style="text-align:center" border="1">
        <tr>
            <td style="text-align: center; height: 36px;">
                <strong>
                    <asp:Label ID="lbdjmc" runat="server" Text="返利设置"></asp:Label>
                </strong>
            </td>
        </tr>
        <tr>
            <td align="center" style=" text-align:center;">
                <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="100%">
                    <tr>
                        <td class="tableBg2" style="text-align: right">
                            有效日期起：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtbgtime" runat="server"></asp:TextBox>
                        </td>
                        <td class="tableBg2" style="text-align: right">
                            有效日期止：
                        </td>
                        <td align="left">
                            <asp:TextBox ID="txtedtime" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: red; height: 4px" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg2" style="text-align: right">
                            车辆类型：
                        </td>
                        <td colspan="3" align="left">
                            <input id="txtCartype" style="width: 146px" type="text" runat="server" />
                            <input type="button" value="..." id="btcartype" runat="server" class="baseButton" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg2" style="text-align: right">
                            部门：
                        </td>
                        <td colspan="3" align="left">
                            <input id="txtdept" style="width: 146px" type="text" runat="server" />
                            <input type="button" value="..." id="btn_choosedept" runat="server" class="baseButton" />
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg2" style="text-align: right">
                            费用类别：
                        </td>
                        <td colspan="3" align="left">
                            <input id="txtfylb" style="width: 146px" type="text" runat="server" />
                            <input type="button" value="..." id="btnAddFykm" runat="server" class="baseButton" />
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: red; height: 4px" colspan="4">
                        </td>
                    </tr>
                    <tr style="display: none">
                        <td class="tableBg2" style="text-align: right;">
                            详细信息：
                        </td>
                        <td colspan="3">
                            <asp:Label ID="lbmcar" runat="server" Text="车辆类型："></asp:Label>
                            <asp:Label ID="lblbm" runat="server" Text="部门："></asp:Label>
                            <asp:Label ID="lblfy" runat="server" Text="费用选择："></asp:Label>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg2" style="text-align: right">
                            期初分配：
                        </td>
                        <td colspan="3" style="text-align: left">
                            <asp:TextBox ID="txtncfp" runat="server" onkeyup="replaceNaN(this);"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg2" style="text-align: right">
                            销售过程：
                        </td>
                        <td colspan="3">
                            <asp:DataGrid ID="DataGrid1" runat="server" BorderWidth="0" CellPadding="0" ItemStyle-HorizontalAlign="Center"
                                AllowSorting="True" AutoGenerateColumns="False" Width="100%" ShowHeader="false">
                                <ItemStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundColumn DataField="PName">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtTc" runat="server" onkeyup="replaceNaN(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg2" style="text-align: right">
                            配置明细：
                        </td>
                        <td colspan="3">
                            <asp:DataGrid ID="Repeater1" runat="server" BorderWidth="0" CellPadding="0" ItemStyle-HorizontalAlign="Center"
                                AllowSorting="True" AutoGenerateColumns="False" Width="100%" ShowHeader="false">
                                <ItemStyle HorizontalAlign="Center" />
                                <Columns>
                                    <asp:BoundColumn DataField="CName">
                                        <ItemStyle Width="150px" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn>
                                        <ItemStyle HorizontalAlign="Left" />
                                        <ItemTemplate>
                                            <asp:TextBox ID="txtpz" runat="server" onkeyup="replaceNaN(this);"></asp:TextBox>
                                        </ItemTemplate>
                                    </asp:TemplateColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: red; height: 4px" colspan="4">
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg2" style="text-align: right">
                            备注：
                        </td>
                        <td colspan="3" align="left">
                            <asp:TextBox ID="txtbz" runat="server" Width="80%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="4" align="right">
                            <label style="margin-right: 30px">
                                <asp:Button ID="btn_test" class="baseButton" runat="server" Text="确 定" OnClick="btn_test_Click" />
                                <asp:Button ID="btn_client" runat="server" Text="取 消"  
                                CssClass="baseButton" onclick="btn_client_Click"/>
                             </label>
                            <asp:HiddenField ID="hdDeptCode" runat="server" Value="" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>