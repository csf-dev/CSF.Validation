using System;
using System.Collections.Generic;

namespace CraigFowler.Web.UI.Controls
{
    /// <summary>
    /// <para>Represents an HTML option element.</para>
    /// </summary>
    public class HtmlOption
    {
        #region fields
        
        private string valueAttr;
        
        #endregion
        
        #region properties
        
        /// <summary>
        /// <para>
        /// Read-only.  Gets the string value that appears within the 'value' attribute of the rendered option.
        /// </para>
        /// </summary>
        public string Value
        {
            get {
                return valueAttr;
            }
            private set {
                if(value == null)
                {
                    throw new ArgumentNullException("value");
                }
                
                valueAttr = value;
            }
        }
        
        /// <summary>
        /// <para>Read-only.  Gets the display text that appears within this rendered option.</para>
        /// </summary>
        public string DisplayText
        {
            get;
            private set;
        }
        
        /// <summary>
        /// <para>Gets and sets whether this option should be rendered as selected or not.</para>
        /// </summary>
        public bool Selected
        {
            get;
            set;
        }
        
        /// <summary>
        /// <para>
        /// Read-only.  Gets whether or not this option represents freetext.  If so then it will be rendered
        /// accordingly.
        /// </para>
        /// </summary>
        public bool IsFreetext {
            get;
            private set;
        }
        
        #endregion
        
        #region methods
        
        /// <summary>
        /// <para>Overridden.  Gets a string representation of this option.</para>
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/>
        /// </returns>
        public override string ToString ()
        {
            return this.Value;
        }
        
        #endregion
        
        #region constructors
        
        /// <summary>
        /// <para>Initialises this instance.</para>
        /// </summary>
        /// <param name="valueAttribute">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="displayText">
        /// A <see cref="System.String"/>
        /// </param>
        public HtmlOption (string valueAttribute, string displayText) : this(valueAttribute, displayText, false) {}
        
        /// <summary>
        /// <para>Initialises this instance.</para>
        /// </summary>
        /// <param name="valueAttribute">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="displayText">
        /// A <see cref="System.String"/>
        /// </param>
        /// <param name="selected">
        /// A <see cref="System.Boolean"/>
        /// </param>
        public HtmlOption (string valueAttribute, string displayText, bool selected)
        {
            this.Value = valueAttribute;
            this.DisplayText = displayText;
            this.Selected = selected;
            this.IsFreetext = false;
        }
        
        #endregion
        
        #region static methods
        
        /// <summary>
        /// <para>
        /// Creates and returns a collection of <see cref="HtmlOption"/> instances from a collection of
        /// <see cref="System.String"/> option values that were selected.
        /// </para>
        /// </summary>
        /// <param name="options">
        /// A collection of <see cref="System.String"/>
        /// </param>
        /// <param name="freetextOption">
        /// A <see cref="System.String"/> that contains the value of the freetext option (if such an option was
        /// selected).
        /// </param>
        /// <returns>
        /// A collection of <see cref="HtmlOption"/>
        /// </returns>
        public static List<HtmlOption> GetSelectedOptions(IList<string> options, string freetextOption)
        {
            List<HtmlOption> output = new List<HtmlOption>();
            
            if(options == null)
            {
                throw new ArgumentNullException("options");
            }
            
            foreach(string option in options)
            {
                output.Add(new HtmlOption(option, null, true));
            }
            
            if(freetextOption != null)
            {
                HtmlOption option = new HtmlOption(freetextOption, null, true);
                option.IsFreetext = true;
                output.Add(option);
            }
            
            return output;
        }
        
        #endregion
    }
}

