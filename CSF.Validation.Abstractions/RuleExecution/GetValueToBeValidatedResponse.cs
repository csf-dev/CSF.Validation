using System;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A response/result type for the <see cref="IGetsValueToBeValidated"/> service.
    /// </summary>
    public abstract class GetValueToBeValidatedResponse : IEquatable<GetValueToBeValidatedResponse>
    {
        /// <summary>
        /// Gets a value that indicates whether ornot the current instance represents success.
        /// </summary>
        public abstract bool IsSuccess { get; }

        /// <summary>
        /// Gets a value that indicates whether or not the current instance is equal to the specified <see cref="GetValueToBeValidatedResponse"/>.
        /// </summary>
        /// <param name="other">The value response.</param>
        /// <returns><see langword="true" /> if the current instance is equal to the specified valuse response; <see langword="false" /> otherwise.</returns>
        public abstract bool Equals(GetValueToBeValidatedResponse other);

        /// <summary>
        /// Gets a value that indicates whether or not the current instance is equal to the specified object.
        /// </summary>
        /// <param name="obj">An object</param>
        /// <returns><see langword="true" /> if the current instance is equal to the specified object; <see langword="false" /> otherwise.</returns>
        public override bool Equals(object obj) => Equals(obj as GetValueToBeValidatedResponse);

        /// <summary>
        /// Gets a value which may serve as a hash code for the current instance.
        /// </summary>
        /// <returns>A hash code value.</returns>
        public override int GetHashCode() => base.GetHashCode();
    }
}