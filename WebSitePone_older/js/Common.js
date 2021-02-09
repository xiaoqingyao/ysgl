
//分页2014-07-14 edit zyl
$(function(){
   $("#page a").each(function(){
      if($(this).next().is("strong")){
            $(this).hide();            
         }
      });
     $("#page a").live("click",function(){
          var url=$(this).attr("href");
         if($(this).find("span").size()==0)
         {
             window.location.href=url;
             return;
         }
         var num=$(this).find("span").eq(0).text();          
         $(this).parent().find("strong").remove();
         $(this).parent().find("a").show();
         $(this).after("<strong><span class='pc'>"+num+"</span></strong>");
         $(this).hide();
         num=parseInt(num)-1;
         if(url.indexOf("?")==-1)
         url=url+"?page="+num;
         else
         url=url+"&page="+num
        window.location=url;
     });
});


//$(document).bind("mobileinit", function()  
//{  
//   if (navigator.userAgent.indexOf("Android") != -1)  
//   {  
//     $.mobile.defaultPageTransition = 'none';  
//     $.mobile.defaultDialogTransition = 'none';  
//   }  
//});  
//2014-06-27
        //正小数
        function replaceNaNDot(obj) {
            if (isNaN(obj.value) || obj.value > 1 || obj.value < 0) {
                obj.value = '';
                alert("比例必须是0~1之间的小数！");
            };
        }
        //必须是小数
        function replaceNaNNum(obj) {
            if (isNaN(obj.value)) {
                obj.value = '';
                alert("必须用阿拉伯数字表示！");
            };
        }

//2014-06-26 edit zy zyl
 function ConvertNum(str){
        if(str==undefined||str==null||str=="")
        {
        return 0;
        }
        else{
        return parseFloat(str);
        }
    }
    
        
    //文本框获取失去焦点动态效果添加
    $(document).ready(function(){
   //  $("input[type='text'],select").parent().css("margin","0px");
      $("input[type='text']").live({
      focus:function(){
      $(this).addClass("txtOnfocus");
      },
      blur:function(){
     $(this).removeClass("txtOnfocus");
      }
      });
      
      //2014-07-02 edit by zyl 
      //为无汉字的jqm按钮去掉ui默认的maigin padding样式，使得行间距不要太大 默认maigin：8 padding：0
       $("input[data-iconpos='notext']").each(function(){
       $(this).parent().css({"margin":0,"padding":0});
       });
  });
  
  
// 确认返回
function ConfirmReturn(url,msg)
{
    if(msg==undefined||msg==null||msg=="")
    window.location.href=url;
    else if(confirm(msg))
    window.location.href=url;
    else
    window.location.href=url;
}

//2014-06-10 edit by zyl
 //获取页面参数
 function GetRequest(url)
        {
        var url = url; //获取url中"?"符后的字串
        var theRequest = new Object();
        if(url.indexOf("?") != -1)
        {
          var str = url.substr(1);
            strs = str.split("&");
          for(var i = 0; i < strs.length; i ++)
            {
             theRequest[strs[i].split("=")[0]]=unescape(strs[i].split("=")[1]);
            }
        }
        return theRequest;
        }
        
        
        //2014-06-25 edit by zyl 
        //关闭弹出框
       function CloseChidren()
       {
     $('.ui-dialog').dialog('close');
       }
       
       
       //2014-06-24 edit by zyl
       // 自动完成 文本框对象 ul对象 请求类型 其他参数
       function AutoComplicated(textObj,ulObj,requestType,otherParm)
       {
       var sugList = $(ulObj);
       $(textObj).on("input", function(e) {
		    var text = $(textObj).val(); 
		    if(text.length < 2)
	        {
			    $(ulObj).html("");
			    $(ulObj).listview("refresh");
		    } 
		    else 
		    {
		        $.get("../Ajax/AutoComplicated.ashx", {type:requestType,search:text,otherParm:otherParm}, function(data,status){       
	        	        var jsonRel = $.parseJSON(data);
				        var str = "";
				        for (var i = 0; i < jsonRel.length; i++) 
			            {
					        str += "<li onclick='AutoCompile(this)' title='a'>"+jsonRel[i]+"</li>";//onclick='AutoCompile(this,"+textObj+","+ulObj+");'
				        }
				        sugList.html(str);
				        sugList.listview("refresh");
		         });
		     }
    	});
      }
  
  //2014-06-24 edit by zyl 
  //将选中项的值设置为文本框框的值     
 function AutoCompile(obj){      
       var ulObj=$(obj).parent();
       var textObj=$(obj).parent().prev().find("input").eq(0);
       if(textObj.size()==0)
       textObj=$(obj).parent().prev();
        
      $(textObj).val(obj.innerHTML);
        var sugList = $(ulObj);
        sugList.html("");
        sugList.listview("refresh");
        }
       
  //2014-06-25 edit by zyl
  //当鼠标离开ul时 清空ul的内容     
 $(function(){
 $(document).click(function(){
    $("ul[data-role='listview']").each(function(){
    $(this).html("");
    $(this).listview("refresh");
    });
    });
 });    
       
  
  
       