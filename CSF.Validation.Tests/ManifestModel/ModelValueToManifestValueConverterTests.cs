using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using CSF.Validation.Manifest;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ModelValueToManifestValueConverterTests
    {
        [Test,AutoMoqData]
        public void ConvertAllValuesShouldSuccessfullyConvertAMultiLevelValueHierarchy([Frozen] IGetsAccessorFunction accessorFactory,
                                                                                       [Frozen] IGetsValidatedType validatedTypeProvider,
                                                                                       [Frozen] IGetsManifestItemFromModelToManifestConversionContext itemFactory,
                                                                                       ModelValueToManifestValueConverter sut,
                                                                                       [ManifestModel] ModelToManifestConversionContext context,
                                                                                       ManifestItem item,
                                                                                       AccessorFunctionAndType accessor)
        {
            Mock.Get(accessorFactory)
                .Setup(x => x.GetAccessorFunction(It.IsAny<Type>(), It.IsAny<string>()))
                .Returns(accessor);
            Mock.Get(validatedTypeProvider)
                .Setup(x => x.GetValidatedType(It.IsAny<Type>(), It.IsAny<bool>()))
                .Returns((Type t, bool b) => t);
            Mock.Get(itemFactory)
                .Setup(x => x.GetManifestItem(It.IsAny<ModelToManifestConversionContext>())).Returns(item);

            context.CurrentValue = new Value
            {
                Children = new Dictionary<string, Value>
                {
                    { "Foo", new Value
                        {
                            Children = new Dictionary<string,Value>
                            {
                                { "Foo", new Value() },
                                { "Bar", new Value { ValidateRecursivelyAsAncestor = 1 } },
                                { "Baz", new Value
                                    {
                                        CollectionItemValue = new CollectionItemValue(),
                                    }
                                },
                            },
                            PolymorphicValues = new Dictionary<string,PolymorphicValue>
                            {
                                { "System.String", new PolymorphicValue() },
                            }
                        }
                    },
                    { "Bar", new Value() },
                }
            };

            sut.ConvertAllValues(context);

            Assert.Multiple(() =>
            {
                Mock.Get(itemFactory)
                    .Verify(x => x.GetManifestItem(It.Is<ModelToManifestConversionContext>(c => c.ConversionType == ModelToManifestConversionType.Manifest)),
                            Times.Exactly(5));
                Mock.Get(itemFactory)
                    .Verify(x => x.GetManifestItem(It.Is<ModelToManifestConversionContext>(c => c.ConversionType == ModelToManifestConversionType.PolymorphicType)),
                            Times.Exactly(1));
                Mock.Get(itemFactory)
                    .Verify(x => x.GetManifestItem(It.Is<ModelToManifestConversionContext>(c => c.ConversionType == ModelToManifestConversionType.CollectionItem)),
                            Times.Exactly(1));
                Mock.Get(itemFactory)
                    .Verify(x => x.GetManifestItem(It.Is<ModelToManifestConversionContext>(c => c.ConversionType == ModelToManifestConversionType.RecursiveManifestValue)),
                            Times.Exactly(1));
            });

            Mock.Get(itemFactory)
                .Verify(x => x.GetManifestItem(It.IsAny<ModelToManifestConversionContext>()), Times.Exactly(8));
        }
    }
}