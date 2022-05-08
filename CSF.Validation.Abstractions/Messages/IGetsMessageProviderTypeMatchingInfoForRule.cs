using System;

namespace CSF.Validation.Messages
{
    /// <summary>
    /// An object which determines whether a corresponding type is a candidate for providing a message
    /// for a specified <see cref="ValidationRuleResult"/>.
    /// </summary>
    public interface IGetsMessageProviderTypeMatchingInfoForRule
    {
        /// <summary>
        /// Gets a numeric priority indicating the preference for using the corresponding type for
        /// providing a validation failure message.
        /// </summary>
        /// <returns>A numeric priority value.</returns>
        int GetPriority();

        /// <summary>
        /// Gets a value that indicates whether this attribute is a candidate for providing a validation
        /// failure message for the specified rule result.
        /// </summary>
        /// <param name="result">A validation rule result.</param>
        /// <returns><see langword="true" /> if this attribute matches the specified result; <see langword="false" /> otherwise.</returns>
        /// <exception cref="ArgumentNullException">If the <paramref name="result"/> is <see langword="null" />.</exception>
        bool IsMatch(ValidationRuleResult result);
    }
}