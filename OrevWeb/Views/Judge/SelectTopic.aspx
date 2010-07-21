<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Orev.Domain.Topic>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Select Topic</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% if (ViewData["Message"] != null)
   {%>
    <div class="messageBox"><%= Html.Encode(ViewData["Message"])%></div>
<% } %>
    <h2>Available topics for <%=Html.Encode(Request.QueryString["lang"]) %></h2>

    <% foreach (var item in Model) { %>
    
        <div style="margin:5px;border:1px solid #ccc;">
            <p><b>Title:</b> <%= Html.Encode(item.Title) %></p>
            <p><b>Description:</b> <%= Html.Encode(item.Description) %></p>
            <p><b>Narrator:</b> <%= Html.Encode(item.Narrator) %></p>
            <p><%= Html.ActionLink("Select", "Index", new { topicId = item.Id, corpusId = ViewData["CorpusId"] })%></p>
        </div>
    
    <% } %>

    <p><%= Html.ActionLink("Add New", "Create", "Topics", new { lang = Request.QueryString["lang"] }, null) %></p>

</asp:Content>

