<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Orev.Domain.Topic>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
<%
        if ((ViewData["EditMode"] as bool?) == true)
            Response.Write("Edit Topic");
        else
            Response.Write("Add Topic");
    %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>
    <%
        bool bEditMode = (ViewData["EditMode"] as bool?) == true;
        if (bEditMode)
            Response.Write("Edit Topic");
        else
            Response.Write("Add Topic");
    %>
    </h2>

    <% using (Html.BeginForm()) {%>
        <%= Html.ValidationSummary(true) %>

        <fieldset>
            <legend>Fields</legend>            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Title) %>
            </div>
            <div class="editor-field">
                <%= Html.TextAreaFor(model => model.Title) %>
                <%= Html.ValidationMessageFor(model => model.Title) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Description) %>
            </div>
            <div class="editor-field">
                <%= Html.TextAreaFor(model => model.Description)%>
                <%= Html.ValidationMessageFor(model => model.Description) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Narrator) %>
            </div>
            <div class="editor-field">
                <%= Html.TextAreaFor(model => model.Narrator)%>
                <%= Html.ValidationMessageFor(model => model.Narrator) %>
            </div>
            
            <div class="editor-label">
                <%= Html.LabelFor(model => model.Language) %>
            </div>
            <div class="editor-field">
                <%= Html.TextBoxFor(model => model.Language) %>
                <%= Html.ValidationMessageFor(model => model.Language) %>
            </div>
            
            <p>
                <input type="submit" value="Create" />
            </p>
        </fieldset>

    <% } %>

    <div>
        <%= Html.ActionLink("Back to Topics List", "Index") %>
    </div>

</asp:Content>

