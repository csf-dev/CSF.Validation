
using System;
using System.Web;
using System.Web.UI;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Text;
using System.Net;

namespace CraigFowler.Web.UI.Controls
{
    /// <summary>
    /// <para>Represents an HTML 'select' element control with some dynamic enhancements.</para>
    /// </summary>
    public partial class DynamicSelect : UserControl
    {
        #region constants
        
        public const string
            FreetextSelectionSuffix                         = "-freetext-selected",
            FreetextValueSuffix                             = "-freetext";
        
        #endregion
        
        #region fields
        
        private Dictionary<string, HtmlOption> availableOptions;
        private string elementName;
        private FreetextRendering freetextMode;
        private int textareaRows, textareaCols;
        
        #endregion
        
        #region properties
        
        /// <summary>
        /// <para>Gets and sets a collection of options that are available for selection.</para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Remember that this property only represents the available options at the point of the control initially
        /// rendering on the HTML page.  JavaScript could retroactively further options to the control.
        /// </para>
        /// </remarks>
        public Dictionary<string, HtmlOption> AvailableOptions
        {
            get {
                return availableOptions;
            }
            set {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                
                availableOptions = value;
            }
        }
        
        /// <summary>
        /// <para>Gets and sets the 'name' attribute for this element on the HTML form.</para>
        /// </summary>
        public string ElementName
        {
            get {
                return elementName;
            }
            set {
                if(String.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("Element name may not be null or empty.", "value");
                }
                
                elementName = value;
            }
        }
        
        /// <summary>
        /// <para>Gets and sets whether multiple selection is permitted in this select control.</para>
        /// </summary>
        public bool PermitMultiSelect
        {
            get;
            set;
        }
        
        /// <summary>
        /// <para>
        /// Read-only.  Gets whether a 'free text' entry is appended to the available options presented in this instance.
        /// </para>
        /// </summary>
        public bool PermitFreetext
        {
            get {
                return (this.FreetextMode != FreetextRendering.None);
            }
        }
        
        /// <summary>
        /// <para>Gets and sets the rendering mode of the freetext portion of this control.</para>
        /// </summary>
        public FreetextRendering FreetextMode
        {
            get {
                return freetextMode;
            }
            set {
                if(!Enum.IsDefined(typeof(FreetextRendering), value))
                {
                    throw new ArgumentOutOfRangeException("value", "Value is not a value enumeration member.");
                }
                
                freetextMode = value;
            }
        }
        
        /// <summary>
        /// <para>
        /// Gets and sets the size (rows) of the HTML textarea control that renders.  Only relevant if
        /// <see cref="FreetextMode"/> is set to <see cref="FreetextRendering.MultiLine"/>.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// This controls the rows="x" property on the rendered HTML textarea.  It can be overridden with CSS.
        /// </para>
        /// </remarks>
        public int FreetextRows
        {
            get {
                return textareaRows;
            }
            set {
                if(value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Invalid rows count");
                }
                
                textareaRows = value;
            }
        }
        
        /// <summary>
        /// <para>
        /// Gets and sets the size (columns) of the HTML textarea control that renders.  Only relevant if
        /// <see cref="FreetextMode"/> is set to <see cref="FreetextRendering.MultiLine"/>.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// This controls the cols="x" property on the rendered HTML textarea.  It can be overridden with CSS.
        /// </para>
        /// </remarks>
        public int FreetextColumns
        {
            get {
                return textareaCols;
            }
            set {
                if(value <= 0)
                {
                    throw new ArgumentOutOfRangeException("value", "Invalid column count");
                }
                
                textareaCols = value;
            }
        }
        
        /// <summary>
        /// <para>
        /// Gets and sets the initial/default value for the freetext portion of this instance. Only relevant if
        /// <see cref="PermitFreetext"/> is true.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// Whilst this property could be used in theory to retrieve the freetext value after the form is posted, this
        /// should be avoided in favour of using an overload of either <see cref="GetSelectedOptions()"/> or
        /// <see cref="GetSingleSelectedOption()"/>, which provide a far more robust way of retrieving options from
        /// this control.
        /// </para>
        /// </remarks>
        public string FreetextValue
        {
            get;
            set;
        }
        
        /// <summary>
        /// <para>
        /// Gets and sets the option value (within the HTML SELECT element) that indicates whether or not the freetext
        /// has been selected or not.
        /// </para>
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is only relevant if <see cref="PermitFreetext"/> is true.  It should be used if one of the options
        /// included within the main &lt;select&gt; element is used as the 'toggle' to indicate whether or not the
        /// freetext has been selected or not (consider the presence of an 'other' option within the HTML select
        /// element).
        /// </para>
        /// <para>
        /// If this property is null then an additional checkbox will be rendered to the page allowing the user to
        /// toggle whether or not the freetext should be used or not.  If this property contains a non-null string then
        /// the freetext will be read and parsed if the corresponding option is selected.
        /// </para>
        /// <para>
        /// Note that in any case, if the checkbox is not selected (if this property is null) or the appropriate option
        /// is not selected (in the case that this property is non-null) then upon posting the form, the freetext
        /// entered will be ignored.  It will only be parsed and made available if it is marked as 'selected' in some
        /// way.
        /// </para>
        /// </remarks>
        public string FreetextSelectedOptionValue {
            get;
            set;
        }
        
        /// <summary>
        /// <para>Gets and sets the 'class' attribute on the rendered 'select' attribute.</para>
        /// </summary>
        public string Class
        {
            get;
            set;
        }
        
        /// <summary>
        /// <para>
        /// Gets and sets whether or not the freetext control is selected or not.  Only relevant if
        /// <see cref="PermitFreetext"/> is true.
        /// </para>
        /// </summary>
        public bool FreetextIsSelected
        {
            get;
            set;
        }
        
        #endregion
        
        #region page events
        
        /// <summary>
        /// <para>Checks that the state of this user control is valid at the point of the Load event.</para>
        /// </summary>
        /// <param name="sender">
        /// A <see cref="System.Object"/>
        /// </param>
        /// <param name="ev">
        /// A <see cref="EventArgs"/>
        /// </param>
        public void Page_PreRender(object sender, EventArgs ev)
        {
            this.CheckAttributesValid();
            
            if(this.FreetextSelectedOptionValue != null &&
               this.FreetextIsSelected &&
               this.AvailableOptions.ContainsKey(this.FreetextSelectedOptionValue))
            {
                this.AvailableOptions[this.FreetextSelectedOptionValue].Selected = true;
            }
        }
        
        #endregion
        
        #region methods
        
        /// <summary>
        /// <para>Overloaded.  Gets the options that have been selected from the specified form data.</para>
        /// </summary>
        /// <param name="formData">
        /// A <see cref="NameValueCollection"/> that contains the form data.
        /// <seealso cref="HttpRequest.Form"/>
        /// <seealso cref="HttpRequest.QueryString"/>
        /// </param>
        /// <returns>
        /// A collection of <see cref="SelectedOption"/>, representing the form keys of the OPTION element(s) that were
        /// selected (possibly none).
        /// </returns>
        public List<HtmlOption> GetSelectedOptions(NameValueCollection formData)
        {
            string freetextSelectionKey;
            
            if(!this.PermitFreetext)
            {
                freetextSelectionKey = null;
            }
            if(this.FreetextSelectedOptionValue != null)
            {
                freetextSelectionKey = this.FreetextSelectedOptionValue;
            }
            else
            {
                freetextSelectionKey = String.Format("{0}{1}", this.ElementName, FreetextSelectionSuffix);
            }
            
            return GetSelectedOptions(this.ElementName,
                                      String.Format("{0}{1}", this.ElementName, FreetextValueSuffix),
                                      freetextSelectionKey,
                                      formData);
        }
        
        /// <summary>
        /// <para>Overloaded.  Gets the options that have been selected from the HTTP POST or GET data.</para>
        /// </summary>
        /// <returns>
        /// A collection of <see cref="HtmlOption"/>, representing the form keys of the OPTION element(s) that were
        /// selected (possibly none).
        /// </returns>
        public List<HtmlOption> GetSelectedOptions()
        {
            List<HtmlOption> output = null;
            
            if(this.Request.HttpMethod == WebRequestMethods.Http.Post)
            {
                output = this.GetSelectedOptions(this.Request.Form);
            }
            else if(this.Request.HttpMethod == WebRequestMethods.Http.Get)
            {
                output = this.GetSelectedOptions(this.Request.QueryString);
            }
            
            return output;
        }
        
        /// <summary>
        /// <para>
        /// Overloaded.  Gets a single option from this instance that was selected, based on the given form data.
        /// </para>
        /// </summary>
        /// <param name="formData">
        /// A <see cref="NameValueCollection"/> that contains the form data.
        /// <seealso cref="HttpRequest.Form"/>
        /// <seealso cref="HttpRequest.QueryString"/>
        /// </param>
        /// <returns>
        /// A <see cref="HtmlOption"/>.  If null then no option was selected.
        /// </returns>
        public HtmlOption GetSingleSelectedOption(NameValueCollection formData)
        {
            List<HtmlOption> output = this.GetSelectedOptions(formData);
            
            return (output != null && output.Count == 1)? output[0] : null;
        }
        
        /// <summary>
        /// <para>
        /// Overloaded.  Gets a single option from this instance that was selected from the HTTP POST or GET data.
        /// </para>
        /// </summary>
        /// <returns>
        /// A <see cref="SelectedOption"/>.  If null then no option was selected.
        /// </returns>
        public HtmlOption GetSingleSelectedOption()
        {
            HtmlOption output = null;
            
            if(this.Request.HttpMethod == WebRequestMethods.Http.Post)
            {
                output = this.GetSingleSelectedOption(this.Request.Form);
            }
            else if(this.Request.HttpMethod == WebRequestMethods.Http.Get)
            {
                output = this.GetSingleSelectedOption(this.Request.QueryString);
            }
            
            return output;
        }
        
        /// <summary>
        /// <para>Overloaded.  Adds a new (non-selected) option to this select element.</para>
        /// </summary>
        /// <param name="optionValue">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="displayText">
        /// A <see cref="System.String"/>
        /// </param>
        public void AddOption(string optionValue, string displayText)
        {
            this.AddOption(optionValue, displayText, false);
        }
        
        /// <summary>
        /// <para>Overloaded.  Adds a new option to this select element.</para>
        /// </summary>
        /// <param name="optionValue">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="displayText">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="isSelected">
        /// A <see cref="System.Boolean"/>
        /// </param>
        public void AddOption(string optionValue, string displayText, bool isSelected)
        {
            this.AddOption(new HtmlOption(optionValue, displayText, isSelected));
        }
        
        /// <summary>
        /// <para>Overloaded.  Adds a new option to this select element.</para>
        /// </summary>
        /// <param name="option">
        /// A <see cref="HtmlOption"/>
        /// </param>
        public void AddOption(HtmlOption option)
        {
            if(option == null)
            {
                throw new ArgumentNullException("option");
            }
            
            this.AvailableOptions.Add(option.Value, option);
        }
        
        /// <summary>
        /// <para>
        /// Overloaded.  Sets a single option within this instance to the 'selected' state.  If this control instance
        /// permits multiple selection then the selected option will be added to those that are selected at the moment.
        /// </para>
        /// <seealso cref="PermitMultiSelect"/>
        /// </summary>
        /// <param name="optionValue">
        /// A <see cref="System.String"/> - the 'value' of the option to set selected.
        /// </param>
        public void SetOptionSelected(string optionValue)
        {
            if(optionValue == null)
            {
                throw new ArgumentNullException("optionValue");
            }
            
            if(!this.AvailableOptions.ContainsKey(optionValue))
            {
                throw new ArgumentException("The available options in this instance do not include the given option",
                                            "optionValue");
            }
            
            if(!this.PermitMultiSelect)
            {
                this.ClearSelectedOptions();
            }
            
            this.AvailableOptions[optionValue].Selected = true;
        }
        
        /// <summary>
        /// <para>
        /// Overloaded.  Sets a single option within this instance to the 'selected' state.  If this control instance
        /// permits multiple selection then the selected option will be added to those that are selected at the moment.
        /// </para>
        /// <seealso cref="PermitMultiSelect"/>
        /// </summary>
        /// <param name="option">
        /// An <see cref="HtmlOption"/> instance.
        /// </param>
        public void SetOptionSelected(HtmlOption option)
        {
            if(option == null)
            {
                throw new ArgumentNullException("option");
            }
            
            this.SetOptionSelected(option.Value);
        }
        
        /// <summary>
        /// <para>
        /// Overloaded.  Sets the freetext option to the 'selected' state.  Only relevant if
        /// <see cref="PermitFreetext"/> is true.
        /// </para>
        /// </summary>
        public void SetFreetextSelected()
        {
            this.SetFreetextSelected(null);
        }
        
        /// <summary>
        /// <para>
        /// Overloaded.  Sets the freetext option to the 'selected' state and passes in a default value.  Only relevant
        /// if <see cref="PermitFreetext"/> is true.
        /// </para>
        /// </summary>
        public void SetFreetextSelected(string optionValue)
        {
            this.FreetextIsSelected = true;
            this.FreetextValue = optionValue;
        }
        
        /// <summary>
        /// <para>
        /// Overloaded.  Sets a single option within this instance to the 'unselected' state.  If this control instance
        /// permits multiple selection then other options may remain selected.
        /// </para>
        /// </summary>
        /// <param name="optionValue">
        /// A <see cref="System.String"/> - the 'value' of the option to set selected.
        /// </param>
        public void SetOptionDeselected(string optionValue)
        {
            if(optionValue == null)
            {
                throw new ArgumentNullException("optionValue");
            }
            
            if(!this.AvailableOptions.ContainsKey(optionValue))
            {
                throw new ArgumentException("The available options in this instance do not include the given option",
                                            "optionValue");
            }
            
            this.AvailableOptions[optionValue].Selected = false;
        }
        
        /// <summary>
        /// <para>
        /// Overloaded.  Sets a single option within this instance to the 'unselected' state.  If this control instance
        /// permits multiple selection then other options may remain selected.
        /// </para>
        /// </summary>
        /// <param name="option">
        /// An <see cref="HtmlOption"/> instance.
        /// </param>
        public void SetOptionDeselected(HtmlOption option)
        {
            if(option == null)
            {
                throw new ArgumentNullException("option");
            }
            
            this.SetOptionDeselected(option.Value);
        }
        
        /// <summary>
        /// <para>
        /// Sets the freetext option to the 'deselected' state.  Only relevant if <see cref="PermitFreetext"/> is true.
        /// </para>
        /// </summary>
        public void SetFreetextDeselected()
        {
            this.FreetextIsSelected = false;
        }
        
        /// <summary>
        /// <para>Clears the selection status from every option contained within this instance.</para>
        /// </summary>
        public void ClearSelectedOptions()
        {
            foreach(string optionValue in this.AvailableOptions.Keys)
            {
                this.SetOptionDeselected(optionValue);
            }
            
            this.SetFreetextDeselected();
        }
        
        /// <summary>
        /// <para>
        /// Sets the selection state of all of the options within this instance.  This implicitly clears all previous
        /// selections.
        /// </para>
        /// </summary>
        /// <param name="options">
        /// A <see cref="IEnumerable<HtmlOption>"/>
        /// </param>
        public void SetOptionsSelected(IEnumerable<HtmlOption> options)
        {
            if(options == null)
            {
                throw new ArgumentNullException("options");
            }
            
            this.ClearSelectedOptions();
            
            foreach(HtmlOption option in options)
            {
                if(!option.IsFreetext)
                {
                    this.SetOptionSelected(option);
                }
                else
                {
                    this.SetFreetextSelected(option.Value);
                }
            }
        }
        
        /// <summary>
        /// <para>Overloaded.  Assembles the attributes that should appear on the rendered 'select' element.</para>
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        protected string AssembleAttributes()
        {
            StringBuilder output = new StringBuilder();
            
            output.AppendFormat(" name=\"{0}\"", this.ElementName);
            output.AppendFormat(" id=\"{0}_select\"", this.ClientID);
            
            if(this.PermitMultiSelect)
            {
                output.AppendFormat(" multiple=\"multiple\"");
            }
            
            return output.ToString();
        }
        
        /// <summary>
        /// <para>Assembles the attributes on an HTML textarea control.</para>
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        protected string AssembleTextareaAttributes()
        {
            StringBuilder output = new StringBuilder();
            
            output.AppendFormat(" id=\"{0}_freetext\"", this.ClientID);
            output.AppendFormat(" name=\"{0}{1}\"", this.ElementName, FreetextValueSuffix);
            output.AppendFormat(" rows=\"{0}\"", this.FreetextRows);
            output.AppendFormat(" cols=\"{0}\"", this.FreetextColumns);
            output.AppendFormat(" class=\"freetext_entry\"");
            
            return output.ToString();
        }

        
        /// <summary>
        /// <para>Overloaded.  Assembles the attributes that should appear on the rendered 'option' element.</para>
        /// </summary>
        /// <param name="option">
        /// A <see cref="HtmlOption"/>
        /// </param>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        protected string AssembleAttributes(HtmlOption option)
        {
            StringBuilder output = new StringBuilder();
            
            output.AppendFormat(" value=\"{0}\"", HttpUtility.HtmlEncode(option.Value));
            
            if(this.FreetextSelectedOptionValue != null && option.Value == this.FreetextSelectedOptionValue)
            {
                output.Append(" class=\"freetext_toggle\"");
            }
            
            if(option.Selected)
            {
                output.Append(" selected=\"selected\"");
            }
            
            return output.ToString();
        }
        
        /// <summary>
        /// <para>Checks that this instance has valid attributes for rendering.</para>
        /// </summary>
        protected void CheckAttributesValid()
        {
            if(String.IsNullOrEmpty(this.ElementName))
            {
                throw new InvalidOperationException("Name attribute cannot be null or empty.");
            }
            else if(String.IsNullOrEmpty(this.ClientID))
            {
                throw new InvalidOperationException("ID attribute cannot be null or empty.");
            }
        }
        
        #endregion
        
        #region constructor
        
        /// <summary>
        /// <para>Initialises this instance with default values.</para>
        /// </summary>
        public DynamicSelect ()
        {
            this.AvailableOptions = new Dictionary<string, HtmlOption>();
            this.PermitMultiSelect = false;
            this.Class = null;
            this.FreetextMode = FreetextRendering.None;
            this.FreetextColumns = 30;
            this.FreetextRows = 10;
            this.FreetextSelectedOptionValue = null;
        }
        
        #endregion
        
        #region static methods
        
        /// <summary>
        /// <para>Gets the selected options from the submitted form data.</para>
        /// </summary>
        /// <param name="postDataKey">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="freetextKey">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="freetextSelectionKey">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="formData">
        /// A <see cref="NameValueCollection"/>
        /// </param>
        /// <returns>
        /// A collection of <see cref="HtmlOption"/>
        /// </returns>
        public static List<HtmlOption> GetSelectedOptions(string postDataKey,
                                                          string freetextKey,
                                                          string freetextSelectionKey,
                                                          NameValueCollection formData)
        {
            List<string> selectedOptions = new List<string>();
            string freetextOption = null;
            
            if(formData[postDataKey] != null)
            {
                selectedOptions.AddRange(formData[postDataKey].Split(new char[] {','}));
            }
            
            if(freetextSelectionKey != null && !String.IsNullOrEmpty(formData[freetextKey]))
            {
                if(((IList<string>) selectedOptions).Contains(freetextSelectionKey))
                {
                    selectedOptions.Remove(freetextSelectionKey);
                    freetextOption = formData[freetextKey];
                }
                else if(formData[freetextSelectionKey] == Boolean.TrueString)
                {
                    freetextOption = formData[freetextKey];
                }
            }
            
            return HtmlOption.GetSelectedOptions(selectedOptions, freetextOption);
        }
        
        #endregion
    }
}

