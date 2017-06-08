var mmm = {};
var i = {};
//структура одного сообщения
var message = {};
message.Id = {};
message.ParentId = {};
message.Question = {};
message.Answer = {};
message.DateTime = {};
message.Type = {};
message.IsProcesses = {};
message.IsRead = {};
message.UserName = {};
message.AnswerDateTime = {};
        
//структурация сообщений
function populateMessages(msgs) {
    var messages = [];//<%= getNode() %>;
    messages = JSON.parse(msgs);
    var map = {}, node, roots = [];
    if(messages) {
        for (var i = 0; i < messages.length; i += 1) {
            if(messages[i].ParentId == "0")
                messages[i].ParentId = 0;
            node = messages[i];
            node.children = [];
            map[node.Id] = i; // use map to look-up the parents
            if (node.ParentId != 0) {
                var a1 = node.ParentId;
                var a2 = map[a1];
                var a3 = messages[a2];
                a3.children.push(node);
            } else {
                roots.push(node);
            }
        }
        console.log(roots); 
        var container = document.getElementById('div_messages');
        createTree(container, roots)
    }
}

//формируем дерево
function createTree(container, obj) {
    container.innerHTML = createTreeText(obj);
    //open();
}

function createTreeText(obj) { // отдельная рекурсивная функция
    var li = '';
    for (var kk in obj) {
        if(obj[kk].ParentId == 0) {
            var IsRoot = "IsRoot";
        }
        else 
            var IsRoot = "";

        if(obj[kk].children.length == 0) {
            var IsLeaf = "ExpandLeaf";
        }
        else
        {
            if(IsChildRead(obj[kk]))
                var IsLeaf = "ExpandOpen";
            else 
                var IsLeaf = "ExpandClosed" ;
        }
                    
        li += '<li class="Node '+ IsLeaf +'">' + '<div class="Expand"></div><div class="Content">'+ CreateNode(obj[kk])+'</div>' + createTreeText(obj[kk].children) + '</li>';
    }
    if (li) {
        var ul = '<ul class="Container '+ IsRoot +'">' + li + '</ul>'
    }
    return ul || '';
}

//function f1(mess) {
//    mmm[mess.Id] = mess;
//    if(mmm[mess.Id].children.length > 0) {
//        for(i[mess.Id] = 0; i[mess.Id] < mmm[mess.Id].children.length; i[mess.Id]++) {
//            if(!mmm[mess.Id].children[i[mess.Id]].IsRead) {
//                return true;
//            }
//            f1(mmm[mess.Id].children[i[mess.Id]]);
//        }
//    }
//}

//до 5го уровня вложенности 
function IsChildRead(mess) {
    for(i=0;i<mess.children.length; i++) {
        var mess1 = mess.children[i];
        if(!mess1.IsRead) {
            return true;
        }
        if(mess1.children) {
            for(i2=0;i2<mess1.children.length; i2++) {
                var mess2 = mess1.children[i2];
                if(!mess2.IsRead) {
                    return true;
                }
                if(mess2.children) {
                    for(i3=0;i3<mess2.children.length; i3++) {
                        var mess3 = mess2.children[i3];
                        if(!mess3.IsRead) {
                            return true;
                        }
                        if(mess3.children) {
                            for(i4=0;i4<mess3.children.length; i4++) {
                                var mess4 = mess3.children[i4];
                                if(!mess4.IsRead) {
                                    return true;
                                }
                                if(mess4.children) {
                                    for(i5=0;i5<mess4.children.length; i5++) {
                                        var mess5 = mess4.children[i5];
                                        if(!mess5.IsRead) {
                                            return true;
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}

    


//создать ветку
function CreateNode(mess) {
    //isRead
    //var div_isread = document.createElement('div');
    //div_isread.className = "div_isread";
    //div_isread.innerHTML = "Прочитано<input type='checkbox' />";
    //время
    var bTime = document.createElement('text');
    bTime.className = "time";
    bTime.innerHTML = /*"<img src=\"/Content/images/time.png\" ></img>"  +*/ mess.DateTime.replace(" ", "<br />");


           
    //иконка
    var aIcon = document.createElement('img');
    if(mess.Type == "1")
        aIcon.setAttribute("src", '/Content/images/error.png');
    else if (mess.Type == "2")
        aIcon.setAttribute("src", '/Content/images/question.png')
    else aIcon.setAttribute("src", '/Content/images/system.png')
    //div для иконки
    var div_icon = document.createElement('div');
    div_icon.className = "div_icon";
    div_icon.appendChild(aIcon);

    //див слева со временем и иконкой
    var dv_left = document.createElement('div');
    dv_left.className = "div_left";
    dv_left.appendChild(div_icon);
    dv_left.appendChild(bTime);

    //див имя
    var textName = document.createElement('div');
    textName.className = "textName";
    textName.innerHTML = "<img src=\"/Content/images/personal.png\" ></img>" + mess.UserName + ": " + mess.Type;

    //див с текстом сообщения
    var text = document.createElement('div');
    text.className = "textMess";
    text.innerHTML = mess.Question.replace("<img", "<br /><img") + " ";
    //кнопка раскрыть 
    var div_expandButton = document.createElement('div');
    div_expandButton.className = "div_expandButton";    
    {
        var div_showhide = document.createElement('div');
        {
            div_showhide.className = "div_showhide"; 
            div_showhide.innerHTML = "<a onclick =\"showhide(this)\" ><img src='/Content/images/expand.png' />Развернуть</a>";
        }
        var div_expand = document.createElement('div');
        {
            div_expand.className = "div_expand";
            {
                var check_img = document.createElement('img');
                if(mess.IsRead) {
                    check_img.setAttribute("src", '/Content/images/checked.png');
                }
                else {
                    check_img.setAttribute("src", '/Content/images/unchecked.png');
                }
            }
            div_expand.innerHTML = "<a onclick=\"javascript:__doPostBack('SetMessageRead','"+ mess.Id + "')\">"/*+ check_img.outerHTML*/ + "Прочитано</a>";
        }
        var div_newmessbtn = document.createElement('div');
        {
            div_newmessbtn.className = "div_newmessbtn";
            div_newmessbtn.innerHTML = "<a onclick=\"show('block', '1','" + mess.Id + "'); return false;\" ><img src = \"/Content/Images/message_1.png\" /> Написать </a >";
        }
        div_expandButton.innerHTML = div_showhide.outerHTML + div_expand.outerHTML + div_newmessbtn.outerHTML;
                
    }
    //главный див
    var dv = document.createElement('div');
    dv.className = "mess";
    if(!mess.IsRead) {
        //text.classList += " not_read show";
    }
    var div_name_text = document.createElement('div_name_text');
    div_name_text.className = "div_name_text";

    //div_name_text.appendChild(textName);
    //div_name_text.appendChild(text);


    dv.appendChild(dv_left);
    dv.appendChild(textName);
    //dv.appendChild(div_expandButton);
    dv.appendChild(text);
    //dv.appendChild(div_name_text);
    
            
    return dv.outerHTML;
}

function showhide(le) {
    el = le.parentElement.parentElement.parentElement.children[3];
    if (jQuery(el).hasClass("show")) {
        jQuery(el).removeClass("show");
        jQuery(el).animate({ height: '1em' }, 1000);
        le.innerHTML = "<img src='/Content/images/expand.png' />Развернуть";
    }
    else {
        jQuery(el).addClass("show");
        jQuery(el).css("height", "100%");
        var h = jQuery(el).height();
        jQuery(el).css("height", "1em");
        jQuery(el).animate({ height: h }, 1000);
        le.innerHTML = "<img src='/Content/images/expand_2.png' />Свернуть";
    }
};

function show(state, reason, parentId) {
    if (reason == '1') {
        document.getElementById('reason').innerHTML = 'Новое сообщение';
        document.getElementById('MainContent_RadioButtonList1').style.display = 'block';
    }
    if (reason === '2') {
        document.getElementById('reason').innerHTML = 'Укажите причину';
        document.getElementById('MainContent_RadioButtonList1').style.display = 'none';
    }
    $("#MainContent_HiddenField1").val(parentId);
    document.getElementById('window').style.display = state;			
    document.getElementById('wrap').style.display = state;
    document.getElementById('window_rejectTask').style.display = state;
}

function showReject(state) {
    document.getElementById('window_rejectTask').style.display = state;
    document.getElementById('wrap').style.display = state;
}

function show_rate(state){
    document.getElementById('rate').style.display = state;			
    document.getElementById('wrap').style.display = state;
}

function tree_toggle(event) {
    event = event || window.event
    var clickedElem = event.target || event.srcElement

    if (!hasClass(clickedElem, 'Expand')) {
        return // клик не там
    }

    // Node, на который кликнули
    var node = clickedElem.parentNode
    if (hasClass(node, 'ExpandLeaf')) {
        return // клик на листе
    }

    // определить новый класс для узла
    var newClass = hasClass(node, 'ExpandOpen') ? 'ExpandClosed' : 'ExpandOpen'
    // заменить текущий класс на newClass
    // регексп находит отдельно стоящий open|close и меняет на newClass
    var re =  /(^|\s)(ExpandOpen|ExpandClosed)(\s|$)/
    node.className = node.className.replace(re, '$1'+newClass+'$3')
}


function hasClass(elem, className) {
    return new RegExp("(^|\\s)"+className+"(\\s|$)").test(elem.className)
}

function SaveFile(uri, filename) {
    var link = document.createElement('a');
    document.body.appendChild(link); // Firefox requires the link to be in the body
    link.download = filename;
    link.href = uri;
    link.click();
    document.body.removeChild(link); // remove the link when done
}
