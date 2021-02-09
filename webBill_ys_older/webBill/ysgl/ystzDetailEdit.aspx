<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ystzDetailEdit.aspx.cs" Inherits="webBill_ysgl_ystzDetailEdit" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml" >
<head runat="server">
    <title>无标题页</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    
    <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>


    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="Text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script language="javascript" type="Text/javascript">
        function openDetail(openUrl)
        {
            window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:460px;status:no;scroll:yes');
        }
        
        function calLj(obj)
        {
            var currentCode=$(obj).parent().parent().find("td:eq(0)").text();
            
            var arrIndex=new Array();
            var arrCode=new Array();
            var arrVal=new Array();
            var arrOldVal=new Array();
            
            var index=0;
            $("#myGrid1").find("tr").each(function(){
                if(index==0)
                {
                    index=index+1;
                }
                else
                {   
                    arrIndex.push(index);
                    arrCode.push($(this).find("td:eq(1)").html());
                    arrVal.push($(this).find(".rightBox:eq(0)").val());
                    arrOldVal.push($(this).find("td:eq(4)").html());
                    index=index+1;
                }
            });
            var list=webBill_ysgl_ystzDetailEdit.getCalResult(currentCode,arrIndex,arrCode,arrVal).value;
            
            //循环赋值
            index=0;
            $("#myGrid1").find("tr").each(function(){
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
                //循环计算被动调整的预算数据
                //循环赋值
            index=0;
            $("#myGrid2").find("tr").each(function(){
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
                            val=webBill_ysgl_ystzDetailEdit.CalNormal(arr[1],arrOldVal[j],$(this).find("td:eq(4)").html()).value;
                        }
                    }
                    $(this).find(".rightBox:eq(0)").val(val);
                    index=index+1;
                }
            });
        }
    </script>

</head>
<body>
    <form id="form1" runat="server">
        <table cellpadding="0" cellspacing="0">
            <tr id="trResult" runat="server">
                <td style="height: 22px"><asp:Button ID="Button1" runat="server" Text="保 存" CssClass="baseButton" OnClick="Button1_Click" />&nbsp;<asp:Button
                        ID="Button3" runat="server" CssClass="baseButton" OnClick="Button3_Click" Text="取 消" />
                    &nbsp; 源预算过程： 【<asp:Label ID="Label3" runat="server" ForeColor="Red"></asp:Label>】<asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>
                </td>
                <td style="height: 22px">
                </td>
                <td style="height: 22px">目标预算过程： 【<asp:Label ID="Label4" runat="server" ForeColor="Red"></asp:Label>】<asp:Label ID="Label2" runat="server" ForeColor="Red"></asp:Label></td>
            </tr>
            <tr>
                <td>
                    <asp:DataGrid ID="myGrid1" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid">
                        <Columns>
                            <asp:BoundColumn DataField="yskm" HeaderText="科目编号">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="kmbm" HeaderText="科目代码">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="kmmc" HeaderText="科目名称">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="调整后金额"><ItemTemplate>
                                <asp:TextBox ID="TextBox2" runat="server" CssClass="rightBox" onblur="calLj(this);"
                                    Text='<%# Eval("je") %>' Width="131px"></asp:TextBox>
                            </ItemTemplate>
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="jeBefore" HeaderText="预算金额" DataFormatString="{0:F2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" HorizontalAlign="Right" />
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid>
                </td>
                <td>
                    &nbsp; &nbsp;
                </td>
                <td>
                    <asp:DataGrid ID="myGrid2" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        CssClass="myGrid">
                        <Columns>
                            <asp:BoundColumn DataField="yskm" HeaderText="科目编号">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="kmbm" HeaderText="科目代码">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="kmmc" HeaderText="科目名称">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:TemplateColumn HeaderText="调整后金额">
                                <ItemTemplate>
                                    <input id="txtJe" runat="server" class="rightBox" readonly="readonly" type="text" value='<%# Eval("je") %>' />
                                </ItemTemplate>
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="jeBefore" HeaderText="预算金额" DataFormatString="{0:F2}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" HorizontalAlign="Right" />
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid></td>
            </tr>
        </table>
    </form>
</body>
</html>
