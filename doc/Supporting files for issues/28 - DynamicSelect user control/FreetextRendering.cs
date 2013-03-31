using System;

namespace CraigFowler.Web.UI.Controls
{
    /// <summary>
    /// <para>
    /// Enumerates the possible rendering options for the free-text portion of a <see cref="DynamicSelect"/> control.
    /// </para>
    /// </summary>
    public enum FreetextRendering : int
    {
        /// <summary>
        /// <para>
        /// The dynamic select control does not permit any free text.  No text-entry control will be rendered.
        /// </para>
        /// </summary>
        None                            = 0,
        
        /// <summary>
        /// <para>Free-text is permitted.  A single-line HTML input element will be presented.</para>
        /// </summary>
        SingleLine,
        
        /// <summary>
        /// <para>Free-text is permitted.  A multi-line HTML textarea element will be presented.</para>
        /// </summary>
        MultiLine
    }
}

