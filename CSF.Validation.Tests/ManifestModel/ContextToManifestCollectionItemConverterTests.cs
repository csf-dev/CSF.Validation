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
                                                               [ManifestModel] ManifestItem parent,
                                                               [ManifestModel] ManifestItem grandparent)
        {
            context.ConversionType = ModelToManifestConversionType.CollectionItem;
            context.ParentManifestValue = parent;
            parent.Parent = grandparent;
            var result = sut.GetManifestItem(context);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsCollectionItem, Is.True, "Is converted to a collection item");
                Assert.That(result.Parent, Is.SameAs(grandparent), "Correct parent item");
                Assert.That(result.ValidatedType, Is.EqualTo(context.ValidatedType), "Correct validated type");
                Assert.That(parent.CollectionItemValue, Is.SameAs(result), "Result added as parent's collection value");
            });
        }
    }
}