<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cgzjDetail.aspx.cs" Inherits="webBill_cgzj_cgzjDetail" %>

<%@ Register assembly="PaginationControl" namespace="PaginationControl" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>采购资金计划详细信息</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    
    <meta http-equiv ="pragma" content="no-cache"/>
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate"/>


    <link href="../Resources/Css/StyleSheet.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/StyleSheet2.css" type="text/css" rel="Stylesheet" />
    <link href="../Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet" type="text/css" />
    <script src="../Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>
    <script src="../Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js" type="text/javascript"></script>
    <script language="javascript" type="text/javascript" src="../Resources/jScript/calender.js"></script>

    

    <style type="Text/css">
        .right{
            text-align:right;
        }
        #txtCgrq
        {
            width: 82px;
        }
    </style>

    <script language="javascript" type="Text/javascript">
        $(function() {
            $('#dwxz').dialog({
                autoOpen: false,
                width: 600,
                buttons: {
                    "确定": function() {
                        if (document.all.DGgys != undefined) {
                            for (i = 1; i < document.all.DGgys.rows.length; i++) {
                                var cb = document.all.DGgys.rows(i).cells(0).children(0);
                                if (cb.checked) {
                                    temp0 = document.all.DGgys.rows(i).cells(1).innerText;
                                    temp1 = document.all.DGgys.rows(i).cells(2).innerText;
                                    if (document.getElementById("TextBox4").value == "") {
                                        document.getElementById("TextBox4").value = "'" + temp0 + "'";
                                        document.getElementById("TextBox5").value = temp0;
                                    }
                                    else {
                                        document.getElementById("TextBox4").value = document.getElementById("TextBox4").value + ",'" + temp0 + "'";
                                        document.getElementById("TextBox5").value = document.getElementById("TextBox4").value + "," + temp0;
                                    }

                                    cb.checked = false;
                                }
                            }
                            $(this).dialog("close");
                            $("#Button2").click();
                        }
                        else {
                            $(this).dialog("close");
                        }
                    },
                    "取消": function() {
                        $(this).dialog("close");
                    }
                }
            });
            $("#btn_close").click(function() {
                window.close();
            });
        });
        function editClick() {
            $("#dwxz").parent().appendTo($("form:first"));        
            $('#dwxz').dialog('open');
            return false;
        }

        function deleteBxdj(djGuid)
        {
            var returnValue=webBill_fysq_cgspDetail.DeleteBxdj(djGuid).value;
            if(returnValue==true)
            {
                document.getElementById("btnRefreshFj").click();
            }
        }
    </script>

</head>
<body style="background-color: #EBF2F5;">

    <form id="form1" runat="server">
    <asp:ScriptManager ID="ScriptManager1" runat="server">
    
    </asp:ScriptManager>
    
    
    <div id="dwxz" title="单位选择" style="display:none" >
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
    <span>
        <table>
        <tr>
        <td>编号:</td>
        <td>
            <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox> 
        </td>
        <td>
            名称:
        </td>
        <td>
            <asp:TextBox ID="TextBox2" runat="server"></asp:TextBox>
        </td>
        <td>
            <asp:Button ID="Button3" runat="server" Text="查 询" CssClass="baseButton" 
                onclick="Button3_Click" />
        </td>
        </tr>
        </table>
    </span>
        <asp:GridView ID="DGgys" runat="server" AutoGenerateColumns="False"  CssClass="ui-widget ui-widget-content" ItemStyle-HorizontalAlign="Center" >
            <Columns>
               <asp:TemplateField HeaderText="选择">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                <asp:BoundField DataField="bh" HeaderText="供应商编号" />
                <asp:BoundField DataField="mc" HeaderText="供应商名称" />
            </Columns>
            <HeaderStyle CssClass="ui-widget-header" />
        </asp:GridView>
        <cc1:PaginationToGV ID="PaginationToGV1" runat="server" ongvbind="PaginationToGV1_GvBind" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
        <table cellpadding="0" id="taball" cellspacing="0" width="100%">
            <tr>
                <td style="text-align: center; height: 27px;">
                    <strong><span style="font-size: 12pt">采购付款计划单</span></strong></td>
            </tr>
            <tr>
                <td style="height: 26px; text-align: center">
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable">
                        <tr>
                            <td class="tableBg">
                                计划日期</td>
                            <td>
                                <input id="txtCgrq" runat="server" type="text" />
                            </td>
                            <td class="tableBg">
                                计划人</td>
                            <td>
                            <asp:Label ID="lblUser" runat="server" Text=""></asp:Label>
                                </td>
                            <td class="tableBg">
                                计划类别</td>
                            <td>
                            <asp:DropDownList ID="ddl_cglb" runat="server">
                                </asp:DropDownList>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg">
                                <asp:Label ID="Label1" runat="server" Text="计划部门" ></asp:Label></td>
                            <td>
                                <asp:Label ID="lblDept" runat="server" Text="[000001]采购蛇皮但单位" Style="display: none"></asp:Label>
                                <asp:Label ID="lblDeptShow" runat="server" Text="[000001]采购蛇皮但单位"></asp:Label></td>

                            <td class="tableBg">
                                计划摘要</td>
                            <td colspan="3">
                                <asp:TextBox ID="txtBxzy" runat="server" Width="97%"></asp:TextBox>
                            </td>
                        </tr>
                          
                        <tr>
                            <td class="tableBg" rowspan="2">
                                付款单位
                            </td>
                            <td class="tableBg2" style="text-align: left" colspan="5">
                                <%--<input type="button" class="baseButton" value="增 加" onclick="opAddDetail();" />--%>
                                <input type="button" class="baseButton" value="增 加" onclick="return editClick()" />
                                <asp:Button ID="Button5" runat="server" CssClass="baseButton" OnClick="Button5_Click"
                                    Text="删 除" />
                                <asp:FileUpload ID="FileUpload1" runat="server"  />  
                                <asp:Button ID="Button4" runat="server" Text="上 传" CssClass="baseButton" 
                                    onclick="Button4_Click" />
                                <asp:TextBox ID="TextBox4" runat="server" style="display:none" ></asp:TextBox> 
                                <asp:TextBox ID="TextBox5" runat="server" style="display:none" ></asp:TextBox>
                                <asp:Button ID="Button2" runat="server" Text="Button" style="display:none" 
                                    onclick="Button2_Click" />    
                                </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" AllowSorting="True" 
                                    BorderWidth="1px" CellPadding="3" CssClass="myGrid" ItemStyle-HorizontalAlign="Center"
                                    PageSize="17" Width="100%">
                                    <PagerStyle BorderColor="Black" BorderStyle="Solid" BorderWidth="1px"  />
                                    <Columns>
                                        <asp:TemplateField HeaderText="选择">
                                        <ItemTemplate>
                                                &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" />
                                            </ItemTemplate>
                                            <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Width="30px"
                                                Wrap="False" />
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="编号">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Width="40px" Wrap="False" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="gysbh" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "gysbh") %>' Enabled="False"  Width="40px" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="供应商名称">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" Width="150px"/>
                                            <ItemTemplate>
                                                <asp:TextBox ID="gysmc" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "gysmc") %>' Enabled="False" Width="98%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="上月入库金额">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"  Width="80px"/>
                                            <ItemTemplate>
                                                <asp:TextBox ID="syrkje" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "syrkje") %>' Enabled="False" Width="98%" ></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="本月计划金额">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"  Width="80px" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="byjhje" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "byjhje") %>' Width="98%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="计划付款金额">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False"  Width="80px"/>
                                            <ItemTemplate>
                                                <asp:TextBox ID="byfkje" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "byfkje") %>' Width="98%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                        <asp:TemplateField HeaderText="备注说明">
                                        <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                                Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" Width="80px" />
                                            <ItemTemplate>
                                                <asp:TextBox ID="bz" runat="server" Text='<%# DataBinder.Eval(Container.DataItem, "bz") %>' Width="98%"></asp:TextBox>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                    </Columns>
                                </asp:GridView>
                                </td>
                        </tr>
                    </table>
                    
                </td>
            </tr>
            <tr>
                <td colspan="6" style="text-align: center; height: 37px;">
                    <asp:Button ID="btn_bc" runat="server" Text="保 存" CssClass="baseButton" OnClick="btn_bc_Click" />
                    &nbsp;
                    <input id="btn_close" type="button" value="取消" class="baseButton" />
                    </td>
            </tr>
            <tr style="display:none;">
                <td colspan="6" style="height: 37px; text-align: center">
                    <asp:Label ID="lbl_BillCode" runat="server"></asp:Label><asp:Label ID="Label2" runat="server"></asp:Label></td>
            </tr>
        </table>
    </form>
</body>
</html>
