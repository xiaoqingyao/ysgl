<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ReportApplicationList.aspx.cs"
    Inherits="SaleBill_ReportApplication_ReportApplicationList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>报告申请单明细</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
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
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
        var status = "none";

        $(function() {
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                if( $(this).find("td")[0]!=null&& $(this).find("td")[0].innerHTML!="")
                {
                    var billCode = $(this).find("td")[0].innerHTML;
                    $("#hd_billCode").val(billCode);
                }
               
                $.post("../../webBill/MyWorkFlow/GetBillStatus.ashx", { "billCode": billCode }, function(data, status) {
                    if (status == "success") {
                        $("#wf").html(data);
                    }
                });
            });
            //刷新
               $("#btn_sx").click(function(){
                 location.replace(location.href);
            });
            //修改
             $("#btn_edit").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行!");
                    return;
                }
//                var zt = checkrow.find("td")[8].innerHTML;
//                if (zt != "未提交") {
//                    alert("该行已提交,不能修改！");
//                    return;
//                }
                var billcode = checkrow.find("td")[0].innerHTML;
                openDetail("ReportApplication.aspx?Ctrl=Edit&Code=" + billcode);
            });
            //删除
              $("#btn_delete").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行!");
                    return;
                }
                if(confirm('是否确定删除？')){
                //                var zt = checkrow.find("td")[8].innerHTML;
//                if (zt != "未提交") {
//                    alert("该行已提交,不能删除！");
//                    return;
//                }
//                if (!confirm("您确定要删除选中的单据吗?")) {
//                    return;
//                }
                      var billcode = checkrow.find("td")[0].innerHTML;
                $.post("../../webBill/MyAjax/DeleteBill.ashx", { "billCode": billcode, "type": "bgsq" }, function(data, status) {
                    if (status == "success") {
                        if (data == "1") {
                            alert("删除成功!");
                            $(".highlight").remove();
                        }
                        else {
                            alert("删除失败!");
                        }
                    }
                    else {
                        alert("删除失败!");
                    }
                });
                }else{
                    return;
                }

              
            });
          
            //取消
            $("#btn_cancle").click(function() {
                document.getElementById("trSelect").style.display = "none";
            });
        
            
            
        
//               //撤销单据提交
//            $("#btn_replace").click(function() {
//                var checkrow = $(".highlight");
//                if (checkrow.val() == undefined) {
//                    alert("请先选择行!");
//                    return;
//                }
//                var zt = checkrow.find("td")[8].innerHTML;
//                if (zt == "未提交" || zt == "审批通过") {
//                    alert("该单据不能撤销操作!");
//                } else {
//                    var billcode = checkrow.find("td")[0].innerHTML;
//                    $.post("../../webBill/MyWorkFlow/WorkFlowReplace.ashx", { "billcode": billcode, "flowid": "tsfl", "billtype": "tzcfld" }, function(data, status) {
//                        if (status == "success") {
//                            if (data == "1") {
//                                alert("撤销成功！");
//                                checkrow.find("td")[8].innerHTML = "未提交";
//                            }
//                            else {
//                                alert("单据以进入审批，不能撤销");
//                            }
//                        }
//                        else {
//                            alert("失败");
//                        }
//                    });
//                }
//            });
//              //审批提交
//            $("#btn_summit").click(function() {
//                var checkrow = $(".highlight");
//                if (checkrow.val() == undefined) {
//                    alert("请先选择行!");
//                    return;
//                }
//                var zt = checkrow.find("td")[8].innerHTML;
//                if (zt != "未提交") {
//                    alert("单据已提交.不要重复操作!");
//                } else {
//                    var billcode = checkrow.find("td")[0].innerHTML;
//                    $.post("../../webBill/MyWorkFlow/WorkFlowSummit.ashx", { "billcode": billcode, "flowid": "tsfl", "billtype": "tzcfld" }, function(data, status) {
//                        if (data == "-1" || data == "-2") {
//                            alert("提交失败！");
//                        } else if (data == "-3") {
//                            alert("提交失败，原因可能是审批流中的人员没有单据对应部门的管理权限，请联系管理员！");
//                        } else {
//                            if (status == "success") {
//                                alert("提交成功！");
//                                checkrow.find("td")[8].innerHTML = data;
//                            }
//                        }
//                    });
//                }
//            });
        });
        
       
        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:350px;dialogWidth:700px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("btn_sx").click();
            }
        } function openSplc(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        function openprint(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:900px;status:no;scroll:yes');
        }
        function openLookSpStep(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
   function gudingbiaotou() {
            var t = document.getElementById("<%=myGrid.ClientID%>");
              if(t==null||t.rows.length<1){
                return;
            }
            var t2 = t.cloneNode(true);
            for (i = t2.rows.length - 1; i > 0; i--) {
                t2.deleteRow(i);
            }
            t.deleteRow(0);
            header.appendChild(t2);
            var mainwidth = document.getElementById("main").style.width;
            mainwidth = mainwidth.substring(0, mainwidth.length - 2);
            mainwidth = mainwidth - 16;
            document.getElementById("header").style.width = mainwidth;
        }

    </script>

</head>
<body onload="gudingbiaotou();">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                <input type="button" value="刷 新" id="btn_sx" class="baseButton" />
                <%-- <input type="button" id="btnSelect" value="查 询" class="baseButton"  visible="false"/>--%>
                <input type="button" value="增 加" runat="server" id="Button1" class="baseButton" onclick="openDetail('ReportApplication.aspx?Ctrl=Add&par=' + Math.random());" />
                <input type="button" value="修 改" id="btn_edit" class="baseButton" />
                <input type="button" value="删 除" id="btn_delete" class="baseButton" ondblclick="return confirm('是否确定删除？');" />
            </td>
        </tr>
        <tr id="trSelect" style="display: none;">
            <td align="left">
                <table class="baseTable" style="text-align: left;">
                    <tr>
                        <td>
                            申请日期从：
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateFrm" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            到：
                        </td>
                        <td>
                            <asp:TextBox ID="txtDateTo" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            报告单号：
                        </td>
                        <td>
                            <asp:TextBox ID="txtCode" runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            车架号：
                        </td>
                        <td>
                            <asp:TextBox ID="txtcarcode" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            有效期始：
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox1" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            有效期末：
                        </td>
                        <td>
                            <asp:TextBox ID="TextBox2" runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="6" style="text-align: center">
                            <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="Button4_Click" />
                            <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                        </td>
                    </tr>
                </table>
            </td>
        </tr>
        <tr>
            <td>
                <div id="header">
                </div>
                <div class="baseDiv" id="main" style="overflow-y: scroll; margin-top: -1px; width: 1100px;
                    height: 380px;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"  Style="table-layout: fixed" Width="100%"
                        CssClass="myGrid" OnItemDataBound="myGrid_ItemDataBound" AllowPaging="True" PageSize="20">
                        <Columns>
                            <asp:BoundColumn DataField="ReportAppCode" HeaderText="报告单号" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader " />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="username" HeaderText="报告人姓名" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptnames" HeaderText="报告单位" DataFormatString="{0:D}"
                                ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ReportDate" HeaderText="报告日期" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ReportExplain" HeaderText="报告说明" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ReportRemark" HeaderText="报告备注" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="true" CssClass="myGridItem" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 30px">
                &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                </asp:DropDownList>
                页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                    runat="server"></asp:Label>条
            </td>
        </tr>
        <tr>
            <td>
                <table>
                    <tr>
                        <td style="height: 30px">
                            审核流程：<asp:Label ID="lblShlc" runat="server"></asp:Label>
                        </td>
                        <td id="wf">
                        </td>
                    </tr>
                </table>
            </td>
            <asp:HiddenField ID="hd_billCode" runat="server" />
        </tr>
    </table>
    </form>
</body>
</html>
