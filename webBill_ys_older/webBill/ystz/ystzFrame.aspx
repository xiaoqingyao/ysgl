<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ystzFrame.aspx.cs" Inherits="webBill_ystz_ystzFrame" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    
    <script language="javascript" type="Text/javascript">
        function openDetail(openUrl)
        {
            var returnValue=window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:960px;status:no;scroll:yes');
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
            $('#tabs').tabs();
        });
        
        </script>
    <title></title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
</head>
<body>
    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    </asp:ScriptManager>
    <div id="tabs">
	<ul>
		<li><a href="#tabs-1">年度预算调整</a></li>
		<li><a href="#tabs-2">月预算调整</a></li>
	</ul>
            <div id="tabs-1">
            <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 27px">
                        <span>
                            &nbsp;
                            <asp:Label ID="Label3" runat="server" Text="部 门:"></asp:Label>
                            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label4" runat="server" Text="科 目:"></asp:Label>
                            <asp:TextBox ID="TextBox6" runat="server"></asp:TextBox>
                            &nbsp;&nbsp;
                            <asp:Button ID="Button4" runat="server" Text="查 询" CssClass="baseButton" 
                                onclick="Button4_Click" />&nbsp;&nbsp;
                        </span>
                            </td>
                    </tr>
                    <tr>
                        <td>
                                <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="3" CssClass="myGrid" PageSize="17" Width="611px"
                                    AllowPaging="True" oneditcommand="myGrid_EditCommand" >
                                <Columns>
                                    <asp:TemplateColumn HeaderText="选择">
                                        <ItemTemplate>
                                            &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" Width="38px" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="xmmc" HeaderText="预算过程" DataFormatString="{0:F2}">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="deptcode" HeaderText="部门编号">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="deptname" HeaderText="部门名称">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="yskmcode" HeaderText="预算科目编号" 
                                        DataFormatString="{0:D}">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="yskmmc" HeaderText="预算科目名称" 
                                        DataFormatString="{0:D}">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="预算金额">
                                        <ItemTemplate>
                                            &nbsp;<asp:TextBox ID="TextBox2" runat="server" Text='<%# bind("ysje") %>' CssClass="baseText"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" Width="38px" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:TemplateColumn>

                                    <asp:BoundColumn DataField="billcode" HeaderText="billcode" Visible="False"></asp:BoundColumn>
                                    <asp:ButtonColumn ButtonType="LinkButton" Text="修改" CommandName="Edit"></asp:ButtonColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px">
                            &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                            <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                            <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                            <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                            第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                            </asp:DropDownList>页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                            <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                                runat="server"></asp:Label>条</td>
                    </tr>
                    <tr style="display:none;">
                        <td style="height: 30px">
                            &nbsp;</td>
                    </tr>
                    <tr>
                        <td style="height: 30px">
                        </td>
                    </tr>
                    </table>
              </div>
              <div id="tabs-2">
               <table cellpadding="0" cellspacing="0" width="100%">
                    <tr>
                        <td style="height: 27px">
                        <span>
                            &nbsp;<asp:Label ID="Label1" runat="server" Text="部 门:"></asp:Label>
                            &nbsp;<asp:TextBox ID="TextBox3" runat="server" CssClass="baseText"></asp:TextBox>
                            &nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
                            <asp:Label ID="Label2" runat="server" Text="科 目"></asp:Label>
                            &nbsp;<asp:TextBox ID="TextBox5" runat="server" CssClass="baseText"></asp:TextBox>
                            <asp:Button ID="Button1" runat="server" Text="查 询" CssClass="baseButton" 
                                onclick="Button1_Click" />
                            
                         </span>
                         </td>
                    </tr>
                    <tr>
                        <td>
                                <asp:DataGrid ID="DataGrid1" runat="server" AutoGenerateColumns="False" 
                                    CellPadding="3" CssClass="myGrid" PageSize="17" Width="611px"
                                    AllowPaging="True" oneditcommand="myGrid_EditCommand2" >
                                <Columns>
                                    <asp:TemplateColumn HeaderText="选择">
                                        <ItemTemplate>
                                            &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" Width="38px" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:TemplateColumn>
                                    <asp:BoundColumn DataField="xmmc" HeaderText="预算过程" DataFormatString="{0:F2}">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="deptcode" HeaderText="部门编号">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItemRight" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="deptname" HeaderText="部门名称">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="yskmcode" HeaderText="预算科目编号" 
                                        DataFormatString="{0:D}">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:BoundColumn DataField="yskmmc" HeaderText="预算科目名称" 
                                        DataFormatString="{0:D}">
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:BoundColumn>
                                    <asp:TemplateColumn HeaderText="预算金额">
                                        <ItemTemplate>
                                            &nbsp;<asp:TextBox ID="TextBox4" runat="server" Text='<%# bind("ysje") %>' CssClass="baseText"></asp:TextBox>
                                        </ItemTemplate>
                                        <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader" Width="38px" />
                                        <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                            Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                    </asp:TemplateColumn>

                                    <asp:BoundColumn DataField="billcode" HeaderText="billcode" Visible="False"></asp:BoundColumn>
                                    <asp:BoundColumn DataField="gcbh" HeaderText="gcbh" Visible="False">
                                    </asp:BoundColumn>
                                    <asp:ButtonColumn ButtonType="LinkButton" Text="修改" CommandName="Edit"></asp:ButtonColumn>
                                </Columns>
                            </asp:DataGrid>
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px">
                            &nbsp;<asp:LinkButton ID="lBtnFirstPage2" runat="server" OnClick="lBtnFirstPage2_Click">首 页</asp:LinkButton>
                            <asp:LinkButton ID="lBtnPrePage2" runat="server" OnClick="lBtnPrePage2_Click">上一页</asp:LinkButton>
                            <asp:LinkButton ID="lBtnNextPage2" runat="server" OnClick="lBtnNextPage2_Click">下一页</asp:LinkButton>
                            <asp:LinkButton ID="lBtnLastPage2" runat="server" OnClick="lBtnLastPage2_Click">尾页</asp:LinkButton>
                            第<asp:DropDownList ID="drpPageIndex2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex2_SelectedIndexChanged">
                            </asp:DropDownList>页 共<asp:Label ID="lblPageCount2" runat="server"></asp:Label>页
                            <asp:Label ID="lblPageSize2" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount2"
                                runat="server"></asp:Label>条</td>
                    </tr>
                    <tr style="display:none;">
                        <td style="height: 30px">
                        </td>
                    </tr>
                    <tr>
                        <td style="height: 30px">
                        </td>
                    </tr>
                    </table>
                
              </div>
           </div>
    </form>
</body>
</html>
