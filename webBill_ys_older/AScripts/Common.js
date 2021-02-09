var common_Root = "/webBill/"; // 需要"/"结尾
//function document.oncontextmenu() {return true};  //是否隐藏右键菜单

//判断浏览器类型
    SysCommonClass = function(){
        var ua = navigator.userAgent.toLowerCase();
        var app = navigator.appVersion;
        this.trident = function () {return ua.indexOf('trideng') > -1;};//IE内核
        this.presto = function () {return ua.indexOf('presto') > -1;}; //opera内核
        this.webkit = function () {return  ua.indexOf('applewebkit') > -1;}; //苹果、谷歌内核
        this.gecko = function () {return ua.indexOf('gecko') > -1 && ua.indexOf('khtml') == -1;}; //火狐内核
        this.mobile = function () {return !!ua.match(/applewebkit.*mobile.*/);}; //是否移动终端
        //this.mobile = function () {return true;};
        this.ios = function () {return !!ua.match(/\(i[^;]+;( u;)? cpu.+mac os x/);}; //ios终端
        this.android = function () {return ua.indexOf('android') > -1 || ua.indexOf('linux') > -1;}; //android终端或者uc浏览器
        this.iphone = function () {return ua.indexOf('iphone') > -1 || ua.indexOf('mac') > -1;}; //是否为iPhone或者QQHD浏览器
        this.ipad = function () {return ua.indexOf('ipad') > -1;}; //是否为iPad
        this.webapp = function () {return ua.indexOf('safari') == -1;}; //是否为WEB应用程序，没有头部和底部
    }
var SysCommon= new SysCommonClass();

//格式化字符串，如String.format(“123456{0}7890”,’aaaa’)返回123456aaaa7890
String.format = function() {
    if( arguments.length == 0 )
        return null; 
    var str = arguments[0]; 
    for(var i=1;i<arguments.length;i++) {
        var re = new RegExp('\\{' + (i-1) + '\\}','gm');
        str = str.replace(re, arguments[i]);
    }
    return str;
}
//去掉尾空格
String.trim = function(str) {
    return str.replace(/(^\s*)|(\s*$)/g, "");
}

//****************** PNG图片格式 //******************
  function timeoutCorrectPng()
  {
    setTimeout('correctPNG()',1000);
  }
   function correctPNG() 
   {
       for(var i=0; i<document.images.length; i++)
      {
          var img = document.images[i]
          var imgName = img.src.toUpperCase()
          if (imgName.substring(imgName.length-3, imgName.length) == "PNG")
         {
             var imgID = (img.id) ? "id='" + img.id + "' " : ""
             var imgClass = (img.className) ? "class='" + img.className + "' " : ""
             var imgTitle = (img.title) ? "title='" + img.title + "' " : "title='" + img.alt + "' "
             var imgStyle = "display:inline-block;" + img.style.cssText 
             if (img.align == "left") imgStyle = "float:left;" + imgStyle
             if (img.align == "right") imgStyle = "float:right;" + imgStyle
             if (img.parentElement.href) imgStyle = "cursor:hand;" + imgStyle        
             var strNewHTML = "<span " + imgID + imgClass + imgTitle
             + " style=\"" + "width:" + img.width + "px; height:" + img.height + "px;" + imgStyle + ";"
             + "filter:progid:DXImageTransform.Microsoft.AlphaImageLoader"
             + "(src=\'" + img.src + "\', sizingMethod='scale');\"></span>" 
             img.outerHTML = strNewHTML
             i = i-1
         }
      }
   }
window.attachEvent("onload", timeoutCorrectPng); 


//****************** 将表格tableId导出到Excel //******************
function exportEXCELPreview(tableId, sheetName) {
    try  
    {   
        var ExApp = new ActiveXObject("Excel.Application");
        var ExWBk = ExApp.workbooks.add();
        var ExWSh = ExWBk.worksheets(1);
        ExApp.DisplayAlerts = false;
        ExApp.visible = true;
        if (sheetName)
            ExWSh.Name=sheetName;
        //复制到Windows粘贴板
        var tableExcel = document.all(tableId);
        window.clipboardData.setData("Text",document.all(tableId).outerHTML);
        ExWSh.Paste;
        //ExApp.Selection.RowHeight = 22;// 行高

        var ExWshColumns = new Array("A","B","C","D","E","F","G","H","I","J","K","L","M",'N','O','P','Q','R','S','T','U','V','W','X','Y','Z');
        for (var j = 0; j < tableExcel.rows.length; j++)
        {
            if (j == 10) break;
            for (var i = 0; i < tableExcel.rows(j).cells.length; i++) {
                if (tableExcel.rows(j).cells(i).style.width) {
                    var w = tableExcel.rows(j).cells(i).style.width.replace('px','') * 8.38 / 72.0 ;
                    
                    if (w > ExWSh.Columns(ExWshColumns[i]).ColumnWidth)
                        ExWSh.Columns(ExWshColumns[i]).ColumnWidth = w;
                    else if (j==0 || (j==1 && tableExcel.rows(0).cells.length == 1))
                        ExWSh.Columns(ExWshColumns[i]).ColumnWidth = w;
                }
            }
        }
        ShowMessage('Export excel succeed!');
    }     
    catch(e) {
        //var message = "您的电脑没有安装 Excel 软件，或IE选项安全设置中禁用了“ActiveX控件和插件”！" + e.message + "<BR><BR><BR>打开“Internet选项…”，选择“安全”标签，选择“可信站点”区域，将该网站添加为可信站点，回到“安全”标签，点击“自定义级别”，将所有“ActiveX控件和插件”设为“启用”或“提示”";
        try {ShowMessage(e.message);}
        catch(e){alert(e.message);}
        return false ;
    } 
 }

/**** formatNumber(number,pattern) javascript 格式化数字 *******
 * javascript 格式化数字 
 * alert(formatNumber(0,''));
 * alert(formatNumber(12432.21,'#,###'));
 * alert(formatNumber(12432.21,'#,###.000#'));
 * alert(formatNumber(12432,'#,###.00'));
 * alert(formatNumber('12432.415','#,###.0#'));
 **************************************************************/
function formatNumber(number,pattern){
    var str            = number.toString();
    var strInt;
    var strFloat;
    var formatInt;
    var formatFloat;
    if(/\./g.test(pattern)){
        formatInt        = pattern.split('.')[0];
        formatFloat        = pattern.split('.')[1];
    }else{
        formatInt        = pattern;
        formatFloat        = null;
    }

    if(/\./g.test(str)){
        if(formatFloat!=null){
            var tempFloat    = Math.round(parseFloat('0.'+str.split('.')[1])*Math.pow(10,formatFloat.length))/Math.pow(10,formatFloat.length);
            strInt        = (Math.floor(number)+Math.floor(tempFloat)).toString();                
            strFloat    = /\./g.test(tempFloat.toString())?tempFloat.toString().split('.')[1]:'0';            
        }else{
            strInt        = Math.round(number).toString();
            strFloat    = '0';
        }
    }else{
        strInt        = str;
        strFloat    = '0';
    }
    if(formatInt!=null){
        var outputInt    = '';
        var zero        = formatInt.match(/0*$/)[0].length;
        var comma        = null;
        if(/,/g.test(formatInt)){
            comma        = formatInt.match(/,[^,]*/)[0].length-1;
        }
        var newReg        = new RegExp('(\\d{'+comma+'})','g');

        if(strInt.length<zero){
            outputInt        = new Array(zero+1).join('0')+strInt;
            outputInt        = outputInt.substr(outputInt.length-zero,zero)
        }else{
            outputInt        = strInt;
        }

        var 
        outputInt            = outputInt.substr(0,outputInt.length%comma)+outputInt.substring(outputInt.length%comma).replace(newReg,(comma!=null?',':'')+'$1')
        outputInt            = outputInt.replace(/^,/,'');

        strInt    = outputInt;
    }

    if(formatFloat!=null){
        var outputFloat    = '';
        var zero        = formatFloat.match(/^0*/)[0].length;

        if(strFloat.length<zero){
            outputFloat        = strFloat+new Array(zero+1).join('0');
            //outputFloat        = outputFloat.substring(0,formatFloat.length);
            var outputFloat1    = outputFloat.substring(0,zero);
            var outputFloat2    = outputFloat.substring(zero,formatFloat.length);
            outputFloat        = outputFloat1+outputFloat2.replace(/0*$/,'');
        }else{
            outputFloat        = strFloat.substring(0,formatFloat.length);
        }
        strFloat    = outputFloat;
    }else{
        if(pattern!='' || (pattern=='' && strFloat=='0')){
            strFloat    = '';
        }
    }
    return strInt+(strFloat==''?'':'.'+strFloat);
}
/**** end of formatNumber javascript 格式化数字 *******/


