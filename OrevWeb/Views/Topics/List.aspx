<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<IEnumerable<Orev.Domain.Topic>>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Topics List</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% if (ViewData["Message"] != null)
   {%>
    <div class="messageBox"><%= Html.Encode(ViewData["Message"])%></div>
<% } %>
<h2>Topics List</h2>
    <table>
        <tr>
            <th></th>
            <th>
                Id
            </th>
            <th>
                Title
            </th>
            <th>
                Description
            </th>
            <th>
                Narrator
            </th>
            <th>
                Language
            </th>
        </tr>

    <% foreach (var item in Model) { %>
    
        <tr>
            <td>
                <%= Html.ActionLink("Edit", "Edit", new { /* id=item.PrimaryKey */ }) %> |
                <%= Html.ActionLink("Judge", "Index", "Judge", new { topicId = item.Id }, new { }) %> |
                <%= Html.ActionLink("Delete", "Delete", new { /* id=item.PrimaryKey */ })%>
            </td>
            <td>
                <%= Html.Encode(item.Id) %>
            </td>
            <td>
                <%= Html.Encode(item.Title) %>
            </td>
            <td>
                <%= Html.Encode(item.Description) %>
            </td>
            <td>
                <%= Html.Encode(item.Narrator) %>
            </td>
            <td>
                <%= Html.Encode(item.Language) %>
            </td>
        </tr>
    
    <% } %>

    </table>

    <p>
        <%= Html.ActionLink("Create New", "Create") %>
    </p>

</asp:Content>

