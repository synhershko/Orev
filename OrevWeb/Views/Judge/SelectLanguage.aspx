<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Select Language</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% if (ViewData["Message"] != null)
   {%>
    <div class="messageBox"><%= Html.Encode(ViewData["Message"])%></div>
<% } %>
    <h2>Available Languages</h2>

<% var langs = ViewData["LanguagesList"] as string[];
   if (langs == null)
   {
       %>
    <div>No topics found. Use this form to add new a topic.</div>
       <%
    }
   else
   {
       foreach (var item in langs)
       { %>
    <div><%=Html.Encode(item)%> (<%= Html.ActionLink("Select a topic in this language", "SelectTopic", new { lang = item })%>)</div>
<%      }
   } %>

<p><%= Html.ActionLink("Create new topic", "Create", "Topics")%></p>
</asp:Content>
