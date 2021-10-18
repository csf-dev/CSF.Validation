using System;
using System.Linq;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using CSF.Validation.Stubs;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class ValidatedValueFactoryTests
    {
        [Test,AutoMoqData]
        public void GetValidatedValueShouldReturnSingleValueForManifestValueWithNoParentOrChildrenOrRules(ValidatedValueFactory sut, object validatedValue)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(object),
            };

            var result = sut.GetValidatedValue(manifestValue, validatedValue);

            Assert.Multiple(() =>
            {
                Assert.That(result.ActualValue, Is.SameAs(validatedValue), nameof(ValidatedValue.ActualValue));
                Assert.That(result.ManifestValue, Is.SameAs(manifestValue), nameof(ValidatedValue.ManifestValue));
                Assert.That(result.ParentValue, Is.Null, nameof(ValidatedValue.ParentValue));
                Assert.That(result.ChildValues, Is.Empty, nameof(ValidatedValue.ChildValues));
            });
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldReturnParentAndChildValuesForObjectWithAChild(ValidatedValueFactory sut,
                                                                                         [NoAutoProperties] ComplexObject validatedValue,
                                                                                         [NoAutoProperties] ComplexObject childValue)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestValue
            {
                Parent = manifestValue,
                AccessorFromParent = obj => ((ComplexObject)obj).Associated,
            };
            manifestValue.Children.Add(childManifest);
            validatedValue.Associated = childValue;

            var result = sut.GetValidatedValue(manifestValue, validatedValue);

            Assert.That(result.ChildValues,
                        Has.One.Matches<ValidatedValue>(v => v.ManifestValue == childManifest
                                                          && v.ActualValue == childValue
                                                          && v.ParentValue == result));
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldThrowIfAChildAccessorThrowsAndExceptionsAreNotIgnored([Frozen] ValidationOptions options,
                                                                                                 ValidatedValueFactory sut,
                                                                                                 [NoAutoProperties] ComplexObject validatedValue,
                                                                                                 Exception exception)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestValue
            {
                Parent = manifestValue,
                AccessorFromParent = obj => throw exception,
                ValidatedType = typeof(ComplexObject),
            };
            manifestValue.Children.Add(childManifest);
            options.IgnoreValueAccessExceptions = false;

            Assert.That(() => sut.GetValidatedValue(manifestValue, validatedValue), Throws.InstanceOf<ValidationException>().And.InnerException.SameAs(exception));
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldNotThrowIfAChildAccessorThrowsButExceptionsAreIgnoredGlobally([Frozen] ValidationOptions options,
                                                                                                         ValidatedValueFactory sut,
                                                                                                         [NoAutoProperties] ComplexObject validatedValue,
                                                                                                         Exception exception)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestValue
            {
                Parent = manifestValue,
                AccessorFromParent = obj => throw exception,
                ValidatedType = typeof(ComplexObject),
            };
            manifestValue.Children.Add(childManifest);
            options.IgnoreValueAccessExceptions = true;

            ValidatedValue result = null;
            Assert.Multiple(() =>
            {
                Assert.That(() => result = sut.GetValidatedValue(manifestValue, validatedValue), Throws.Nothing, "Throws no exception");
                Assert.That(result?.ChildValues.Single().ActualValue, Is.Null, "Actual value has been set to null");
            });
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldNotThrowIfAChildAccessorThrowsButExceptionsAreIgnoredForThisManifestValue([Frozen] ValidationOptions options,
                                                                                                                     ValidatedValueFactory sut,
                                                                                                                     [NoAutoProperties] ComplexObject validatedValue,
                                                                                                                     Exception exception)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestValue
            {
                Parent = manifestValue,
                AccessorFromParent = obj => throw exception,
                IgnoreAccessorExceptions = true,
                ValidatedType = typeof(ComplexObject),
            };
            manifestValue.Children.Add(childManifest);
            options.IgnoreValueAccessExceptions = false;

            ValidatedValue result = null;
            Assert.Multiple(() =>
            {
                Assert.That(() => result = sut.GetValidatedValue(manifestValue, validatedValue), Throws.Nothing, "Throws no exception");
                Assert.That(result?.ChildValues.Single().ActualValue, Is.Null, "Actual value has been set to null");
            });
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldReturnValueUsingIdentityIfAnAccessorIsProvided(ValidatedValueFactory sut, [NoAutoProperties] ComplexObject validatedValue)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
                IdentityAccessor = obj => ((ComplexObject) obj).Identity,
            };

            var result = sut.GetValidatedValue(manifestValue, validatedValue);

            Assert.That(result.ValueIdentity, Is.EqualTo(validatedValue.Identity));
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldReturnValueWithNullIdentityIfAccessorIsNotProvided(ValidatedValueFactory sut, [NoAutoProperties] ComplexObject validatedValue)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };

            var result = sut.GetValidatedValue(manifestValue, validatedValue);

            Assert.That(result.ValueIdentity, Is.Null);
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldReturnACollectionOfChildValuesWhenEnumerateItemsIsEnabled(ValidatedValueFactory sut,
                                                                                                     [NoAutoProperties] ComplexObject validatedValue,
                                                                                                     [NoAutoProperties] ComplexObject child1,
                                                                                                     [NoAutoProperties] ComplexObject child2,
                                                                                                     [NoAutoProperties] ComplexObject child3)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestValue
            {
                Parent = manifestValue,
                AccessorFromParent = obj => ((ComplexObject)obj).Children,
                EnumerateItems = true,
            };
            manifestValue.Children.Add(childManifest);
            validatedValue.Children.Add(child1);
            validatedValue.Children.Add(child2);
            validatedValue.Children.Add(child3);

            var result = sut.GetValidatedValue(manifestValue, validatedValue);

            Assert.Multiple(() =>
            {
                Assert.That(result.ChildValues, Has.One.Matches<ValidatedValue>(v => v.ActualValue == child1 && v.CollectionItemOrder == 0), "Child 1");
                Assert.That(result.ChildValues, Has.One.Matches<ValidatedValue>(v => v.ActualValue == child2 && v.CollectionItemOrder == 1), "Child 2");
                Assert.That(result.ChildValues, Has.One.Matches<ValidatedValue>(v => v.ActualValue == child3 && v.CollectionItemOrder == 2), "Child 3");
            });
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldThrowIfEnumerateItemsIsEnabledButValueIsNotEnumerable(ValidatedValueFactory sut,
                                                                                                 [NoAutoProperties] ComplexObject validatedValue,
                                                                                                 [NoAutoProperties] ComplexObject childValue)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestValue
            {
                Parent = manifestValue,
                AccessorFromParent = obj => ((ComplexObject)obj).Associated,
                EnumerateItems = true,
            };
            manifestValue.Children.Add(childManifest);
            validatedValue.Associated = childValue;

            Assert.That(() => sut.GetValidatedValue(manifestValue, validatedValue), Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldReturnParentThroughGrandchildForObjectWithAGrandchild(ValidatedValueFactory sut,
                                                                                                 [NoAutoProperties] ComplexObject validatedValue,
                                                                                                 [NoAutoProperties] ComplexObject childValue,
                                                                                                 [NoAutoProperties] ComplexObject grandchildValue)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestValue
            {
                Parent = manifestValue,
                AccessorFromParent = obj => ((ComplexObject)obj).Associated,
            };
            var grandchildManifest = new ManifestValue
            {
                Parent = childManifest,
                AccessorFromParent = obj => ((ComplexObject)obj).Associated,
            };
            manifestValue.Children.Add(childManifest);
            childManifest.Children.Add(grandchildManifest);
            validatedValue.Associated = childValue;
            childValue.Associated = grandchildValue;

            var result = sut.GetValidatedValue(manifestValue, validatedValue);

            Assert.That(result.ChildValues.Single().ChildValues.Single().ActualValue, Is.SameAs(grandchildValue));
        }

        [Test,AutoMoqData]
        public void GetValidatedValueShouldNotIncludeAChildObjectIfTheParentIsNull(ValidatedValueFactory sut)
        {
            var manifestValue = new ManifestValue
            {
                ValidatedType = typeof(ComplexObject),
            };
            var childManifest = new ManifestValue
            {
                Parent = manifestValue,
                AccessorFromParent = obj => ((ComplexObject)obj).Associated,
            };
            manifestValue.Children.Add(childManifest);

            var result = sut.GetValidatedValue(manifestValue, null);

            Assert.That(result.ChildValues, Is.Empty);
        }
    }
}