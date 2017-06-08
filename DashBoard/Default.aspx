<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="DashBoard._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
        <div style="padding-left:10px""><asp:HyperLink ID="HyperLink1" runat="server" NavigateUrl="~/Tasks/Tasks.aspx">Задачи</asp:HyperLink></div>
</asp:Content>
