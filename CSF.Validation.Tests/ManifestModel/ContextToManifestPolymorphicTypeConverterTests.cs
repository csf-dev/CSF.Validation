using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture,Parallelizable]
    public class ContextToManifestPolymorphicTypeConverterTests
    {
        [Test,AutoMoqData]
        public void GetManifestItemShouldReturnAPolymorphicType([ManifestModel] ModelToManifestConversionContext context,
                                                               ContextToManifestPolymorphicTypeConverter sut,
                                                               [ManifestModel] ManifestValue parent)
        {
            context.ConversionType = ModelToManifestConversionType.PolymorphicType;
            context.ParentManifestValue = parent;
            context.PolymorphicTypeName = "System.String";

            var result = sut.GetManifestItem(context);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<ManifestPolymorphicType>(), "Correct type");
                Assert.That(result.Parent, Is.SameAs(parent), "Correct parent item");
                Assert.That(result.ValidatedType, Is.EqualTo(typeof(string)), "Correct polymorphic type");
                Assert.That(parent.PolymorphicTypes, Does.Contain(result), "Result added as one of parent's polymorphic types");
            });
        }
    }
}