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
                .AddTransient<IGetsRuleExecutor, RuleExecutorFactory>()
                .AddTransient<IGetsValidatedValue, ValidatedValueFactory>()
                .AddTransient<IGetsSingleRuleExecutor, SingleRuleExecutorFactory>()
                .AddTransient<IGetsAllExecutableRules, ExecutableRulesFromValidatedValueProvider>()
                .AddTransient<IGetsRuleExecutionContext, RuleExecutionContextFactory>()
                .AddTransient<IGetsValidatedValueFromBasis, ValidatedValueFromBasisFactory>()
                .AddTransient<IGetsEnumerableItemsToBeValidated, EnumerableItemProvider>()
                .AddTransient<IGetsValueToBeValidated, ValueToBeValidatedProvider>()
                .AddTransient<IGetsAccessorExceptionBehaviour, ValueAccessExceptionBehaviourProvider>()
                ;

        }

        static IGetsAllExecutableRulesWithDependencies GetExecutableRulesAndDependenciesProvider(IServiceProvider resolver)
        {
            IGetsAllExecutableRulesWithDependencies output;

            output = new ExecutableRulesAndDependenciesProvider(resolver.GetRequiredService<IGetsAllExecutableRules>());
            output = new CircularDependencyPreventingRulesWithDependenciesDecorator(output, resolver.GetRequiredService<IDetectsCircularDependencies>());

            return output;
        }
    }
}