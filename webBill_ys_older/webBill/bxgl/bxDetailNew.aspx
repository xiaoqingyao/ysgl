<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bxDetailNew.aspx.cs" Inherits="webBill_bxgl_bxDetailNew" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>费用报销单</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8">
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
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

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>

    <script language="javascript" type="Text/javascript">
        $(function() {
            htjeChange();
            $("#fykm").accordion({
                header: "h3",
                autoHeight: true,
                navigation: true,
                fillSpace: true
            });

            var isgk = $("#rb_ok").attr("checked");
            if (!isgk) {
                $("#txt_gk").addClass("basehidden");
            }
            $("#tb_fysq tr").click(function() {
                $("#tb_fysq tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
            $("#btnFyspCx").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行");
                    return;
                }
                $(".highlight").remove();
            });

            $("#btn_test").click(function() {

                //检测必填项
                var zy = $("#txtBxzy").val();


                if (zy == undefined || zy == '') {
                    alert("请填写报销摘要");
                    return;
                }
                var json = jsonMaker();
                if (json == "") {
                    alert("部门分配金额与科目金额不相等!");
                    return;
                }
                $.post("../MyAjax/YbbxBillSave.ashx", json, function(data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            alert("保存成功");
                            window.returnValue = "1";
                            window.close();
                        } else if (data == "-2") {
                            alert("有预算超支了,不能保存!");
                        } else if (data == "-3") {
                            alert("该报销类型需要附单据,请添加单据!");
                        }
                        else if (data == "-4") {
                            alert("修改单据日期不能跨月!");
                        }
                        else if (data == "-5") {
                            alert("该费用科目有部门核算，请选择使用部门!");
                        }
                        else if (data == "-6") {
                            alert("该费用科目有项目核算，请选择使用项目!");
                        }
                        else if (data == "-7") {
                            alert("该月份单据已做月结，不能再做本月份的单据!");
                        }
                        else {
                            alert("保存失败2");
                        }
                    }
                    else {
                        alert("保存失败");
                    }
                });
            });
            function jsonMaker() {
                //构造提交服务器的字符串
                var djtype = $("#hd_djtype").val();
                var pd_test = true;
                var billcode = $("#HiddenField1").val();
                var usercode = $("#txtBxr").val();
                usercode = usercode.split(']')[0];
                usercode = usercode.substring(1, usercode.length);
                var bxr = $("#txtJbr").val();
                bxr = bxr.split(']')[0];
                bxr = bxr.substring(1, bxr.length);
                var varzh = $("#txtbxzh").val(); //报销人账号
                var varkhh = $("#txt_khh").val(); //开户行
                varzh = varkhh + "|&|" + varzh;
                var bxzy = $("#txtBxzy").val();
                var bxsm = $("#txtBxsm").val();
                var bxlx = $("#drpBxmxlx").val();
                var isgk = $("#rb_ok").attr("checked");
                var bxDate = $("#txtSqrq").val();
                if (isgk) {
                    gkbmbh = $("#txt_gk").val();
                } else {
                    gkbmbh = $("#txtDept").val();
                }
                gkbmbh = gkbmbh.split(']')[0];
                gkbmbh = gkbmbh.substring(1, gkbmbh.length);
                var fysqCode = "";
                $("#cgdj tbody tr td:first-child").each(function() {
                    fysqCode += $(this).html() + "|";
                });
                if (fysqCode.length > 0) {
                    fysqCode = fysqCode.substring(0, fysqCode.length - 1);
                }
                var djs = $("#txt_djs").val();
                var ret = '{"djlx":"' + djtype + '","billuser":"' + usercode + '","bxr":"' + bxr + '","bxrzh":"' + varzh + '","zy":"' + bxzy + '","sm":"' + bxsm + '","bxlx":"' + bxlx + '","gkbmbh":"' + gkbmbh + '","billcode":"' + billcode + '","fysq":"' + fysqCode + '","bxDate":"' + bxDate + '","sfgf":"0","djs":"' + djs + '","list":[';
                $("#fykm h3 a").each(function(i) {
                    var tempkm = $(this).html();
                    tempkm = tempkm.split(']')[0];
                    tempkm = tempkm.substring(1, tempkm.length);
                    var tempje = $(this).parent().next().find(".ysje").val();
                    var tempse = $(this).parent().next().find(".ysse").val();
                    ret += '{"km":"' + tempkm + '","je":"' + tempje + '","se":"' + tempse + '","bm":[';
                    var bmret = "";
                    var test_je = tempje * 100;
                    $(this).parent().next().find("#dv_bm" + i + " ul li").each(function() {
                        var tempbmbh = $(this).find("span").html();
                        tempbmbh = tempbmbh.split(']')[0];
                        tempbmbh = tempbmbh.substring(1, tempbmbh.length);
                        var tempbmje = $(this).find("input").val();
                        bmret += '{"bmbh":"' + tempbmbh + '","bmje":"' + tempbmje + '"},';
                        test_je = test_je - tempbmje * 100;
                    });
                    if (test_je != 0 && test_je != tempje * 100) {
                        //alert(test_je);
                        //pd_test = false;
                    }
                    bmret = bmret.substring(0, bmret.length - 1);
                    ret += bmret + '] ,"xm":[';
                    var xmret = "";
                    $(this).parent().next().find("#dv_xm" + i + " ul li").each(function() {
                        var tempxmbh = $(this).find("span").html();
                        tempxmbh = tempxmbh.split(']')[0];
                        tempxmbh = tempxmbh.substring(1, tempxmbh.length);
                        var tempxmje = $(this).find("input").val();
                        xmret += '{"xmbh":"' + tempxmbh + '","xmje":"' + tempxmje + '"},';
                    });
                    xmret = xmret.substring(0, xmret.length - 1);
                    ret += xmret + ']},';
                });
                ret = ret.substring(0, ret.length - 1);
                ret += "]}";
                if (pd_test) {
                    return ret;
                }
                else {
                    return "";
                }
            }
            $("#btnAddFysq").click(function() {
                openCgsp();
            });

            $("#btnAddFysq2").click(function() {
                openCgsp2();
            });

            $("#txtBxr").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function(data, status) {
                        if (status == "success") {
                            $("#txtDept").val(data);
                        }
                        else {
                            alert("获取部门失败");
                        }
                    });
                }
            });
            $("#txtSqrq").datepicker();
            /*
            $("#txtBxr").blur(function() {
            var nowSelect = $(this).data("autocomplete").menu.element.find("li:first")[0];
            if (nowSelect != undefined && nowSelect != "") {
            $(this).val(nowSelect.innerText);
            $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": $(this).val() }, function(data, status) {
            if (status == "success") {
            $("#txtDept").val(data);
            }
            else {
            alert("获取部门失败");
            }
            });
            }
            });
            */
            $("#rb_can").click(function() {
                $("#fykm").accordion('destroy');
                $("#fykm").html("");
                $("#txt_gk").addClass("basehidden");
            });
            var gkbm_cache = {};
            $("#btnAddFykm").click(function() {
                var gkbmbh = "";
                var isgk = $("#rb_ok").attr("checked");
                if (isgk) {
                    gkbmbh = $("#txt_gk").val();
                } else {
                    gkbmbh = $("#txtDept").val();
                }
                if (gkbmbh == "") {
                    alert("请先选择部门");
                    return;
                }
                openKm(gkbmbh, isgk);
            });
            $("#rb_ok").click(function() {
                $("#txt_gk").removeClass();
                $("#fykm").accordion('destroy');
                $("#fykm").html("");
                $.post("../MyAjax/GetDept.ashx", { "action": "gk", "code": "" }, function(data, status) {
                    if (status == "success") {
                        gkbm_cache = $.parseJSON(data);
                        $("#txt_gk").autocomplete({
                            source: gkbm_cache
                        });
                    }
                    else {
                        alert("获取部门失败");
                    }
                });
            });
        });

        function htjeChange() {
            var je = 0;
            $(".ysje").each(function() {
                je += Number($(this).val()) * 100;
            });
            $(".ysse").each(function() {
                je += Number($(this).val()) * 100;
            });
            $("#txtHjjeXx").val(je / 100);
            $("#txtHjjeDx").val(cmycurd($("#txtHjjeXx").val()));
        }

        function bmChoose(obj) {
            var tempid = $(obj).attr("id");
            tempid = tempid.substring(tempid.length, 4);
            openBm(tempid);
        }

        function xmChoose(obj) {
            var tempid = $(obj).attr("id");
            tempid = tempid.substring(tempid.length, 4);
            var gkbmbh = "";
            var isgk = $("#rb_ok").attr("checked");
            if (isgk) {
                gkbmbh = $("#txt_gk").val();
            } else {
                gkbmbh = $("#txtDept").val();
            }
            if (gkbmbh == "") {
                alert("请先选择部门");
                return;
            }
            openXm(gkbmbh, tempid);
        }

        function openBm(dvid) {
            var deptCode = "";
            $("#dv_bm" + dvid + " ul span").each(function() { deptCode += $(this).html(); });
            var str = window.showModalDialog("YDeptSelectNew.aspx?deptCode=" + deptCode, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var depts = str.split('|');
                var innerval = "<ul>";
                for (var i = 0; i < depts.length; i++) {
                    innerval += "<li style='height:36px'><span>" + depts[i] + ":</span><input type='text' value='0.00' /></li>";
                }
                innerval += "</ul>";
                $("#dv_bm" + dvid).html("[使用部门]");
                $("#dv_bm" + dvid).append(innerval);
                $("#fykm").accordion('destroy');
                $("#fykm").accordion({
                    header: "h3",
                    autoHeight: true,
                    navigation: true,
                    fillSpace: true
                });
            }
        }

        function openXm(deptCode, dvid) {
            var xmCode = "";
            $("#dv_xm" + dvid + " ul span").each(function() { xmCode += $(this).html(); });
            $("#dv_xm" + dvid + " ul span").each(function() { xmCode += $(this).html(); });
            var tempurl = "YXmSelectNew.aspx?deptCode=" + deptCode + "&xmCode=" + xmCode;
            var str = window.showModalDialog("YXmSelectNew.aspx?deptCode=" + deptCode + "&xmCode=" + xmCode, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var xms = str.split('|');
                var innerval = "<ul>";
                for (var i = 0; i < xms.length; i++) {
                    innerval += "<li style='height:36px'><span>" + xms[i] + ":</span><input type='text' value='0.00' /></li>"
                }
                innerval += "</ul>";
                $("#dv_xm" + dvid).html("[科目项目]");
                $("#dv_xm" + dvid).append(innerval);
                $("#fykm").accordion('destroy');
                $("#fykm").accordion({
                    header: "h3",
                    autoHeight: true,
                    navigation: true,
                    fillSpace: true
                });
            }
        }
        function openCgsp() {//选择附加的单据，打开单据选择
            var tempInner = window.showModalDialog('selectCgsp.aspx', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');
            if (tempInner == undefined || tempInner == "")
            { }
            else {
                $("#cgdj tbody").append(tempInner);
            }
        }

        //mxl 2012.04.09 添加报告单
        function openCgsp2() {//选择附加的单据，打开单据选择
            var tempInner = window.showModalDialog('selectlscg.aspx', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');
            if (tempInner == undefined || tempInner == "")
            { }
            else {
                $("#cgdj tbody").append(tempInner);
            }
        }
        //


        function openKm(deptCode, isGk) {
            var kmcode = "";
            $("#fykm h3 a").each(function() {
                kmcode += $(this).html() + ",";
            });
            if (kmcode != "") {
                kmcode = kmcode.substring(0, kmcode.length - 1);
            }
            var billDate = $("#txtSqrq").val();
            var str = window.showModalDialog("YskmSelectNew.aspx?deptCode=" + deptCode + "&kmcode=" + kmcode + "&isgk=" + isGk + "&billDate=" + billDate, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                /*
                $("#fykm").accordion('destroy'); 
                $("#fykm").html("");
                var yskm = str.split('|');
                for (var i = 0; i < yskm.length; i++) {
                var innerval = "<h3><a href='#'>" + yskm[i] + "</a></h3><div><ul><li style='height:36px'>[金额] <input type='text' value='0.00' class='ysje' onblur='htjeChange()' id='je" + i + "' /><input type='button' value='部门选择' id='bmch" + i + "' onclick='bmChoose(this)' /><input type='button' value='项目选择' id='xmch" + i + "' onclick='xmChoose(this)' /> </li><li style='height:36px'>[税额] <input type='text' onblur='htjeChange()' class='ysse' value='0.00' id='se" + i + "' /></li><li><div id='dv_bm" + i + "'>[使用部门]</div></li><li><div id='dv_xm" + i + "'>[科目项目]</div></li></ul></div>";
                $("#fykm").append(innerval);
                }
                $("#fykm").accordion({
                header: "h3",
                autoHeight: true,
                navigation: true,
                fillSpace: true
                });
                */
                $("#hf_kmselect").val(str);
                $("#btn_kmselect").click();
            }
        }


    </script>

    <style type="text/css">
        .style1
        {
            background-color: #EDEDED;
            text-align: center;
        }
    </style>
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
        <tr>
            <td style="text-align: center; height: 36px;">
                <strong>
                    <asp:Label ID="lbdjmc" runat="server" Text="Label"></asp:Label>
                    <asp:Label ID="Label1" runat="server" Text="Label"></asp:Label>
                </strong>
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="900">
                    <tr>
                        <td colspan="2" class="tableBg2">
                            经办人
                        </td>
                        <td colspan="2" style="width: 200px">
                            <asp:TextBox ID="txtJbr" runat="server" ReadOnly="True"></asp:TextBox>
                        </td>
                        <td class="tableBg2">
                            报销人
                        </td>
                        <td colspan="4">
                            <input id="txtBxr" style="width: 148px" type="text" runat="server" />
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tableBg2">
                            所在部门
                        </td>
                        <td colspan="2">
                            <input id="txtDept" readonly="readonly" style="width: 95%" type="text" runat="server" /><input
                                id="txtbxdept" readonly="readonly" style="display: none; width: 1px" type="text"
                                runat="server" />
                        </td>
                        <td class="tableBg2">
                            申请日期
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtSqrq" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tableBg2">
                            报销明细类型
                        </td>
                        <td colspan="2" style="width: 200px">
                            <asp:DropDownList ID="drpBxmxlx" runat="server">
                            </asp:DropDownList>
                        </td>
                        <td class="tableBg2">
                            报销摘要
                        </td>
                        <td colspan="4">
                            <asp:TextBox ID="txtBxzy" runat="server" Width="97%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg2" colspan="2">
                            开户银行
                        </td>
                        <td colspan="2">
                            <input type="text" runat="server" id="txt_khh" />
                        </td>
                        <td class="tableBg2" colspan="2">
                            银行账号
                        </td>
                        <td colspan="3">
                            <asp:TextBox ID="txtbxzh" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tableBg2">
                            报销说明
                        </td>
                        <td colspan="10">
                            <asp:TextBox ID="txtBxsm" runat="server" TextMode="MultiLine" Width="98%"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tableBg2">
                            报销单据
                        </td>
                        <td colspan="5">
                            <input type="File" runat="server" id="upLoadFiles" style="width: 401px" class="baseButton" />
                            <asp:Button ID="btnScdj" runat="server" CausesValidation="False" Text="上传单据" CssClass="baseButton"
                                OnClick="btnScdj_Click" /><br />
                            <div id="divBxdj" runat="server">
                            </div>
                        </td>
                        <td>
                            单据数:
                        </td>
                        <td>
                            <asp:TextBox ID="txt_djs" runat="server"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: red; height: 4px" colspan="9">
                        </td>
                    </tr>
                    <tr>
                        <td class="tableBg2" colspan="2">
                            归口预算
                        </td>
                        <td colspan="2">
                            <asp:RadioButton ID="rb_ok" runat="server" Text="是" GroupName="is_gk" />
                            <asp:RadioButton ID="rb_can" runat="server" Text="否" GroupName="is_gk" Checked="true" />
                        </td>
                        <td colspan="5" id="gkbm">
                            <div id='dv_gk'>
                                <span>归口部门:
                                    <input type='text' id='txt_gk' runat="server" /></span></div>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="tableBg2">
                            费用科目
                        </td>
                        <td colspan="6">
                            <div id="fykm" style="width: 500px; text-align: left;" runat="server">
                            </div>
                        </td>
                        <td colspan="1" style="text-align: center">
                            <input type="button" value="选择费用" id="btnAddFykm" runat="server" class="baseButton" />
                        </td>
                    </tr>
                    <span id="divFykm" runat="server"></span>
                    <tr class="fykmRow">
                        <td colspan="2" class="tableBg2">
                            合计金额小写
                        </td>
                        <td colspan="2" style="width: 200px">
                            <input type="text" id="txtHjjeXx" runat="server" readonly="readonly" style="width: 226px;
                                text-align: right; background-color: #cccccc;" />
                        </td>
                        <td colspan="2" class="tableBg2" style="width: 200px">
                            合计金额大写
                        </td>
                        <td colspan="3">
                            <input type="text" id="txtHjjeDx" runat="server" readonly="readonly" style="background-color: #cccccc" />
                        </td>
                    </tr>
                    <tr>
                        <td style="background-color: red; height: 4px" colspan="9">
                        </td>
                    </tr>
                    <tr>
                        <td colspan="2" class="style1">
                            费用申请
                        </td>
                        <td colspan="7" style="text-align: right">
                            <input id="btnAddFysq2" runat="server" type="button" value="附加报告单" class="baseButton" />&nbsp;
                            <input id="btnAddFysq" runat="server" type="button" value="附加采购单" class="baseButton" />&nbsp;
                            <input id="btnFyspCx" value="附件撤销" type="button" class="baseButton" />
                            <input id="btnFysqXX" value="单据信息" type="button" class="baseButton" />
                        </td>
                    </tr>
                    <tr class="cgspInfo" runat="server" id="cgspInfo">
                        <td colspan="9">
                            <table id="cgdj" width="100%">
                                <thead class="myGridHeader">
                                    <tr>
                                        <td>
                                            单据编号
                                        </td>
                                        <td>
                                            采购单位
                                        </td>
                                        <td>
                                            承办人
                                        </td>
                                        <td>
                                            申请日期
                                        </td>
                                        <td>
                                            申请类别
                                        </td>
                                        <td>
                                            采购总额
                                        </td>
                                        <td>
                                            原因说明
                                        </td>
                                        <td>
                                            单据状态
                                        </td>
                                    </tr>
                                </thead>
                                <tbody runat="server" id="tb_fysq">
                                </tbody>
                            </table>
                            <asp:HiddenField ID="HiddenField1" runat="server" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td style="text-align: center; height: 30px;">
                <asp:HiddenField ID="hf_kmselect" runat="server" />
                <asp:HiddenField ID="hd_djtype" runat="server" />
                <asp:Button ID="btn_kmselect" runat="server" Text="Button" OnClick="btn_kmselect_Click"
                    CssClass="hiddenbill" />
                <input type="button" value="保 存" id="btn_test" class="baseButton" runat="server" />
                &nbsp; &nbsp; &nbsp; &nbsp;
                <input type="button" onclick="javascript:window.close();" value="关 闭" class="baseButton" />
            </td>
        </tr>
        <tr>
            <td style="text-align: center">
                <asp:ValidationSummary ID="ValidationSummary1" runat="server" ShowMessageBox="True"
                    ShowSummary="False" />
            </td>
        </tr>
        <tr style="display: none;">
            <td style="text-align: center; height: 24px;">
                <asp:Label ID="lblBillCode" runat="server"></asp:Label><asp:Label ID="lblType" runat="server"></asp:Label>
                <asp:Button ID="btnRefresh" runat="server" Text="刷新脚本数据" />
                <input type="button" id="btn_Print" runat="server" value="打印预览" cssclass="baseButton"
                    onclick="prn1_preview()" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
