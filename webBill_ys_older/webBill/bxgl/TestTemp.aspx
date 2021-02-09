<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TestTemp.aspx.cs" Inherits="webBill_bxgl_TestTemp" %>


<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>一般费用报销单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    
    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js" type="text/javascript" charset="UTF-8"></script>

    <style type="text/css">
        .style1
        {
            background-color: #EDEDED;
            text-align: center;
        }

    </style>
    <script type="text/javascript">
        $(function() {
            $("#fykm").accordion({ fillSpace: true });
        });
    </script>
</head>
<body style="background-color: #EBF2F5;"> 
    <form id="form1" runat="server">
        <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
            <tr>
                <td style="text-align: center; height: 26px;">
                    <strong>一 般 费 用 报 销 单</strong></td>
            </tr>
            <tr>
                <td style="text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="900">
                        <tr>
                            <td colspan="2" class="tableBg2">
                                经办人</td>
                            <td colspan="2" style="width: 200px">
                                <asp:TextBox ID="txtJbr" runat="server" ReadOnly="True"></asp:TextBox></td>
                            <td class="tableBg2">
                                报销人</td>
                            <td colspan="4">
                                <input id="txtBxr" style="width: 148px" type="text" runat="server" /></td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tableBg2">
                                所在部门</td>
                            <td colspan="2" >
                                <input id="txtDept" readonly="readonly" style="width: 95%" type="text" runat="server" /><input id="txtbxdept" readonly="readonly" style=" display:none; width:1px" type="text" runat="server" />
                                </td>
                            <td class="tableBg2">
                                申请日期</td>
                            <td colspan="4">
                                <asp:TextBox ID="txtSqrq" runat="server"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tableBg2">
                                报销明细类型</td>
                            <td colspan="2" style="width: 200px">
                                <asp:DropDownList ID="drpBxmxlx" runat="server">
                                </asp:DropDownList></td>
                            <td class="tableBg2">
                                报销摘要</td>
                            <td colspan="4">
                                <asp:TextBox ID="txtBxzy" runat="server" Width="97%"></asp:TextBox></td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tableBg2">
                                报销说明</td>
                            <td colspan="10">
                                <asp:TextBox ID="txtBxsm" runat="server" TextMode="MultiLine" Width="98%"></asp:TextBox>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="2" class="tableBg2">
                                报销单据</td>
                            <td colspan="7">
                                <input type="File" runat="server" id="upLoadFiles" style="width: 401px" class="baseButton" />
                                <asp:Button ID="btnScdj" runat="server" CausesValidation="False" Text="上传单据" CssClass="baseButton" /><br />
                                <div id="divBxdj" runat="server">
                                </div>
                            </td>
                        </tr>
            <tr>
                <td style="background-color: red; height: 4px" colspan="9">
                </td>
            </tr>
            <tr>
                <td class="tableBg2" colspan="2">
                    归口预算</td>
                <td colspan="2">
                    <asp:RadioButton ID="rb_ok" runat="server"  Text="是" GroupName="is_gk" />
                    <asp:RadioButton ID="rb_can" runat="server" Text="否" GroupName="is_gk" Checked="true" />
                </td>
                <td colspan="5" id="gkbm">
                    <div id ='dv_gk'><input class="basehidden" type='text' id='txt_gk'/></div>
                </td>
            </tr>
            <tr>
                <td colspan="2" class="tableBg2">
                    费用科目</td>
                   <td colspan="6">
                        <div id="fykm" style="width:500px; text-align:left;">

				                 
				                 <H3><A href="#">[0102]折旧费</A></H3>
<DIV>
<UL>
<LI style="HEIGHT: 36px">[金额]<INPUT id=je0 type=text> </LI>
<LI style="HEIGHT: 36px">[税额]<INPUT id=se0 type=text></LI>
<LI>
<DIV>[使用部门] <ul><li>财务部</li><li>生产部</li></ul></DIV></LI>
<LI>
<DIV>[科目项目]</DIV></LI></UL></DIV>
                        </div>
                    </td>
                <td colspan="1" style="text-align: right">
                    <input type="button" value="选择费用" id="btnAddFykm" runat="server" class="baseButton"  />&nbsp<input
                        type="button" value="取消费用" id="btnDelFykm" runat="server" class="baseButton" />&nbsp;</td>
            </tr>
            <span id="divFykm" runat="server"></span>
            <tr class="fykmRow">
                <td colspan="2" class="tableBg2">
                    合计金额小写</td>
                <td colspan="2" style="width: 200px">
                    <input type="text" id="txtHjjeXx" runat="server" readonly="readonly" style="width: 226px;
                        text-align: right; background-color: #cccccc;" />
                </td>

                <td colspan="2" class="tableBg2" style="width: 200px">
                    合计金额大写</td>
                <td colspan="3">
                    <input type="text" id="txtHjjeDx" runat="server" readonly="readonly" style="background-color: #cccccc" /></td>
            </tr>
           
            <tr>
                <td style="background-color: red; height: 4px" colspan="9">
                </td>
            </tr>
            <tr>
                <td colspan="2" class="style1">
                    费用申请</td>
                <td colspan="7" style="text-align: right">
                    <input id="btnAddFysq" runat="server" type="button" value="附件申请" class="baseButton"
                        onclick="AddFysq();" />&nbsp;<asp:Button ID="btnDelteFysq" Text="取消单据" CssClass="baseButton"
                            runat="server"  />
                    <asp:Button ID="Button2" Text="单据信息" CssClass="baseButton" runat="server" />&nbsp;</td>
            </tr>
            <tr class="cgspInfo" runat="server" id="cgspInfo">
                <td colspan="9">
                    <asp:DataGrid ID="myGrid" runat="server" AllowSorting="True" AutoGenerateColumns="False"
                        BorderWidth="1px" CellPadding="3" CssClass="myGrid" ItemStyle-HorizontalAlign="Center"
                        PageSize="17" Width="100%">
                        <PagerStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px" Mode="NumericPages"
                            Position="Top" Visible="False" />
                        <ItemStyle HorizontalAlign="Center" />
                        <Columns>
                            <asp:TemplateColumn HeaderText="选择">
                                <ItemTemplate>
                                    &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                                </ItemTemplate>
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Width="38px"
                                    Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:TemplateColumn>
                            <asp:BoundColumn DataField="billCode" HeaderText="billCode" Visible="False"></asp:BoundColumn>
                            <asp:BoundColumn DataField="cgDept" HeaderText="采购单位">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cbr" HeaderText="承办人">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="sj" DataFormatString="{0:D}" HeaderText="申请日期">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cglb" HeaderText="申请类别">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="cgze" DataFormatString="{0:F2}" HeaderText="采购总额">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItemRight" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepID_ID" HeaderText="stepID_ID" Visible="False">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="sm" HeaderText="原因说明">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="stepID" HeaderText="单据状态">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                        </Columns>
                    </asp:DataGrid></td>
            </tr>
            
        </table>
        </td> </tr>
        <tr>
            <td style="text-align: center; height: 30px;">
                <asp:Button ID="btn_Save" runat="server" Text="保 存" CssClass="baseButton" />
                &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:Button ID="btn_Cancle" runat="server" CausesValidation="False" Text="关 闭" CssClass="baseButton" /></td>
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
                
                <asp:Button ID="btnRefresh" runat="server" Text="刷新脚本数据"  />
                        <input type="button" id="btn_Print" runat="server" value="打印预览" CssClass="baseButton" onClick="prn1_preview()" /></td>
        </tr>
        </table>
    </form>
</body>
</html>

