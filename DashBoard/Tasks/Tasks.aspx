<%@ Page Title="Главная" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"  EnableEventValidation="false" CodeBehind="Tasks.aspx.cs" Inherits="DashBoard.Tasks.Tasks" %>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <style type="text/css">
        .HandleCSS
        {
            width: 20px;
            height: 20px;
            background-color: Red;
        }
        body::-webkit-scrollbar-track
        {
	        -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
	        background-color: #F5F5F5;
        }

    </style>
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Content/colresizable/colResizable-1.6.min.js"></script>
    <script src="../Content/colresizable/colResizable-1.6.js"></script>
    <script src="../Scripts/jquery.mousewheel.js"></script>
    <%--<script type='text/javascript' src='http://ajax.googleapis.com/ajax/libs/jquery/1.3.2/jquery.min.js?ver=1.3.2'></script>--%>
  <%--  <script src="jquery-1.8.2.js" type="text/javascript"></script>
    <link href="jquery-ui.css" rel="stylesheet" type="text/css" />
    <script src="jquery-ui.js" type="text/javascript"></script>--%>

    <%--<script src="../Content/resizable/jquery-1.8.2.js"></script>--%>
    <%--<link href="../Content/resizable/jquery-ui.css" rel="stylesheet" />
    <script src="../Content/resizable/jquery-ui.js"></script>--%>

    <script type="text/javascript">
        $(document).ready(function () {
            //setColResize();
        });

        $(document).bind("click", function (event) {
            document.getElementById("rmenu").className = "hide";
        });

        function DoubleScroll(element) {
            var scrollbar = document.createElement('div');
            scrollbar.appendChild(document.createElement('div'));
            scrollbar.className = "topScroll";
            scrollbar.style.marginTop = "-20px";
            scrollbar.style.overflow = 'auto';
            scrollbar.style.overflowY = 'hidden';
            scrollbar.style.marginLeft = "10px";
            scrollbar.style.overflowX = '-moz-scrollbars-horizontal';
            scrollbar.firstChild.style.width = element.scrollWidth + 'px';
            scrollbar.firstChild.style.minWidth = element.scrollWidth + 'px';
            scrollbar.firstChild.style.paddingTop = '1px';
            scrollbar.firstChild.appendChild(document.createTextNode('\xA0'));
            scrollbar.onscroll = function () {
                element.scrollLeft = scrollbar.scrollLeft;
            };
            element.onscroll = function () {
                scrollbar.scrollLeft = element.scrollLeft;
            };
            element.parentNode.insertBefore(scrollbar, element);
        }

        function setColResize() {

            $(".main_table").mousewheel(function (event, delta) {

                this.scrollLeft -= (delta * 30);

                event.preventDefault();

            });
     


            //DoubleScroll(document.getElementById('main_table'));

            var paginator = $('.paginator').clone();//добавить в фильтры пагинацию
            paginator.appendTo('.pager');
            //var $filters = $('.filters').clone();
            //$('.filters').html(paginator); 

            $("#ctl00_MainContent_GridView_TaskList").colResizable({
                minWidth:25,
                disabledColumns:[0],
                //resizeMode:"overflow",
                liveDrag: true,
                //headerOnly:true,
                postbackSafe: true,
                partialRefresh: true,
            });

            //$("#ctl00_MainContent_GridView_TaskList").resizable();

            $(function () {
                $("#tooltipContainer").hide();
                var timeoutId;
                $(".withtooltip").hover(function (event) {
                    if (!timeoutId) {
                        timeoutId = window.setTimeout(function () {
                            timeoutId = null;
                            $("#tooltip").html("<img src=\"/Content/images/preloader_task.gif\" />");
                            x = event.pageX;
                            y = event.pageY;
                            if (y > 400) { y = y - 400; }
                            $("#tooltipContainer").css({
                                left: x,
                                top: y
                            });
                            $("#tooltipContainer").show();
                            var taskNumber = $(event.target).parent().attr('id');
                            $.ajax({
                                type: "GET",
                                url: "/api/Values/GetTaskDescr?id=" + taskNumber,
                                data: "{}",
                                contentType: "application/json; charset=utf-8",
                                dataType: "json",
                                async: true,
                                cache: false,
                                success: function (data) {
                                    $("#tooltip").html(data);
                                },
                                error: function (data) {
                                    $("#tooltip").html(data);
                                }
                            });
                            x = event.pageX;
                            y = event.pageY;
                            if (y > 400) {

                                y = y - 400;
                            }
                            $("#tooltipContainer").css({
                                left: x,
                                top: y
                            });
                        }, 1000);
                    }
                },

                function () {
                    if (timeoutId) {
                        window.clearTimeout(timeoutId);
                        timeoutId = null;
                    }
                    else if (!($("#tooltipContainer").is(":hover"))) {
                        $("#tooltipContainer").hide();
                    }
                });
            });

            $(function () {
                $("#tooltipContainer").on('mouseleave', function () {
                    $("#tooltipContainer").hide();
                });

                //$("#tooltipContainer").hover(function () {
                //    $("#tooltipContainer").show();
                //});
            });

            //function closeTooltip() {
            //    console.log("close tooltip")
            //    $("#tooltipContainer").hide();
            //}
        }

        function OpenNewTab(number) {
            var url = number;
            //var win = window.open(url, '_blank');
            //win.focus();

            document.getElementById("rmenu").className = "show";
            document.getElementById("rmenu").style.top = mouseY(event) + 'px';
            document.getElementById("rmenu").style.left = mouseX(event) + 'px';
            document.getElementById("rmenu").innerHTML = "<a  href=\"" + url + "\" target='_blank'>Открыть в новой вкладке</a>";
            //window.event.returnValue = false;

            //setTimeout(function () {
            //    document.getElementById("rmenu").className = "hide";
            //}, 3000);

            return false;
        }

        function mouseX(evt) {
            if (evt.pageX) {
                return evt.pageX;
            } else if (evt.clientX) {
                return evt.clientX + (document.documentElement.scrollLeft ?
                    document.documentElement.scrollLeft :
                    document.body.scrollLeft);
            } else {
                return null;
            }
        }

        function mouseY(evt) {
            if (evt.pageY) {
                return evt.pageY;
            } else if (evt.clientY) {
                return evt.clientY + (document.documentElement.scrollTop ?
                document.documentElement.scrollTop :
                document.body.scrollTop);
            } else {
                return null;
            }
        }

    </script>

    <div id="filters" style="vertical-align: top; text-align: center;">
    </div>
    <div>
        <asp:Panel runat="server" DefaultButton="Button2" >
            <asp:UpdatePanel ID="UpdatePanel1" runat="server">
                <ContentTemplate>
                    <script type="text/javascript">
                        Sys.Application.add_load(setColResize);
                    </script>
                    <div style="float:left; padding:10px">
                        <a class="btn_new_white" style="background:-webkit-linear-gradient(to top, #ffffff 0%,#f1f2f4 100%)" href="/Tasks/Task/Add">
                        <img src="/Content/images/add.gif" />
                        <span class="btnName">
                          Создать задание
                        </span>
                      </a>
                    </div>
                    <div style="float:left;  padding:10px; padding-bottom: 1px; width:100px;">
                        <asp:UpdateProgress ID="updateProgress" runat="server">
                            <ProgressTemplate>
                                <div style="text-align: left;vertical-align:middle; height: 100%; width: 100%; top: 2; right: 0; left: 10px; z-index: 9999999;">
                                    <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="/Content/images/table_preloader_old.gif" AlternateText="Loading ..." ToolTip="Loading ..." Height="20px" Width="103px"  />
                                </div>
                            </ProgressTemplate>
                        </asp:UpdateProgress>
                    </div>
                    <div style="width:100%; height: 1px; clear: both;" >.</div>
                    <div class="main_wrap">
                        <div class="wrap_filters" style="">
                            <table class="filters">
                                <tr>
                                    <td>
                                        <asp:Label ID="Label8" runat="server" Text="Статусы"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownList_Statuses" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="True" runat="server" CssClass="filters_dropbox"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label9" runat="server" Text="Типы"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownList_Types" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="True" runat="server" CssClass="filters_dropbox"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label10" runat="server" Text="Ответственный"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownList_Contacts" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="True" runat="server" CssClass="filters_dropbox"></asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Label ID="Label11" runat="server" Text="Показать"></asp:Label>
                                    </td>
                                    <td>
                                        <asp:DropDownList ID="DropDownList_IsActive" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_IsActive_SelectedIndexChanged" CssClass="filters_dropbox">
                                            <asp:ListItem Selected="True" Value="Только активные">Только активные</asp:ListItem>
                                            <asp:ListItem Value="Все">Все</asp:ListItem>
                                        </asp:DropDownList>
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:TextBox ID="TextBox8" Width="120" runat="server"></asp:TextBox>
                                    </td>
                                    <td>
                                        <asp:ImageButton ID="ButtonClearSearch" ImageUrl="~/Content/images/delete.png" runat="server" CssClass="btn_clearsearch" OnClick="ButtonClearSearch_Click" Text="Очистить" ForeColor="Black" />
                                        <asp:Button ID="Button2" runat="server" CssClass="btn_new_white" Text="Поиск" OnClick="Button1_Click" ForeColor="Black" OnClientClick=" " />
                                    </td>
                                </tr>
                                <tr>
                                    <td>
                                        <asp:Button ID="ClearFilrters" runat="server" CssClass="btn_new_white" OnClick="ClearFilters_Click" Text="Очистить фильтры" ForeColor="Black" />
                                    </td>
                                    <td>

                                    </td>
                                </tr>
                            </table>
                        </div>
                        
                        <div class="main_table" id="main_table">
                            <div id="topScroll" style="width:1800px; height:auto; overflow-x:auto"></div>
                            <asp:GridView
                                runat="server"
                                AllowSorting="True"
                                ClientIDMode="AutoID"
                                Width="1800"
                                CellPadding="2"
                                OnSorted="GridView_TaskList_Sorted"
                                OnSorting="GridView_TaskList_Sorting1"
                                AutoGenerateColumns="False"
                                OnRowDataBound="GridView_TaskList_RowDataBound"
                                ShowHeaderWhenEmpty="True"
                                BorderColor="#dedfe1"
                                BorderStyle="Solid"
                                BorderWidth="1px"
                                                                
                                BackColor="White"
                                Font-Size="Small"
                                ForeColor="#666666" EmptyDataText="Список пуст"
                                AllowPaging="True"
                                OnPageIndexChanging="GridView_TaskList_PageIndexChanging"
                                PageSize="20" PagerSettings-NextPageText=">"
                                PagerSettings-PreviousPageText="<"
                                PagerSettings-LastPageText=">>"
                                PagerSettings-FirstPageText="<<"
                                ToolTip=""
                                ID="GridView_TaskList"
                                OnSelectedIndexChanged="GridView_TaskList_SelectedIndexChanged" PageIndex="0">
                                <Columns>
                                    <asp:TemplateField HeaderText="" SortExpression="PaymentImage">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("PaymentImage") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="td_money " />
                                        <ItemStyle Width="" CssClass="td_payment" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Дата" SortExpression="DateTime">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="td_date " />
                                        <ItemStyle Width="" CssClass="td_date" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Тип" SortExpression="Type">
                                        <ItemTemplate>
                                            <asp:Label ID="Label2" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="td_type" />
                                        <ItemStyle Width="" CssClass="td_type" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="<text alt='Планируемая дата передачи на проверку' title='Планируемая дата передачи на проверку'>План.Дата</text>" SortExpression="ExpectedTime">
                                        <ItemTemplate>
                                            <asp:Label ID="Label1" runat="server" Text='<%# Bind("ExpectedTime") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="td_date " />
                                        <ItemStyle Width="" CssClass="td_date" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Статус" SortExpression="Status">
                                        <ItemTemplate>
                                            <asp:Label ID="Label3" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="td_status " />
                                        <ItemStyle Width="" CssClass="td_status" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Номер" SortExpression="Number">
                                        <ItemTemplate>
                                            <asp:Label ID="Label4" runat="server" Text='<%# Bind("Number") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="td_number " />
                                        <ItemStyle Width="" Wrap="False" CssClass="td_number" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Задание" SortExpression="Name">
                                        <ItemTemplate>
                                            <div class="" id='<%# Eval("Number") %>' style="text-overflow: ellipsis; overflow: hidden">
                                                <asp:Label ID="Label5" CssClass="withtooltip" runat="server" Text='<%# Bind("Name") %>'></asp:Label> 
                                            </div>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="td_name " />
                                        <ItemStyle Width="" Wrap="False" CssClass="td_name" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Контакт" SortExpression="Contact">
                                        <ItemTemplate>
                                            <asp:Label ID="Label6" runat="server" Text='<%# Bind("Contact") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="td_contact " />
                                        <ItemStyle Width="" CssClass="td_contact" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Номер акта" SortExpression="InvoiceNumber">
                                        <ItemTemplate>
                                            <asp:Label ID="Label_InvoiceNumber" runat="server" Text='<%# Bind("InvoiceNumber") %>'></asp:Label>
                                        </ItemTemplate>
                                        <HeaderStyle CssClass="td_invoiceNumber " />
                                        <ItemStyle Width="" CssClass="td_invoiceNumber" />
                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Дата принятия" SortExpression="DateAccept" Visible="False">
                                        <ItemTemplate>
                                            <asp:Label ID="Label7" runat="server" Text='<%# Bind("DateAccept") %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>
                                </Columns>
                               
                                <EditRowStyle Wrap="False"></EditRowStyle>
                                <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="True" ForeColor="#6699FF" Font-Size="Small" CssClass="tasks_header" />

                                <PagerSettings FirstPageText="" LastPageText="" NextPageText="" PreviousPageText="" Mode="NumericFirstLast" FirstPageImageUrl="~/Content/images/resultset_first.png" LastPageImageUrl="~/Content/images/resultset_last.png" NextPageImageUrl="~/Content/images/resultset-next.png" PageButtonCount="4" PreviousPageImageUrl="~/Content/images/resultset-previous.png" Position="Bottom"></PagerSettings>

                                <PagerStyle HorizontalAlign="Center" CssClass="paginator" />
                                <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="tasks_row" Wrap="False" />
                                <SortedAscendingHeaderStyle Wrap="True" CssClass="sortasc" BackColor="#FF33CC" />
                                <SortedDescendingHeaderStyle VerticalAlign="Middle" CssClass="sortdesc" BackColor="#003300" />
                            </asp:GridView>
                        </div>
                        
                    </div>
                    <div class="hide" id="rmenu">
                    </div>
                    <div id="tooltipContainer" style="display: table-cell; vertical-align: middle;position:absolute; background:rgb(255, 255, 255); border:2px groove gray; border-radius:3px; padding:5px; height:400px;min-width:800px;max-width:1000px; z-index:9999; overflow:auto"><%--<div onclick="closeTooltip()" class="xclose" style="float:right;cursor:pointer">[x]</div>--%><div id="tooltip"></div></div>
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
    </div>
    
    <style>
        .tasks_header {
            background: #fefcea; /* Для старых браузеров */
            background: linear-gradient(to top, #add5ff, #fefcea);
        }

        .tasks_header th {
            padding-left:10px
        }

        .filters_dropbox {
            color:#666666
        }
        table
        {
          table-layout:fixed;
        }
        .tasks_row td
        { 
            overflow:hidden;
            text-overflow:ellipsis;
            word-wrap:break-word;
        }
        .tasks_row {
            min-height:32px
        }
        /*.td_payment       { width:24px;}*/
        .td_money         { width:25px;min-width:25px;max-width:25px;}
        .td_date          { width:150px;}
        /*.td_type          { width:140px;}
        .td_status        { width:140px;}
        .td_number        { width:120px;}
        .td_name          { width:200px;}
        .td_contact       { width:70px;}*/
        /*.td_invoiceNumber { width:70px;}*/

        .paginator span {
            background: #26B;
            color: #fff;
            border: solid 1px #AAE;
        }
        a.button {
	        background: url(../img/button-gr.png) repeat-x; /* Old browsers */
	        background: -moz-linear-gradient(top,  #ffffff 0%, #f1f2f4 100%); /* FF3.6+ */
	        background: -webkit-gradient(linear, left top, left bottom, color-stop(0%,#ffffff), color-stop(100%,#f1f2f4)); /* Chrome,Safari4+ */
	        background: -webkit-linear-gradient(top,  #ffffff 0%,#f1f2f4 100%); /* Chrome10+,Safari5.1+ */
	        background: -o-linear-gradient(top,  #ffffff 0%,#f1f2f4 100%); /* Opera 11.10+ */
	        background: -ms-linear-gradient(top,  #ffffff 0%,#f1f2f4 100%); /* IE10+ */
	        background: linear-gradient(top,  #ffffff 0%,#f1f2f4 100%); /* W3C */
	        filter: progid:DXImageTransform.Microsoft.gradient( startColorstr='#ffffff', endColorstr='#f1f2f4'); /* IE6-9 */
	        -ms-filter: "progid:DXImageTransform.Microsoft.gradient(startColorstr='#ffffff', endColorstr='#f1f2f4')";
	        clear: left;
	        display: inline-block;
	        padding: 2px 4px 1px;
	        padding-bottom: 1px;
	        -webkit-border-radius: 3px;
	        -moz-border-radius: 3px;
	        border-radius: 3px;

	        border: 1px solid #dedfe1;
	        color: #434343;
	        font-family: Tahoma;
	        font-size:11px;
	        margin: 4px;
	        vertical-align: middle;
	        text-decoration:none;
        }

        th.sortasc a  
        {
            display:block; padding:0 4px 0 15px; 
            background:url(/Content/images/sort_asc.png) no-repeat 
        }
         
        th.sortdesc a 
        {
            display:block; padding:0 4px 0 15px; 
            background:url(/Content/images/sort_desc.png) no-repeat;
        }
        .btn_clearsearch {
            all:unset;
            height:18px;
            vertical-align:middle;
        }
         
        .filters td {
            width:120px;
            padding:5px;
            overflow:hidden;
        }

        .filters {
            border: 1px solid #DEDFE1; 
            border-radius: 10px; 
        }
        .main_wrap > div {
            
        }
        .main_wrap {
            padding:10px;
            
            white-space: nowrap;
        }
        
        .wrap_filters {
            
            float:right;
            text-align:left;
            vertical-align:top; 
            font-family: 'Times New Roman'; 
            color: #666666; 
            font-weight: bold; 
            height: inherit; 
            margin-left: 10px;
        }

        td select {
            width:130px;
        }

        .main_table {
            overflow:auto;
            overflow-x:-moz-scrollbars-horizontal;
            
            min-width:300px;
            border-style: none; 
            /*float: left;*/ 
            /*max-width: 75%;*/
            /*min-width:950px; */
            margin-left: 10px;
        }

        .paginator input[type=image]{
            all:unset;
        }

        #ctl00_MainContent_GridView_TaskList {
            min-width:1800px;
        }
        /*context menu*/
        .show {
            z-index:1000;
            position: absolute;
            background-color: azure;
            border: 1px solid #18d41e;
            -webkit-box-shadow: 0px 0px 14px 1px #18d41e;
            -moz-box-shadow: 0px 0px 14px 1px #18d41e;
            box-shadow: 0px 0px 14px 1px #18d41e;
            padding: 5px;
            display: block;
            margin: 0;
            list-style-type: none;
            list-style: none;
        }

        .hide {
            display: none;
        }

        /*.show li{ list-style: none; }*/
        .show a { 
            font-size: medium;
            border: 0 !important; 
            text-decoration: none; 
            font-family: monospace;
        }
        .show a:hover { text-decoration: !important; }
        /*// context menu*/

        /*------------------------------------------------------------*/
        #main_table::-webkit-scrollbar-track,.topScroll::-webkit-scrollbar-track
        {
	        -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,0.3);
	        border-radius: 10px;
	        background-color: #F5F5F5;
        }

        #main_table::-webkit-scrollbar,.topScroll::-webkit-scrollbar
        {
            height:10px;
	        width: 12px;
	        background-color: #F5F5F5;
        }

        #main_table::-webkit-scrollbar-thumb,.topScroll::-webkit-scrollbar-thumb
        {
	        border-radius: 10px;
	        -webkit-box-shadow: inset 0 0 6px rgba(0,0,0,.3);
	        background-color: lightgray;
        }
        /*------------------------------------------------------------*/
    </style>
    <script type="text/javascript">
        function closeTooltip() {
            console.log("close tooltip")
            $("#tooltipContainer").hide();
        }

        $("#topScroll").onscroll=function(){
            $(".main_wrap").scrollLeft = $("#topScroll").scrollLeft;
            console.log("scroll");
        }
    </script>

</asp:Content>



