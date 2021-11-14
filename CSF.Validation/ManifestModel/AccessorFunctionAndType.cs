using System;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A simple model which describes a function that provides access to a value.
    /// </summary>
    public class AccessorFunctionAndType
    {
        /// <summary>
        /// Gets or sets the function that provides access to the value.
        /// </summary>
        public Func<object,object> AccessorFunction { get; set; }
        
        /// <summary>
        /// Gets or sets the expected output/return type of the <see cref="AccessorFunction"/>.
        /// </summary>
        public Type ExpectedType { get; set; }
    }
}