<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SaleProcessList.aspx.cs"
    Inherits="SaleBill_Salepreass_SaleProcessList" %>

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
        gudingbiaotounew($("#myGrid"), 380);
            $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
                $("#spcode").val($(this).find("td")[0].innerHTML);
            });
            $("#btn_new").click(function() {
//                var deptCode = $("#hf_sp").val();
//                alert(deptCode);
                openDetail("SalepreassDetails.aspx?ctrl=add");
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
                        alert("请先选择行！");
                    }
                    else
                    {
                    var deptCode=rows.find("td")[0].innerHTML;
                   openDetail("SalepreassDetails.aspx?ctrl=view&Code=" + deptCode);
                 
                    }
            });
            $("#btn_edit").click(function() {
                var checkrow = $(".highlight");
                if (checkrow.val() == undefined) {
                    alert("请先选择行！");
                    return;
                }
                var rows=$(".highlight");
                if(rows.val()==undefined)
                {
                    alert("请先选择行！");
                }
                else
                {
                var bh=rows.find("td")[0].innerHTML;
                openDetail("SalepreassDetails.aspx?ctrl=edit&Code="+bh);
             
                }

            });
        });
    
        function openDetail(openUrl)
        {
            var returnValue=window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:300px;dialogWidth:480px;status:no;scroll:no');
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
      function gudingbiaotounew(obj, height) {
            var gvn = obj.clone(true).removeAttr("id");
            $(gvn).find("tr:not(:first)").remove();
            $(gvn).css("margin-bottom", "0px");
            obj.css("margin", "0 0 0 0");
            obj.before(gvn);
            obj.find("tr:first").remove();
            obj.wrap("<div style='height:" + height + "px; margin-top:0px; border-bottom:solid 1px #C1DAD7 ;width:" + (parseFloat(obj.width()) + 20) + "px; overflow-y: scroll;overflow-x:hidden; overflow: auto; padding: 0;margin: 0;'></div>");
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                <input class="baseButton" type="button" id="btn_new" value="新 增" />
                &nbsp;<input class="baseButton" type="button" id="btn_edit" value="修 改" />
                <%--  &nbsp;<asp:Button ID="Button3" runat="server" Text="删 除" CssClass="baseButton" 
                        OnClientClick="return confirm('是否确定删除该销售过程？');" 
                        onclick="Button3_Click"  />--%>
                &nbsp;<input class="baseButton" type="button" id="Button1" value="查 看" />
                &nbsp;<asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                &nbsp;<asp:Button ID="Button4" runat="server" Text="查 询" CssClass="baseButton" OnClick="Button4_Click" />
                <asp:Button ID="Button5" runat="server" CssClass="baseButton" Text="禁 用" OnClick="Button5_Click" />
                <asp:Button ID="Button6" runat="server" CssClass="baseButton" Text="启 用" OnClick="Button6_Click" />
            </td>
        </tr>
        <tr>
            <td align="left">
                <div id="header" style="overflow: hidden;">
                </div>
                 <div style="position: relative; word-warp: break-word; word-break: break-all">
                    <asp:DataGrid ID="myGrid" runat="server" AllowPaging="True" AutoGenerateColumns="False"
                        CellPadding="3"  CssClass="myGrid" PageSize="17">
                        <Columns>
                            <asp:BoundColumn DataField="Code" HeaderText="编号" HeaderStyle-Width="200" ItemStyle-Width="200">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="PName" HeaderText="名称" HeaderStyle-Width="300" ItemStyle-Width="300">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="status" HeaderText="状态" HeaderStyle-Width="200" ItemStyle-Width="200">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>&nbsp;<asp:HiddenField ID="hf_sp" runat="server" />
                    <asp:HiddenField ID="spcode" runat="server" />
                </div>
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
