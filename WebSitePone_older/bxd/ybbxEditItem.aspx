<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ybbxEditItem.aspx.cs" Inherits="bxd_ybbxEditItem"
    EnableViewState="false" %>

<!DOCTYPE>
<html>
<head id="Head1" runat="server">
    <title>一般报销明细信息添加页</title>
    <meta name="viewport" content="width=device-width, initial-scale=1.0, minimum-scale=1.0, maximum-scale=1.0, user-scalable=no">
    <!--<meta name="viewport" content="width=device-width, user-scalable=no ,target-densitydpi=device-dpi ,initial-scale= 0.5" />-->
    <meta charset="utf-8">
    <meta http-equiv="pragma" content="no-cache" />
    <link href="../js/jquery.mobile-1.3.2.min.css" rel="stylesheet" type="text/css" />

    <script src="../js/jquery-1.8.3.min.js" type="text/javascript"></script>

    <script src="../js/jquery.mobile-1.3.2.min.js" type="text/javascript"></script>

    <link href="../Css/CommenCss.css" rel="stylesheet" type="text/css" />
    <style>
        .div-yskm
        {
            margin-top: 10px;
        }
        .tab-yskm
        {
        }
        .div-hs
        {
            margin-top: 10px;
            border-top: 2px solid red;
            font-weight: 700;
            text-align: left;
        }
        .tab-hs
        {
            margin: 0px;
            padding: 0px;
        }
        .tab-hs
        {
        }
        .abc
        {
            width: 50%;
            display: inherit;
        }
    </style>

    <script type="text/javascript">
    
    
           

    
    var lastIndex,lastValue;

    function saveLast(){
        var select = document.getElementById("DropDownList1");
        lastIndex = select.selectedIndex;
        var midValue = select.options[lastIndex].value; 
        //var text = select.options[index].text;
        if(midValue!="0"){
        lastIndex = select.selectedIndex;
        lastValue = select.options[lastIndex].value;
    } 
    }
    
   $(function(){
          $(".div-hs,.div-yskm,.c-option").hide();  
          $("input[type='text']").one("focus",function(){
          $(this).val("");
          });
          
         $("#DropDownList1 option").each(function(index,obj){
             if( $(this).attr("islast")=="0")
             {
                 $(this).css("background-color","LightGray");
             }
         });
            
          
          //保存
          $("#btn_save").click(function(){
           var billCode= '<%=Request["billCode"] %>';
            if(billCode==undefined ||billCode==null ||billCode=="")
            {
                window.location.href="ybbxEditMain.aspx";
                return;
            }
             var json=jsonMaker(billCode,"add");
            if(json=="")
            return;
             $.post("ybbxBillSaveHandler.ashx",json,function(data,status){
                if(status=="success")
                {
                if(data=="1")
                {
               alert("保存成功！");
               window.location.href="ybbxList.aspx";
                }
               else if (data == "-2") {
                   alert("有预算超支了,不能保存!");
               } else if (data == "-3") {
                   alert("该报销类型需要附单据,请添加单据!");
               }
               else if (data == "-4") {
                   alert("修改单据日期不能跨月!");
               }
               else if (data == "-5") {
                   alert("该费用科目有部门核算，请选择使用部门!");
               }
               else if (data == "-6") {
                   alert("该费用科目有项目核算，请选择使用项目!");
               }
               else if (data == "-7") {
                   alert("该月份单据已做月结，不能再做本月份的单据!");
               } else if (data == "-8") {
                   alert("申请人必须包括在附加的申请单据内！");
               }
               else {
                   alert(data);
               }
               }
             });
          });         
          //保存并添加明细
           $("#btn_saveAndMx").click(function(){  
            var val= $(this).val();       
            var billCode= '<%=Request["billCode"] %>';
             var km=$("#DropDownList1").val();
            if(billCode==undefined ||billCode==null ||billCode=="")
            {
                window.location.href="ybbxEditMain.aspx";
                return;
            }
            var type='<%=Request["type"] %>';
            var json="";
            var url=location.href;
            if(type==undefined ||type==null ||type=="")
            {
             json=jsonMaker(billCode,"add");
             url=url+"&type=addmx";
            }
            else
            {
             json=jsonMaker(billCode,"addmx");
            }
            
             if(json=="")
             return;
             $.post("ybbxBillSaveHandler.ashx",json,function(data,status){
                if(status=="success")
                {
                if(data=="1")
                {
                if($("#btn_saveAndMx").val()=='保存并添加明细')
                window.location.href=url;
                else if(confirm("保存成功，是否继续添加？"))
                window.location.href=url;
                else
                window.location.href="ybbxList.aspx";
                }
               else if (data == "-2") {
                   alert("有预算超支了,不能保存!");
               } else if (data == "-3") {
                   alert("该报销类型需要附单据,请添加单据!");
               }
               else if (data == "-4") {
                   alert("修改单据日期不能跨月!");
               }
               else if (data == "-5") {
                   alert("该费用科目有部门核算，请选择使用部门!");
               }
               else if (data == "-6") {
                   alert("该费用科目有项目核算，请选择使用项目!");
               }
               else if (data == "-7") {
                   alert("该月份单据已做月结，不能再做本月份的单据!");
               } else if (data == "-8") {
                   alert("申请人必须包括在附加的申请单据内！");
               }
               else {
                   alert(data);
               }
               }
             });
           
           });
   });
   
   function jsonMaker(billCode,type)
   {
   
    //①剩余金额 和 填报金额 相比较（填报金额不能大于剩余金额）
      var kmcode=$("#DropDownList1").val();//$("table#yskm tr:eq(0)").find("td:eq(1)").text();
     var syje=$("table#yskm tr:eq(3)").find("td:eq(1)").text();
     var texts=$("table#yskm input[type='text']");
     var tbzje=parseFloat(texts.eq(0).val())+parseFloat(texts.eq(1).val());
     
     //如果
       if(tbzje==0)
       {
       texts.eq(0).focus;
       return "";
       }
     //判断是否必须要受控制 如果报销类型 对应的数据字典是要受控制 就比较填报金额 和报销金额 如果为 否 则 忽略该步的比较
     
     //②获取总核算部门金额
     var ret = '{"km":"' + kmcode + '","je":"' + ConvertNum(texts.eq(0).val()) + '","se":"' + ConvertNum(texts.eq(1).val()) + '","billcode":"'+billCode+'","type":"'+type+'","bm":[';
     var bmret='';
     var bmcount= $("table#yskm_dept tr").size();
     if(bmcount>2) 
     {
         $("table#yskm_dept tr:gt(0)").each(function(index,obj){
          if(index!=bmcount-2)
          {
         // var temp= $(this).find("input[type='text']");
         // bmret += '{"bmbh":"' +temp.eq(0).val() + '","bmje":"' + temp.eq(1).val() + '"},';
         var temp= $(this).find("td");
         var bmstr=temp.eq(1).text();
         bmstr=bmstr.substring(0,bmstr.length-1);
          bmret += '{"bmbh":"' +temp.eq(0).text() + '","bmje":"' + ConvertNum(bmstr) + '"},';
          }
         });
          bmret = bmret.substring(0, bmret.length - 1);
      }
       ret += bmret + '] ,"xm":[';
       
     //③获取总核算项目金额
       //如果项目核算为“是” 必须要有核算项目 如果为“否” 则不受控制
       var xmret = "";
       var xmcount= $("table#yskm_hsxm tr").size();
       if(xmcount>2) 
       {
             $("table#yskm_hsxm tr:gt(0)").each(function(index,obj){
             if(index!=xmcount-2 )
             {
             //var temp1= $(this).find("input[type='text']");
             // xmret += '{"xmbh":"' + temp1.eq(0).val() + '","xmje":"' +temp1.eq(1).val() + '"},'; 
             var temp1= $(this).find("td");
             var xmstr=temp1.eq(1).text();
             xmstr=xmstr.substring(0,xmstr.length-1);    
             xmret += '{"xmbh":"' + temp1.eq(0).text() + '","xmje":"' +ConvertNum(xmstr) + '"},'; 
             }
             });
              xmret = xmret.substring(0, xmret.length - 1);
       }
      ret += xmret + ']}';
      return ret;
     //④与填报金额相比较
   
   }
    var v="";
    function YskmChange()
    {
     if($("#DropDownList1").find("option:selected").attr("islast")=="0")
     { 
     alert("请选择末级科目");
     if(v!="")
        $("#DropDownList1 ").get(0).value = v;
     else
        $("#DropDownList1 ").get(0).selectedIndex=0;
     return;
     }
      $(".div-hs,.div-yskm,.c-option").hide();

     v= $("#DropDownList1").val();
      if(v!=undefined&&v!=null &&v!="")
      {
        var dept=$("#hfDept").val();
        var isgk=$("#hfIsgk").val();
        var billDate=$("#hfBillDate").val();
        var dydj=$("#hfDydj").val();
        
        $.post("GetYskmHandler.ashx",{"deptCode":dept,"kmcode":v,"isgk":isgk,"billDate":billDate,"dydj":dydj},function(data,status){
           if(status=="success")
           {   
               data=eval('('+data+')');
                var result="<table class='tab-yskm' id='yskm'>";
                result+="<tr><td  class='tdEnenRight tdborder' >费用科目：</td><td class='tdborder'>"+data.Yscode+"</td></tr>";
                result+="<tr><td  class='tdEnenRight tdborder'>预算金额：</td><td class='tdborder'>"+data.Ysje+"￥</td></tr>";
                // result+="<tr><td class='tdEnenRight'>销售提成金额：</td><td class='tdEnenRight'>"+data.Tcje+"￥</td></tr>";
                result+="<tr><td class='tdEnenRight tdborder'>剩余金额：</td><td class='tdborder'>"+data.Syje+"￥</td></tr>";
                result+="<tr><td class='tdEnenRight tdborder'>可用金额：</td><td class='tdborder'> "+data.Kyje+"￥</td></tr>";
                result+="<tr><td class='tdEnenRight tdborder'>核算部门：</td><td class='tdborder'>"+data.Bmhs+"</td></tr>";
                result+="<tr><td class='tdEnenRight tdborder'>核算项目：</td><td class='tdborder'>"+data.XiangMuHeSuan+"</td></tr>";
                result+="<tr><td class='tdEnenRight tdborder'>报销金额：</td><td class='tdborder' ><input type='text' data-role='none' class='myInputRight'  placeholder='0.00' onkeyup='replaceNaNNum(this)' /></td></tr>";
                result+="<tr><td class='tdEnenRight tdborder'>税额：</td><td class='tdborder'><input type='text' data-role='none' class='myInputRight'  placeholder='0.00' onkeyup='replaceNaNNum(this)'/></td></tr>";
                result+="</table>";
                $("#yskm").html(result);
                $(".div-hs,.div-yskm,.c-option").show();
                $("#yskm_dept tr:not(:first):not(:last)").remove();
                $("#yskm_hsxm tr:not(:first):not(:last)").remove();
           } 
        });
      }
    }
    
    
    
    function AddRow(obj)
    {
       var tr= $(obj).parentsUntil("tr").parent().eq(0);
//       alert(tr.size());
//       alert(tr.html());
//       tr.before("<tr><td>1</td><td>2</td><td>3</td></tr>");
      var v=tr.find("input");
      v1=v.eq(0).val();
      v2=v.eq(1).val();
      if(v1.length==0||v2.length==0)
      {
      return;
      }
      //tr.before("<tr><td><input type='text' disable='disable' class='myInput'  value='"+v1+"' readonly ></td><td><input type='text' class='myInputRight'  value='"+v2+"' readonly></td><td><input type='button' data-iconpos='notext' data-icon='delete' onclick='RemoveRow(this);'/></td></tr>");
      tr.before("<tr><td  class='mytdEnenRight tdborder'>"+v1+":</td><td class='mytdEnenRight tdborder'>"+v2+"￥</td><td  class='tdborder'><input type='button' data-iconpos='notext' data-icon='delete' onclick='RemoveRow(this);'/></td></tr>");
      tr.prev().find("td:eq(2)").trigger('create');
      tr.find("input[type='text']").val("");
      tr.prev().find("input[type='button']").eq(0).parent().css({"margin":0,"padding":0});
     
    }
    
    function RemoveRow(obj)
    {
     var tr= $(obj).parentsUntil("tr").parent().eq(0);
     tr.remove();
    }
    
    function IsReturn()
    {
    var url='../Index.aspx';
    var msg;
        if('<%=Request["type"] %>'=="addmx")
        {
            var con=$("#DropDownList1").val().length;
            if(con>0)
                msg="明细未保存，确定要返回吗?";
            else
            {
                ConfirmReturn(url,"");
                return;
            }
        }
        else
        {
            g="单据还未保存，确定要返回吗?";
        }
        ConfirmReturn(url,msg);
    }
    
   
    </script>

</head>
<body>
    <form id="form1" runat="server" data-role="page" data-theme="b" method="post" data-ajax="false">
    <div>
        <div data-role="header">
            <a data-icon="home" data-ajax="false" onclick="IsReturn()">返回</a>
            <h1>
                费用报销单→报销明细</h1>
        </div>
        <div data-role="content">
            <h3 style="text-align: left;">
                报销单号：<asp:Label ID="Label1" runat="server" Text=""></asp:Label></h3>
                <label style="color:Red;font-size:14px">【友情提示】:单据主表信息已保存。请添加子表信息。</label>
            <div class="div-left">
               <asp:DropDownList ID="DropDownList1" runat="server" onchange="YskmChange()" onclick="saveLast();">
                            </asp:DropDownList>
            </div>
            <div class="div-yskm" id="yskm">
            </div>
            <div class="div-hs">
                核算部门
                <table class="tab-hs" id="yskm_dept">
                    <tr>
                        <th>
                            部门
                        </th>
                        <th>
                            核算金额
                        </th>
                        <th>
                        </th>
                    </tr>
                    <tr>
                        <td class="tdborder">
                            <input type='text' value="" data-role="none" class="myInput" id="txt_bm" />
                            <ul id="ul_bm" data-role="listview" data-inset="true" data-theme="c" style="position: absolute;
                                z-index: 99999">
                            </ul>
                        </td>
                        <td class="tdborder">
                            <input type='text' value="" data-role="none" class="myInputRight" onkeyup='replaceNaNNum(this)' />
                        </td>
                        <td class="tdborder">
                            <input type="button" data-iconpos="notext" data-icon='plus' onclick="AddRow(this);" />
                        </td>
                    </tr>
                </table>
            </div>
            <div class="div-hs">
                核算项目
                <table class="tab-hs" id="yskm_hsxm">
                    <tr>
                        <th>
                            项目
                        </th>
                        <th>
                            核算金额
                        </th>
                        <th>
                        </th>
                    </tr>
                    <tr>
                        <td class="tdborder">
                            <input type='text' value="" data-role="none" class="myInput" id="txt_xm" />
                            <ul id="ul_xm" data-role="listview" data-inset="true" data-theme="c" style="position: absolute;
                                z-index: 99999">
                        </td>
                        <td class="tdborder">
                            <input type='text' value="" data-role="none" class="myInputRight" onkeyup='replaceNaNNum(this)' />
                        </td>
                        <td class="tdborder">
                            <input type="button" data-iconpos="notext" data-icon='plus' onclick='AddRow(this);'>
                        </td>
                    </tr>
                </table>
            </div>
            <div class="c-option">
                <input type="button" id="btn_save" runat="server" value="保存" data-inline="true" />
                <input type="button" id="btn_saveAndMx" runat="server" value="添加明细" data-inline="true" />
            </div>
            <asp:HiddenField ID="hfDept" runat="server" />
            <asp:HiddenField ID="hfIsgk" runat="server" />
            <asp:HiddenField ID="hfBillDate" runat="server" />
            <asp:HiddenField ID="hfDydj" runat="server" />
            <asp:HiddenField ID="hfBxlx" runat="server" />
        </div>
        <div data-role="footer" data-position="fixed">
             <footer data-role="footer" id="footer"><h1>©2015<a href="http://www.jnhxsoft.com/">行信科技</a> 全面预算-移动版</h1></footer>
        </div>
    </div>
    </form>

    <script>
         $("#form1").on("pageshow", function(e) {            
           var bmObj=$("#txt_bm");
           var bmulObj=$("#ul_bm");
           AutoComplicated(bmObj,bmulObj,"hsbm","");
           
           var billCode= '<%=Request["billCode"] %>';
           var xmObj=$("#txt_xm");
           var xmulObj=$("#ul_xm");
           AutoComplicated(xmObj,xmulObj,"hsxm",billCode);
        });
    </script>

    <script src="../js/Common.js" type="text/javascript"></script>

</body>
</html>
