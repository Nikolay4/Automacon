<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="_Task.aspx.cs" Inherits="DashBoard.Tasks._Task" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <link rel="stylesheet" type="text/css" href="https://cdn.bootcss.com/font-awesome/4.4.0/css/font-awesome.css">
    <link rel="stylesheet" type="text/css" href="https://cdn.bootcss.com/bootstrap/3.3.5/css/bootstrap.css">
    <link rel="stylesheet" type="text/css" href="https://cdn.bootcss.com/summernote/0.8.1/summernote.css">
    <link href="/Content/Site.css" rel="stylesheet" />

    <%-- CKEditor --%>
    <script type="text/javascript" src="/Scripts/jquery-1.10.2.js"></script>
    <script type="text/javascript" src="/Content/ckeditor/ckeditor.js"></script>
    <script type="text/javascript" src="/Content/ckeditor/adapters/jquery.js"></script>
    <%-- /CKEditor --%>
    <asp:UpdatePanel ID="UpdatePanel1" runat="server">
        <ContentTemplate>
            <script type="text/javascript">
                $(function () {
                    CKEDITOR.replace('<%=txtCkEditor.ClientID %>',
                    {
                        filebrowserImageBrowseLinkUrl: '/UploadScreenshot.ashx',
                        filebrowserBrowseUrl: '/UploadScreenshot.ashx',
                        filebrowserUploadUrl: '/UploadScreenshot.ashx',
                        filebrowserImageUploadUrl: '/Upload.ashx',
                    }); //path to “Upload.ashx”
                });

                $(function () {
                    CKEDITOR.replace('<%=TextBox1.ClientID %>',
                    {
                        toolbar: 'Custom',
                        toolbarStartupExpanded : false,
                        toolbarCanCollapse  : false,
                        toolbar_Custom: [],
                        filebrowserImageBrowseLinkUrl: '/UploadScreenshot.ashx',
                        filebrowserBrowseUrl: '/UploadScreenshot.ashx',
                        filebrowserUploadUrl: '/UploadScreenshot.ashx',
                        filebrowserImageUploadUrl: '/Upload.ashx',
                        height: '500px',
                    });
                });
            </script>
            <div id="div_task" style="border: 1px solid #C0C0C0; padding: 20px; overflow:auto; margin: 10px; text-align: left; clip: rect(auto, auto, auto, auto);">
                <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="true">
                    <p class="text-danger">
                        <asp:Literal runat="server" ID="FailureText" Text="" />
                    </p>
                </asp:PlaceHolder>
                <div style="float: left;">
                    <asp:Button ID="Button_Save" CssClass="Button" runat="server" Text="Записать" OnClick="Button_SaveClick" />
                    <asp:Button ID="Button_Back" CssClass="Button" runat="server" Text="Вернуться без записи" OnClick="Button_Back_Click" />
                </div>
                <div style="text-align: center;" id="accept">
                    <button id="accept_button" onclick="show_rate('block'); return false;" class="Button_ok">
                        <img src="/Content/Images/button_ok.png" />Принято</button>
                    <button id="accept_button" class="Button_error" style="padding-left: 10px;" onclick="show('block', '2'); return false;">
                        <img src="/Content/Images/button_close.png" />Не принято</button>
                </div>
                <div style="padding-top: 8px; float: left; clear: left">
                    <asp:Label ID="Label_Descr" runat="server" Font-Size="Large" ForeColor="#5D7B9D"></asp:Label></div>
                <div style="float: left; clear: left">
                    <asp:PlaceHolder runat="server" Visible="false" ID="PlaceHolder_Newtask">
                        <img src="/Content/images/task.png" /><asp:Label ID="Label_NewTask" Text="Новое задание" runat="server" ForeColor="#5D7B9D"></asp:Label>
                    </asp:PlaceHolder>
                </div>
                <div style="padding-top: 8px; clear: left; float: left;">
                    <asp:Label CssClass="task_label" ID="Label_Number" runat="server" Text="Номер: "></asp:Label><asp:TextBox ID="TextBox_Number" runat="server" Text="" BorderColor="#C0C0C0" BorderStyle="Solid" BorderWidth="1" Enabled="False" CssClass="task_textbox"></asp:TextBox>
                    <asp:Label CssClass="task_label" ID="Label_Status" runat="server" Text="Статус: "></asp:Label><asp:TextBox ID="TextBox_Status" runat="server" Text="" BorderColor="#C0C0C0" BorderStyle="Solid" BorderWidth="1" Enabled="False" CssClass="task_textbox"></asp:TextBox>
                    <asp:Label CssClass="task_label" ID="Label_Type" runat="server" Text="Тип задачи<text class='textred'>*</text>: "></asp:Label><asp:DropDownList ID="DropDownList_Type" runat="server" BackColor="#E7EEF5" BorderColor="#C0C0C0" Height="24px" BorderStyle="Solid" BorderWidth="1" CssClass="task_textbox"></asp:DropDownList>
                    <asp:Label CssClass="task_label" ID="Label_Date" runat="server" Text="От: "></asp:Label><asp:TextBox ID="TextBox_Date" runat="server" Text="" BorderColor="#C0C0C0" BorderStyle="Solid" BorderWidth="1" Enabled="False" CssClass="task_textbox"></asp:TextBox>
                    <asp:Label CssClass="task_label" ID="Label_Contact" runat="server" Text="Ответственный: "></asp:Label><asp:TextBox ID="TextBox_Contact" runat="server" Text="" BorderColor="#C0C0C0" BorderStyle="Solid" BorderWidth="1" Enabled="False" Wrap="False" CssClass="task_textbox" Width="300px"></asp:TextBox>
                </div>

                <div style="padding-top:8px; float:left; width:100%;clear:left">
                    <div style="float:left">
                        <asp:Label CssClass="task_label" ID="Label_Title" runat="server" Text="Название<text class='textred'>*</text>: "></asp:Label>
                    </div>
                    <div style="float:left; width:93%">
                        <asp:TextBox ID="TextBox_Title" runat="server" BorderColor="#C0C0C0" BorderStyle="Solid" BorderWidth="1" CssClass="task_textbox" Enabled="false" Text="" Width="100%" Wrap="False"></asp:TextBox>
                    </div>
                </div>
                <div class="korpus" style="width:100%; padding-top:20px;float:left;">
                    <input type="radio" name="odin" checked="checked" id="vkl1"/><label class="lbl" for="vkl1">описание</label><input type="radio" name="odin" id="vkl2"/><label class="lbl" for="vkl2">история по заданию</label>
                    <div style="overflow:auto;">
                        <div style="width: 100%; float: left;">
                            <asp:Label ID="Label3" runat="server" Text="Описание<text class='textred'>*</text>: "></asp:Label>
                            <asp:TextBox ID="txtCkEditor" TextMode="MultiLine" runat="server"></asp:TextBox>
                        </div>
                        <div style="width: 100%; float: left; padding-top: 10px; height: 500px;">
                            <asp:Label ID="Label2" runat="server" Text="Результат: "></asp:Label>
                            <div id="div_result" style="border: 1px solid #C0C0C0; height: 100%; width: 100%; float: right; background-color: #FFFFFF; overflow: scroll; text-align: left;">
                                <asp:Literal ID="Literal_2" runat="server" />
                            </div>
                        </div>
                    </div>
                    <div>
                        <div style="padding-left: 10px; height:auto" onclick="tree_toggle(arguments[0])"  id="div_messages">

                            <%--<asp:TreeView ID="sampleTree" runat="server"></asp:TreeView>--%>
                        </div>
                    </div>
                </div>
                <!-- Задний прозрачный фон-->
                <div onclick="show('none'); show_rate('none')" id="wrap"></div>

                <!-- Само окно-->
                <div id="window">
                    <img class="close" onclick="show('none')" src="http://sergey-oganesyan.ru/wp-content/uploads/2014/01/close.png">
                    <div id="reason" style="text-align: center; margin-top: 10px; font-size: large"></div>
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem><img src="/Content/Images/question.png" />Вопрос</asp:ListItem>
                        <asp:ListItem><img src="/Content/Images/error.png" />Ошибка</asp:ListItem>
                    </asp:RadioButtonList>
                    <div style="padding-top: 20px">
                        <asp:TextBox ID="TextBox1" runat="server"></asp:TextBox>
                    </div>
                    <div style="float: right; padding: 10px">
                        <asp:Button CssClass="Button" ID="Button_SendMessage" runat="server" Text="Записать" OnClick="Button_SendMessage_Click" ForeColor="#0066FF" />
                    </div>
                </div>
                    
                <div id="rate">
                    <div class="rate_title">Пожалуйста, оцените выполнение задания</div>
                    <div class="rate_radio">
                        <asp:RadioButtonList ID="RadioButtonList2" runat="server">
                            <asp:ListItem><div class="rate star">☆</div><div class="rate text">Плохо</div></asp:ListItem>
                            <asp:ListItem><div class="rate star">☆☆</div><div class="rate text">Хорошо</div></asp:ListItem>
                            <asp:ListItem><div class="rate star">☆☆☆</div><div class="rate text">Отлично</div></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div class="rate_button">
                        <asp:Button CssClass="ButtonAcceptTask" runat="server" ID="ButtonAcceptTask" Text="Ок" Enabled="false" />
                    </div>
                </div>
            </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <style>
    </style>
    <script type="text/javascript">
        $(document).ready(function () {

            populateMessages();
        });

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
        function populateMessages(id, el) {
            var messages = <%= getNode() %>;
            var map = {}, node, roots = [];
            //if(messages.lenght > 0)
                for (var i = 0; i < messages.length; i += 1) {
                    node = messages[i];
                    node.children = [];
                    map[node.Id] = i; // use map to look-up the parents
                    if (node.ParentId !== 0) {
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

        //формируем дерево
        function createTree(container, obj) {
            container.innerHTML = createTreeText(obj);
            open();
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
                    var aa = IsChildRead(obj[kk]);
                    if(IsChildRead(obj[kk]))
                        var IsLeaf = "ExpandOpen";
                    else 
                        var IsLeaf = "" ;
                }
                    
                li += '<li class="Node '+ IsLeaf +'">' + '<div class="Expand"></div><div class="Content">'+ CreateNode(obj[kk])+'</div>' + createTreeText(obj[kk].children) + '</li>';
            }
            if (li) {
                var ul = '<ul class="Container '+ IsRoot +'">' + li + '</ul>'
            }
            return ul || '';
        }


        function IsChildRead(mess) {
            for(i=0;i<mess.children.length; i++) {
                if(!mess.children[i].IsRead)
                    return true;
            }
        }

        //раскрыть ветвь, если есть непрочитанные(не полностью работает)
        function open() {
            var lis=$("li.Node");
            for(i=0;i<lis.length;i++)
            {
                if($(lis[i]).find("*").hasClass("ExpandOpen") > 0) {
                    $(lis[i]).addClass("ExpandOpen");
                }
                else {
                    $(lis[i]).addClass("ExpandClose");
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
            bTime.innerHTML = mess.DateTime;
            //иконка
            var aIcon = document.createElement('img');
            if(mess.Type)
                aIcon.setAttribute("src", '/Content/images/error.png');
            else 
                aIcon.setAttribute("src", '/Content/images/question.png')
            aIcon.attribute
            //див слева со временем и иконкой
            var dv_left = document.createElement('div');
            dv_left.className = "div_left";
            dv_left.appendChild(aIcon);
            dv_left.appendChild(bTime);
            //див с текстом сообщения
            var text = document.createElement('div');
            text.className = "textMess";
            text.innerHTML = mess.Question.replace("<img", "<br /><img");
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
                    div_expand.innerHTML = "<a class=''>"+ check_img.outerHTML + "Прочитано</a>";
                }
                var div_newmessbtn = document.createElement('div');
                {
                    div_newmessbtn.className = "div_newmessbtn";
                    div_newmessbtn.innerHTML = "<a onclick=\"show('block', '1'); return false;\" ><img src = \"/Content/Images/new_message.png\" /> Написать </a >";
                }
                div_expandButton.innerHTML = div_showhide.outerHTML + div_expand.outerHTML + div_newmessbtn.outerHTML;
                
            }
                //главный див
            var dv = document.createElement('div');
            dv.className = "mess";
            if(!mess.IsRead)
                dv.classList += " not_read";
            dv.appendChild(dv_left);
            dv.appendChild(div_expandButton);
            //dv.appendChild(div_isread);
            dv.appendChild(text);
            return dv.outerHTML;
        }
    </script>
    <style>
        text.textred {
            color:red;
        }

        .rate_button {
            float: right;
        }
        .rate_title {
            font-family: cursive;
            text-align: center;
        }
        .rate_radio {
            padding-top: 15px;
        }
        .rate.star {
            color:gold;
            text-align:center;
            width:60px;
            border:1px solid blue;
            border-radius:20px;
        }

        .rate.text {
            margin-left:5px;
            color:blue;
        }

        .rate {
            display:inline-block;
        }

        #rate {
            border-radius: 30px;
            border: 3px dashed blue;
            width:400px;
            height:200px;
            margin: 50px auto;
            display: none;
            background: #fff;
            z-index: 200;
            position: fixed;
            left: 0;
            right: 0;
            top: 0;
            bottom: 0;
            padding: 16px;
        }

        /* открытое поддерево */
        .ExpandClosed > .ExpandOpen .Container{
            display:block;
        }
 
        .ExpandOpen .Expand {  
            background: url(/Content/images/minus.gif) no-repeat;  
        }

        /* закрытое поддерево */
        .ExpandClosed .Expand {
            background: url(/Content/images/plus.gif) no-repeat;
        }

        /* лист */
        .ExpandLeaf .Expand {
            /*background-image: url(/Content/images/leaf.gif);*/
            background-image:none;
        }

        /*открытый контейнер*/
        .ExpandOpen .Container {
	        display: block;
        }

        /*закрытый контейнер*/
        .ExpandClosed .Container {
	        display: none;
        }

          /* иконка скрытого/раскрытого поддерева или листа
            сами иконки идут дальше, здесь общие свойства
         */
        .Expand {
            margin: 2px;
            width: 11px;
            height: 11px;
            /* принцип двухколоночной верстки.*/ 
            /*float:left; и width дива Expand + margin-left дива Content */ 
            float: left; 
        } 

        /* содержание (заголовок) узла 
         .Content {
             чтобы не налезать на Expand 
            margin-left:18px;
             высота заголовка - как минимум равна Expand 
                Т.е правая колонка всегда выше или равна левой.
                Иначе нижний float будет пытаться разместиться на получившейся ступеньке
             
            min-height: 18px; 
        } */  

         /* все правила после * html выполняет только IE6 
        * html .Content {
            height: 18px; 
        }*/

        /*div с сообщением*/
        .mess {
            margin-left: 15px;
            position:relative;
        }

        /*у непрочитанных другой фон*/
        .not_read {
            background-color:bisque;
        }

        /**/
        .div_left{
            position:absolute;
            float: left;
            width: 85px;
            height: 100%;
            border-right: 1px solid silver;
        }

        /*листья без отступа */
        .ExpandLeaf .mess {
            margin-left:0;
        }

        .div_isread {
            padding-left:5px
        }

        /*время*/
        .time {
            width:80px;
            white-space: nowrap;
            text-align: center;
        }

        .div_expandButton {
            margin-bottom:10px;
           
        }

        .div_expandButton, .textMess {
            margin-left: 100px;
        }


        .textMess {
            height: 1em;
            line-height: 1em; 
            overflow: hidden;
        }

        .Container {
            border-radius:2px;
            border:1px solid silver;
            margin: -1px;
            /*border-bottom:none;
            border-top:none;*/

            /*padding:3px;*/
            padding-left:100px;
        }
        .Container li {
            list-style-type: none; /* убрать кружочки/точечки */
        }
        .Container.IsRoot {
            border: none;
            -webkit-padding-start: 0px;
            padding-left:0px;
        }
        .Content {
            border-radius:2px;
            border:1px solid silver;
            margin:-1px;
        }

        .Content:hover {
            -webkit-box-shadow: 0px 0px 19px -2px rgba(0,0,0,0.48);
            -moz-box-shadow: 0px 0px 19px -2px rgba(0,0,0,0.48);
            box-shadow: 0px 0px 19px -2px rgba(0,0,0,0.48);
        }

        .div_showhide, .div_expand, .div_newmessbtn {
            min-width:100px;
            padding: 5px 5px 0 5px;
            display: inline-block;
            cursor:pointer
        }

        ul img {
            vertical-align: sub;
        }
    </style>


    <script type="text/javascript">
        function SetRead(Id) {

        }

        function showhide(le) {
            el = le.parentElement.parentElement.parentElement.children[2];
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
    </script>
    <script type="text/javascript">
        function show(state, reason) {
            if (reason == '1') {
                document.getElementById('reason').innerHTML = 'Новое сообщение';
                document.getElementById('MainContent_RadioButtonList1').style.display = 'block';
            }
            if (reason === '2') {
                document.getElementById('reason').innerHTML = 'Укажите причину';
                document.getElementById('MainContent_RadioButtonList1').style.display = 'none';
            }
		    document.getElementById('window').style.display = state;			
		    document.getElementById('wrap').style.display = state;
        }

        function show_rate(state){
            document.getElementById('rate').style.display = state;			
            document.getElementById('wrap').style.display = state;
        }

    </script>
    <script type="text/javascript">
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
    </script>
</asp:Content>

