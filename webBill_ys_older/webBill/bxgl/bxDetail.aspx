<%@ Page Language="C#" AutoEventWireup="true" CodeFile="bxDetail.aspx.cs" Inherits="webBill_bxgl_bxDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>一般费用报销单</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />

    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    <script language="javascript" type="text/javascript" src="../bxgl/toDaxie.js"></script>

    <script language="javascript" type="text/javascript" src="../Resources/jScript/jQuery/jquery1.32.min.js"></script>

    <script language="javascript" type="text/javascript" src="../bxgl/bxDetail.js"></script>
   
    <script language="javascript" type="Text/javascript">

        $().ready(function() {
            changeJkStatus();
            $("#txtHjjeDx").val(cmycurd($("#txtHjjeXx").val()));
            $("#txtHjse").val($("#txtHjjeXx").val() * 0.17);
            if ($("#lblType").html() == "look") {
                $("#upLoadFiles").css("display", "none");
                $("#btnScdj").css("display", "none");
                $("#btnAddFykm").css("display", "none");
                $("#btnDelFykm").css("display", "none");
                $("#btnAddFysq").css("display", "none");
                $("#btnDelteFysq").css("display", "none");
            }
        });
            
            function openFysqDetail(billCode)
            {
                //showModalDialog 显示的页面不刷新，加随机数即可实现随时刷新 
                window.showModalDialog('../fysq/sqDetail.aspx?type=look&billCode='+billCode+'' , 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:843px;status:no;scroll:yes');
            }
            
            function confrimInput()
            {
                if(document.getElementById("txtHjjeXx").value=="")
                {
                    alert("请增加报销科目明细！");
                    return false;
                }
                else if(document.getElementById("txtHjjeXx").value=="0.00")
                {
                    alert("请输入报销科目金额！");
                    return false;
                }
            }
            
            function AddFysq()
            {
                var billCode=$("#lblBillCode").html();
                var returnValue=window.showModalDialog('selectCgsp.aspx?billCode='+billCode+'', 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:860px;status:no;scroll:yes');
                
                if(returnValue==undefined||returnValue=="")
                {}
                else
                {
                    document.getElementById("btnRefresh").click();
                }
            }
            
            function openLookShgc(openUrl)
            {
                window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:880px;status:no;scroll:yes');
            }
            
            function SetDept(mxGuid)
            {
                var returnValue=window.showModalDialog('ybbxSetDept.aspx?mxGuid='+mxGuid, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:460px;status:no;scroll:yes');
                document.getElementById("btnRefresh").click();
            }
            function SetYskm(mxGuid)
            {
                var dept=document.getElementById("txtDept").value;
                var returnValue=window.showModalDialog('ybbxSetYskm.aspx?mxGuid='+mxGuid+'&deptCode='+dept, 'newwindow', 'center:yes;dialogHeight:600px;dialogWidth:460px;status:no;scroll:yes');
                document.getElementById("btnRefresh").click();
            }
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
                                <input id="txtBxr" readonly="readonly" style="width: 148px" type="text" runat="server" /><asp:Button
                                    ID="btnSelectBxr" runat="server" CausesValidation="False" Text="选择" CssClass="baseButton" />
                                    </td>
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
                                <asp:Button ID="btnScdj" runat="server" CausesValidation="False" Text="上传单据" CssClass="baseButton"
                                    OnClick="btnScdj_Click" /><br />
                                <div id="divBxdj" runat="server">
                                </div>
                            </td>
                        </tr>
            <tr>
                <td style="background-color: red; height: 4px" colspan="9">
                </td>
            </tr>
            <tr>
                <td colspan="2" class="tableBg2">
                    费用科目</td>

                <td colspan="7" style="text-align: right">
                    <input type="button" value="选择费用" id="btnAddFykm" runat="server" class="baseButton" onclick="AddYskm();" />&nbsp;<input
                        type="button" value="取消费用" id="btnDelFykm" runat="server" class="baseButton" onclick="DeleteYskm();" />&nbsp;</td>
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
            <tr style="display: none;">
                <td style="background-color: red; height: 4px" colspan="9">
                </td>
            </tr>
            <tr style="display: none;">
                <td colspan="2" class="tableBg2">
                    使用借款抵扣</td>
                <td colspan="7">
                    <asp:RadioButton ID="rdoJkdk0" runat="server" Checked="True" GroupName="Jkdk" Text="否" /><asp:RadioButton
                        ID="rdoJkdk1" runat="server" GroupName="Jkdk" Text="是" /></td>
            </tr>
            <tr class="sfjk" style="display: none;">
                <td colspan="9" style="text-align: right">
                    <input type="button" runat="server" value="增加借款" class="baseButton" onclick="AddJkmx();" />&nbsp;<input
                        type="button" value="删除借款" class="baseButton" onclick="deleteJkmx();" />&nbsp;</td>
            </tr>
            <tr class="sfjk" style="display: none;">
                <td class="tableBg2" style="text-align: center">
                    选择</td>
                <td colspan="2" class="tableBg2" style="width: 200px">
                    借款类型</td>
                <td class="tableBg2">
                    借款金额</td>
                <td class="tableBg2" colspan="3" style="text-align: center">
                    借款日期</td>
                <td class="tableBg2" colspan="2">
                    详细信息</td>
            </tr>
            <div id="divJkdk" runat="server">
            </div>
            <tr class="sfjk" style="display: none;">
                <td colspan="2" class="tableBg2">
                    单位应退</td>
                <td colspan="2" style="width: 200px">
                    <asp:Label ID="lblYtje" runat="server" Text="0.00" Style="display: none;"></asp:Label>
                    <input type="text" id="txtytje" runat="server" readonly="readonly" style="border: 0px" />
                </td>
                <td colspan="3" class="tableBg2">
                    个人应补</td>
                <td colspan="2">
                    <asp:Label ID="lblYbje" runat="server" Text="0.00" Style="display: none;"></asp:Label>
                    <input type="text" id="txtYbje" runat="server" readonly="readonly" style="border: 0px" />
                </td>
            </tr>
            <tr>
                <td style="background-color: red; height: 4px" colspan="9">
                </td>
            </tr>
            <tr>
                <td colspan="2" class="tableBg2">
                    费用申请</td>
                <td colspan="7" style="text-align: right">
                    <input id="btnAddFysq" runat="server" type="button" value="附件申请" class="baseButton"
                        onclick="AddFysq();" />&nbsp;<asp:Button ID="btnDelteFysq" Text="取消单据" CssClass="baseButton"
                            runat="server" OnClick="btnDelteFysq_Click" />
                    <asp:Button ID="Button1" Text="单据信息" CssClass="baseButton" runat="server" OnClick="Button1_Click" />&nbsp;</td>
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
                <asp:Button ID="btn_Save" runat="server" OnClientClick="return confrimInput();" OnClick="btn_Save_Click" Text="保 存" CssClass="baseButton" />
                &nbsp; &nbsp; &nbsp; &nbsp;
                <asp:Button ID="btn_Cancle" runat="server" CausesValidation="False" OnClick="btn_Cancle_Click"
                    Text="关 闭" CssClass="baseButton" /></td>
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
                <%--<input type="button" id="btnRefresh" value="刷新脚本数据" onclick="refreshInfo();" />--%>
                <asp:Button ID="btnRefresh" runat="server" Text="刷新脚本数据" OnClick="btnRefresh_Click1" />
                        <input type="button" id="btn_Print" runat="server" value="打印预览" CssClass="baseButton" onClick="prn1_preview()" /></td>
        </tr>
        </table>
    </form>
</body>
</html>
