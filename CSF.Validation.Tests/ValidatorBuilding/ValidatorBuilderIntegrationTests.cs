
using System;
using System.Linq;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture, Parallelizable,Ignore("Temporarily broken, to be restored")]
    public partial class ValidatorBuilderIntegrationTests
    {
        [Test,AutoMoqData]
        public void GetManifestValueShouldNotReturnNull([IntegrationTesting] IServiceProvider services)
        {
            var sut = GetValidatorBuilderForComplexObjectValidator(services);
            var manifestValue = sut.GetManifestValue();
            Assert.That(manifestValue, Is.Not.Null);
        }

        [Test,AutoMoqData]
        public void GetManifestValueShouldHavePreciselyOneRuleAtTheRootLevel([IntegrationTesting] IServiceProvider services)
        {
            var sut = GetValidatorBuilderForComplexObjectValidator(services);
            var manifestValue = sut.GetManifestValue();
            Assert.That(manifestValue.Rules, Has.Count.EqualTo(1));
        }

        [Test,AutoMoqData]
        public void GetManifestValueShouldHavePreciselyThreeChildValues([IntegrationTesting] IServiceProvider services)
        {
            var sut = GetValidatorBuilderForComplexObjectValidator(services);
            var manifestValue = sut.GetManifestValue();
            Assert.That(manifestValue.Children, Has.Count.EqualTo(3));
        }

        [Test,AutoMoqData]
        public void GetManifestValueShouldHaveTwoRulesForTheStringPropertyValue([IntegrationTesting] IServiceProvider services)
        {
            var sut = GetValidatorBuilderForComplexObjectValidator(services);
            var manifestValue = sut.GetManifestValue();
            Assert.That(manifestValue.Children.Single(x => x.MemberName == nameof(ComplexObject.StringProperty)).Rules, Has.Count.EqualTo(2));
        }

        // [Test,AutoMoqData]
        // public void GetManifestValueShouldEnumerateItemsInTheChildrenValue([IntegrationTesting] IServiceProvider services)
        // {
        //     var sut = GetValidatorBuilderForComplexObjectValidator(services);
        //     var manifestValue = sut.GetManifestValue();
        //     Assert.That(manifestValue.Children.Single(x => x.MemberName == nameof(ComplexObject.Children)).EnumerateItems, Is.True);
        // }

        // [Test,AutoMoqData]
        // public void GetManifestValueShouldNotEnumerateItemsInTheAssociatedValue([IntegrationTesting] IServiceProvider services)
        // {
        //     var sut = GetValidatorBuilderForComplexObjectValidator(services);
        //     var manifestValue = sut.GetManifestValue();
        //     Assert.That(manifestValue.Children.Single(x => x.MemberName == nameof(ComplexObject.Associated)).EnumerateItems, Is.False);
        // }

        [Test,AutoMoqData]
        public void GetManifestValueShouldHaveTwoChildValuesForTheAssociatedValue([IntegrationTesting] IServiceProvider services)
        {
            var sut = GetValidatorBuilderForComplexObjectValidator(services);
            var manifestValue = sut.GetManifestValue();
            Assert.That(manifestValue.Children.Single(x => x.MemberName == nameof(ComplexObject.Associated)).Children, Has.Count.EqualTo(2));
        }

        [Test,AutoMoqData]
        public void GetManifestValueShouldHaveOneRuleForTheAssociatedValue([IntegrationTesting] IServiceProvider services)
        {
            var sut = GetValidatorBuilderForComplexObjectValidator(services);
            var manifestValue = sut.GetManifestValue();
            Assert.That(manifestValue.Children.Single(x => x.MemberName == nameof(ComplexObject.Associated)).Rules, Has.Count.EqualTo(1));
        }

        [Test,AutoMoqData]
        public void GetManifestValueShouldHaveARuleAtChildLevelWithTheCorrectDependency([IntegrationTesting] IServiceProvider services)
        {
            var sut = GetValidatorBuilderForComplexObjectValidator(services);
            var manifestValue = sut.GetManifestValue();
            var childRule = manifestValue
                .Children
                .Single(x => x.MemberName == nameof(ComplexObject.Associated))
                .Children
                .Single(x => x.MemberName != nameof(ComplexObject.StringProperty))
                .Rules
                .Single();
            var expectedManifestValue = manifestValue
                .Children
                .Single(x => x.MemberName == nameof(ComplexObject.Associated))
                .Children
                .Single(x => x.MemberName == nameof(ComplexObject.StringProperty));
            var expectedIdentifier = new ManifestRuleIdentifier(expectedManifestValue, typeof(StringValueRule));

            Assert.That(childRule.DependencyRules, Has.Count.EqualTo(1).And.One.EqualTo(expectedIdentifier));
        }

        static IGetsManifestValue GetValidatorBuilderForComplexObjectValidator(IServiceProvider services)
        {
            var factory = services.GetRequiredService<IGetsValidatorBuilder>();
            var sut = factory.GetValidatorBuilder<ComplexObject>();
            new ComplexObjectValidator().ConfigureValidator(sut);
            return sut;
        }
    }
}