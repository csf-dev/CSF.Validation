using System;
using System.Collections.Generic;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// An options type that indicates the collection of <see cref="Type"/> that are to be
    /// used as failure message providers.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is the collection of types from which the method
    /// <see cref="IGetsCandidateMessageTypes.GetCandidateMessageProviderTypes(ValidationRuleResult)"/>
    /// could possibly draw its results.
    /// </para>
    /// </remarks>
    public class MessageProviderTypeOptions
    {
        private ICollection<Type> messageProviderTypes = new HashSet<Type>();

        /// <summary>
        /// Gets or sets the collection of types that are to be used as message providers.
        /// </summary>
        public ICollection<Type> MessageProviderTypes
        {
            get => messageProviderTypes;
            set => messageProviderTypes = value ?? throw new ArgumentNullException(nameof(value));
        }
    }
}