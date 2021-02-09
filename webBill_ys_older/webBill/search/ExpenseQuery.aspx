<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ExpenseQuery.aspx.cs" Inherits="webBill_search_ExpenseQuery" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>报销单统计查询</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
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
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" type="text/css" rel="Stylesheet" />

    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script language="javascript" type="Text/javascript">
        $(function() {
        $(this).keypress(function(event) {
                var key = event.keyCode;
                if (key == '104') {
                 parent.helptoggle();
                }
            });
          
            //刷新
           $("#btn_sx").click(function(){
                 location.replace(location.href);
            });
//            $("#btn_look").click(function() {
//                var checkrow = $(".highlight");
//                if (checkrow.val() == undefined) {
//                    alert("请先选择行");
//                    return;
//                }
//                var billcode = checkrow.find("td")[0].innerHTML;
//                openDetail("travelApplicationDetails.aspx?Ctrl=View&Code=" + billcode);
//            });
        
            //取消
//            $("#btn_cancle").click(function() {
//                document.getElementById("trSelect").style.display = "none";
//            });

            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
        
        });

        function openDetail(openUrl) {
            //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新
            var returnValue = window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:700px;dialogWidth:900px;status:no;scroll:yes');

            if (returnValue == undefined || returnValue == "")
            { }
            else {
                document.getElementById("btn_cx").click();
            }
        } function openSplc(openUrl) {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        
         function onSelectBillChanged(strBillVal) {
            if (strBillVal == null || strBillVal == "") {
                return;
            } else if (strBillVal == "cc") {//出差
                openTravelApplication();
            } else if (strBillVal == "bg") {//报告单
                openCgsp2();
            } else if (strBillVal == "cg") {//采购单
                openCgsp();
            } else { }
            $("#selectBill").val("");
        }
        
         function openTravelApplication() {
            var url ='../bxgl/selectTravelAppBill.aspx';
            if($("#hdHsCCBG").val()=="1"){
                url+="?Status=HasRepBill";
            }
            var tempInner = window.showModalDialog(url, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');
            
            if (tempInner == undefined || tempInner == "")
            { }
            else {
                //给返回的结果添加上一个单选框 
                var strTemp = tempInner.substring(4, tempInner.length);
               var end=tempInner.indexOf("</td>");
                var code=tempInner.substring(8,end);
                $("#txtBillCode").val(code);
            }
        }
        
          function openCgsp2() {//选择附加的单据，打开单据选择
            var tempInner = window.showModalDialog('../bxgl/selectlscg.aspx', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');
            //alert(tempInner);
            if (tempInner == undefined || tempInner == "")
            { }
            else {
                //给返回的结果添加上一个单选框 
                var strTemp = tempInner.substring(4, tempInner.length);
                var end=tempInner.indexOf("</td>");
                var code=tempInner.substring(8,end);
                $("#txtBillCode").val(code);
               

            }
        }
        
           function openCgsp() {//选择附加的单据，打开单据选择
            var tempInner = window.showModalDialog('../bxgl/selectCgsp.aspx', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');
            if (tempInner == undefined || tempInner == "")
            { }
            else {
                //给返回的结果添加上一个单选框 
                var strTemp = tempInner.substring(4, tempInner.length);
                  var end=tempInner.indexOf("</td>");
                var code=tempInner.substring(8,end);
                $("#txtBillCode").val(code);
            }
        }
        
        
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table border="0" cellpadding="0" cellspacing="0" style="width: 100%">
        <tr style="height: 27px">
            <td>
                <input type="button" value="刷 新" id="btn_sx" class="baseButton" />
                 <asp:DropDownList ID="selectBill" runat="server" onchange="onSelectBillChanged(this.options[this.selectedIndex].value);">
                    <asp:ListItem Value="">--请选择单据--</asp:ListItem>
                </asp:DropDownList>
                <asp:TextBox ID="txtBillCode" runat="server" Width="400px"></asp:TextBox>

                <asp:Button ID="Button4" runat="server" Text="查 询" CssClass="baseButton" OnClick="btn_cx_Click" />
                  <input type="button" class="baseButton" value="帮助" onclick="javascript:parent.helptoggle();" />
            </td>
        </tr>
        <tr>
            <td>
                <div class="baseDiv">
                    <asp:DataGrid ID="myGrid" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid"
                        ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                        AllowPaging="True" PageSize="17" Width="842px" OnItemDataBound="myGrid_ItemDataBound">
                        <ItemStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:BoundColumn DataField="billCode" HeaderText="单据编号">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="hiddenbill" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="hiddenbill" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billName" HeaderText="单据编号">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptname" HeaderText="报销部门">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="username" HeaderText="报销人">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billJe" HeaderText="报销金额" DataFormatString="{0:N2}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="billDate" HeaderText="报销时间" DataFormatString="{0:D}">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="审批状态" DataField="stepID">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                            <asp:BoundColumn HeaderText="附加单据编码" DataField="sqCode">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" Position="Top" Mode="NumericPages" BorderColor="Black"
                            BorderStyle="Solid" BorderWidth="1px"></PagerStyle>
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
                    runat="server" />
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
