using System;
using System.Collections.Generic;
using CSF.Validation.Manifest;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A model which represents a value which is being validated by the validator.
    /// </summary>
    public class ValidatedValue
    {
        /// <summary>
        /// Gets or sets the manifest value to which the current instance relates.
        /// </summary>
        public ManifestValueBase ManifestValue { get; set;  }

        /// <summary>
        /// Gets or sets the actual value to be validated.
        /// </summary>
        public object ActualValue { get; set; }

        /// <summary>
        /// Gets or sets the identity of the value to be validated.
        /// </summary>
        public object ValueIdentity { get; set; }

        /// <summary>
        /// Gets or sets a parent value, where applicable.
        /// </summary>
        public ValidatedValue ParentValue { get; set; }

        /// <summary>
        /// Gets a collection of the child values, which treat the current instance as their parent.
        /// </summary>
        public IList<ValidatedValue> ChildValues { get; } = new List<ValidatedValue>();

        /// <summary>
        /// Gets a collection of the rules which should be executed upon the current value.
        /// </summary>
        public IList<ExecutableRule> Rules { get; set; } = new List<ExecutableRule>();

        /// <summary>
        /// Where this validated value represents a collection of values, and there is a separate
        /// validated value representing the items of that collection, this property should contain
        /// a reference to the value representing the collection items.
        /// </summary>
        public ValidatedValue CollectionItemValue { get; set; }

        /// <summary>
        /// Gets or sets a numeric item order, indicating the order in which this value was retrieved from a collection.
        /// </summary>
        /// <remarks>
        /// <para>
        /// This is not neccesarily a collection index, because a collection of values to be validated
        /// might only be <see cref="IEnumerable{T}"/> and not (for example) <see cref="IList{T}"/>.
        /// Thus, the order in which values are retrieved might not be meaningful and might not even
        /// be stable.
        /// For informational purposes, this value is still retrieved and made available by
        /// this property.
        /// </para>
        /// <para>
        /// If the current validated value does not represent an item from a collection then this property will contain
        /// <see langword="null" />.
        /// </para>
        /// </remarks>
        public long? CollectionItemOrder { get; set; }
    }
}