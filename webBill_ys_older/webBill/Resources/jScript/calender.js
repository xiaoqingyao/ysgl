//在页面控件上直接添加对该控件方法的调用 setday(this) 如，需要在textbox1获取焦点时弹出选择 则在 this.TextBox1.Attribute.Add("onfocus","javascript:setday(this);");
var bMoveable=true;  
var _VersionInfo="";
var strFrame;  
document.writeln('<iframe id=meizzDateLayer Author=wayx frameborder=0 style="position: absolute; width: 144px; height: 211px; z-index: 9998; display: none"></iframe>');
strFrame='<style>';
strFrame+='INPUT.button{BORDER-RIGHT: #ff9900 1px solid;BORDER-TOP: #ff9900 1px solid;BORDER-LEFT: #ff9900 1px solid;';
strFrame+='BORDER-BOTTOM: #ff9900 1px solid;BACKGROUND-COLOR: #fff8ec;}';
strFrame+='TD{FONT-SIZE: 9pt;}';
strFrame+='</style>';
strFrame+='<scr' + 'ipt>';
strFrame+='var datelayerx,datelayery; ';
strFrame+='var bDrag; ';
strFrame+='function document.onmousemove() ';
strFrame+='{if(bDrag && window.event.button==1)';
strFrame+=' {var DateLayer=parent.document.all.meizzDateLayer.style;';
strFrame+='  DateLayer.posLeft += window.event.clientX-datelayerx;';
strFrame+='  DateLayer.posTop += window.event.clientY-datelayery;}}';
strFrame+='function DragStart()  ';
strFrame+='{var DateLayer=parent.document.all.meizzDateLayer.style;';
strFrame+=' datelayerx=window.event.clientX;';
strFrame+=' datelayery=window.event.clientY;';
strFrame+=' bDrag=true;}';
strFrame+='function DragEnd(){  ';
strFrame+=' bDrag=false;}';
strFrame+='</scr' + 'ipt>';
strFrame+='<div style="z-index:9999;position: absolute; left:0; top:0;" onselectstart="return false"><span id=tmpSelectYearLayer Author=wayx style="z-index: 9999;position: absolute;top: 3; left: 19;display: none"></span>';
strFrame+='<span id=tmpSelectMonthLayer Author=wayx style="z-index: 9999;position: absolute;top: 3; left: 48;display: none"></span>';
strFrame+='<table border=1 cellspacing=0 cellpadding=0 width=142 height=160 bordercolor=#ff9900 bgcolor=#ff9900 Author="wayx">';
strFrame+='  <tr Author="wayx"><td width=142 height=23 Author="wayx" bgcolor=#FFFFFF><table border=0 cellspacing=1 cellpadding=0 width=140 Author="wayx" height=23>';
strFrame+='      <tr align=center Author="wayx"><td width=16 align=center bgcolor=#ff9900 style="font-size:12px;cursor: hand;color: #ffffff" ';
strFrame+='        onclick="parent.meizzPrevM()" title="1 Month Before" Author=meizz><b Author=meizz>&lt;</b>';
strFrame+='        </td><td width=30 align=center style="font-size:12px;cursor:default" Author=meizz ';
strFrame+='onmouseover="style.backgroundColor=\'#FFD700\'" onmouseout="style.backgroundColor=\'white\'" ';
strFrame+='onclick="parent.tmpSelectYearInnerHTML(this.innerText)" title="Click to select year"><span Author=meizz id=meizzYearHead></span></td>';
strFrame+='<td width=78 align=center style="font-size:12px;cursor:default" Author=meizz onmouseover="style.backgroundColor=\'#FFD700\'" ';
strFrame+=' onmouseout="style.backgroundColor=\'white\'" onclick="parent.tmpSelectMonthInnerHTML(parent.athosMonthNameToNum(this.innerText))"';
strFrame+='        title="Click to select month"><span id=meizzMonthHead Author=meizz></span></td>';
strFrame+='        <td width=16 bgcolor=#ff9900 align=center style="font-size:12px;cursor: hand;color: #ffffff" ';
strFrame+='         onclick="parent.meizzNextM()" title="1 Month Later" Author=meizz><b Author=meizz>&gt;</b></td></tr>';
strFrame+='    </table></td></tr>';
strFrame+='  <tr Author="wayx"><td width=142 height=18 Author="wayx">';
strFrame+='<table border=1 cellspacing=0 cellpadding=0 bgcolor=#ff9900 ' + (bMoveable? 'onmousedown="DragStart()" onmouseup="DragEnd()"':'');
strFrame+=' BORDERCOLORLIGHT=#FF9900 BORDERCOLORDARK=#FFFFFF width=140 height=20 Author="wayx" style="cursor:' + (bMoveable ? 'move':'default') + '">';
strFrame+='<tr Author="wayx" align=center valign=bottom>';
strFrame+='<td width=18px style="font-size:12px;color:#FFFFFF" Author=meizz title="Sunday">S</td>';
strFrame+='<td width=18px style="font-size:12px;color:#FFFFFF" Author=meizz title="Monday">M</td>';
strFrame+='<td width=18px style="font-size:12px;color:#FFFFFF" Author=meizz title="Tuesday">T</td>';
strFrame+='<td width=18px style="font-size:12px;color:#FFFFFF" Author=meizz title="Wednesday">W</td>';
strFrame+='<td width=18px style="font-size:12px;color:#FFFFFF" Author=meizz title="Thursday">T</td>';
strFrame+='<td width=18px style="font-size:12px;color:#FFFFFF" Author=meizz title="Friday">F</td>';
strFrame+='<td width=20px style="font-size:12px;color:#FFFFFF" Author=meizz title="Saturday">S</td>';
strFrame+='</tr>'; 
strFrame+='</table></td></tr><!-- Author:F.R.Huang(meizz) http://www.meizz.com/ mail: meizz@hzcnc.com 2002-10-8 -->';
strFrame+='  <tr Author="wayx"><td width=142 height=120 Author="wayx">';
strFrame+='    <table border=1 cellspacing=2 cellpadding=0 BORDERCOLORLIGHT=#FF9900 BORDERCOLORDARK=#FFFFFF bgcolor=#fff8ec width=140 height=120 Author="wayx">';
var n=0; for (j=0;j<5;j++){ strFrame+= ' <tr align=center Author="wayx">'; for (i=0;i<7;i++){
strFrame+='<td width=20 height=20 id=meizzDay'+n+' style="font-size:12px" Author=meizz onclick=parent.meizzDayClick(this.innerText,0)></td>';n++;}
strFrame+='</tr>';}
strFrame+='      <tr align=center Author="wayx">';
for (i=35;i<39;i++)strFrame+='<td width=20 height=20 id=meizzDay'+i+' style="font-size:12px" Author=wayx onclick="parent.meizzDayClick(this.innerText,0)"></td>';
strFrame+='        <td align=right Author=meizz><span onclick=parent.clearAndCloseLayer() style="font-size:12px;cursor: hand;color:#00aaaa;"';
strFrame+='         Author=meizz title="Clear"><b>C</b></span>&nbsp;</td>';
strFrame+='        <td align=right Author=meizz><span onclick=parent.closeLayer() style="font-size:12px;cursor: hand;color:Red;"';
strFrame+='         Author=meizz title="Close"><b>X</b></span>&nbsp;</td>';
strFrame+='</tr>';
strFrame+='</table></td></tr><tr Author="wayx"><td Author="wayx">';
strFrame+='  <table border=0 cellspacing=1 cellpadding=0 width=100% Author="wayx" bgcolor=#FFFFFF>';
strFrame+='    <tr Author="wayx"><td Author=meizz align=left>';
strFrame+='       <input Author=meizz type=button class=button value="&larr;" title="10 Year Before" onclick="parent.meizzPrevY(10)" ';
strFrame+='          onfocus="this.blur()" style="font-size: 12px; height: 20px">';
strFrame+='       <input Author=meizz type=button class=button value="&#171;" title="1 Year Before" onclick="parent.meizzPrevY(1)" ';
strFrame+='          onfocus="this.blur()" style="font-size: 12px; height: 20px">';
strFrame+='        <input Author=meizz class=button title="1 Month Before" type=button ';
strFrame+='          value="&lsaquo;" onclick="parent.meizzPrevM()" onfocus="this.blur()" style="font-size: 12px; height: 20px"></td><td ';
strFrame+='             Author=meizz align=center><input Author=meizz type=button class=button value=td style="color:#00007f;background-color:FFD700;font:italic bolder 10pt;" onclick="parent.meizzToday()" ';
strFrame+='             onfocus="this.blur()" title="Today" style="font-size: 12px; height: 20px; cursor:hand"></td><td ';
strFrame+='             Author=meizz align=right>';
strFrame+='        <input Author=meizz type=button class=button value="&rsaquo;" onclick="parent.meizzNextM()" ';
strFrame+='             onfocus="this.blur()" title="1 Month Later" class=button style="font-size: 12px; height: 20px">';
strFrame+='        <input Author=meizz type=button class=button value="&#187;" title="1 Year Later" onclick="parent.meizzNextY(1)"';
strFrame+='             onfocus="this.blur()" style="font-size: 12px; height: 20px">';
strFrame+='       <input Author=meizz type=button class=button value="&rarr;" title="10 Year Later" onclick="parent.meizzNextY(10)" ';
strFrame+='          onfocus="this.blur()" style="font-size: 12px; height: 20px">';
strFrame+='      </td></tr></table></td></tr></table></div>';


window.frames.meizzDateLayer.document.writeln(strFrame);
window.frames.meizzDateLayer.document.close();  

var outObject;
var outButton; 
var outDate=""; 
var odatelayer=window.frames.meizzDateLayer.document.all;  
function setday(tt,obj) 
{
//debugger;
 if (arguments.length >  2){alert("Sorry, too many parameters");return;}
 if (arguments.length == 0){alert("Sorry, none parameter!");return;}
 var dads  = document.all.meizzDateLayer.style;
 var th = tt;
 var ttop  = tt.offsetTop;
 var thei  = tt.clientHeight;  
 var tleft = tt.offsetLeft;  
 var ttyp  = tt.type;        
 
// alert(ttop+","+thei+","+tleft+++",arguments.length:"+arguments.length);
 while (tt = tt.offsetParent){ttop+=tt.offsetTop; tleft+=tt.offsetLeft;}
 dads.top  = (ttyp=="image")? ttop+thei : ttop+thei+6;
 dads.left = tleft;
 outObject = (arguments.length == 1) ? th : obj;
 outButton = (arguments.length == 1) ? null : th; 
 var reg = /^(\d{1,2})\/(\d{1,2})\/(\d+)$/; 
 var r = outObject.value.match(reg); 
 
 if(r!=null){
  
  r[0]=r[3];
  r[3]=r[2];
  r[2]=r[1];
  r[1]=r[0];

  r[2]=r[2]-1; 
  var d= new Date(r[1], r[2],r[3]); 
  if(d.getFullYear()==r[1] && d.getMonth()==r[2] && d.getDate()==r[3]){
   outDate=d;
  }
  else outDate="";
   meizzSetDay(r[1],r[2]+1);
 }
 else{
  outDate="";
  meizzSetDay(new Date().getFullYear(), new Date().getMonth() + 1);
 }
 dads.display = '';

 event.returnValue=false;
}

var MonHead = new Array(12);         
    MonHead[0] = 31; MonHead[1] = 28; MonHead[2] = 31; MonHead[3] = 30; MonHead[4]  = 31; MonHead[5]  = 30;
    MonHead[6] = 31; MonHead[7] = 31; MonHead[8] = 30; MonHead[9] = 31; MonHead[10] = 30; MonHead[11] = 31;

var meizzTheYear=new Date().getFullYear(); 
var meizzTheMonth=new Date().getMonth()+1; 
var meizzWDay=new Array(39);               

function document.onclick() 
{ 
  with(window.event)
  {
   if (srcElement.getAttribute("Author")==null && srcElement != outObject && srcElement != outButton)
    closeLayer();
  }
}

function document.onkeyup() 
  {
    if (window.event.keyCode==27){
  if(outObject)outObject.blur();
  closeLayer();
 }
 else if(document.activeElement)
  if(document.activeElement.getAttribute("Author")==null && document.activeElement != outObject && document.activeElement != outButton)
  {
   closeLayer();
  }
  }

function meizzWriteHead(yy,mm) 
  {
 odatelayer.meizzYearHead.innerText  = String(yy);
    odatelayer.meizzMonthHead.innerText = athosMonthNumToName(String(mm));
  }

function athosMonthNameToNum(mn)
{
 switch (String(mn)) 
 {
   case "1" :
      return String(1);
   case "2" :
      return String(2);
   case "3" :
   return String(3);
   case "4" :
      return String(4);
   case "5" :
      return String(5);
   case "6" :
      return String(6);
   case "7" :
   return String(7);
   case "8" :
      return String(8);
   case "9" :
      return String(9);
   case "10" :
      return String(10);
   case "11" :
   return String(11);
   case "12" :
      return String(12);
   default :
      return String(0);
 } 
}

function athosMonthNumToName(mm)
{
 switch (mm) 
 {
   case "1":
  return String("1");
   case "2":
  return String("2");
   case "3":
  return String("3");  
   case "4":
  return String("4");
   case "5":
  return String("5");
   case "6":
  return String("6");
   case "7":
  return String("7");
   case "8":
  return String("8");
   case "9":
  return String("9");
   case "10":
  return String("10");
   case "11":
  return String("11");
   case "12":
  return String("12");
   default :
      return String("UnknownMonth");
 } 
}


function tmpSelectYearInnerHTML(strYear) 
{
  if (strYear.match(/\D/)!=null){alert("Year shall be a number.");return;}
  var m = (strYear) ? strYear : new Date().getFullYear();
  if (m < 1000 || m > 9999) {alert("Year shall between 1000 to 9999.");return;}
  var n = m - 10;
  if (n < 1000) n = 1000;
  if (n + 26 > 9999) n = 9974;
  var s = "<select Author=meizz name=tmpSelectYear style='font-size: 12px' "
     s += "onblur='document.all.tmpSelectYearLayer.style.display=\"none\"' "
     s += "onchange='document.all.tmpSelectYearLayer.style.display=\"none\";"
     s += "parent.meizzTheYear = this.value; parent.meizzSetDay(parent.meizzTheYear,parent.meizzTheMonth)'>\r\n";
  var selectInnerHTML = s;
  for (var i = n; i < n + 26; i++)
  {
    if (i == m)
       {selectInnerHTML += "<option Author=wayx value='" + i + "' selected>" + i + "</option>\r\n";}
    else {selectInnerHTML += "<option Author=wayx value='" + i + "'>" + i + "</option>\r\n";}
  }
  selectInnerHTML += "</select>";
  odatelayer.tmpSelectYearLayer.style.display="";
  odatelayer.tmpSelectYearLayer.innerHTML = selectInnerHTML;
  odatelayer.tmpSelectYear.focus();
}

function tmpSelectMonthInnerHTML(strMonth)
{
 if (strMonth.match(/\D/)!=null){alert("Month shall be a number");return;}
  var m = (strMonth) ? strMonth : new Date().getMonth() + 1;
  var s = "<select Author=meizz name=tmpSelectMonth style='font-size: 12px' "
     s += "onblur='document.all.tmpSelectMonthLayer.style.display=\"none\"' "
     s += "onchange='document.all.tmpSelectMonthLayer.style.display=\"none\";"
     s += "parent.meizzTheMonth = this.value; parent.meizzSetDay(parent.meizzTheYear,parent.meizzTheMonth)'>\r\n";
  var selectInnerHTML = s;
  for (var i = 1; i < 13; i++)
  {
    if (i == m)
       {selectInnerHTML += "<option Author=wayx value='"+i+"' selected>"+  athosMonthNumToName(String(i))        +"</option>\r\n";}
    else {selectInnerHTML += "<option Author=wayx value='"+i+"'>"+        athosMonthNumToName(String(i))         +"</option>\r\n";}
  }
  selectInnerHTML += "</select>";
  odatelayer.tmpSelectMonthLayer.style.display="";
  odatelayer.tmpSelectMonthLayer.innerHTML = selectInnerHTML;
  odatelayer.tmpSelectMonth.focus();
}

function closeLayer()               
  {
    document.all.meizzDateLayer.style.display="none";
  }
function clearAndCloseLayer()               
  {
  if (outObject)
  {
   outObject.value= ""; 
   closeLayer(); 
  }
  else {closeLayer(); alert("None control to output!");}    
  }

function IsPinYear(year)            
  {
    if (0==year%4&&((year%100!=0)||(year%400==0))) return true;else return false;
  }

function GetMonthCount(year,month)  
  {
    var c=MonHead[month-1];if((month==2)&&IsPinYear(year)) c++;return c;
  }
function GetDOW(day,month,year)     
  {
    var dt=new Date(year,month-1,day).getDay()/7; return dt;
  }


function meizzPrevY(intYears)  
  {
    if(meizzTheYear > 999 && meizzTheYear <10000){meizzTheYear-=intYears;}
    else{alert("Year beyond (1000-9999)!");}
    meizzSetDay(meizzTheYear,meizzTheMonth);
  }
function meizzNextY(intYears)  
  {
    if(meizzTheYear > 999 && meizzTheYear <10000){meizzTheYear+=intYears;}
    else{alert("Year beyond (1000-9999)!");}
    meizzSetDay(meizzTheYear,meizzTheMonth);
  }
function meizzToday()  
  {
 var today;
    meizzTheYear = new Date().getFullYear();
    meizzTheMonth = new Date().getMonth()+1;
    today=new Date().getDate();
    //meizzSetDay(meizzTheYear,meizzTheMonth);
    if(outObject){
  outObject.value=meizzTheYear + "-" + meizzTheMonth + "-" + today;
    }
    closeLayer();
  }
function meizzPrevM()  
  {
    if(meizzTheMonth>1){meizzTheMonth--}else{meizzTheYear--;meizzTheMonth=12;}
    meizzSetDay(meizzTheYear,meizzTheMonth);
  }
function meizzNextM()  
  {
    if(meizzTheMonth==12){meizzTheYear++;meizzTheMonth=1}else{meizzTheMonth++}
    meizzSetDay(meizzTheYear,meizzTheMonth);
  }

function meizzSetDay(yy,mm)   
{
  meizzWriteHead(yy,mm);
  meizzTheYear=yy;
  meizzTheMonth=mm;
  
  for (var i = 0; i < 39; i++){meizzWDay[i]=""};  
  var day1 = 1,day2=1,firstday = new Date(yy,mm-1,1).getDay();  
  for (i=0;i<firstday;i++)meizzWDay[i]=GetMonthCount(mm==1?yy-1:yy,mm==1?12:mm-1)-firstday+i+1 
  for (i = firstday; day1 < GetMonthCount(yy,mm)+1; i++){meizzWDay[i]=day1;day1++;}
  for (i=firstday+GetMonthCount(yy,mm);i<39;i++){meizzWDay[i]=day2;day2++}
  for (i = 0; i < 39; i++)
  { var da = eval("odatelayer.meizzDay"+i)
    if (meizzWDay[i]!="")
      { 
  da.borderColorLight="#FF9900";
  da.borderColorDark="#FFFFFF";
  if(i<firstday)  
  {
   da.innerHTML="<b><font color=gray>" + meizzWDay[i] + "</font></b>";
   da.title=(mm==1?12:mm-1) +"Mn" + meizzWDay[i] + "Dt";
   da.onclick=Function("meizzDayClick(this.innerText,-1)");
   if(!outDate)
    da.style.backgroundColor = ((mm==1?yy-1:yy) == new Date().getFullYear() && 
     (mm==1?12:mm-1) == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate()) ?
      "#FFD700":"#e0e0e0";
   else
   {
    da.style.backgroundColor =((mm==1?yy-1:yy)==outDate.getFullYear() && (mm==1?12:mm-1)== outDate.getMonth() + 1 && 
    meizzWDay[i]==outDate.getDate())? "#00ffff" :
    (((mm==1?yy-1:yy) == new Date().getFullYear() && (mm==1?12:mm-1) == new Date().getMonth()+1 && 
    meizzWDay[i] == new Date().getDate()) ? "#FFD700":"#e0e0e0");
    if((mm==1?yy-1:yy)==outDate.getFullYear() && (mm==1?12:mm-1)== outDate.getMonth() + 1 && 
    meizzWDay[i]==outDate.getDate())
    {
     da.borderColorLight="#FFFFFF";
     da.borderColorDark="#FF9900";
    }
   }
  }
  else if (i>=firstday+GetMonthCount(yy,mm))
  {
   da.innerHTML="<b><font color=gray>" + meizzWDay[i] + "</font></b>";
   da.title=(mm==12?1:mm+1) +"Mn" + meizzWDay[i] + "Dt";
   da.onclick=Function("meizzDayClick(this.innerText,1)");
   if(!outDate)
    da.style.backgroundColor = ((mm==12?yy+1:yy) == new Date().getFullYear() && 
     (mm==12?1:mm+1) == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate()) ?
      "#FFD700":"#e0e0e0";
   else
   {
    da.style.backgroundColor =((mm==12?yy+1:yy)==outDate.getFullYear() && (mm==12?1:mm+1)== outDate.getMonth() + 1 && 
    meizzWDay[i]==outDate.getDate())? "#00ffff" :
    (((mm==12?yy+1:yy) == new Date().getFullYear() && (mm==12?1:mm+1) == new Date().getMonth()+1 && 
    meizzWDay[i] == new Date().getDate()) ? "#FFD700":"#e0e0e0");
    if((mm==12?yy+1:yy)==outDate.getFullYear() && (mm==12?1:mm+1)== outDate.getMonth() + 1 && 
    meizzWDay[i]==outDate.getDate())
    {
     da.borderColorLight="#FFFFFF";
     da.borderColorDark="#FF9900";
    }
   }
  }
  else  
  {
   da.innerHTML="<b>" + meizzWDay[i] + "</b>";
   da.title=mm +"Mn" + meizzWDay[i] + "Dt";
   da.onclick=Function("meizzDayClick(this.innerText,0)");  
   
   if(!outDate)
    da.style.backgroundColor = (yy == new Date().getFullYear() && mm == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate())?
     "#FFD700":"#e0e0e0";
   else
   {
    da.style.backgroundColor =(yy==outDate.getFullYear() && mm== outDate.getMonth() + 1 && meizzWDay[i]==outDate.getDate())?
     "#00ffff":((yy == new Date().getFullYear() && mm == new Date().getMonth()+1 && meizzWDay[i] == new Date().getDate())?
     "#FFD700":"#e0e0e0");
    if(yy==outDate.getFullYear() && mm== outDate.getMonth() + 1 && meizzWDay[i]==outDate.getDate())
    {
     da.borderColorLight="#FFFFFF";
     da.borderColorDark="#FF9900";
    }
   }
  }
        da.style.cursor="hand"
      }
    else{da.innerHTML="";da.style.backgroundColor="";da.style.cursor="default"}
  }
}

function meizzDayClick(n,ex)
{
  var yy=meizzTheYear;
  var mm = parseInt(meizzTheMonth)+ex; 
 if(mm<1){
  yy--;
  mm=12+mm;
 }
 else if(mm>12){
  yy++;
  mm=mm-12;
 }
 
  if (mm < 10){mm = "0" + mm;}
  if (outObject)
  {
    if (!n) {
      return;}
    if ( n < 10){n = "0" + n;}
    outObject.value= yy+ "-" + mm + "-" + n ; 
    closeLayer(); 
  }
  else {closeLayer(); alert("None control to output!");}
}


function pf(id)
{
    if(id=="1")
    {
        document.getElementById("img_pf").src="../images/pfa2.gif";
        document.getElementById("img_lf").src="../images/pfa11.gif";
        document.getElementById("tr2").style.display="none";
        document.getElementById("tr1").style.display="";
        document.getElementById("tr4").style.display="none";
        document.getElementById("tr3").style.display="";
        document.getElementById("tr6").style.display="none";
        document.getElementById("tr5").style.display="";
    }
    else
    {
        document.getElementById("img_pf").src="../images/pfa21.gif";
        document.getElementById("img_lf").src="../images/pfa1.gif";
        document.getElementById("tr2").style.display="";
        document.getElementById("tr1").style.display="none";
        document.getElementById("tr4").style.display="";
        document.getElementById("tr3").style.display="none";
        document.getElementById("tr6").style.display="";
        document.getElementById("tr5").style.display="none";
    }
}
function lh(id)
{
    if(id=="1")
    {
        document.getElementById("img_pf").src="../images/lhxz1.gif";
        document.getElementById("img_lf").src="../images/ndlh.gif";
        document.getElementById("tr2").style.display="none";
        document.getElementById("tr1").style.display="";
    }
    else
    {
        document.getElementById("img_pf").src="../images/lhxz.gif";
        document.getElementById("img_lf").src="../images/ndlh1.gif";
        document.getElementById("tr2").style.display="";
        document.getElementById("tr1").style.display="none";
    }
}
function ws(id)
{
    if(id=="1")
    {
        document.getElementById("img_wkg").src="../images/wkg1.gif";
        document.getElementById("img_zj").src="../images/zj.gif";
        document.getElementById("img_jg").src="../images/jg.gif";
        document.getElementById("tr2").style.display="none";
        document.getElementById("tr1").style.display="";
        document.getElementById("tr3").style.display="none";
    }
    else if(id=="2")
    {
        document.getElementById("img_wkg").src="../images/wkg.gif";
        document.getElementById("img_zj").src="../images/zj1.gif";
        document.getElementById("img_jg").src="../images/jg.gif";
        document.getElementById("tr2").style.display="";
        document.getElementById("tr1").style.display="none";
        document.getElementById("tr3").style.display="none";
    }
     else
    {
        document.getElementById("img_wkg").src="../images/wkg.gif";
        document.getElementById("img_zj").src="../images/zj.gif";
        document.getElementById("img_jg").src="../images/jg1.gif";
        document.getElementById("tr2").style.display="none";
        document.getElementById("tr1").style.display="none";
        document.getElementById("tr3").style.display="";
    }
}
function xylh(id)
{
    if(id=="1")
    {
        document.getElementById("img_pf").src="../images/xylh11.gif";
        document.getElementById("img_lf").src="../images/ndlh.gif";
        document.getElementById("tr2").style.display="none";
        document.getElementById("tr1").style.display="";
    }
    else
    {
        document.getElementById("img_pf").src="../images/xylh12.gif";
        document.getElementById("img_lf").src="../images/ndlh1.gif";
        document.getElementById("tr2").style.display="";
        document.getElementById("tr1").style.display="none";
    }
}