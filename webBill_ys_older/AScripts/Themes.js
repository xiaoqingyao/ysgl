// Unimind Themes 
//   by Jemmy 2010-09-28
// 依赖：Common.js
// 功能：样式表切换

//(function(){
var Theme = {
	cookieName: "themeIndexUnimind_Erp",
	themeList: null,
	init: function(){
		Theme.themeList = document.getElementById('themeListItem');
		if (Theme.themeList)
		{
			var list = Theme.themeList.getElementsByTagName('a');
			oThis = this;
			for( var i = 0; i < list.length; i++ ){
				(function(){
					var index = i + 1;
					list[index - 1].onclick = function(){
						oThis.setCss(index);
						oThis.setCurrent(index);
						oThis.setCookie(Theme.cookieName, index);
						oThis.ReloadAllFrames();
	//					oThis.changeHuangliImage();
						return false;
					};
				})();
			}
		}
		
		var cookieIndex = this.getCookie(this.cookieName);
		if(cookieIndex == null){
			this.setCookie(this.cookieName, 1);	
			Theme.setCss(1);
		}else{
			Theme.setCss(cookieIndex);
			Theme.setCurrent(cookieIndex);
		}
//        this.changeHuangliImage();
	},
	
	setCurrent: function(index){
		if (Theme.themeList)
		{
			var list = Theme.themeList.getElementsByTagName('li');
			for( var i = 0; i < list.length; i++ ){
				if(index == i + 1)
					list[i].className = 'current';
				else
					list[i].className = '';
			}
		}
	},
	setCss: function(index){
		var themescss = new Array("","App_Themes/Blues.css","App_Themes/BluesPhone.css","App_Themes/Grays.css","App_Themes/Vistas.css","App_Themes/Reds.css");
		var themescssphone = new Array("", "App_Themes/BluesPhone.css");
		
		var cssUrl = (SysCommon.mobile()) ? themescssphone[index] : themescss[index];

		var css = document.getElementById('themesMainCss');
		if (!css)
		    css = document.getElementById('ctl00_themesMainCss');

		if (css)
		    css.href = common_Root + cssUrl;
		//alert(themescss[index]);
		
	},
	
	getCookie: function(name){
		var sRE = "(?:; )?" + name + "=([^;]*);?";
		var oRE = new RegExp(sRE);
		if(oRE.test(document.cookie)){
			return unescape(RegExp.$1);
		}else{
			return null;
		}
	},

	setCookie: function	(name,value)//两个参数，一个是cookie的名子，一个是值   
	{   
		var Days = 365; //此 cookie 将被保存 30 天   
		var exp  = new Date();    //new Date("December 31, 9998");   
		exp.setTime(exp.getTime() + Days*24*60*60*1000);   
		//document.cookie = name + "="+ escape (value) + ";expires=" + exp.toGMTString(); 
		document.cookie = name + "="+ escape (value) + ";path=" + common_Root + ";expires=" + exp.toGMTString();     
	},
	
	changeHuangliImage: function () {
		var cookieIndex = this.getCookie(this.cookieName);
		cookieIndex = cookieIndex ? cookieIndex : 1;
	},
	ReloadAllFrames: function ()  // 刷新页面中的IFrame嵌套子页
	{
	    var frm = document.frames("divWestSystems");
	    if (frm) frm.location.reload();
		frm = document.frames("divMessages");
		if (frm) frm.location.reload();
		frm = document.frames("divHelpDocs");
		if (frm) frm.location.reload();
		frm = document.frames("divDesktop");
		if (frm) frm.location.reload();
	}

}

Theme.init();
//})();

