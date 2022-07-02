using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CSF.Validation.RuleExecution
{
    /// <summary>
    /// A model which describes a singular circular dependency.
    /// </summary>
    public class CircularDependency
    {
        const string indentString = "    ", nextIndicator = "->  ";

        IList<ExecutableRule> dependencyChain = new List<ExecutableRule>();

        /// <summary>
        /// Gets or sets an ordered list which describes the chain of executable
        /// rules whose interdependencies lead to a circular dependency.
        /// </summary>
        public IList<ExecutableRule> DependencyChain { get => dependencyChain; set => dependencyChain = value ?? throw new ArgumentNullException(nameof(value)); }

        /// <summary>
        /// Gets a string which represents the current instance.
        /// </summary>
        /// <returns>A human-readable string.</returns>
        public override string ToString()
        {
            var builder = new StringBuilder();

            for (var i = 0; i < DependencyChain.Count; i++)
                builder.AppendLine(GetIndentedRule(DependencyChain[i], i));

            return builder.ToString();
        }

        static string GetIndentedRule(ExecutableRule rule, int iteration)
        {
            if (iteration == 0) return rule.ToString();

            var indentation = String.Concat(Enumerable.Repeat(indentString, iteration));
            var firstIndentation = String.Concat(Enumerable.Repeat(indentString, iteration - 1).Union(new[] { nextIndicator }));

            return rule.ToString()
                .Replace(Environment.NewLine, String.Concat(Environment.NewLine, indentation))
                .Insert(0, firstIndentation);
        }
    }
}