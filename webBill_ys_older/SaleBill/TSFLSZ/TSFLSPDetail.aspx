<%@ Page Language="C#" AutoEventWireup="true" CodeFile="TSFLSPDetail.aspx.cs" Inherits="SaleBill_TSFLSZ_TSFLSPDetail" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>特殊返利设置（废弃的页面）</title> <meta http-equiv="X-UA-Compatible" content="IE=8" >
    <base target="_self" />
    <meta http-equiv="pragma" content="no-cache" />
    <meta http-equiv="Cache-Control" content="no-cache, must-revalidate" />
    <link href="../../webBill/Resources/Css/StyleSheet.css" rel="stylesheet" type="text/css" />

    <script src="../../webBill/bxgl/bxDetail.js" type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-1.4.2.min.js" type="text/javascript"></script>

    <link href="../../webBill/Resources/Css/jquery-ui-1.8.16.custom.css" rel="stylesheet"
        type="text/css" />
    <style type="text/css">
        .highlight
        {
            background: #EBF2F5;
        }
        .hiddenbill
        {
            display: none;
        }
    </style>

    <script src="../../webBill/Resources/jScript/jQuery/jquery-ui-1.8.16.custom.min.js"
        type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/jQuery/jquery.ui.datepicker-zh-CN.js"
        type="text/javascript"></script>

    <script src="../../webBill/Resources/jScript/calender.js" type="text/javascript"></script>

    <script language="javascript" type="Text/javascript">
//    var availableTagsfy=new Array();
    $(function() {

      
          //车辆类型
            $("#txtCartype").autocomplete({
                source: availableTags,
                select: function(event, ui) {
                    var carTypeCode=ui.item.value;
                    $("#lbmcar").text("车辆类型："+carTypeCode);
                }
                
            });
            
            //部门选择
            
              $("#txtdept").autocomplete({
                source: availableTagsdt,
                select: function(event, ui) {
                    var deptCode = ui.item.value;
                    $("#lblbm").text("部门："+deptCode);
                    $("#hdDeptCode").val(deptCode);
//                  
//                    alert($("#txtdept").text());
//                    //$("#btnHF").click();
//                     
//                    //根据部门改变费用类别的可选项
//                    var fylbsource='';
//                     $.post("../../webBill/MyAjax/GetYSKMByDept.ashx", { "deptCode":deptCode }, function(data, status) {
//                     alert(status == "success");
//                     alert(data);
//                        if (status == "success") {
//                           fylbsource=data;
//                           alert("赋值成功结果为");
//                           alert(fylbsource);
//                        }
//                    });
//                    alert("再次显示:"+fylbsource);
//                    if(fylbsource!=''){
//                        availableTagsfy=fylbsource;
//                    }
//                    alert(availableTagsfy);
                }
            });
            
            //费用类别
               $("#txtfylb").autocomplete({
              
                source: availableTagsfy,
                select: function(event, ui) {
              
                    var rybh = ui.item.value;
                   $("#lblfy").text("费用："+rybh);
                }
            });
            
             $("#btnAddFykm").click(function() {
               
                var gkbmbh = $("#txtdept").text();
               
                if (gkbmbh == "") {
                    alert("请先选择部门");
                    return;
                }
                openfy(gkbmbh);
            });
            
              $("#btcartype").click(function() {
             
                opencar();
            });
            
             $("#btn_choosedept").click(function() {
                openBm();
            });
    });
    
    
    //选择部门dvid
    
     
        function openBm() {
             var str = window.showModalDialog("../select/DeptSelct.aspx", 'newwindow', 'center:yes;dialogHeight:720px;dialogWidth:300px;status:no;scroll:yes');
           
            if (str != undefined && str != "") {
                var depts = str.split('|');
                var innerval = '';
                for (var i = 0; i < depts.length; i++) {
                    innerval += "<li><span>" + depts[i] + ":</span><input type='text' value='0.00' /></li>";
                }
                innerval += "";
                $("#txtdept").text(depts);
                $("#lblbm").text("部门："+depts);
            }
        }
        
        
         //选择车辆类型dvid
    
        function opencar() {
            var deptCode = "";
            var str = window.showModalDialog("../select/CarType.aspx", 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            alert(str);
            if (str != undefined && str != "") {
                var depts = str.split('|');
                
                var innerval = '';
                for (var i = 0; i < depts.length; i++) {
                    innerval += "<li><span>" + depts[i] + ":</span><input type='text' value='0.00' /></li>";
                }
                innerval += "";
              
                $("#txtCartype" ).text(depts);
                $("#lbmcar").text("车辆类型："+depts);
            }
        }
        
        
        
        //费用类别
        
          function openfy(deptCode) {
          
            var str = window.showModalDialog("../select/YSKMSelct.aspx?deptCode=" + deptCode, 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
            if (str != undefined && str != "") {
                var json = $.parseJSON(str);
               
                var inner = "";
                var checkArray = new Array();
                for (var i = 0; i < json.length; i++) {
                    if (str.indexOf(json[i].Yscode) < 0) {
                        inner += '<tr id="tr_' + kmIndex + '">';
                        inner += "<td>" + json[i].Yscode + "</td>";
                        inner += "<td>" + json[i].Ysje + "</td>";
                        inner += "<td>" + json[i].Syje + "</td>";
                        inner += '<td><input type="text" class="baseText ysje" onblur="htjeChange();" value="0.00" /></td>';
                        inner += '<td><input type="text" class="baseText ysse" onblur="htjeChange();" value="0.00" /></td>';
                        inner += "</tr>";
                        $("#td_dept").append('<ul id="bm_' + kmIndex.toString() + '"></ul>');
                        $("#td_xm").append('<ul id="xm_' + kmIndex.toString() + '"></ul>');
                        kmIndex++;
                    }
                    checkArray[i] = json[i].Yscode;
                   
                }
                  $("#txtfylb").val(checkArray);
                   $("#lblfy").text("费用："+checkArray);
            }
        }
        //替换非数字
          function replaceNaN(obj) {
                var objval=obj.value;
                if(objval.indexOf("-")==0){
                    objval=objval.substr(1);
                }
                if (isNaN(objval)) {
                    obj.value = '';
                    alert("必须用阿拉伯数字表示！");
                    }
            }

    </script>

    <style type="text/css">
        .style1
        {
            background-color: #EDEDED;
        }
        ul
        {
            list-style: none;
            margin-left: 0px;
            margin-top: 0px;
            padding-left: 5px;
        }
        ul li
        {
            margin: 5px 5px 5px 5px;
        }
        #tab_fykm
        {
            margin-top: 5px;
            margin-bottom: 5px;
        }
        #td_dept input
        {
            width: 100px;
        }
    </style>
</head>
<body style="background-color: #EBF2F5;">
    <form id="form1" runat="server">
    <div style="width: 500px; height: 600px;">
        <table cellpadding="0" id="taball" cellspacing="0" class="style1" width="100%">
            <tr>
                <td style="text-align: center; height: 36px;">
                    <strong>
                        <asp:Label ID="lbdjmc" runat="server" Text=""></asp:Label>
                    
                    </strong>
                </td>
            </tr>
            <tr>
                <td>
                    <table border="0" cellpadding="0" cellspacing="0" class="myTable" width="900">
                        <tr>
                            <%--<td class="tableBg2">
                            <input type="button" value="adf" style="display:none" onclick="javascript:alert(availableTagsfy);" />
                                有效日期起：
                            </td>
                            <td>
                                <asp:TextBox ID="txtSqrq1" runat="server"></asp:TextBox>
                            </td>
                            <td class="tableBg2">
                                有效日期止：
                            </td>
                            <td class="tableBg2">
                                <asp:TextBox ID="txtSqrq" runat="server"></asp:TextBox>
                            </td>--%>
                            <td class="tableBg2" colspan="2">
                                详细信息：
                            </td>
                            <td colspan="4">
                                <asp:Label ID="lbmcar" runat="server" Text="车辆类型："></asp:Label>
                                <asp:Label ID="lblbm" runat="server" Text="部门："></asp:Label>
                                <asp:Label ID="lblfy" runat="server" Text="费用选择："></asp:Label>
                            </td>
                        </tr>
                        <tr>
                            <td colspan="6" class="tableBg2">
                                <table class="baseTable" style="width: 100%; height: 100%">
                                    <tr>
                                        <td>
                                            车辆类型：
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="txtCartype" runat="server" Text=""></asp:Label>
                                          <%--  <input id="" style="width: 148px" type="text" runat="server" />--%>
                                        </td>
                                        <td style="display:none">
                                            <input type="button" value="车辆类型" id="btcartype" runat="server" class="baseButton" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            部门：
                                        </td>
                                        <td colspan="2">
                                            <asp:Label ID="txtdept" runat="server" Text=""></asp:Label>
                                          <%--  <input id="" style="width: 148px" type="text" runat="server"/>--%>
                                        </td>
                                        <td style=" display:none">
                                            <input type="button" value="选择部门" id="btn_choosedept" runat="server" class="baseButton" />
                                        </td>
                                    </tr>
                                    <tr>
                                        <td>
                                            费用类别：
                                        </td>
                                        <td>
                                            <asp:TextBox ID="txtfylb" runat="server"></asp:TextBox>
                                           <%-- <input id="" style="width: 148px" type="text" runat="server" />--%>
                                        </td>
                                        <td id="tdfylb" runat="server">
                                            <input type="button" value="选择费用" id="btnAddFykm" runat="server" class="baseButton" />
                                        </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                        <tr>
                            <td class="tableBg2" colspan="6">
                                <table class="baseTable" style="width: 100%; height: 100%">
                                    <tr>
                                        <td>
                                            <table style="width: 100%; height: 100%">
                                                <tr>
                                                    <td>
                                                        销售过程：
                                                    </td>
                                                    <td id="tdxsgadd" runat="server">
                                                        <asp:DataGrid ID="DataGrid1" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid" Width="100%"
                                                            ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                                                            ShowHeader="false">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:BoundColumn DataField="PName"></asp:BoundColumn>
                                                                <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtTc" runat="server" onkeyup="replaceNaN(this);"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </td>
                                                     <td id="tdxsgcview" runat="server">
                                                        <asp:DataGrid ID="DataGrid2" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid" Width="100%"
                                                            ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                                                            ShowHeader="false">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <Columns>
                                                                  <asp:BoundColumn DataField="SaleProcessCode"></asp:BoundColumn>
                                                                  <asp:BoundColumn DataField="Fee"></asp:BoundColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </td>
                                                </tr>
                                               
                                            </table>
                                        </td>
                                        <td>
                                            <table style="width: 100%; height: 100%">
                                                <tr>
                                                    <td>
                                                        配置明细：
                                                    </td>
                                                    <td id="tdpzadd" runat="server">
                                                       
                                                       <asp:DataGrid ID="Repeater1" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid" Width="100%"
                                                            ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                                                            ShowHeader="false">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:BoundColumn DataField="CName"></asp:BoundColumn>
                                                                <asp:TemplateColumn>
                                                                    <ItemTemplate>
                                                                        <asp:TextBox ID="txtpz" runat="server" onkeyup="replaceNaN(this);"></asp:TextBox>
                                                                    </ItemTemplate>
                                                                </asp:TemplateColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </td>
                                                      <td id="tdpzview" runat="server">
                                                       <asp:DataGrid ID="DataGrid3" runat="server" BorderWidth="1px" CellPadding="3" CssClass="myGrid" Width="100%"
                                                            ItemStyle-HorizontalAlign="Center" AllowSorting="True" AutoGenerateColumns="False"
                                                            ShowHeader="false">
                                                            <ItemStyle HorizontalAlign="Center" />
                                                            <Columns>
                                                                <asp:BoundColumn DataField="ControlItemCode"></asp:BoundColumn>
                                                              <asp:BoundColumn DataField="Fee"></asp:BoundColumn>
                                                            </Columns>
                                                        </asp:DataGrid>
                                                    </td>
                                                </tr>
                                                
                                            </table>
                                        </td>
                                    </tr>
                                    <tr>
                                        <td colspan="2">
                                            <asp:Button ID="btn_test" class="baseButton" runat="server" Text="确 定" OnClick="btn_test_Click" />
                                             <asp:Button ID="Button1" class="baseButton" runat="server" Text="取 消" OnClick="btn_cancel_Click" />
                                            <asp:HiddenField ID="hdDeptCode" runat="server" Value="" />
                                            
                                        </td>
                                    </tr>
                                    <tr style=" display:none">
                                    <td colspan="2">
                                         <asp:Button ID="btnHF" class="baseButton" runat="server" Text="" />
                                    </td>
                                    </tr>
                                </table>
                            </td>
                        </tr>
                    </table>
                </td>
            </tr>
        </table>
    </div>
    </form>
</body>
</html>
