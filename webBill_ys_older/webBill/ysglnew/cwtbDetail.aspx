<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cwtbDetail.aspx.cs" Inherits="webBill_ysglnew_cwtbDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <meta http-equiv="X-UA-Compatible" content="IE=8" />
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <style type="text/css">
        .unEdit {
            background-color: #dedede;
        }
    </style>
    <script language="javascript" type="text/javascript">


        $(function () {
            gudingbiaotounew($("#GridView1"), $(window).height() - 200);
            $("#btn_cxysgc").click(function () {
                var deptcode = '<%=Request["deptCode"]%>';
                var Rvnum = openDetail("cwtbSelectYsgc.aspx?deptCode=" + deptcode);
                if (Rvnum != undefined && Rvnum != "") {
                    $("#txtysgc").val(Rvnum);
                }
            });


            $("#btn_hz").click(function () {
                var deptcode = '<%=Request["deptCode"]%>';

                var nd = '<%=Request["nd"]%>';
                var xmcode = '<%=Request["xmcode"]%>';
                if (xmcode != undefined && xmcode.length > 0) {//项目预算
                    window.location.href = "Cwtbhz_List.aspx?deptcode=" + deptcode + "&nian=" + nd + "&xmcode=" + xmcode;

                } else {
                    window.location.href = "Cwtbhz_List.aspx?deptcode=" + deptcode + "&nian=" + nd;

                }
            });
            $("#btn_hznian").click(function () {
                //if (confirm("确定将年预算覆盖为十二个月份预算总和吗？")) {
                $("#btn_hznianhidden").click();
                // } else {
                //    return;
                // }
            });
            $("#btn_mx").click(function () {
                var deptcodes = '<%=deptcodes %>';
                var gkdeptcode = '<%=Request["deptCode"]%>';
                window.location.href = "ystbHzDetail.aspx?ctrl=view&deptcodes=" + deptcodes + "&fromurl=-1&gkdept=" + gkdeptcode;
            });
            $("#btn_zt").click(function () {
                var deptcode = '<%=Request["deptCode"]%>';
                var nd = '<%=Request["nd"]%>';
                window.showModalDialog("gkdept_zt_list.aspx?deptcode=" + deptcode + "&nian=" + nd, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:500px;status:no;scroll:yes');
                //openDetail("gkdept_zt_list.aspx?deptcode=" + deptcode + "&nian=" + nd);
            });

            $("#btn_save").click(function () {
                $("#btn_hznian").click();
            });
        });
        function openDetail(openUrl) {
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:900px;status:no;scroll:yes');
            return returnValue;
        }
        function toexcel() {

            var deptcode = '<%=Request["deptCode"]%>';
            var tblx = '<%=Request["type"]%>';
            var nd = '<%=Request["nd"]%>';
            var yskmtype = '<%=Request["yskmtype"]%>';
            var xmcode = '<%=Request["xmcode"]%>';
            window.location.href = "tbToExcelbyNOPI.aspx?deptCode=" + deptcode + "&nd=" + nd + "&type=" + tblx + "&yskmtype=" + yskmtype + "&xmcode=" + xmcode;
            //var returnval = openDetail("tbToExcel.aspx?deptCode=" + deptcode + "&nd=" + nd + "&type=" + tblx + "&yskmtype=" + yskmtype);
            //if (returnval != undefined && returnval == 'y') {

            //}
        }
        function importexcel() {
            var returnval = openDetail("ExcelImport.aspx");
            if (returnval != undefined && returnval == 'y') {
                $("#btn_reload").click();
            }
        }
        function GetYsSum(obj) {

            var tr = $(obj).parent().parent();
            var km = tr.find("td:eq(1)").text();
            var ndje = tr.find("td:eq(2)").find("input").val();
            if (km.length > 0 && ndje.length > 0)
                $("#tempMsg").text("【" + km + "】" + "年度预算：" + ndje);
            else
                $("#tempMsg").text("");
        }
        //年预算        

        function jsnys(obj) {
            var yearmoney = 0;
            var Rows = obj.parentNode.parentNode;
            var RowsInput = $(Rows).find("input:text ");


            for (var j = 1; j < 13; j++) {
                var yueJe = RowsInput[j].value;
                if (yueJe != '' && !isNaN(yueJe)) {
                    yearmoney += parseFloat(yueJe);
                }
                RowsInput[0].value = yearmoney;
            }
        }
        function SetQuarter(obj, flag) {
            //GetYsSum(obj);
            var Rows = obj.parentNode.parentNode;
            var RowsInput = $(Rows).find("input:text");
            var MoneyCounnt = 0;


            switch (flag) {
                case "1": //  算第一个季度的金额
                    for (var i = 1; i < 4; i++) {
                        if (RowsInput[i].value != "") {
                            if (!isNaN(RowsInput[i].value)) {
                                MoneyCounnt += parseFloat(RowsInput[i].value);

                            }
                            else {
                                alert("请输入阿拉伯数字");
                                obj.value = "";
                            }
                        }
                    }
                    $(Rows).find("td")[6].innerHTML = MoneyCounnt == "0" ? "" : MoneyCounnt;

                    break;
                case "2": //  算第二个季度的金额
                    for (var i = 4; i < 7; i++) {
                        if (RowsInput[i].value != "") {
                            if (!isNaN(RowsInput[i].value)) {
                                MoneyCounnt += parseFloat(RowsInput[i].value);

                            }
                            else {
                                alert("请输入阿拉伯数字");
                                obj.value = "";
                            }
                        }
                    }
                    $(Rows).find("td")[10].innerHTML = MoneyCounnt == "0" ? "" : MoneyCounnt;

                    //RowsInput[0].value = parseFloat($(Rows).find("td")[6].innerHTML) + parseFloat(MoneyCounnt);
                    break;
                case "3": //  算第三个季度的金额
                    for (var i = 7; i < 10; i++) {
                        if (RowsInput[i].value != "") {
                            if (!isNaN(RowsInput[i].value)) {
                                MoneyCounnt += parseFloat(RowsInput[i].value);
                            }
                            else {
                                alert("请输入阿拉伯数字");
                                obj.value = "";
                            }
                        }
                    }
                    $(Rows).find("td")[14].innerHTML = MoneyCounnt == "0" ? "" : MoneyCounnt;

                    //RowsInput[0].value = parseFloat($(Rows).find("td")[6].innerHTML) + parseFloat($(Rows).find("td")[10].innerHTML) + parseFloat(MoneyCounnt);
                    break;
                case "4": //  算第四个季度的金额 
                    for (var i = 10; i < 13; i++) {
                        if (RowsInput[i].value != "") {
                            if (!isNaN(RowsInput[i].value)) {
                                MoneyCounnt += parseFloat(RowsInput[i].value);
                            }
                            else {
                                alert("请输入阿拉伯数字");
                                obj.value = "";
                            }
                        }
                    }

                    $(Rows).find("td")[18].innerHTML = MoneyCounnt == "0" ? "" : MoneyCounnt;
                    break;
                default: break;
            }

            //计算年预算
            //yearmoney = parseFloat(jd1) + parseFloat(jd2) + parseFloat(jd3) + parseFloat(jd4);


        }
        function EnbleTxt() {
            //如果该td被设定为不可编辑的颜色 通过js实现
            $("body td[class='unEdit'] input").attr("readonly", "readonly");
            //如果改行tr被设定为不可编辑的颜色 其所属的td也变为不可编辑 通过js实现
            $("body tr[class='unEdit'] td input").attr("readonly", "readonly");

            ////如果该td被设定为不可编辑的颜色 通过js实现
            //var ArrTd = $("body td");
            //for (var i = 0; i < ArrTd.length; i++) {
            //    if (ArrTd[i].style.backgroundColor == "#dedede") {

            //        ArrTd[i].childNodes[0].readOnly = true;
            //    }
            //}
            ////如果改行tr被设定为不可编辑的颜色 其所属的td也变为不可编辑 通过js实现
            //var ArrTr = $("body tr");
            //$(ArrTr).each(function (index) {
            //    if ($(ArrTr[index]).css("background-color") == "#dedede") {

            //        $(this).find("input").attr("readonly", "readonly");
            //    }
            //});
        }
        function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: -10px;margin: 0;'></div>");
        }
        // 返回到列表页deptCode=0104&nd=2016&type=ystb&yskmtype=01&tbtype=zsex&limittotal=0&jecheckflg=0
        function fh() {
            //先判断是否是审核页过来的
            var checking = '<%=Request["checking"]%>';
            if (checking != null && checking != undefined && checking != '') {
                window.history.go(-1);
                return;
            }
            //返回列表页
            var yskmtype = '<%=Request["yskmtype"]%>';
            var tbtype = '<%=Request["tbtype"]%>';
            var limittotal = '<%=Request["limittotal"]%>';
            var jecheckflg = '<%=Request["jecheckflg"]%>';
            var xmcode = '<%=Request["xmcode"]%>';
            var wdheight = '<%=Request["wdheight"]%>';
            var deptCode = '<%=Request["deptCode"]%>';

            var url = "ystbFrame.aspx?yskmtype=" + yskmtype + "&tbtype=" + tbtype + "&limittotal=" + limittotal + "&jecheckflg=" + jecheckflg;
            var isdz = '<%= Request["isdz"]%>';//是否是大智学校的标记
            if (isdz != null || isdz != undefined || isdz != "") {
                url = url + "&isdz=" + isdz;
            }
            var xkfx = '<%=Request["xkfx"]%>';//新开分校标记
            if (xkfx != null || xkfx != undefined || xkfx != "") {
                url = url + "&xkfx=" + xkfx;
            }
            if (xmcode != null || xmcode != undefined || xmcode != "") {
                url = url + "&xmcode=" + xmcode
            }
            if (wdheight != null || wdheight != undefined || wdheight != "") {
                url = url + "&wdheight=" + wdheight;
            }
            if (deptCode != null || deptCode != undefined || deptCode != "") {
                url = url + "&deptCode=" + deptCode;
            }

            window.location.href = url;
        }
        function pingjun() {//.not(":first")
            //alert();
            
            $("#GridView1 tr[class!=unEdit]").not(":last").each(function (i, e) {
                var rowInputs = $(this).find("input[type=text]");
                var nd = rowInputs[0].value//年度预算

                if (nd == null || nd == undefined || nd == "" || isNaN(nd)) {
                    return;
                }
                var eve = (nd / 12).toFixed(2);
                $(rowInputs).each(function (i, e) {
                    if (i > 0) {
                        $(this).val(eve);
                    }

                });
                //倒挤数
                var daoji = nd - (eve * 12);
                var daoji_je = parseFloat(eve) + parseFloat(daoji);
                rowInputs.eq(12).val(daoji_je.toFixed(2));
            });
        }

    </script>

    <style type="text/css">
        .righttxt {
            text-align: right;
            background-color: Transparent;
        }
    </style>
</head>
<body style="background: #CCE8CF; overflow-y: hidden; overflow-x: auto;">
    <form id="form1" runat="server">

        <div class="baseDiv">
            <div>
                <input type="button" value="返回列表页" class="baseButton" onclick="fh()" id="fanhui" runat="server" />
                <asp:Button ID="btn_save" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_save_Click" />
                <input type="button" id="btn_toexcel" value="导出excel" class="baseButton" onclick="toexcel()" />
                <%--<asp:Button ID="btn_ExportExcel" runat="server" Text="导出excel" CssClass="baseButton"
                    OnClientClick="return toexcel();" />--%>
                <input type="button" value="导入excel" class="baseButton" onclick="importexcel();" style="display: none" />
                <asp:Button ID="btn_hznianhidden" Text="合计年预算" CssClass="hiddenbill" runat="server" OnClick="btn_hznian_Click" />
                <input type="button" value="平均分配年预算" class="baseButton" onclick="pingjun();" />
                <input type="button" id="btn_hznian" value="合计年预算" class="hiddenbill" />
                <asp:Button ID="btn_hzfx" Text="归口部门汇总" OnClick="btn_hzfx_Click" CssClass="baseButton" runat="server" Visible="false" />
                <input type="button" id="btn_hz" runat="server" value="查看区域预算汇总" class="baseButton" />
                <input type="button" id="btn_mx" runat="server" value="查看区域预算明细" class="baseButton" />
                <input type="button" id="btn_zt" value="查看分校预算状态" class="baseButton" runat="server" />


                <asp:Button ID="btn_reload" runat="server" OnClick="btn_reload_Click" CssClass="hiddenbill" />

            </div>
            <div style="margin-top: 5px; margin-left: 3px;">
                <asp:Label ID="ltdept" runat="server" ForeColor="Red"></asp:Label>
                <asp:Label ID="LaTbfs" runat="server" Text="" ForeColor="Red"></asp:Label>
                <asp:Label runat="server" ForeColor="Red" ID="lbtotalamout"></asp:Label>
                <asp:Label runat="server" ForeColor="Red" ID="lbmonemooney" Visible="false"></asp:Label>
            </div>
            <div style="float: right">
                <table>
                    <tr>
                        <td></td>
                        <td>
                            <div style="height: 15px; border: solid 2px red; background-color: white">
                                可以填报
                            </div>
                        </td>
                        <td>(预算过程开始)
                        </td>
                        <td>
                            <div style="height: 15px; border: solid 2px red; background-color: #DEDEDE">
                                不允许填报
                            </div>
                        </td>
                        <td>(预算过程未开始，已经结束，填报提交审核，填报审核完毕。)
                        </td>
                    </tr>
                </table>
            </div>
            <div style="margin-top: 5px; margin-left: 5px;">
                附件：
                <asp:FileUpload ID="upLoadFiles" runat="server" Width="240px" />
                <asp:Label ID="Lafilename" runat="server" Text=""></asp:Label>
                <asp:HiddenField ID="hiddFileDz" runat="server" />
                <asp:Button ID="btn_sc" runat="server" Text="上 传" CssClass="baseButton" OnClick="btnScdj_Click" />
                <asp:Literal ID="Literal1" runat="server"></asp:Literal>
                <%--提示信息--%>
                <asp:Label ID="laFilexx" runat="server" Text="" ForeColor="Red"></asp:Label>
            </div>
            <div style="text-align: center">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="false" CssClass="baseTable" ShowFooter="true" OnRowDataBound="GridView1_RowDataBound"
                    Width="1650px">
                    <Columns>
                        <asp:BoundField HeaderText="序号" DataField="" ItemStyle-Width="32" HeaderStyle-Width="30" />
                        <%--<asp:BoundField DataField="km" HeaderText="月份\科目" HtmlEncode="false" ItemStyle-Width="100"
                            HeaderStyle-Width="100" />--%>
                        <asp:TemplateField HeaderText="月份\科目" ItemStyle-Width="150" HeaderStyle-Width="150">
                            <ItemTemplate>
                                [<%#Eval("kmbh") %>]<%#Eval("km") %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:BoundField DataField="year" HeaderText="年<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="年" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtyear" runat="server" Width="90%" Text='<%#Eval("year") %>' CssClass="righttxt"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdyearYsnZj" Value='<%#Eval("yearYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:BoundField DataField="January"  HeaderText="一月份<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="一月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtJanuary" runat="server" Width="90%" Text='<%#Eval("January") %>'
                                    CssClass="righttxt" onblur="SetQuarter(this,'1');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdJanuaryYsnzj" Value='<%#Eval("JanuaryYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--     <asp:BoundField DataField="February" HeaderText="二月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="二月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtFebruary" runat="server" Width="90%" Text='<%#Eval("February") %>'
                                    CssClass="righttxt" onblur="SetQuarter(this,'1');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdFebruaryYsnZj" Value='<%#Eval("FebruaryYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--    <asp:BoundField DataField="march" HeaderText="三月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="三月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtmarch" runat="server" Width="90%" Text='<%#Eval("march") %>'
                                    CssClass="righttxt" onblur="SetQuarter(this,'1');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdmarchYsnZj" Value='<%#Eval("marchYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="" HeaderText="第一季度" HtmlEncode="false" ItemStyle-Width="50"
                            HeaderStyle-Width="50" />
                        <asp:TemplateField HeaderText="四月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtApril" runat="server" Width="90%" Text='<%#Eval("April") %>'
                                    CssClass="righttxt" onblur="SetQuarter(this,'2');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdAprilYsnZj" Value='<%#Eval("AprilYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--  <asp:BoundField DataField="May" HeaderText="五月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="五月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtMay" runat="server" Width="90%" Text='<%#Eval("May") %>' CssClass="righttxt"
                                    onblur="SetQuarter(this,'2');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdMayYsnZj" Value='<%#Eval("MayYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%-- <asp:BoundField DataField="June" HeaderText="六月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="六月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtJune" runat="server" Width="90%" Text='<%#Eval("June") %>' CssClass="righttxt"
                                    onblur="SetQuarter(this,'2');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdJuneYsnZj" Value='<%#Eval("JuneYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="" HeaderText="第二季度" HtmlEncode="false" ItemStyle-Width="50"
                            HeaderStyle-Width="50" />
                        <asp:TemplateField HeaderText="七月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtJuly" runat="server" Width="90%" Text='<%#Eval("July") %>' CssClass="righttxt"
                                    onblur="SetQuarter(this,'3');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdJulyYsnZj" Value='<%#Eval("JulyYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--  <asp:BoundField DataField="August" HeaderText="八月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="八月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtAugust" runat="server" Width="90%" Text='<%#Eval("August") %>'
                                    CssClass="righttxt" onblur="SetQuarter(this,'3');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdAugustYsnZj" Value='<%#Eval("AugustYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--        <asp:BoundField DataField="September" HeaderText="九月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="九月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtSeptember" runat="server" Width="90%" Text='<%#Eval("September") %>'
                                    CssClass="righttxt" onblur="SetQuarter(this,'3');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdSeptemberYsnZj" Value='<%#Eval("SeptemberYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="" HeaderText="第三季度" HtmlEncode="false" ItemStyle-Width="50"
                            HeaderStyle-Width="50" />
                        <asp:TemplateField HeaderText="十月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtOctober" runat="server" Width="90%" Text='<%#Eval("October") %>'
                                    CssClass="righttxt" onblur="SetQuarter(this,'4');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdOctoberYsnZj" Value='<%#Eval("OctoberYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--   <asp:BoundField DataField="November" HeaderText="十一月<br/>(预算金额)" HtmlEncode="false" />--%>
                        <asp:TemplateField HeaderText="十一月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtNovember" runat="server" Width="90%" Text='<%#Eval("November") %>'
                                    CssClass="righttxt" onblur="SetQuarter(this,'4');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdNovemberYsnZj" Value='<%#Eval("NovemberYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="十二月" ItemStyle-Width="80" HeaderStyle-Width="80">
                            <ItemTemplate>
                                <asp:TextBox ID="txtDecember" runat="server" Width="90%" Text='<%#Eval("December") %>'
                                    CssClass="righttxt" onblur="SetQuarter(this,'4');jsnys(this)" onclick="GetYsSum(this)"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdDecemberYsnZj" Value='<%#Eval("DecemberYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="" HeaderText="第四季度" HtmlEncode="false" ItemStyle-Width="50"
                            HeaderStyle-Width="50" />
                        <asp:TemplateField ItemStyle-CssClass="basehidden" HeaderStyle-CssClass="basehidden" FooterStyle-CssClass="basehidden">
                            <ItemTemplate>
                                <asp:HiddenField ID="HiddenKmbh" runat="server" Value='<%#Eval("kmbh") %>' />
                                <asp:HiddenField ID="hdIszyys" runat="server" Value='<%#Eval("iszyys") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="km" HeaderText="月份\科目" HtmlEncode="false" ItemStyle-Width="100"
                            HeaderStyle-Width="100" />
                    </Columns>
                </asp:GridView>
                <asp:GridView ID="GridView2" runat="server" AutoGenerateColumns="false" CssClass="baseTable"
                    Width="1500px">
                    <Columns>
                        <asp:BoundField HeaderText="序号" DataField="" />
                        <asp:BoundField DataField="km" HeaderText="\月份<br/>\<br/> 科目\" HtmlEncode="false" />
                        <asp:TemplateField HeaderText="年<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtyear" runat="server" Width="90%" Text='<%#Eval("year") %>' CssClass="righttxt"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdyearYsnZj" Value='<%#Eval("yearYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="第一季度<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtspring" runat="server" Width="90%" Text='<%#Eval("spring") %>'
                                    CssClass="righttxt"></asp:TextBox>
                                <asp:HiddenField ID="HiddenKmbh" runat="server" Value='<%#Eval("kmbh") %>' />
                                <asp:HiddenField runat="server" ID="hdspringYsnZj" Value='<%#Eval("springYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="第二季度<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtsummer" runat="server" Width="90%" Text='<%#Eval("summer") %>'
                                    CssClass="righttxt"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdsummerYsnZj" Value='<%#Eval("summerYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="第三季度<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtautumn" runat="server" Width="90%" Text='<%#Eval("autumn") %>'
                                    CssClass="righttxt"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdautumnYsnZj" Value='<%#Eval("autumnYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="第四季度<br/>(预算金额)">
                            <ItemTemplate>
                                <asp:TextBox ID="txtwinter" runat="server" Width="90%" Text='<%#Eval("winter") %>'
                                    CssClass="righttxt"></asp:TextBox>
                                <asp:HiddenField runat="server" ID="hdwinterYsnZj" Value='<%#Eval("winterYsnZj") %>' />
                            </ItemTemplate>
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>

                <div id="tempMsg" style="position: fixed; right: 2px; top: 2px; font-size: 14px; color: red;"></div>
                <asp:HiddenField ID="HiddenisFjtb" runat="server" />
            </div>
            <div style="color: red">
                填报说明：
                  <ul>
                      <%= HelpMsg %>
                  </ul>
                <asp:Label ID="lb_msg" runat="server" ForeColor="Red"></asp:Label>
                <asp:HiddenField ID="hdUserCode" runat="server" />

            </div>

        </div>
    </form>
</body>
</html>
