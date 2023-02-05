using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ContextToManifestCollectionItemConverterTests
    {
        [Test,AutoMoqData]
        public void GetManifestItemShouldReturnACollectionItem([ManifestModel] ModelToManifestConversionContext context,
                                                               ContextToManifestCollectionItemConverter sut,
                                                               [ManifestModel] ManifestValue parent,
                                                               [ManifestModel] ManifestValue grandparent)
        {
            context.ConversionType = ModelToManifestConversionType.CollectionItem;
            context.ParentManifestValue = parent;
            parent.Parent = grandparent;
            var result = sut.GetManifestItem(context);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<ManifestCollectionItem>(), "Correct type");
                Assert.That(result.ParentItem, Is.SameAs(grandparent), "Correct parent item");
                Assert.That(result.ValidatedType, Is.EqualTo(context.ValidatedType), "Correct validated type");
                Assert.That(parent.CollectionItemValue, Is.SameAs(result), "Result added as parent's collection value");
            });
        }
    }
}