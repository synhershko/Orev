<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Home</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<div><%=Html.Encode(ViewData["Message"]) %></div>
    <h2>Welcome!</h2>
    <p>
        Some explanation on what OpenRelevance is
    </p>
    <p><a href="http://lucene.apache.org/openrelevance/">OpenRelevance homepage</a></p>
</asp:Content>
