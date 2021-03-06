﻿<%@ Page Title="Главная" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true"  EnableEventValidation="false" CodeBehind="Tasks.aspx.cs" Inherits="DashBoard.Tasks.Tasks" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <style type="text/css">
        .HandleCSS
        {
            width: 20px;
            height: 20px;
            background-color: Red;
        }
    </style>
    <script src="../Scripts/jquery-1.10.2.js"></script>
    <script src="../Content/colresizable/colResizable-1.6.min.js"></script>
    <script type="text/javascript">
        //$(document).ready(function () {
        //    setColResize();
        //});

        function setColResize() {
            $("table").eq(0).colResizable({
                liveDrag:true,
                postbackSafe: true,
                partialRefresh: true,
            });
        }
    </script>
    <div id="filters" style="vertical-align: top; text-align: center;">
    </div>
    <div>
        <asp:UpdatePanel ID="UpdatePanel1" runat="server">
            <ContentTemplate>
                <script type="text/javascript">
                    Sys.Application.add_load(setColResize);
                </script>
                <div style="float:left; padding-left:10px">
                    <a class="button" style="background:-webkit-linear-gradient(to top, #ffffff 0%,#f1f2f4 100%)" href="/Tasks/Task/Add">
                    <img src="/Content/images/add.gif" />
                    <span class="btnName">
                      Создать задание
                    </span>
                  </a>
                </div>
                <div style="float:left; height:25px; width:100px;">
                    <asp:UpdateProgress ID="updateProgress" runat="server">
                        <ProgressTemplate>
                            <div style="text-align: left; height: 100%; width: 100%; top: 0; right: 0; left: 10px; z-index: 9999999;">
                                <asp:Image ID="imgUpdateProgress" runat="server" ImageUrl="/Content/images/table_preloader.gif" AlternateText="Loading ..." ToolTip="Loading ..."  />
                            </div>
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                </div>
                <div style="width: 100%; height: 1px; clear: both;">.</div>
                <div style="border-style: none; float: left; width: 75%; margin-left: 10px;">
                    <asp:GridView
                        runat="server"
                        AllowSorting="True"
                        ClientIDMode="AutoID"
                        HorizontalAlign="Center"
                        Width="100%"
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
                        PageSize="30" PagerSettings-NextPageText=">"
                        PagerSettings-PreviousPageText="<"
                        PagerSettings-LastPageText=">>"
                        PagerSettings-FirstPageText="<<"
                        ID="GridView_TaskList"
                        OnSelectedIndexChanged="GridView_TaskList_SelectedIndexChanged">
                        <Columns>
                            <asp:TemplateField HeaderText="" SortExpression="PaymentImage">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("PaymentImage") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="td_money kiketable-th" />
                                <ItemStyle Width="3%" CssClass="td_date" />
                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Дата" SortExpression="DateTime">
                                <ItemTemplate>
                                    <asp:Label ID="Label1" runat="server" Text='<%# Bind("Date") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="td_date kiketable-th" />
                                <ItemStyle Width="10%" CssClass="td_date" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Тип" SortExpression="Type">
                                <ItemTemplate>
                                    <asp:Label ID="Label2" runat="server" Text='<%# Bind("Type") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="td_type" />
                                <ItemStyle Width="15%" CssClass="td_type" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Статус" SortExpression="Status">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Bind("Status") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="td_status kiketable-th" />
                                <ItemStyle Width="5%" CssClass="td_status" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Номер" SortExpression="Nomber">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Bind("Nomber") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="td_number kiketable-th" />
                                <ItemStyle Width="10%" Wrap="False" CssClass="td_number" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Задание" SortExpression="Name">
                                <ItemTemplate>
                                    <div style="text-overflow: ellipsis; overflow: hidden">
                                        <asp:Label ID="Label5" runat="server" Text='<%# Bind("Name") %>'></asp:Label></div>
                                </ItemTemplate>
                                <HeaderStyle CssClass="td_name kiketable-th" />
                                <ItemStyle Width="45%" Wrap="False" CssClass="td_name" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Контакт" SortExpression="Contact">
                                <ItemTemplate>
                                    <asp:Label ID="Label6" runat="server" Text='<%# Bind("Contact") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="td_contact kiketable-th" />
                                <ItemStyle Width="15%" CssClass="td_contact" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Номер акта" SortExpression="InvoiceNomber">
                                <ItemTemplate>
                                    <asp:Label ID="Label_InvoiceNomber" runat="server" Text='<%# Bind("InvoiceNomber") %>'></asp:Label>
                                </ItemTemplate>
                                <HeaderStyle CssClass="td_invoicenomber kiketable-th" />
                                <ItemStyle Width="10%" CssClass="td_invoicenomber" />
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Дата принятия" SortExpression="DateAccept" Visible="False">
                                <ItemTemplate>
                                    <asp:Label ID="Label7" runat="server" Text='<%# Bind("DateAccept") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>

                        <EditRowStyle Wrap="False"></EditRowStyle>
                        <HeaderStyle HorizontalAlign="Center" VerticalAlign="Middle" Wrap="True" ForeColor="#6699FF" Font-Size="Small" CssClass="tasks_header" />

                        <PagerSettings FirstPageText="<<" LastPageText=">>" NextPageText=">" PreviousPageText="<" Mode="NumericFirstLast"></PagerSettings>

                        <PagerStyle HorizontalAlign="Center" CssClass="paginator" />
                        <RowStyle HorizontalAlign="Left" VerticalAlign="Middle" CssClass="tasks_row" Wrap="False" />
                        <SortedAscendingHeaderStyle Wrap="True" CssClass="sortasc" BackColor="#FF33CC" />
                        <SortedDescendingHeaderStyle VerticalAlign="Middle" CssClass="sortdesc" BackColor="#003300" />
                    </asp:GridView>
                </div>
                <div style="border: 1px solid #808080; border-radius: 10px; float:left; width:23%; text-align:left; vertical-align:top; font-family: 'Times New Roman'; color: #666666; font-weight: bold; height: inherit; margin-left: 10px;">
                    <div style="padding: 10px; float: left; width: 40%; left: 0px; height: 20px;">
                        <asp:Label ID="Label8" runat="server" Text="Статусы"></asp:Label>
                    </div>
                    <div style="padding: 10px; float: left; width: 40%">
                        <asp:DropDownList ID="DropDownList_Statuses" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="True" runat="server" CssClass="filters_dropbox"></asp:DropDownList>
                    </div>
                    <div style="padding: 10px; float: left; width: 40%">
                        <asp:Label ID="Label9" runat="server" Text="Типы"></asp:Label>
                    </div>
                    <div style="padding: 10px; float: left; width: 40%">
                        <asp:DropDownList ID="DropDownList_Types" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="True" runat="server" CssClass="filters_dropbox"></asp:DropDownList>
                    </div>
                    <div style="padding: 10px; float: left; width: 40%">
                        <asp:Label ID="Label10" runat="server" Text="Ответственный"></asp:Label>
                    </div>
                    <div style="padding: 10px; float: left; width: 40%">
                        <asp:DropDownList ID="DropDownList_Contacts" OnSelectedIndexChanged="DropDownList_SelectedIndexChanged" AutoPostBack="True" runat="server" CssClass="filters_dropbox"></asp:DropDownList>
                    </div>
                    <div style="padding: 10px; float: left; width: 40%">
                        <asp:Label ID="Label11" runat="server" Text="Показать"></asp:Label>
                    </div>
                    <div style="padding: 10px; float: left; width: 40%">
                        <asp:DropDownList ID="DropDownList_IsActive" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList_IsActive_SelectedIndexChanged" CssClass="filters_dropbox">
                            <asp:ListItem Selected="True" Value="1">Только активные</asp:ListItem>
                            <asp:ListItem Value="0">Все</asp:ListItem>
                        </asp:DropDownList>
                    </div>
                    <div style="padding: 10px; float: left; width: 40%">
                        <asp:TextBox ID="TextBox8" runat="server"  Width="100%"></asp:TextBox>
                    </div>
                    <div style="padding: 10px; float: left; width: 40%">
                        <asp:Button ID="Button2" runat="server" Text="Поиск" OnClick="Button1_Click" ForeColor="Black" />
                    </div>
                    <div style="padding: 10px; width: 100%;">
                        <asp:Button ID="ClearFilrters" runat="server" OnClick="ClearFilters_Click" Text="Очистить фильтры" ForeColor="Black" />
                    </div>
                    <div style="padding: 10px; width: 100%;">
                    </div>
                </div>
                <div style="width: 100%; height: 1px; clear: both;">.</div>
            </ContentTemplate>
        </asp:UpdatePanel>
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
            max-width:200px;
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
        .td_money         { width:2%}
        .td_date          { width:10%;}
        .td_type          { width:10%;}
        .td_status        { width:8%;}
        .td_number        { width:10%;}
        .td_name          { width:32%;}
        .td_contact       { width:18%;}
        .td_invoicenomber { width:10%;}

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
    </style>

</asp:Content>



