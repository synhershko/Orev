<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">Corpus Judgment</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
<% if (ViewData["Message"] != null)
   {%>
    <div class="messageBox"><%= Html.Encode(ViewData["Message"])%></div>
<% } else { %>
    <h2>Corpus Judgment</h2>

<div id="judgingContainer">
<input type="button" id="relevantBtn" value="Yes, it is relevant" />
<input type="button" id="irrelevantBtn" value="No, it is off topic" />

<div>
<iframe name="documentViewer" id="documentViewer" src=""></iframe>
</div>

</div>

    <script type="text/javascript" language="javascript">
        var corpusId = <%=ViewData["CorpusId"].ToString() %>;
        var corpusBaseUrl = '<%=ViewData["CorpusUrl"].ToString() %>';
        var topicId = <%=ViewData["TopicId"].ToString() %>;
        var currentDocumentId = '';
        
        function handleResponse(result)
        {
            if (result.corpusDone)
            {
                $('#documentViewer').attr("src","");
                $('#judgingContainer').hide();
                alert("done with this corpus");
            }
            else
            {
                currentDocumentId = result.nextDoc;
                $('#documentViewer').attr("src", corpusBaseUrl + currentDocumentId);
            }
        }
    
        function saveJudgment(docId, jdgmnt) {
            $.ajax({
                type: "POST",
                url: "/Judge/SaveJudgment",
                data: { 'topicId': topicId, 'corpusId': corpusId, 'docId': docId, 'judgment': jdgmnt },
                dataType: "json",
                async: true,
                cache: false,
                success: function(jsonData) {
                    handleResponse(jsonData);
                },
                error: function(XMLHttpRequest, textStatus, errorThrown) {
                    alert("Error while trying to save judgment: " + textStatus);
                }
            });
        }

        $(document).ready(function () {
            saveJudgment('', false);
            
            $("#relevantBtn").click(function(e) {
                saveJudgment(currentDocumentId, true);
            });
            
            $("#irrelevantBtn").click(function(e) {
                saveJudgment(currentDocumentId, false);
            });
            
        });
    </script>
<% } %>
</asp:Content>
