/***********************************************************************
* Created By : 薛新峰
* AuthorEmail: xuexf@telchina.net
* Purpose: 
* EditDate: 2010-12-09
* Description:
    该脚本实现功能：
    （1）选择上级，级联选择下级
    （2）选择一个下级，上级同时选中(递归上溯)
    （3）selectType == "all"时，可以实现下级全选或全不选时的上级级联===============该部分方法还有待完善，存在问题
***********************************************************************/


function CheckSubNodes(selectType) {
    var obj = window.event.srcElement;      //获取事件对象的来源
    var objYellow = obj;
    var objReliy = obj;

    var CheckState; //定义一个变量，用于存储选中状态(true or false)，以便于在子节点级联
    if (obj.tagName == "INPUT" && obj.type == "checkbox")    //如果事件的来源是checkbox 
    {
        CheckState = obj.checked;            //获得该checkbox的选中状态
//        do {
//            obj = obj.parentNode;
//        }
//        while (obj.tagName != "TABLE");
//        obj = obj.nextSibling;
//        if (obj != null && obj.tagName == "DIV") {
//            var inputs = obj.getElementsByTagName("INPUT");
//            for (var i = 0; i < inputs.length; i++) {
//                inputs[i].checked = CheckState;
//            }
//        }

        //父节点的选择状态
        if (CheckState == true) {
            do {
                objYellow = objYellow.parentNode;
            }
            while (objYellow != null && objYellow.tagName != "DIV"); //找到该节点所在的div，则兄节点就在该div的兄节点中
            objYellow = objYellow.previousSibling;
            if (objYellow != null && objYellow.tagName == "TABLE") {
                var inputs = objYellow.getElementsByTagName("INPUT");
                if (inputs.length >= 1) {
                    inputs[0].checked = CheckState;
                    RecursiveSelect(inputs[0]);
                }
            }
        }

        //设置下级全部选中或未选中时的上级状态
//        if (selectType == "all") {
//            do {
//                objReliy = objReliy.parentNode;
//            }
//            while (objReliy != null && objReliy.tagName != "DIV"); //找到该节点所在的div，则兄节点就在该div的兄节点中

//            var inputs = objReliy.getElementsByTagName("INPUT");
//            var selectNo = true;
//            for (var i = 0; i <= inputs.length - 1; i++) {
//                if (inputs[i].checked == true) {
//                    selectNo = false;
//                }
//            }

//            if (selectNo == true) {
//                objReliy = objReliy.previousSibling;
//                if (objReliy != null && objReliy.tagName == "TABLE") {
//                    var inputs = objReliy.getElementsByTagName("INPUT");
//                    if (inputs.length >= 1) {
//                        inputs[0].checked = false;
//                        RecursiveSelectNo(inputs[0]);
//                    }
//                }
//            }
//        }
    }
}

function RecursiveSelect(objYellow) {
    //父节点的选择状态
    if (objYellow.tagName == "INPUT" && objYellow.type == "checkbox") {
        CheckState = objYellow.checked;            //获得该checkbox的选中状态
        if (CheckState == true) {
            do {
                objYellow = objYellow.parentNode;
            }
            while (objYellow != null && objYellow.tagName != "DIV"); //找到该节点所在的div，则兄节点就在该div的兄节点中
            objYellow = objYellow.previousSibling;
            if (objYellow != null && objYellow.tagName == "TABLE") {
                var inputs = objYellow.getElementsByTagName("INPUT");
                if (inputs.length >= 1) {
                    inputs[0].checked = CheckState;
                    RecursiveSelect(inputs[0]);
                }
            }
        }
    }
}

function RecursiveSelectNo(objReliy) {
    do {
        objReliy = objReliy.parentNode;
    }
    while (objReliy != null && objReliy.tagName != "DIV"); //找到该节点所在的div，则兄节点就在该div的兄节点中

    var inputs = objReliy.getElementsByTagName("INPUT");
    var selectNo = true;
    for (var i = 0; i <= inputs.length - 1; i++) {
        if (inputs[i].checked == true) {
            selectNo = false;
        }
    }

    if (selectNo == true) {
        objReliy = objReliy.previousSibling;
        if (objReliy != null && objReliy.tagName == "TABLE") {
            var inputs = objReliy.getElementsByTagName("INPUT");
            if (inputs.length >= 1) {
                inputs[0].checked = false;
                RecursiveSelectNo(inputs[0]);
            }
        }
    }
}