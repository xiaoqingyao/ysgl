<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ysnzjEdit.aspx.cs" Inherits="webBill_ysgl_ysnzjEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>预算内追加</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <script language="javascript" type="Text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>
    <script language="javascript" type="Text/javascript">
     
        
        function calLj(obj)
        {
            var currentCode=$(obj).parent().parent().find("td:eq(0)").text();
            
            var arrIndex=new Array();
            var arrCode=new Array();
            var arrVal=new Array();
            
            var index=0;
            $("#myGrid").find("tr").each(function(){
                if(index==0)
                {
                    index=index+1;
                }
                else
                {   
                    arrIndex.push(index);
                    arrCode.push($(this).find("td:eq(1)").html());
                    arrVal.push($(this).find(".rightBox:eq(0)").val());
                    index=index+1;
                }
            });
            var list=ysgl_yszjAdd.getCalResult(currentCode,arrIndex,arrCode,arrVal).value;
            
            //循环赋值
            index=0;
            $("#myGrid").find("tr").each(function(){
                if(index==0)
                {
                    index=index+1;
                }
                else
                {   
                    var val="";
                    for(var j=0;j<=list.length-1;j++)
                    {
                        var arr=list[j].split(',');
                        if(arr[0]==index)
                        {
                            val=arr[1];
                        }
                    }
                    $(this).find(".rightBox:eq(0)").val(val);
                    index=index+1;
                }
            });
        }
        
           //替换非数字
        function replaceNaN(obj) {
            if (isNaN(obj.value)) {
                obj.value = '0.00';
                alert("必须用阿拉伯数字表示！");
            };
        }
    </script>
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0" width="100%">
            <tr>
                <td style="height: 27px">
                    <asp:Label ID="Label1" runat="server" ForeColor="Red">预算内追加</asp:Label>
                     &nbsp; 部门：
                    <asp:Label ID="labedeptcode" runat="server" Text="Label"></asp:Label>
                     <asp:DropDownList runat="server" ID="LaDept" AutoPostBack="True"  Visible="false"  OnSelectedIndexChanged="LaDept_SelectedIndexChanged">
                    </asp:DropDownList>
                    <asp:Button ID="Button1" runat="server" Text="保 存" CssClass="baseButton" Visible="false" OnClick="Button1_Click" />
                    <asp:Button ID="Button2" runat="server" Text="取 消" CssClass="baseButton" Visible="false" OnClick="Button2_Click" /></td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"   CssClass="myGrid">
                        <Columns>
                            <asp:BoundColumn DataField="yskmCode" HeaderText="科目编号">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="yskmBm" HeaderText="科目代码">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="yskmMc" HeaderText="科目名称">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <%--<asp:TemplateColumn HeaderText="追加金额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="TextBox2" runat="server" Width="131px" CssClass="rightBox" Text="0.00" onkeyup="replaceNaN(this);"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>--%>
                            <asp:BoundColumn DataField="je" HeaderText="追加金额" DataFormatString="{0:F2}">
                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                Font-Strikeout="False" Font-Underline="False" Wrap="False" HorizontalAlign="Right" />
                        </asp:BoundColumn>
                            <asp:BoundColumn DataField="tblx" HeaderText="填报类型">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="可追加金额">
                                <HeaderStyle CssClass="hiddenbill" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="hiddenbill" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemTemplate>
                                    <asp:Label ID="txtkzjmoney" runat="server" Width="131px" CssClass="hiddenbill" Text="0.00"></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
            </tr>
        </table>
    </form>
</body>
</html>
