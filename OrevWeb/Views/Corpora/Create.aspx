<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Orev.Domain.Corpus>" %>
<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Add a new corpus</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Add a new corpus</h2>

    <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>

        <fieldset>
            <legend>Fields</legend>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Name) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Name) %>
                <%= Html.ValidationMessageFor(model => model.Name) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Description) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Description) %>
                <%= Html.ValidationMessageFor(model => model.Description) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Language) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Language) %>
                <%= Html.ValidationMessageFor(model => model.Language) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Path) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Path) %>
                <%= Html.ValidationMessageFor(model => model.Path) %>
            </div>
            
            <p><input type="submit" value="Create" /></p>
        </fieldset>

    <% } %>

</asp:Content>

