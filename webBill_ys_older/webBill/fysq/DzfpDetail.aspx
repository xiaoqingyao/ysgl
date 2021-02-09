<%@ Page Language="C#" AutoEventWireup="true" CodeFile="DzfpDetail.aspx.cs" Inherits="webBill_fysq_DzfpDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>电子发票附加单</title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <style type="text/css">
        .highlight {
            background: #EBF2F5;
        }

        .hiddenbill {
            display: none;
        }

        .item-help {
            border: 2px solid blue;
            height: auto;
            width: 300px;
            position: absolute;
            overflow-y: auto;
            font-size: 14px;
            padding: 15px;
            background-color: White;
        }
    </style>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript"
        charset="UTF-8"></script>


    <script language="javascript" type="text/javascript">
        $(function () {
            var Ctrl = '<%=Request["Ctrl"]%>';
            if (Ctrl == "View") {
                $("#btn_test").hide();
            }
            //$("#lbesendDept").autocomplete({
            //    source: availableTagsdept,
            //});
            $("#txtSqrq").datepicker();

            //申请人输入时
            $("#lbeAppPersion").autocomplete({
                source: availableTags,
                select: function (event, ui) {
                    var rybh = ui.item.value;
                    $.post("../MyAjax/GetDept.ashx", { "action": "user", "code": rybh }, function (data, status) {
                        if (status == "success") {
                            $("#lbeDept").text(data);
                        }
                        else {
                            alert("获取部门失败");
                        }
                    });
                }
            });

            getmx();


            $("#btn_test").click(function () {

                var json = getfpmx();
                $.post("../MyAjax/DZfpSave.ashx", json, function (data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            alert("保存成功");
                            window.returnValue = "1";
                            window.close();
                        }
                        else {
                            alert(data);
                            alert("保存失败2");
                        }
                    }
                    else {
                        alert("保存失败");
                    }
                });
            });
        });



        //获取发票明细
        function getfpmx() {
            var billcode = $("#txtbillcode").val();

            var fprq = $("#txtSqrq").val();
            var deptCode = $("#lbeDept").text();
            deptCode = deptCode.split(']')[0];
            deptCode = deptCode.substring(1, deptCode.length);

            var deptName = $("#lbeDept").text();
            deptName = deptName.substring(0, deptName.length);

            var fpusercode = $("#lbeAppPersion").val();
            fpusercode = fpusercode.split(']')[0];
            fpusercode = fpusercode.substring(1, fpusercode.length);

            var fpusername = $("#lbeAppPersion").val();
            fpusername = fpusername.substring(0, fpusername.length);

            var bz = $("#txtbz").text();


            // var ret = '{"list":[';
            var ret = '{"billcode":"' + billcode + '","fprq":"' + fprq + '","deptCode":"' + deptCode + '","deptName":"' + deptName + '","fpusercode":"' + fpusercode + '","fpusername":"' + fpusername + '","bz":"' + bz + '","list":[';
            //预算科目明细
            $("#tab_fpmx tbody tr").each(function (i) {

                var fph = $(this).find("input:eq(0)").val(); //发票号
                if (fph != "" && fph != undefined) {
                    var fpdw = $(this).find("input:eq(1)").val(); //发票单位
                    var fpje = $(this).find("input:eq(2)").val(); //发票金额
                    var bz = $(this).find("input:eq(3)").val(); //备注
                    ret += '{"fpdh":"' + fph + '","fpdw":"' + fpdw + '","fpje":"' + fpje + '","fpbz":"' + bz + '"},';

                }


            });
            ret = ret.substring(0, ret.length - 1);
            ret += "]}";

            return ret;
        }
        function htjeChange() { }

        function getmx() {

            var inner = "";

            for (var i = 0; i < 5; i++) {

                inner += '<tr id="tr_' + i + '">';
                inner += '<td><input type="text" class="baseText " value="" /></td>';
                inner += '<td><input type="text" class="baseText "  value="" /></td>';
                inner += '<td><input type="text" class="baseText ysje"" value="0.00" /></td>';
                inner += '<td><input type="text" class="baseText "  value="" /></td>';

                inner += "</tr>";
                //$("#td_dept").append('<ul id="bm_' + kmIndex.toString() + '"> onblur="htjeChange();</ul>');
                //$("#td_xm").append('<ul id="xm_' + kmIndex.toString() + '"></ul>');

                $("#tab_fpmx tbody").append(inner);
            }
        }
        function OnApproveSuccess(data, status) {
            if (data > 0 && status == "success") {
                alert("操作成功！");
                self.close();
            } else {
                alert("审批失败！");
            }
        }
        function opAddDetail() {
            var returnValue = window.showModalDialog('../select/userFrame.aspx?Flg=All', 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:900px;status:no;scroll:yes');
            if (returnValue == undefined || returnValue == "") { } else {
                $("#hdUerCode").val('');
                $("#hdUerCode").val(returnValue);
                document.getElementById("btnAdd_Server").click();
            }
        }
        //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }
        function checkEdit() {
            return true;
        }

    </script>

</head>
<body style="background-color: #EBF2F5;">
    <center>
        <form id="form1" runat="server">
            <table cellpadding="0" cellspacing="0" class="style1" width="90%" border="0">
                <tr>
                    <td style="text-align: center; height: 26px;" colspan="6"></td>
                </tr>
                <tr>
                    <td style="text-align: center; height: 26px;" colspan="6">
                        <strong><span style="font-size: 15pt">电子发票附加单</span></strong>
                    </td>
                </tr>
              <%--  <tr>



                    <td style="text-align: right; width: 10px; display: none"></td>
                    <td style="width: 200px; display: none">部门：<asp:TextBox ID="lbesendDept" runat="server"></asp:TextBox>
                    </td>

                </tr>--%>

                <tr>
                    <td colspan="6" style="height: 3px;"></td>
                </tr>
                <tr>

                    <td style="text-align: right; width: 10px;"></td>
                    <td style="height: 26px; text-align: center" colspan="6">
                        <table border="0" cellpadding="0" cellspacing="0" width="100%" class="myTable">
                            <tr>

                                <td style="width: 200px">编号：<asp:TextBox ID="txtbillcode" runat="server"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right">申请人：
                                </td>
                                <td style="text-align: center">
                                    <asp:TextBox ID="lbeAppPersion" runat="server" Width="90%"></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right">所属部门：
                                </td>
                                <td>
                                    <asp:Label ID="lbeDept" runat="server"></asp:Label>
                                </td>
                            </tr>
                            <tr>

                                <td style="width: 200px">日期： 
                                    <asp:TextBox ID="txtSqrq" runat="server" ></asp:TextBox>
                                </td>
                                <td class="tableBg2" style="text-align: right">备注：
                                </td>
                                <td colspan="4" style="text-align: center">
                                    <asp:TextBox ID="txtbz" runat="server" TextMode="MultiLine" Height="20px"
                                        Width="98%"></asp:TextBox>
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6" style="text-align: center; font-size: 13pt;">发票明细
                                </td>
                            </tr>
                            <tr>
                                <td colspan="6">
                                    <div style="margin-left: 2px">
                                        <table id="tab_fpmx" class="myTable" style="width: 95%">
                                            <thead class="myGridHeader">
                                                <tr>
                                                    <th class="tableBg2">发票号
                                                    </th>
                                                    <th class="tableBg2">发票单位
                                                    </th>
                                                    <th class="tableBg2">发票金额
                                                    </th>
                                                    <th class="tableBg2">备注
                                                    </th>

                                                </tr>
                                            </thead>
                                            <tbody id="body_fpmx" runat="server">
                                            </tbody>
                                        </table>
                                    </div>
                                </td>
                            </tr>
                        </table>
                    </td>
                </tr>
                <tr>
                    <td colspan="6" style="text-align: center; height: 37px;">
                        <input type="button" value="保 存" id="btn_test" class="baseButton" runat="server" />

                        <%--<asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click"
                            OnClientClick="return checkEdit();" />--%>&nbsp;
                  
                    <asp:Button ID="btn_fh" runat="server" Text="关 闭" CssClass="baseButton" OnClick="btn_fh_Click" />
                        <asp:HiddenField ID="hdUerCode" runat="server" />
                    </td>
                </tr>
            </table>
        </form>
    </center>
</body>
</html>
