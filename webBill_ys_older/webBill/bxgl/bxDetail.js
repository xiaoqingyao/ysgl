    function refreshInfo()
    {
        var billCode=document.getElementById("lblBillCode").innerHTML;
        var type=document.getElementById("lblType").innerHTML;
            var info=webBill_bxgl_bxDetail.bindJsInfoJson(billCode,type).value;
            document.getElementById("divBxdj").innerHTML=info[0];
            document.getElementById("divFykm").innerHTML=info[1];
            document.getElementById("divJkdk").innerHTML=info[2];
//            $("#divBxdj").html(info[0]);
//            $("#divFykm").html(info[1]);
//            $("#divJkdk").html(info[2]);
            
            var YtybArr=info[3].split(',');
            //document.getElementById("lblYtje").innerHTML=YtybArr[0];
            //document.getElementById("lblYbje").innerHTML=YtybArr[1]; 
            $("#txtytje").val(YtybArr[0].toString()); 
            $("#txtYbje").val(YtybArr[1].toString());
//            $("#lblYtje").html(YtybArr[0]);
//            $("#lblYbje").html(YtybArr[1]);
    }
    
    function selectry(openUrl)
    {
        var str=window.showModalDialog(openUrl, 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:750px;status:no;scroll:yes');
        if(str!=undefined&&str!="")
        { 
            var dept = webBill_bxgl_bxDetail.getUserInfo(str).value;
            document.getElementById("txtDept").value=dept[0].toString();
            document.getElementById("txtbxdept").value=dept[1].toString();
             document.getElementById("txtBxr").value=str;
             
             $("#btnSelectBxr").css("display","none");
        }
    }
    //是否借款
    function changeJkStatus()
    {
        if($("#rdoJkdk1").attr("checked")==true)//借款
        {
            $(".sfjk").each(function(){
                $(this).css("display","");
            });
        }
        else
        {
            $(".sfjk").each(function(){
                $(this).css("display","none");
            });
        }
    }
    
    
    
    function deleteBxdj(djGuid)
    {
        var returnValue=webBill_bxgl_bxDetail.DeleteBxdj(djGuid).value;
        if(returnValue==true)
        {
            document.getElementById("btnRefresh").click();
        }
    }
    
    function AddYskm()//增加科目
    {
        var deptCode=document.getElementById("txtDept").value;
        if(deptCode=="")
        {
            alert('请选择报销人(单位)！');
            return;
        }
        var sqrq=document.getElementById("txtSqrq").value;
        if(sqrq=="")
        {
            alert('请输入申请日期！');
            return;
        }
        var billCode=document.getElementById("lblBillCode").innerHTML;
        openKm3(sqrq,deptCode,billCode);
    }
    function openKm3(sqrq,deptCode,billCode)
    {
        var str=window.showModalDialog('selectYskm.aspx?sqrq='+sqrq+'&deptCode='+deptCode+'&billCode='+billCode+'', 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
        if(str!=undefined&&str!="")
        { 
            document.getElementById("btnRefresh").click();//刷新数据
        }
    }
    function openKm(billCode)
    {
        var str=window.showModalDialog('selectYskm.aspx?billCode='+billCode+'', 'newwindow', 'center:yes;dialogHeight:520px;dialogWidth:300px;status:no;scroll:yes');
        if(str!=undefined&&str!="")
        { 
            document.getElementById("btnRefresh").click();//刷新数据
        }
    }
    function DeleteYskm()
    {
        var billCode=document.getElementById("lblBillCode").innerHTML;
        var fykmGuid="";
        $(".fykmMx").each(function(){
            if($(this).attr("checked")==true)//选中
            {
                fykmGuid+=$(this).val()+",";
            }
        });
        var returnValue=webBill_bxgl_bxDetail.DeleteFykmMx(fykmGuid,billCode).value;
        if(returnValue==true)
        {
            calYtYb();//计算应退应补
            document.getElementById("btnRefresh").click();//刷新数据
        }
        else
        {
            alert("删除费用科目失败！");
        }
    }
    
    
    function AddFT(kmmxGuid)
    {
        var billCode=document.getElementById("lblBillCode").innerHTML;
        var returnValue=webBill_bxgl_bxDetail.AddFtMxb(billCode,kmmxGuid).value;
        if(returnValue!="")
        { 
            alert(returnValue);
        }
        else
        {
            document.getElementById("btnRefresh").click();//刷新数据
        }
    }
    
    function DeleteFT(mxGuid)
    {
        var billCode=document.getElementById("lblBillCode").innerHTML;
        var fykmGuid="";
        $("."+mxGuid).each(function(){
            if($(this).attr("checked")==true)//选中
            {
                fykmGuid+=$(this).val()+",";
            }
        });
        var returnValue=webBill_bxgl_bxDetail.DeleteFyFTMx(fykmGuid,billCode).value;
        if(returnValue==true)
        {   
            calYtYb();
            document.getElementById("btnRefresh").click();//刷新数据
            //计算应退应补
        }
        else
        {
            alert("删除费用分摊失败！");
        }
    }
    //计算合计并更新数据库
    function onInputChange()
    {
        var billCode=document.getElementById("lblBillCode").innerHTML;
        var arr=new Array();
        var hj=0;
        $(".fykm").each(function(){
            arr.push($(this).attr("id"));
            arr.push($(this).val());
            try
            {
                hj+=parseFloat($(this).val());
            }
            catch(e)
            {}
        });

        $(".fyse").each(function() {
            try {
                hj += parseFloat($(this).val());
            }
            catch (e)
            { }
        });
        
        var returnValue=webBill_bxgl_bxDetail.UpdateFyftMxb(arr,billCode).value;
        
        if(returnValue==true)
        {
            $("#txtHjjeXx").val(webBill_bxgl_bxDetail.ToDoubleFormate(hj).value); toDaxieInfo();
        }
        else
        {
            alert("计算合计费用失败！");
        }
    }

    //更新税额
    function onSeChange() {
        var billCode = document.getElementById("lblBillCode").innerHTML;
        var arr = new Array();
        var hj = 0;
        $(".fyse").each(function() {
            arr.push($(this).attr("id"));
            arr.push($(this).val());
            try {
                hj += parseFloat($(this).val());
            }
            catch (e)
            { }
        });

        $(".fykm").each(function() {
            try {
                hj += parseFloat($(this).val());
            }
            catch (e)
            { }
        });
        
        var returnValue = webBill_bxgl_bxDetail.UpdateFySe(arr, billCode).value;

        if (returnValue == true) {
            $("#txtHjjeXx").val(webBill_bxgl_bxDetail.ToDoubleFormate(hj).value); toDaxieInfo();
        }
        else {
            alert("计算税额失败！");
        }
    }
    
    //计算
    
    function onInputChangeMoneyDept(mxGuid)
    {
        var returnValue=webBill_bxgl_bxDetail.ChangeMoneyDept(mxGuid,document.getElementById("txt"+mxGuid).value).value;
        
        if(returnValue==true)
        {
            
        }
        else
        {
            alert("单位使用金额设置失败！");
        }
    }
    function onInputChangeMoneyXm(mxGuid)
    {
        var returnValue=webBill_bxgl_bxDetail.ChangeMoneyXm(mxGuid,document.getElementById("txt"+mxGuid).value).value;
        
        if(returnValue==true)
        {
            
        }
        else
        {
            alert("项目使用金额设置失败！");
        }
    }
    
    //选择成本中心
    function changeSelect(obj,ftmxGuid)
    {
        var id=$(obj).attr("id");
        var val=document.getElementById(id).options[document.getElementById(id).selectedIndex].value
        
        var returnValue=webBill_bxgl_bxDetail.UpdateFtmxCbzx(ftmxGuid,val).value;
        if(returnValue=="")
        {
        
        }
        else
        {
            alert(returnValue);
        }
    }
    
    function toDaxieInfo()
    {
        $("#txtHjjeDx").val(cmycurd($("#txtHjjeXx").val()));
    }
    
    //增加借款
    function AddJkmx()
    {
        var billCode=document.getElementById("lblBillCode").innerHTML;
        var userCode=document.getElementById("txtBxr").value;
        if(userCode=="")
        {
            alert('请选择报销人！');
            return;
        }
        openJkmx(billCode,userCode);
        calYtYb();
    }
    function openJkmx(billCode,userCode)
    {
        var str=window.showModalDialog('selectJkmx.aspx?billCode='+billCode+'&userCode='+userCode+'', 'newwindow', 'center:yes;dialogHeight:420px;dialogWidth:450px;status:no;scroll:yes');
        if(str!=undefined&&str!="")
        { 
            calYtYb();//计算应退应补
            document.getElementById("btnRefresh").click();//刷新数据
        }
    }
    
    function deleteJkmx()
    {
        var dkGuid="";
        $(".jkmxList").each(function(){
            if($(this).attr("checked")==true)//选中
            {
                dkGuid+=$(this).val()+",";
            }
        });
        var returnValue=webBill_bxgl_bxDetail.DeleteJkmxInfo(dkGuid).value;
        if(returnValue==true)
        {
            calYtYb();//计算应退应补
            document.getElementById("btnRefresh").click();//刷新数据
        }
        else
        {
            alert("删除借款抵扣失败！");
        }
    }
    
    //计算应退应补
    function calYtYb()
    {
        var billCode=document.getElementById("lblBillCode").innerHTML;
        var returnValue=webBill_bxgl_bxDetail.CaluateYtYb(billCode).value;
        var arr=returnValue.split(',');
        //document.getElementById("lblYtje").innerHTML=arr[0]; 
        $("#txtytje").val(arr[0].toString());
        //document.getElementById("lblYbje").innerHTML=arr[1]; 
        $("#txtYbje").val(arr[1].toString());
        
    }
    
    