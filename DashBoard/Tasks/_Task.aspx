<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" ValidateRequest="false" AutoEventWireup="true" CodeBehind="_Task.aspx.cs" Inherits="DashBoard.Tasks._Task" %>
<%@Register Assembly="CKEditor.NET" Namespace="CKEditor.NET" TagPrefix="CKEditor"%>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent"  runat="server">
    <link href="/Content/Site.css" rel="stylesheet" />
        <asp:UpdatePanel ID="UpdatePanel2" runat="server" UpdateMode="Always" EnableViewState="true">
           <ContentTemplate>
                <link rel="stylesheet" type="text/css" href="https://cdn.bootcss.com/font-awesome/4.4.0/css/font-awesome.css">
                <link rel="stylesheet" type="text/css" href="https://cdn.bootcss.com/bootstrap/3.3.5/css/bootstrap.css">
                <link rel="stylesheet" type="text/css" href="https://cdn.bootcss.com/summernote/0.8.1/summernote.css">
                <link href="/Content/Site.css" rel="stylesheet" />
                <script src="/Scripts/Task.js"></script>
                <script type="text/javascript">
                    $(document).ready(function () {
                        sm();
                        document.getElementById('window_rejectTask').style.display = "none";
                    });
        
                    function sm() {
                        var msgs = $("#MainContent_HiddenField2").val();
                        //populateMessages(msgs);
                    }
                </script>
                <script type="text/javascript">
                    Sys.Application.add_load(sm);
                </script>
        <div id="div_task" style="border: 1px solid #C0C0C0; padding: 20px; overflow: auto; margin: 10px; text-align: left; clip: rect(auto, auto, auto, auto);">
            <asp:PlaceHolder runat="server" ID="ErrorMessage" Visible="true">
                <p class="text-danger">
                    <asp:Literal runat="server" ID="FailureText" Text="" />
                </p>
            </asp:PlaceHolder>
            <div style="white-space:nowrap; display:flex;">
                <asp:Button ID="Button_Save" CssClass="btn_new_white margin_right" runat="server" Text="Записать" OnClick="Button_SaveClick" />
                <asp:Button ID="Button_Back" CssClass="btn_new_white" runat="server" Text="Вернуться без записи" OnClick="Button_Back_Click" />
                <asp:UpdateProgress ID="updateProgress" runat="server">
                    <ProgressTemplate>
                        <div style="text-align: left; display:inline-block; height: 100%; width: 100%; top: 0; right: 0; left: 10px; z-index: 9999999;">
                            <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="/Content/images/preloader_task.gif" AlternateText="Loading ..." ToolTip="Loading ..."  />
                        </div>
                    </ProgressTemplate>
                </asp:UpdateProgress>
            </div>
            <asp:PlaceHolder Visible="false" runat="server" ID="PlaceHolder_AcceptTask">
                <div style="text-align: center;" id="accept">
                    <button id="accept_button" onclick="show_rate('block'); return false;" class="btn_new_white">
                        <img src="/Content/Images/button_ok.png" />Принято</button>
                    <button id="accept_button" class="btn_new_white" style="padding-left: 10px;" onclick="showReject('block'); return false;">
                        <img src="/Content/Images/button_close.png" />Не принято</button>
                </div>
            </asp:PlaceHolder>
            <div style="padding-top: 8px; float: left; clear: left">
                <asp:Label ID="Label_Descr" runat="server" Font-Size="Large" ForeColor="#5D7B9D"></asp:Label>
            </div>
            <div style="float: left; clear: left">
                <asp:PlaceHolder runat="server" Visible="false" ID="PlaceHolder_Newtask">
                    <img src="/Content/images/task.png" /><asp:Label ID="Label_NewTask" Text="Новое задание" runat="server" ForeColor="#5D7B9D"></asp:Label>
                </asp:PlaceHolder>
            </div>
            <div style="padding-top: 8px; clear: left; float: left;">
                <asp:Label CssClass="task_label" ID="Label_Type" runat="server" Text="Тип задачи<text class='textred'>*</text>: "></asp:Label><asp:DropDownList ID="DropDownList_Type" runat="server" Height="24px" CssClass="task_textbox"></asp:DropDownList>
                <asp:Label CssClass="task_label" ID="Label_Contact" runat="server" Text="Ответственный: "></asp:Label><asp:DropDownList ID="DropDownList_Contact" runat="server" Height="24px" CssClass="task_textbox" Width="300px"></asp:DropDownList>

                <asp:Label CssClass="task_label" ID="Label_ExpectedTime" runat="server" Text="Плановая дата передачи на проверку: "></asp:Label><asp:TextBox ID="TextBox_ExpectedTime" runat="server" Text="" Enabled="False" CssClass="task_textbox"></asp:TextBox>

                <%--<asp:Label CssClass="task_label" ID="Label_Number" runat="server" Text="Номер: "></asp:Label><asp:TextBox ID="TextBox_Number" runat="server" Text="" Enabled="False" CssClass="task_textbox"></asp:TextBox>--%>
                <asp:Label CssClass="task_label" ID="Label_Status" runat="server" Text="Статус: "></asp:Label><asp:TextBox ID="TextBox_Status" runat="server" Text="" Enabled="False" CssClass="task_textbox"></asp:TextBox>
                <%--<asp:Label CssClass="task_label" ID="Label_Date" runat="server" Text="От: "></asp:Label><asp:TextBox ID="TextBox_Date" runat="server" Text="" Enabled="False" CssClass="task_textbox"></asp:TextBox>--%>
            </div>

            <div style="padding-top: 16px; width: 100%; clear: both; position: relative;">
                <div style="float: left; width: 75px">
                    <asp:Label CssClass="task_label" ID="Label_Title" runat="server" Text="Название<text class='textred'>*</text>: "></asp:Label>
                </div>
                <div style="margin-left: 80px;">
                    <asp:TextBox ID="TextBox_Title" runat="server"
                        CssClass="task_textbox" Enabled="false" Text="" Width="100%" Wrap="False"></asp:TextBox>
                </div>
            </div>
            <div class="korpus" style="width: 100%; padding-top: 20px; float: left;">
                <input type="radio" name="odin" checked="checked" id="vkl1" /><label class="lbl" for="vkl1">описание</label><%--<input type="radio" name="odin" id="vkl2" /><label class="lbl" for="vkl2">история по заданию</label>--%><div style="overflow: auto;">
                    <div style="width: 100%; float: left;">
                        <asp:Label ID="Label3" runat="server" Text="Описание<text class='textred'>*</text>: "></asp:Label>
                        <CKEditor:CKEditorControl AutoUpdateElement="true" Width="100%" ID="txtCkEditor" Height="400px" AutoPostBack="false" BasePath="/Content/ckeditor" runat="server"></CKEditor:CKEditorControl>
                        <%--<asp:TextBox ID="txtCkEditor" TextMode="MultiLine" runat="server"></asp:TextBox>--%>
                    </div>
                    <div style="width: 100%; float: left; padding-top: 10px; height: inherit;">
                        <asp:Label ID="Label2" runat="server" Text="Результат: "></asp:Label><br />
                        <asp:PlaceHolder ID="PlaceHolder_AttachedFiles" Visible="false" runat="server">
                            <asp:Label CssClass="attachment_title" ID="Label1" Font-Bold="false" Font-Italic="true" runat="server" Text="Прикрепленные файлы" Font-Size="Small"></asp:Label>
                            <asp:DataList ClientIDMode="Inherit" CssClass="attachment_table" ID="AttachedFiles" runat="server" RepeatDirection="Horizontal">
                                <ItemTemplate>
                                    <div id="files" style="display:flex">
                                        <div id="download_file" style="padding-bottom:5px">
                                            <asp:Image ImageUrl="~/Content/images/attachment.png" runat="server" ID="Image" />
                                            <a href="<%# Eval("Url") %>" download><%# Eval("FileName") %></a>
                                            <%--<asp:LinkButton runat="server" ID="LinkButton1" OnClientClick="" OnClick="Download" ToolTip='<%# Eval("Url") %>' Text='<%# Eval("FileName") %>' />--%>
                                             </div>
                                        <div id="open_file" style="padding-left:5px">
                                            <a target="_blank" href='<%# Eval("Url") %>'><input class="btn_new_white" type="button" value="Открыть"></a>
                                        </div>
                                    </div>
                                </ItemTemplate>
                            </asp:DataList>
                        </asp:PlaceHolder>
                        <div id="div_result" style="border: 1px solid #C0C0C0; height: 400px; width: 100%; float: right; background-color: #FFFFFF; overflow: scroll; text-align: left;">
                            <asp:Literal ID="Literal_2" runat="server" />
                        </div>
                    </div>
                </div>
                <div>
                    <%--<div style="padding:0 0 10px 10px"><button class="btn_new_white" onclick="show('block', '1', 0); return false;">
                        <img src="/Content/Images/message_1_big.png" />Новое сообщение</button></div>--%>
                    <div style="padding-left: 10px; height: auto; min-height:400px" onclick="tree_toggle(arguments[0])" id="div_messages">
                    </div>
                </div>
            </div>
            <!-- Задний прозрачный фон-->
                <div onclick="show('none'); show_rate('none')" id="wrap"></div>

                <!-- Само окно-->
                <div id="window">
                    <img class="close" onclick="show('none')" src="http://sergey-oganesyan.ru/wp-content/uploads/2014/01/close.png">
                    <div id="reason" style="text-align: center; margin-top: 10px; font-size: large">
                    </div>
                    <asp:RadioButtonList ID="RadioButtonList1" runat="server" RepeatDirection="Horizontal">
                        <asp:ListItem><img src="/Content/Images/question.png" />Вопрос</asp:ListItem>
                        <asp:ListItem><img src="/Content/Images/error.png" />Ошибка</asp:ListItem>
                    </asp:RadioButtonList>
                    <div style="padding-top: 20px">
                       <%-- <asp:HiddenField ID="HiddenField2" Value="<%# DashBoard.Tools.Serializer.SerializeJSon(Task.Messages) %>" runat="server" />--%>
                        <asp:HiddenField ID="HiddenField1" runat="server" />
                        <CKEditor:CKEditorControl ID="TextBox1" ClientIDMode="Static" AutoPostBack="true" Height="500px" runat="server" BasePath="/Content/ckeditor" ToolbarStartupExpanded="False" ToolbarCanCollapse="False" Toolbar="None" ResizeMaxHeight="500" ResizeMaxWidth="3000" DefaultLanguage="ru"></CKEditor:CKEditorControl>
                    </div>
                    <div style="float: right; padding: 10px">
                        <asp:Button CssClass="btn_new_white" ID="Button_SendMessage" runat="server" Text="Записать" OnClick="Button_SendMessage_Click" />
                    </div>
                </div>
                <div id="window_rejectTask">
                     <div class="rate_title">Укажите причину</div>
                     <div style="padding-top: 20px">
                        <CKEditor:CKEditorControl ID="CKEditor_Reject" AutoPostBack="true" Height="500px" runat="server" BasePath="/Content/ckeditor" ToolbarStartupExpanded="False" ToolbarCanCollapse="False" Toolbar="None" ResizeMaxHeight="500" ResizeMaxWidth="3000" DefaultLanguage="ru"></CKEditor:CKEditorControl>
                    </div>
                    <div style="float: right; padding: 10px">
                        <asp:Button CssClass="btn_new_white" ID="Button_RejectTask" runat="server" Text="Записать" OnClick="Button_RejectTask_Click" />
                    </div>
                    <div style="float: right; padding: 10px">
                        <button class="btn_new_white" onclick="show('none'); show_rate('none');return false;">Закрыть</button>
                    </div>

                </div>

                <div id="rate">
                    <div class="rate_title">Пожалуйста, оцените выполнение задания</div>
                    <div class="rate_radio">
                        <asp:RadioButtonList ID="RadioButtonList2"  runat="server">
                            <asp:ListItem ><div class="rate star">☆</div><div class="rate text">Плохо</div></asp:ListItem>
                            <asp:ListItem Selected="True"><div class="rate star">☆☆</div><div class="rate text">Хорошо</div></asp:ListItem>
                            <asp:ListItem><div class="rate star">☆☆☆</div><div class="rate text">Отлично</div></asp:ListItem>
                        </asp:RadioButtonList>
                    </div>
                    <div style="float:right">
                        <asp:Button CssClass="btn_new_white" OnClick="ButtonAcceptTask_Click" runat="server"  ID="ButtonAcceptTask" Text="Ок" />
                    </div>
                </div>
        </div>
        </ContentTemplate>
    </asp:UpdatePanel>
    <iframe id="downloadframe" style="display:none;"></iframe>
    <script>
        function Download(url) {
            window.open(url);
       // document.getElementById('downloadframe').src = url;
        };
    </script>
    <%--        </ContentTemplate>
    </asp:UpdatePanel>--%>
    <style>
    </style>

    <style>
        #container {
            display:inherit;
        }

        .display_none {
            display:none;
        }

        .attachment_table, .attachment_table tr, .attachment_table td, .attachment_table th {
            /*border:none !important;*/
        }
        .attachment_table td {
            /*border:1px solid silver;
            padding:5px;*/
            padding-right:15px;
            border-left: 2px solid gray;
            padding-left: 2px;
        }

        .attachment_table, .attachment_title {
            margin-left:10px;
            margin-bottom:5px;
        }

        .download {
            border:1px solid #C0C0C0;
        }

        text.textred {
            color:red;
        }

        .rate_
         {
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
        .ExpandClosed .ExpandOpen .Container{
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



        /**/
        .div_left{
            overflow:hidden;
            display: inline-flex;
            position:absolute;
            float: left;
            width: 110px;
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
            font-style:oblique;
            width:80px;
            white-space: nowrap;
            text-align: center;
        }

        .div_expandButton {
            /*display:none;*/
            font-size:small;
        }

        .div_expandButton, .textMess, .textName {
            margin-left: 125px;
        }

        .ExpandLeaf .textMess {
            margin-left:140px;
        }

        .textMess {
            clear:left;
            font-style:oblique;
            padding: 0 5px 0 5px;
            height: 100%;/*2em;*/
            /*line-height: 1em;*/ 
            overflow: hidden;
        }

        .textMess p {
            margin:initial;
        }

        .textName {
            float:left;
            font:italic;
            font-size: small;
        }

        .Container {
            border-radius:2px;
            border:1px solid silver;
            margin: -1px;
            /*border-bottom:none;
            border-top:none;*/

            /*padding:3px;*/
            padding-left:125px;
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
            /*min-width:100px;*/
            padding-left:5px;
            /*padding: 5px 5px 0 5px;*/
            display: inline-block;
            cursor:pointer
        }

        .div_expand {
            display:none;
        }

        .div_name_text {
            position:relative;
        }

        ul img {
            vertical-align: sub;
        }


        /*у непрочитанных другой фон*/
        .not_read {
            height:100%;
            background-color: ActiveCaption;
        }

        .margin_right {
            margin-right:8px;
        }
        /*это нечто ужасное*/
        /*div#container {
            top:0px;
            left:0px;
            right:0px;
        }*/
    </style>
</asp:Content>

  