using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using CSF.Validation.ValidatorBuilding;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation
{
    /// <summary>
    /// This is not a test in the traditional sense, none of the code here is exercised.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This is a class which makes use of the validation builder API in order to verify
    /// that the fluent interface may be used in the way it is intended.
    /// In essence, the compiler is the test, because it verifies that the validator-building
    /// API may use the syntax in the way which is desired.
    /// </para>
    /// </remarks>
    public class ValidatorBuilderApiVerification : IBuildsValidator<ValidatorBuilderApiVerification.StubValidatedObject>
    {
        public void ConfigureValidator(IConfiguresValidator<StubValidatedObject> config)
        {
            config
                .UseObjectIdentity(x => x.Identity)
                .AddRule<SampleSpecificRule>(r => {
                    r.Name = "My rule";
                    r.Dependencies.Add(new RelativeRuleIdentifier(typeof(SampleObjectRule), ruleName: "A rule name"));
                })
                .AddRule<SampleObjectRule>(r => r.ConfigureRule(c => c.RuleProperty = "A value"))
                .ForMember(x => x.StringProperty, m =>
                {
                    m.AddRule<StringValueRule>();
                })
                .ForMemberItems(x => x.ObjectCollection, m =>
                {
                    m.AddRules<ContainedObjectValidator>();
                });
        }

        #region Stub types

        public class StubValidatedObject
        {
            public Guid Identity { get; set; } = Guid.NewGuid();

            public string StringProperty { get; set; }

            public ICollection<InnerValidatedObject> ObjectCollection { get; } = new List<InnerValidatedObject>();
        }

        public class InnerValidatedObject
        {
            public Guid Identity { get; set; } = Guid.NewGuid();

            public int NumericProperty { get; set; }
        }

        public class SampleSpecificRule : IRule<StubValidatedObject>
        {
            public Task<RuleResult> GetResultAsync(StubValidatedObject validated, RuleContext context, CancellationToken token = default)
                => PassAsync();
        }

        public class SampleObjectRule : IRule<object>
        {
            public string RuleProperty { get; set; }

            public Task<RuleResult> GetResultAsync(object validated, RuleContext context, CancellationToken token = default)
                => PassAsync();
        }

        public class StringValueRule : IValueRule<string, object>
        {
            public Task<RuleResult> GetResultAsync(string value, object validated, ValueRuleContext context, CancellationToken token = default)
                => PassAsync();
        }

        #endregion
    }

    public class ContainedObjectValidator : IBuildsValidator<ValidatorBuilderApiVerification.InnerValidatedObject>
    {
        public void ConfigureValidator(IConfiguresValidator<ValidatorBuilderApiVerification.InnerValidatedObject> config)
        {
            config.UseObjectIdentity(x => x.Identity);
            
            config.ForMember(x => x.NumericProperty, m =>
            {
                m.AddRule<IntegerValueRule>();
            });
        }

        #region Inner validated object stub types

        public class IntegerValueRule : IValueRule<int, object>
        {
            public Task<RuleResult> GetResultAsync(int value, object validated, ValueRuleContext context, CancellationToken token = default)
                => PassAsync();
        }

        #endregion
    }
}