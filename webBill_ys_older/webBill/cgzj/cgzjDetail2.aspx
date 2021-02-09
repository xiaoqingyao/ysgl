<%@ Page Language="C#" AutoEventWireup="true" CodeFile="cgzjDetail2.aspx.cs" Inherits="webBill_cgzj_cgzjDetail2" %>

<%@ Register assembly="PaginationControl" namespace="PaginationControl" tagprefix="cc1" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>费用申请详细信息</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
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
                        var dggys = document.getElementById("<% =DGgys.ClientID%>");
                        for (i = 1; i < dggys.rows.length; i++) {
                                var checkbox = dggys.rows[i].cells[0].getElementsByTagName("INPUT")[0];
                                if (checkbox.checked == true) {
                                    var gysbh=dggys.rows[i].cells[1].innerHTML;
                                    var temp="<tr><td></td><td></td></tr>";
                                    $("#tb_list tbody").append();
                                }
                            }
                        }

                        $("#Button2").click();
                        $(this).dialog("close");
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
    
    
    <div id="dwxz" title="单位选择">
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
    <ContentTemplate>
        <asp:GridView ID="DGgys" runat="server" AutoGenerateColumns="False" CssClass="myGrid" ItemStyle-HorizontalAlign="Center" >
            <Columns>
               <asp:TemplateField HeaderText="选择">
                            <ItemTemplate>
                                <asp:CheckBox ID="CheckBox1" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                <asp:BoundField DataField="bh" HeaderText="供应商编号" />
                <asp:BoundField DataField="mc" HeaderText="供应商名称" />
            </Columns>
            <HeaderStyle CssClass="myGridHeader" />
        </asp:GridView>
        <cc1:PaginationToGV ID="PaginationToGV1" runat="server" ongvbind="PaginationToGV1_GvBind" />
        </ContentTemplate>
    </asp:UpdatePanel>
    </div>
        <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
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
                                计划单号</td>
                            <td>
                                <asp:Label ID="lblCgbh" runat="server" Text="No.201109120001"></asp:Label><asp:Button
                                    ID="Button1" runat="server" CssClass="baseButton" OnClick="Button1_Click" Text="生成编号"
                                    Visible="False" /></td>
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
                                计划人</td>
                            <td>
                                <asp:Label ID="Label4" runat="server" Text="[000001]采购蛇皮但单位" Style="display: none"></asp:Label>
                            </td>
                            <td class="tableBg">
                                计划摘要</td>
                            <td>
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
                                <asp:TextBox ID="TextBox4" runat="server"  ></asp:TextBox> 
                                <asp:TextBox ID="TextBox5" runat="server" style="display:none" ></asp:TextBox>
                                <asp:Button ID="Button2" runat="server" Text="Button" style="display:none" 
                                    onclick="Button2_Click" />    
                                    </td>
                        </tr>
                        <tr>
                            <td colspan="5">
                            <table width="100%" id="tb_list">
                            		<thead>
			                        <tr class="myGridHeader">
				                    <th scope="col">选择</th>
				                    <th scope="col">编号</th>
				                    <th scope="col">名称</th>
				                    <th scope="col">上月入库金额</th>
				                    <th scope="col">本月计划金额</th>
				                    <th scope="col">本月付款金额</th>
				                    <th scope="col">计划付款金额</th>
				                    <th scope="col">备注说明</th>
			                        </tr>
		                            </thead>
		                            <tbody>
		                            </tbody>
                            </table>
                                <asp:GridView ID="GridView1" runat="server">
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
