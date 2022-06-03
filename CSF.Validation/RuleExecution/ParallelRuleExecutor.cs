using System;
using System.Collections.Generic;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Channels;
using System.Linq;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// An implementation of <see cref="IExecutesAllRules"/> which executes validation rules in parallel.
    /// </summary>
    public class ParallelRuleExecutor : IExecutesAllRules
    {
        readonly IExeucutesSingleRule ruleExecutor;
        readonly ResolvedValidationOptions options;

        /// <inheritdoc/>
        public Task<IReadOnlyCollection<ValidationRuleResult>> ExecuteAllRulesAsync(IRuleExecutionContext executionContext,
                                                                                    CancellationToken cancellationToken = default)
        {

        }

        /// <summary>
        /// Initialises a new instance of <see cref="ParallelRuleExecutor"/>.
        /// </summary>
        /// <param name="ruleExecutor">A service which executes rules.</param>
        /// <exception cref="ArgumentNullException">If any parameter value is <see langword="null" />.</exception>
        public ParallelRuleExecutor(IExeucutesSingleRule ruleExecutor, ResolvedValidationOptions options)
        {
            this.ruleExecutor = ruleExecutor ?? throw new System.ArgumentNullException(nameof(ruleExecutor));
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }
    }
}