<%@ Page Language="C#" AutoEventWireup="true" CodeFile="RebatesStandardlist.aspx.cs"
    Inherits="SaleBill_Flsz_RebatesStandardlist" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>一般返利设置</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/StyleSheet2.css" rel="stylesheet" type="text/css" />
    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
          var status = "none";
         $(function(){
          $("#<%=myGrid.ClientID%> tr").filter(":not(:has(table, th))").click(function() {
                $("#<%=myGrid.ClientID%> tr").removeClass("highlight");
                $(this).addClass("highlight");
            });
             //查询
            $("#btnSelect").click(function() {
                     $("#trSelect").toggle();
            });
           
            $("#btn_sx").click(function() {
       
            location.replace(location.href);
            });
        
         //取消
            $("#btn_cancle").click(function() {
                document.getElementById("trSelect").style.display = "none";
            });
            
              //车辆类型
            $("#txtcartype").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var carTypeCode = ui.item.value;
                    $("#lbmcar").text("车辆类型：" + carTypeCode);
                }

            });

            //部门选择

            $("#txtdept").autocomplete({
                source: availableTagsdt,
                select: function(event, ui) {
                    var deptCode = ui.item.value;
                    $("#lblbm").text("部门：" + deptCode);
                    $("#hdDeptCode").val(deptCode);


                    //$("#btnHF").click();

                    //根据部门改变费用类别的可选项
                    var fylbsource = '';
                    $.post("../../webBill/MyAjax/GetYSKMByDept.ashx", { "deptCode": deptCode }, function(data, status) {

                        if (status == "success") {
                            fylbsource = data;

                        }
                    });

                    if (fylbsource != '') {
                        availableTagsfy = fylbsource;
                    }

                }
            });

            //费用类别
            $("#txtfeetype").autocomplete({

                source: availableTagsfy,
                select: function(event, ui) {
                    var rybh = ui.item.value;
                  
                }
            });
            
            //费用控制
              $("#txtfeekz").autocomplete({

                source: availablekz,
                select: function(event, ui) {

                var rybh = ui.item.value;
                  
                }
            });
           
       });
        function openDetail(openUrl)
        {
            var returnValue=window.showModalDialog(openUrl , 'newwindow', 'center:yes;dialogHeight:500px;dialogWidth:920px;status:no;scroll:yes');
            
            if(returnValue==undefined||returnValue=="")
            {
                return false;
            }
            else
            {
                document.getElementById("btn_sx").click();
            }
        }
        
          function openDetailedit(openUrl)
        {
            var returnValue=window.showModalDialog(openUrl , 'newwindow', 'center:yes;dialogHeight:280px;dialogWidth:520px;status:no;scroll:yes');
            
            if(returnValue==undefined||returnValue=="")
            {
                return false;
            }
            else
            {
                document.getElementById("btn_sx").click();
            }
        }
          
              function SelectAll(aControl) 
               { 
                      var chk=document .getElementById("myGrid").getElementsByTagName("input");
                      for(var s=0;s<chk.length;s++)
                      {
                            chk[s].checked=aControl.checked;
                      }
                }   
//                
//        function submitData(oCheckbox)
//        {
//           var code=oCheckbox.name.substr(13,2);     
//           var gvList = document.getElementById("<%=myGrid.ClientID %>");
//           var yj;
//           yj=gvList.rows[code-1].cells[1].innerText;
//           for(i = 1;i < gvList.rows.length; i++)
//           {         
//                if( gvList.rows[i].cells[1].innerText>2)
//                {
//                   if(gvList.rows[i].cells[1].innerText.substr(0,2)==yj)
//                   {
//                    gvList.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked=oCheckbox.checked;
//                   }
//                }                            
//            }
//         }
         
         
         function gudingbiaotou() {
       var t = document.getElementById("<%=myGrid.ClientID%>");
            if(t==null||t.rows.length<1){
                return;
            }
            var t2 = t.cloneNode(true);
            t2.id = "cloneGridView";
            for (i = t2.rows.length - 1; i > 0; i--) {
                t2.deleteRow(i);
            }
            t.deleteRow(0);
            document.getElementById("header").appendChild(t2);
            var mainwidth = document.getElementById("main").style.width;
            mainwidth = mainwidth.substring(0, mainwidth.length - 2);
            mainwidth = mainwidth - 16;
            document.getElementById("header").style.width = mainwidth;
        }
 
       
    
    </script>

</head>
<body onload="gudingbiaotou();">
    <form id="form1" runat="server">
    <table cellpadding="0" cellspacing="0" width="100%">
        <tr>
            <td style="height: 27px">
                <input type="button" id="btn_sx" value="刷 新" class="baseButton" />
                <input type="button" id="btnSelect" value="查 询" class="baseButton" />
                <asp:Button ID="btn_add" runat="server" Visible="false" Text="增 加" CssClass="baseButton"
                    OnClick="btn_add_Click" />
                <asp:Button ID="btn_edit" runat="server" Text="修 改" CssClass="baseButton" OnClick="btn_edit_Click" />
                <asp:Button ID="btn_del" runat="server" Text="删 除" CssClass="baseButton" OnClick="btn_del_Click" />
                <asp:Button ID="btn_pf" runat="server" Text="确认批复" CssClass="baseButton" OnClick="btn_pf_Click" />
                <asp:Button ID="bt_jy" runat="server" Text="禁 用" CssClass="baseButton" OnClick="bt_jy_Click" />
                <asp:Button ID="bt_qy" runat="server" Text="启 用" CssClass="baseButton" OnClick="bt_qy_Click" />
                <asp:Button ID="Button1" runat="server" CssClass="baseButton" Text="导出Excel" OnClick="Button2_Click" />
            </td>
        </tr>
        <tr id="trSelect" style="display: none;">
            <td align="left">
                <div style="float:left">
                <table class="baseTable" style="text-align: left;">
                    <tr>
                        <td>
                            车辆类型：
                        </td>
                        <td>
                            <asp:TextBox ID="txtcartype" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            部门：
                        </td>
                        <td>
                            <asp:TextBox ID="txtdept" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            费用类别：
                        </td>
                        <td>
                            <asp:TextBox ID="txtfeetype" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            费用控制：
                        </td>
                        <td>
                            <asp:TextBox ID="txtfeekz" runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            类别：
                        </td>
                        <td>
                            <asp:DropDownList ID="txttype" runat="server" Width="120px">
                                <asp:ListItem Value="">全部</asp:ListItem>
                                <asp:ListItem Value="0">期初分配</asp:ListItem>
                                <asp:ListItem Value="1">销售提成</asp:ListItem>
                                <asp:ListItem Value="2">配置项</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            状态：
                        </td>
                        <td>
                            <asp:DropDownList ID="txtstatus" runat="server" Width="120px">
                                <asp:ListItem Value="">全部</asp:ListItem>
                                <asp:ListItem Value="0">禁用</asp:ListItem>
                                <asp:ListItem Value="1">未批复</asp:ListItem>
                                <asp:ListItem Value="2">已批复</asp:ListItem>
                            </asp:DropDownList>
                        </td>
                        <td>
                            有效期起：
                        </td>
                        <td>
                            <asp:TextBox ID="txtbgtime" runat="server" Width="120px"></asp:TextBox>
                        </td>
                        <td>
                            有效期止：
                        </td>
                        <td>
                            <asp:TextBox ID="txtedtime" runat="server" Width="120px"></asp:TextBox>
                        </td>
                    </tr>
                    <tr>
                        <td colspan="8" style="text-align: center">
                            <asp:Button ID="Button4" runat="server" Text="确 定" CssClass="baseButton" OnClick="btn_sel_Click" />
                            <input id="btn_cancle" value="取 消" class="baseButton" type="button" />
                        </td>
                    </tr>
                </table>
                </div>
            </td>
        </tr>
        <tr>
            <td align="left">
                <div id="header">
                </div>
                <div id="main" style="overflow-y: scroll; margin-top: -1px; width: 1400px; height: 390px;">
                    <asp:DataGrid ID="myGrid" runat="server" AutoGenerateColumns="False" CellPadding="3"
                        OnRowDataBound="thisTreeListView_OnRowDataBound" CssClass="myGrid" AllowPaging="True"
                        Style="table-layout: fixed" Width="100%" PageSize="17">
                        <Columns>
                            <asp:TemplateColumn ItemStyle-Width="45" HeaderStyle-Width="45">
                                <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                    />
                                <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                    Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                                <HeaderTemplate>
                                    <asp:CheckBox ID="chk_SelectedHeader" runat="server" AutoPostBack="False" onclick="javascript:SelectAll(this);"
                                        Text="全选" />
                                </HeaderTemplate>
                                <ItemTemplate>
                                    <asp:CheckBox ID="CheckBox1" runat="server" EnableViewState="false" />
                                </ItemTemplate>
                            </asp:TemplateColumn>
                            <%--<asp:TemplateColumn HeaderText="选择">
                            <ItemTemplate>
                                &nbsp;<asp:CheckBox ID="CheckBox1" runat="server" class="chkDataList" />
                            </ItemTemplate>
                            <HeaderStyle Font-Bold="True" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" HorizontalAlign="Center" Wrap="False" CssClass="myGridHeader"
                                Width="38px" />
                            <ItemStyle Font-Bold="False" Font-Italic="False" Font-Overline="False" Font-Strikeout="False"
                                Font-Underline="False" Wrap="False" CssClass="myGridItem" />
                        </asp:TemplateColumn>--%>
                            <asp:BoundColumn DataField="NID" HeaderText="编号" Visible="false" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="caname" HeaderText="车辆类型" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="160" HeaderStyle-Width="160">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="deptname" HeaderText="部门" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="120" HeaderStyle-Width="120">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="feename" HeaderText="费用类别" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="145" HeaderStyle-Width="145">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="feekz" HeaderText="费用控制" ItemStyle-HorizontalAlign="Left" ItemStyle-Width="145" HeaderStyle-Width="145">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Fee" HeaderText="费用" DataFormatString="{0:N2}" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SaleCountFrm" HeaderText="辆数起" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SaleCountTo" HeaderText="辆数止" ItemStyle-HorizontalAlign="Right">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="alltype" HeaderText="类别" ItemStyle-HorizontalAlign="Left">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="astatus" HeaderText="状态" ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="username" HeaderText="批复人" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100" HeaderStyle-Width="100">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="EffectiveDateFrm" HeaderText="有效日期起" DataFormatString="{0:D}"
                                ItemStyle-HorizontalAlign="Center">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="EffectiveDateTo" HeaderText="有效日期止" DataFormatString="{0:D}">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Center" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" HorizontalAlign="Center" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="Remark" HeaderText="备注">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="TruckTypeCode" HeaderText="车辆类型编号" Visible="false">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="true" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="DeptCode" HeaderText="部门编号" Visible="false">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                            <asp:BoundColumn DataField="SaleFeeTypeCode" HeaderText="费用类别编号" Visible="false">
                                <HeaderStyle CssClass="myGridHeader" Font-Bold="True" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" HorizontalAlign="Left" Wrap="False" />
                                <ItemStyle CssClass="myGridItem" Font-Bold="False" Font-Italic="False" Font-Overline="False"
                                    Font-Strikeout="False" Font-Underline="False" Wrap="False" HorizontalAlign="Left" />
                            </asp:BoundColumn>
                        </Columns>
                        <PagerStyle Visible="False" />
                    </asp:DataGrid>
                </div>
            </td>
        </tr>
        <tr>
            <td style="height: 10px">
                &nbsp;<asp:LinkButton ID="lBtnFirstPage" runat="server" OnClick="lBtnFirstPage_Click">首 页</asp:LinkButton>
                <asp:LinkButton ID="lBtnPrePage" runat="server" OnClick="lBtnPrePage_Click">上一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnNextPage" runat="server" OnClick="lBtnNextPage_Click">下一页</asp:LinkButton>
                <asp:LinkButton ID="lBtnLastPage" runat="server" OnClick="lBtnLastPage_Click">尾页</asp:LinkButton>
                第<asp:DropDownList ID="drpPageIndex" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpPageIndex_SelectedIndexChanged">
                </asp:DropDownList>
                页 共<asp:Label ID="lblPageCount" runat="server"></asp:Label>页
                <asp:Label ID="lblPageSize" runat="server"></asp:Label>条/页 共<asp:Label ID="lblItemCount"
                    runat="server"></asp:Label>条
            </td>
        </tr>
    </table>
    </form>
</body>
</html>
