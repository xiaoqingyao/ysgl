
function CreateXMLHttp() {
    var xmlhttp = null; //创建一个新变量 request 并赋值 false。使用 false 作为判断条件，它表示还没有创建 XMLHttpRequest 对象。 
    try {
        xmlhttp = new XMLHttpRequest();  //尝试创建 XMLHttpRequest 对象，除 IE 外的浏览器都支持这个方法。
    }
    catch (e) {
        try {
            xmlhttp = new ActiveXObject("Msxml2.XMLHTTP");  //使用较新版本的 IE 创建 IE 兼容的对象（Msxml2.XMLHTTP）
        }
        catch (e) {
            try {
                xmlhttp = new ActiveXObject("Microsoft.XMLHTTP"); //使用较老版本的 IE 创建 IE 兼容的对象（Microsoft.XMLHTTP）。
            }
            catch (ex) {
                xmlhttp = false;  //如果失败则保证 request 的值仍然为 false。
            }
        }
    }
    return xmlhttp;
}