using System;
using AutoFixture.NUnit3;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture,Parallelizable]
    public class RuleResolverTests
    {
        [Test,AutoMoqData]
        public void ResolveRuleShouldReturnInstanceFromServiceProviderIfItIsNotNull([Frozen] IServiceProvider resolver,
                                                                                    RuleResolver sut,
                                                                                    object rule,
                                                                                    Type ruleType)
        {
            Mock.Get(resolver).Setup(x => x.GetService(ruleType)).Returns(rule);
            Assert.That(() => sut.ResolveRule(ruleType), Is.SameAs(rule));
        }

        [Test,AutoMoqData]
        public void ResolveRuleShouldReturnInstanceCreatedUsingActivatorIfServiceProviderReturnsNull([Frozen] IServiceProvider resolver,
                                                                                                     RuleResolver sut)
        {
            Mock.Get(resolver).Setup(x => x.GetService(typeof(ObjectRule))).Returns(() => null);
            Assert.That(() => sut.ResolveRule(typeof(ObjectRule)), Is.InstanceOf<ObjectRule>());
        }

        [Test,AutoMoqData]
        public void ResolveRuleShouldThrowIfServiceProviderReturnsNullAndRuleHasConstructorDependencies([Frozen] IServiceProvider resolver,
                                                                                                        RuleResolver sut)
        {
            Mock.Get(resolver).Setup(x => x.GetService(typeof(RuleWithConstructorDependency))).Returns(() => null);
            Assert.That(() => sut.ResolveRule(typeof(RuleWithConstructorDependency)), Throws.InstanceOf<ValidationException>());
        }
    }
}