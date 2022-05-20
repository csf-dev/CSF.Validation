using System;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A <see cref="GetValueToBeValidatedResponse"/> which indicates failure and that an error should be added to the results.
    /// </summary>
    public class ErrorGetValueToBeValidatedResponse : GetValueToBeValidatedResponse
    {
        /// <inheritdoc/>
        public override bool IsSuccess => false;

        /// <summary>
        /// Gets the exception which lead to this result.
        /// </summary>
        public Exception Exception { get; }

        /// <summary>
        /// Initialises a new instance of <see cref="ErrorGetValueToBeValidatedResponse"/>.
        /// </summary>
        /// <param name="exception">The exception which lead to this result.</param>
        public ErrorGetValueToBeValidatedResponse(Exception exception)
        {
            Exception = exception;
        }
    }
}