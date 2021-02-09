<%@ Page Language="C#" AutoEventWireup="true" CodeFile="WorkFlowList_Dept.aspx.cs" Inherits="webBill_MyWorkFlow_WorkFlowList_Dept" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/jquery.multiselect.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.multiselect.min.js" type="text/javascript"></script>

    <style type="text/css">
        #sortable {
            list-style-type: none;
            margin: 0;
            padding: 0;
            width: 90%;
        }

            #sortable li {
                margin: 0 3px 3px 3px;
                padding: 0.4em;
                padding-left: 1.5em;
                font-size: 1.4em;
                height: 20px;
            }

                #sortable li table {
                    position: absolute;
                    margin-left: 0em;
                }

                #sortable li input {
                    position: absolute;
                    margin-left: -1.5em;
                }

        .highlight {
            background: #EBF2F5;
        }

        .hiddenbill {
            display: none;
        }

        .style1 {
            background-color: #EDEDED;
            width: 100px;
            text-align: center;
        }
    </style>

    <script type="text/javascript" language="javascript">
        $(function () {
            $("#TextBox3").datepicker();
            //kmTypeChange();
            var uicont = $("#sortable>li").length;
            $("#dialog").dialog({
                autoOpen: false,
                width: 600,
                buttons: {
                    "确定": function () {
                        var sptype = $("#DropDownList1").val() + "[" + $("#DropDownList1 option:selected").text() + "]";
                        var spr = "";
                        if ($("#txt_spr").val() != null) {
                            spr = $("#txt_spr").val();
                            var index1 = spr.indexOf(']');
                            if (index1 == '-1' && $("#DropDownList1").val() == "01") {
                                alert("请以 编号[姓名]的格式正确填写审批人。");
                                $("#txt_spr").val('');
                                $("#txt_spr").focus();
                                return false;
                            }
                        }
                        var sxje = inputnum($("#TextBox2").val());

                        var sxrq = $("#TextBox3").val();
                        var checktype = $("#drp_type").val() + "[" + $("#drp_type option:selected").text() + "]";//审批类型（单签/会签）
                        var isKmzhuguan = $("#drp_isKeMuZhuGuan").val() + "[" + $("#drp_isKeMuZhuGuan option:selected").text() + "]";//是否设置金额控制流程
                        var maxje = inputnum($("#txtmaxmoney").val());
                        var checkfylx = $("#drp_fylx").val() + "[" + $("#drp_fylx option:selected").text() + "]";//审批类型
                      
                        uicont++;
                        $("#sortable").append("<li id='li_" + uicont + "' class='ui-state-default'><input type='checkbox' id='chk_" + uicont + "' /><table><tr><td>审批类型:</td><td>" + sptype + "</td><td>审批人:</td><td>" + spr + "</td><td>生效金额:</td><td>" + sxje + "</td><td>限额:</td><td>" + maxje + "</td><td>生效日期:</td><td>" + sxrq + "</td><td>类型:</td><td>" + checktype + "</td><td>是否科目主管：</td><td>" + isKmzhuguan + "</td><td>费用类型：</td><td>" + checkfylx + "</td></tr></table></li>");
                        //把备注和备注的字拿出来 备注: " + bz + "
                        $(this).dialog("close");
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                }
            });
            //显示多选人员框
            $("#duoxuan").dialog({
                autoOpen: false,
                width: 450,
                height: 260,
                modal: false,
                buttons: {
                    "确定": function () {
                        $("#txt_spr").val($("#dx_hdyxz").val());
                        $(this).dialog("close");
                    },
                    "取消": function () {
                        $(this).dialog("close");
                    }
                }
            });

            $("#sortable li").click(function () {
                $("#sortable li").each(function () { $(this).removeClass("highlight"); });
                $(this).addClass("highlight");
            });
            $("#sortable").sortable(); //拖拽方法
            $("#sortable").disableSelection();
            $("#btn_insert").click(function () {
                $('#dialog').dialog('open');
            });
            $("#btn_save").click(function () {
                var result = $('#sortable').sortable('toArray', {
                    connected: true,
                    attribute: 'id'
                });
                var test = "";
                var flowid = $("#hd_flowid").val();
                if (flowid == "tkd") {
                    if (result.length != 2) {
                        alert("提款单审批流程：1.设专业化公司财务;2.结算中心会计");
                        return;
                    }
                }
                for (var i = 0; i < result.length; i++) {
                    var typecode = $("#" + result[i] + " table tr td")[1].innerHTML;//审批类型
                    var checkcode = $("#" + result[i] + " table tr td")[3].innerHTML;//审批人
                    var cje = $("#" + result[i] + " table tr td")[5].innerHTML;//生效金额
                    var cdate = $("#" + result[i] + " table tr td")[9].innerHTML;// 生效日期
                    var maxje = $("#" + result[i] + " table tr td")[7].innerHTML;//限额
                    var checktype = $("#" + result[i] + " table tr td")[11].innerHTML;//审批类型（单签/会签）
                    var isKmZhuguan = $("#" + result[i] + " table tr td")[13].innerHTML;//是否科目主管
                    var fylx = $("#" + result[i] + " table tr td")[15].innerHTML;//费用类型
                    var bz = '<%=Request["billdept"]%>';// $("#" + result[i] + " table tr td")[13].innerHTML;               
                    test += typecode + "," + checkcode + "," + isKmZhuguan + "," + cje + "," + checktype + "," + cdate + "," + maxje + "," + bz + "," + fylx + ",|";

                }
                //document.write("总长度：" + result.length+"</br>");

                //document.write("生效金额：" + cje + "</br>");
                //document.write("费用类型：" + fylx + "</br>");
                //document.write("生效日期：" + cdate + "</br>");
                //document.write("限额：" + maxje + "</br>");
                //document.write("是否科目主管：" + isKmZhuguan + "</br>");
                //document.write("审批类型：" + checktype + "</br>");
                //document.write("  test:"+test);
                //return;
                WorkFlowService.UpdateWorkFlow(test.substring(0, test.length - 1), flowid, bz, UpdateSuccess, UpdateError);

            });
            //复制部门
            $("#btn").click(function () {
                var deptcode = '<%=Request["billdept"] %>';
                var djlx = '<%=Request["flowid"] %>';

                if (djlx.length == 0) {
                    alert("请选择流程类型。");
                    return;
                }
                if (deptcode.length == 0) {
                    alert("请选择部门。");
                    return;
                }

                window.showModalDialog('dept_spl_list.aspx?deptcode=' + deptcode + '&djlx=' + djlx, 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:350px;status:no;scroll:no');
                window.location.href = window.location.href;
            });


            $("#btn_delete").click(function () {
                var lid = $("input[type=checkbox]:checked").parent().attr("id");
                $("#sortable li").remove("#" + lid);
            });
            $("#DropDownList1").change(function () {
                var typecode = $(this).val();
                $("#txt_spr").val("");
                if (typecode == "01") {
                    $("#txt_spr").removeClass("hiddenbill");
                    $("#btn_muti").removeClass("hiddenbill");
                    //$("#drp_type").addClass("hiddenbill");
                    //$("#drp_type option").eq(0).attr('selected', 'true');
                }
                else {
                    $("#txt_spr").addClass("hiddenbill");
                    $("#btn_muti").addClass("hiddenbill");
                    //$("#drp_type").removeClass("hiddenbill");
                }
            });
            //$("#btn_choose").click(function () {
            //    //selectry();
            //    $('#selectuser').dialog('open');
            //});
            //多选 单击多选按钮 弹出多选操作框
            $("#btn_muti").click(function () {
                $("#txt_spr").val();
                $("#dx_reset").click();
                $('#duoxuan').dialog('open');
            });


            //多选 选择完一个人员后 单击添加按钮  dx_yxz 
            $("#dx_add").click(function () {
                var val = $("#dx_name").val();
                if (val.indexOf("[") == -1 || val.indexOf("]") == -1) {
                    alert("请以 编号[姓名]的格式正确填写审批人。");
                    $("#dx_name").val('');
                    $("#dx_name").focus();
                    return false;
                }
                var name = val.substring(val.indexOf("[") + 1, val.indexOf("]"));
                //显示出来
                var yxzname = $("#dx_yxz").text();
                if (yxzname.length == 0) {
                    yxzname = name;
                } else {
                    yxzname += '，' + name;
                }
                $("#dx_yxz").text(yxzname);
                ////放到隐藏域里
                $("#dx_hdyxz").val($("#dx_hdyxz").val() + val + ":");
                ////清空本文本框
                $("#dx_name").val('');
            });
            //多选 重选
            $("#dx_reset").click(function () {
                $("#dx_hdyxz").val('');
                $("#dx_yxz").text('');
            });

        });

        function UpdateSuccess(result, context, methodName) {

            if (result > 0) {
                alert("保存成功!");
            }
            else {
                if (result == -2) {
                    alert("该单据类型有老的审批流程中没有处理的单据，请处理完毕后再设置新的审批流程。");
                } else {
                    alert("保存失败!");

                }
            }
        }
        function UpdateError(error, context, methodName) {
            alert(error);
        }

        function ChangeSuccess(result, context, methodName) {
            $("#txt_spr").val("");
            if (result != "") {
                var json = $.parseJSON(result);
                for (var i = 0; i < json.length; i++) {
                    $("#DropDownList2").append("<option value='" + json[i].ccode + "'>" + json[i].cname + "</option>");
                }
            }
        }

        function ChangeError(error, context, methodName) {
            $("#DropDownList2").empty();
            alert("AJAX失败!");
        }
        function lxTypeChange() {

            if ($("#drp_fylx").val() == "0") {
                $("#TextBox2").val("");
                $("#TextBox2").attr("readonly", 'true');
                $("#txtmaxmoney").val("");
                $("#txtmaxmoney").attr("readonly", 'true');
            }
            else {
                $("#txtmaxmoney").removeAttr("readonly");
                $("#TextBox2").removeAttr("readonly");
            }

        }
        function selectry() {
            var str = window.showModalDialog('../select/SelectMoreUserFrame.aspx', 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:750px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                if ($("#txt_spr").val() != "") {
                    $("#txt_spr").val($("#txt_spr").val() + ":" + str);

                } else {
                    $("#txt_spr").val($("#txt_spr").val() + str);

                }
            }
        }
        function inputnum(value) {
            if (!isNaN(value)) {
                var num = parseFloat(value);
                //如果不是数字 直接返回原值
                if (isNaN(num)) {
                    return value;
                }
                return num.toFixed(4);
            } else {
                return value;
            }
        }
    </script>



</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server">
            <Services>
                <asp:ServiceReference Path="~/webBill/MyWorkFlow/WorkFlowService.asmx" />
            </Services>
        </asp:ScriptManager>
        <div id="dialog" title="编 辑">
            <table class="myTable">
                <tr>
                    <td class="style1">审批类型:
                    </td>
                    <td>
                        <asp:DropDownList ID="DropDownList1" runat="server">
                        </asp:DropDownList>
                    </td>
                    <td class="style1">审批人:
                    </td>
                    <td>
                        <input type="text" id="txt_spr" />
                        <input type="button" id="btn_muti" value="多选" />
                        <%--<input type="button" id="btn_choose" value="选择" class="hiddenbill" />--%>
                    </td>
                </tr>
                <tr>
                    <td class="tableBg">审批类型:
                    </td>
                    <td>
                        <select id="drp_type">
                            <option value="1">单签</option>
                            <option value="2">会签</option>

                        </select>
                    </td>
                    <td class="tableBg">必须为科目主管:
                    </td>
                    <td>
                        <select id="drp_isKeMuZhuGuan">
                            <option value="0">否</option>
                            <option value="1">是</option>
                        </select>
                    </td>

                </tr>
                <tr>
                    <td class="tableBg">费用类型：
                    </td>
                    <td>
                        <asp:DropDownList ID="drp_fylx" runat="server" onchange="lxTypeChange()">
                            <asp:ListItem Value="0" Selected="True">不设置</asp:ListItem>
                            <asp:ListItem Value="1">总金额</asp:ListItem>
                        </asp:DropDownList>
                    </td>
                     <td class="tableBg">生效金额(万元):
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
                    </td>
                  
                </tr>
                <tr>
                     <td class="tableBg">限额(万元)</td>
                    <td>
                        <asp:TextBox ID="txtmaxmoney" runat="server"></asp:TextBox></td>

                    <td class="tableBg">生效日期:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox3" runat="server"></asp:TextBox>
                    </td>

                    <%--   <td class="tableBg">备注:
                    </td>
                    <td>
                        <asp:TextBox ID="TextBox4" runat="server"></asp:TextBox>
                    </td>--%>
                </tr>

            </table>
        </div>
        <div style="margin-top: 20px">
            <asp:Label ID="lb_msg" runat="server" ForeColor="Red" Text="你当前选择的单据：未选择。"></asp:Label>
            <table>
                <tr>

                    <td>
                        <input type="button" class="ui-widget-content ui-corner-all" id="btn_insert" value="新 增" />
                    </td>
                    <td>
                        <input type="button" class="ui-widget-content ui-corner-all" id="btn_delete" value="删 除" />
                    </td>
                    <td>
                        <input id="btn_save" type="button" value="保 存" class="ui-widget-content ui-corner-all" />
                    </td>
                    <td>
                        <input type="button" id="btn" value="复制其他部门" class="ui-widget-content ui-corner-all" />
                    </td>
                </tr>
            </table>
        </div>
        <div>
            <ul id="sortable" runat="server">
            </ul>
        </div>
        <div>
            <asp:HiddenField ID="hd_flowid" runat="server" />
            <asp:HiddenField ID="hd_spr" runat="server" />
            <br />
        </div>
        <%-- <div id="selectuser" title="选择人员..." >
            <iframe src='../select/SelectMoreUserFrame.aspx' frameborder='0' width="98%" height="99%" style='margin-left: 0px;'></iframe>
        </div>--%>
        <div id="duoxuan" title="多选">
            <label style="color: red">【操作提示】：在审批人员框中输入姓名或编号，系统根据提示自动检索，在检索出的提示信息中单击选择要找的人员，然后单击添加按钮，如果有多个人重复操作。选择完毕后，单击确定按钮即可。</label><br />
            <input type="hidden" id="dx_hdyxz" />
            已选择：<label id="dx_yxz"></label><br />
            审批人员：<input type="text" id="dx_name" />&nbsp;&nbsp;&nbsp;&nbsp;<input type="button" value="添加" id="dx_add" class="baseButton" />
            <input type="button" value="重选" id="dx_reset" class="baseButton" />

        </div>
    </form>
    <script language="javascript">

        //选择人员
        $("#txt_spr").autocomplete({
            source: availableTags,
            select: function (event, ui) {
                var rybh = ui.item.value;
            }
        });

        //多选  选择人员
        $("#dx_name").autocomplete({
            source: availableTags,
            select: function (event, ui) {
                var rybh = ui.item.value;
            }
        });


    </script>
</body>
</html>
