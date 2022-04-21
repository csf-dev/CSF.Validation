using System;
using System.Linq;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture,Parallelizable]
    public class ValidationManifestFromModelConverterIntegrationTests
    {
        [Test,AutoMoqData]
        public void GetValidationManifestShouldReturnANonNullValidationManifest([IntegrationTesting] IServiceProvider services)
        {
            var result = GetValidationManifest(services);
            Assert.That(result, Is.Not.Null);
        }

        [Test,AutoMoqData]
        public void GetValidationManifestShouldHavePreciselyOneRuleAtTheRootLevel([IntegrationTesting] IServiceProvider services)
        {
            var result = GetValidationManifest(services);
            Assert.That(result.RootValue.Rules, Has.Count.EqualTo(1));
        }

        [Test,AutoMoqData]
        public void GetValidationManifestShouldHavePreciselyThreeChildValues([IntegrationTesting] IServiceProvider services)
        {
            var result = GetValidationManifest(services);
            Assert.That(result.RootValue.Children, Has.Count.EqualTo(3));
        }

        [Test,AutoMoqData]
        public void GetValidationManifestShouldHaveTwoRulesForTheStringPropertyValue([IntegrationTesting] IServiceProvider services)
        {
            var result = GetValidationManifest(services);
            Assert.That(result.RootValue.Children.Single(x => x.MemberName == nameof(ComplexObject.StringProperty)).Rules, Has.Count.EqualTo(2));
        }

        [Test,AutoMoqData]
        public void GetValidationManifestShouldEnumerateItemsInTheChildrenValue([IntegrationTesting] IServiceProvider services)
        {
            var result = GetValidationManifest(services);
            Assert.That(result.RootValue.Children.Single(x => x.MemberName == nameof(ComplexObject.Children)).CollectionItemValue, Is.Not.Null);
        }

        [Test,AutoMoqData]
        public void GetValidationManifestShouldNotEnumerateItemsInTheAssociatedValue([IntegrationTesting] IServiceProvider services)
        {
            var result = GetValidationManifest(services);
            Assert.That(result.RootValue.Children.Single(x => x.MemberName == nameof(ComplexObject.Associated)).CollectionItemValue, Is.Null);
        }

        [Test,AutoMoqData]
        public void GetValidationManifestShouldHaveOneChildValueForTheAssociatedValue([IntegrationTesting] IServiceProvider services)
        {
            var result = GetValidationManifest(services);
            Assert.That(result.RootValue.Children.Single(x => x.MemberName == nameof(ComplexObject.Associated)).Children, Has.Count.EqualTo(1));
        }

        [Test,AutoMoqData]
        public void GetValidationManifestShouldHaveOneRuleForTheAssociatedValue([IntegrationTesting] IServiceProvider services)
        {
            var result = GetValidationManifest(services);
            Assert.That(result.RootValue.Children.Single(x => x.MemberName == nameof(ComplexObject.Associated)).Rules, Has.Count.EqualTo(1));
        }

        static ValidationManifest GetValidationManifest(IServiceProvider services)
        {
            var sut = services.GetRequiredService<IGetsValidationManifestFromModel>();
            var value = GetValidationModel();
            return sut.GetValidationManifest(value, typeof(ComplexObject));
        }

        static Value GetValidationModel()
        {
            return new Value
            {
                IdentityMemberName = nameof(ComplexObject.Identity),
                Rules =  {
                    new Rule { RuleTypeName = typeof(ObjectRule).AssemblyQualifiedName, }
                },
                Children = {
                    { nameof(ComplexObject.Associated), new Value {
                        IdentityMemberName = nameof(ComplexObject.Identity),
                        Rules =  {
                            new Rule { RuleTypeName = typeof(ObjectRule).AssemblyQualifiedName, }
                        },
                        Children = {
                            { nameof(ComplexObject.StringProperty), new Value {
                                Rules = {
                                    new Rule { RuleTypeName = typeof(StringValueRule).AssemblyQualifiedName },
                                }
                            } },
                        },
                    } },
                    { nameof(ComplexObject.Children), new Value {
                        CollectionItemValue = new CollectionItemValue
                        {
                            IdentityMemberName = nameof(ComplexObject.Identity),
                            Rules =  {
                                new Rule { RuleTypeName = typeof(ObjectRule).AssemblyQualifiedName, }
                            },
                            Children = {
                                { nameof(ComplexObject.StringProperty), new Value {
                                    Rules = {
                                        new Rule { RuleTypeName = typeof(StringValueRule).AssemblyQualifiedName },
                                    }
                                } },
                            },
                        }
                    } },
                    { nameof(ComplexObject.StringProperty), new Value {
                        Rules = {
                            new Rule { RuleTypeName = typeof(StringValueRule).AssemblyQualifiedName },
                            new Rule { RuleTypeName = typeof(ObjectRule).AssemblyQualifiedName },
                        }
                    } },
                },
            };
        }
    }
}