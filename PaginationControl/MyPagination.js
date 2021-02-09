function PreClick() {
    var nowPageIndex = document.getElementById("<% =drpPageIndex.ClientID%>");
    if (nowPageIndex[0].selected) {
        alert("已经是第一页了！");
        return false;
    }
    else {
        return true;
    }
}
function NextClick() {
    var nowPageIndex = document.getElementById("<% =drpPageIndex.ClientID%>");
    if (nowPageIndex[nowPageIndex.options.length - 1].selected) {
        alert("已经是最后一页了！");
        return false;
    }
    else {
        return true;
    }
}