using System;
using CSF.Validation.RuleExecution;
using Microsoft.Extensions.DependencyInjection;

namespace CSF.Validation.Bootstrap
{
    static class RuleExecutionServiceCollectionExtensions
    {
        internal static IServiceCollection AddRuleExecutionServices(this IServiceCollection serviceCollection)
        {
            return serviceCollection
                .AddTransient<IDetectsCircularDependencies, CircularDependencyDetector>()
                .AddTransient<IGetsAllExecutableRulesWithDependencies>(GetExecutableRulesAndDependenciesProvider)
                .AddTransient<IGetsResultsAndUpdatesRulesWhichHaveDependencyFailures, FailedDependencyUpdaterAndResultProvider>()
                .AddTransient<IGetsRuleExecutor, RuleExecutorFactory>()
                .AddTransient<IGetsValidatedValue, ValidatedValueFactory>()
                .AddTransient<IGetsSingleRuleExecutor, SingleRuleExecutorFactory>()
                ;

        }

        static IGetsAllExecutableRulesWithDependencies GetExecutableRulesAndDependenciesProvider(IServiceProvider resolver)
        {
            IGetsAllExecutableRulesWithDependencies output;

            output = new ExecutableRulesAndDependenciesProvider(resolver.GetService<IGetsAllExecutableRules>());
            output = new CircularDependencyPreventingRulesWithDependenciesDecorator(output, resolver.GetService<IDetectsCircularDependencies>());

            return output;
        }
    }
}