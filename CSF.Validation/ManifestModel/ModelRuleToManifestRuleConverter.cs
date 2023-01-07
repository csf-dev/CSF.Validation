using System;
using System.Collections.Generic;
using System.Linq;
using CSF.Validation.Manifest;
using CSF.Validation.ValidatorBuilding;

namespace CSF.Validation.ManifestModel
{
    /// <summary>
    /// A service which converts the rules within a <see cref="Value"/> into manifest rules
    /// and to add those rules to a corresponding <see cref="CSF.Validation.Manifest.ManifestValue"/>
    /// </summary>
    public class ModelRuleToManifestRuleConverter : IConvertsModelRulesToManifestRules
    {
        readonly IGetsRuleConfiguration configProvider;
        readonly IResolvesRuleType ruleTypeResolver;
        readonly IGetsManifestRuleIdentifierFromRelativeIdentifier relativeToManifestIdentifierConverter;

        /// <summary>
        /// Converts all of the rules present in each of the <see cref="Value"/> models into
        /// <see cref="CSF.Validation.Manifest.ManifestRule"/> instances and then adds those rules to
        /// the corresponding <see cref="CSF.Validation.Manifest.ManifestValue"/> instance in
        /// the model/manifest value pair.
        /// </summary>
        /// <remarks>
        /// <para>
        /// The input parameter is a collection of objects, each of which contains both a
        /// <see cref="CSF.Validation.Manifest.ManifestValue"/> and also the model <see cref="Value"/>
        /// from which that manifest value was created.  This method gets the rules from within
        /// that model value, converts them and then adds those the the corresponding manifest value.
        /// This process occurs for each item in the input collection.
        /// </para>
        /// </remarks>
        /// <param name="values">A collection of <see cref="ModelAndManifestValuePair"/>.</param>
        public void ConvertAllRulesAndAddToManifestValues(IEnumerable<ModelAndManifestValuePair> values)
        {
            var valuesAndRules = (from value in values
                                  from rule in value.ModelValue.Rules
                                  select new { Value = value, Rule = rule })
                .ToList();

            foreach (var valueAndRule in valuesAndRules)
            {
                var convertedRule = ConvertRule(valueAndRule.Rule, valueAndRule.Value.ManifestValue);
                valueAndRule.Value.ManifestValue.Rules.Add(convertedRule);
            }
        }

        ManifestRule ConvertRule(Rule rule, ManifestItem value)
        {
            var ruleType = ruleTypeResolver.GetRuleType(rule.RuleTypeName);

            return new ManifestRule(value, new ManifestRuleIdentifier(value, ruleType, rule.RuleName))
            {
                RuleConfiguration = configProvider.GetRuleConfigurationAction(ruleType, rule.RulePropertyValues),
                DependencyRules = GetDependencyRules(value, rule.Dependencies),
            };
        }

        ICollection<ManifestRuleIdentifier> GetDependencyRules(ManifestItem value, IEnumerable<RelativeIdentifier> dependencyIdentifiers)
        {
            return dependencyIdentifiers
                .Select(x => new RelativeRuleIdentifier(ruleTypeResolver.GetRuleType(x.RuleTypeName), x.MemberName, x.RuleName, x.AncestorLevels))
                .Select(x => relativeToManifestIdentifierConverter.GetManifestRuleIdentifier(value, x))
                .ToList();
        }

        /// <summary>
        /// Initialises an instance of <see cref="ModelRuleToManifestRuleConverter"/>.
        /// </summary>
        /// <param name="configProvider">A rule-configuration provider.</param>
        /// <param name="ruleTypeResolver">A rule-type resolver.</param>
        /// <param name="relativeToManifestIdentifierConverter">A converter which turns relative identifiers into manifest identifiers.</param>
        public ModelRuleToManifestRuleConverter(IGetsRuleConfiguration configProvider,
                                                IResolvesRuleType ruleTypeResolver,
                                                IGetsManifestRuleIdentifierFromRelativeIdentifier relativeToManifestIdentifierConverter)
        {
            this.configProvider = configProvider ?? throw new ArgumentNullException(nameof(configProvider));
            this.ruleTypeResolver = ruleTypeResolver ?? throw new ArgumentNullException(nameof(ruleTypeResolver));
            this.relativeToManifestIdentifierConverter = relativeToManifestIdentifierConverter ?? throw new ArgumentNullException(nameof(relativeToManifestIdentifierConverter));
        }
    }
}