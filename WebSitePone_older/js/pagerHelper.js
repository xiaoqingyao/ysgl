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
          window.location=url+"&page="+parseInt(num)-1;
          });

});