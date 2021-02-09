<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleConfigList.aspx.cs" Inherits="SaleBill_SaleConfig_SaleConfigList" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
         //高亮显示
        $(function(){
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
              
                if($(this).find("td")[0]=null)
                {
                   $("#spcode").val($(this).find("td")[0].innerHTML);

                }
            });
            $("#btn_new").click(function() {
                var deptCode = $("#hf_sp").val();
                openDetail("SaleConfigDail.aspx?ctrl=add");
            });
             $("#Button1").click(function() {
              var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                        alert("请先选择行！");
                        return;
                    }
                    var rows=$(".highlight");
                    if(rows.val()==undefined)
                    {
                        alert("未选择行！");
                    }
                    else
                    {
                    var deptCode=rows.find("td")[0].innerHTML;
                   openDetail("SaleConfigDail.aspx?ctrl=view&Code=" + deptCode);
                 
                    }
            });
            //刷新
            $("#btn_refresh").click(function(){
          
                window.location.reload(); 
            });
            //修改
            $("#btn_edit").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行！");
                    return;
                }
                var rows=$(".highlight");
                if(rows.val()==undefined)
                {
                    alert("未选择行！");
                }
                else
                {
                var bh=rows.find("td")[0].innerHTML;
                openDetail("SaleConfigDail.aspx?ctrl=edit&Code="+bh);
             
                }

            });
        });
    
        function openDetail(openUrl)
        {
            var returnValue=window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:230px;dialogWidth:480px;status:no;scroll:no');
            if(returnValue==undefined||returnValue=="")
            {
                return false;
            }
            else
            {
                document.getElementById("Button4").click();
            }
        }
        $(function() {
            $("#btn_toExcel").click(function() {
                window.open("YskmToExcel.aspx");
            });
        });
        
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
    <table>
        <tr>
            <td style="height: 27px">
                <input class="baseButton" type="button" id="btn_refresh" value="刷新" />
                <input class="baseButton" type="button" id="btn_new" value="新 增" />
                <input class="baseButton" type="button" id="btn_edit" value="修 改" />
                <input class="baseButton" type="button" id="Button1" value="查 看" />
                <asp:Button ID="Button3" runat="server" Text="删 除" CssClass="baseButton" OnClientClick="return confirm('是否确定删除该销售过程？');"
                    OnClick="Button3_Click" />
                <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                <asp:Button ID="Button4" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button4_Click" />
                <asp:Button ID="Button5" runat="server" CssClass="baseButton" Text="启 用" OnClick="Button5_Click" />
                <asp:Button ID="Button6" runat="server" CssClass="baseButton" Text="禁 用" OnClick="Button6_Click" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <div id="header">
                </div>
                <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 800px; height: 400px;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid" AllowPaging="True" PageSize="17" Style="table-layout: fixed" Width="100%">
                        <Columns>
                            <asp:BoundColumn DataField="Code" HeaderText="编号">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="CName" HeaderText="配置项名称">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ControlNameFirst" HeaderText="从控制点">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="ControlNameSecond" HeaderText="到控制点">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Months" HeaderText="超出月份">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Status" HeaderText="状态">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Remark" HeaderText="备注">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid><asp:HiddenField ID="hf_sp" runat="server" />
                    <asp:HiddenField ID="spcode" runat="server" />
                </div>
            </td>
        </tr>
        <%-- <tr>
                <td style="height: 30px">
                    &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                    <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                    第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                    </asp:DropDownList>页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                    <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                        runat="server"></asp:Label>条</td>
            </tr>--%>
    </table>
    </form>
</body>
</html>
