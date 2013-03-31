<%@ Control Language="C#" Inherits="CraigFowler.Web.UI.Controls.DynamicSelect" %>
<%@ Import Namespace="CraigFowler.Web.UI.Controls" %>
<%@ Import Namespace="System.Web" %>
<div id="<%= this.ClientID %>"
    <%= String.IsNullOrEmpty(this.Class)? String.Empty : String.Format("class=\"{0}\"", HttpUtility.HtmlEncode(this.Class)) %>>
  <select<%= this.AssembleAttributes() %>><%
foreach(HtmlOption option in this.AvailableOptions.Values)
{
  %>
    <option<%= AssembleAttributes(option) %>><%= HttpUtility.HtmlEncode(option.DisplayText) %></option><%
}
%>
  </select><%
if(this.PermitFreetext)
{
  if(this.FreetextSelectedOptionValue == null)
  {
    %>
  <label for="<%= this.ClientID %>_freetext_select">Other</label>
  <input type="checkbox" name="<%= String.Concat(this.ElementName, FreetextSelectionSuffix) %>"
         id="<%= this.ClientID %>_freetext_select"<%= this.FreetextIsSelected? " checked=\"checked\"" : String.Empty %>
         value="<%= Boolean.TrueString %>" class="freetext_toggle" /><%
  }
  
  if(this.FreetextMode == FreetextRendering.SingleLine)
  {
    %>
  <input type="text" id="<%= this.ClientID %>_freetext" name="<%= String.Concat(this.ElementName, FreetextValueSuffix) %>"
         value="<%= HttpUtility.HtmlEncode(this.FreetextValue) %>" class="freetext_entry" /><%
  }
  else if(this.FreetextMode == FreetextRendering.MultiLine)
  {
    %>
    <textarea<%= AssembleTextareaAttributes()%>><%= HttpUtility.HtmlEncode(this.FreetextValue) %></textarea><%
  }
  else
  {
    throw new NotSupportedException("Unsupported freetext rendering mode (exception thrown from markup file).");
  }
}
%>
</div>