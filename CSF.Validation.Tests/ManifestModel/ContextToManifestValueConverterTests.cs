using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture,Parallelizable]
    public class ContextToManifestValueConverterTests
    {
        [Test,AutoMoqData]
        public void GetManifestItemShouldReturnAManifestValue([ManifestModel] ModelToManifestConversionContext context,
                                                              ContextToManifestValueConverter sut,
                                                              [ManifestModel] ManifestValue parent)
        {
            context.ConversionType = ModelToManifestConversionType.Manifest;
            context.ParentManifestValue = parent;
            context.PolymorphicTypeName = "System.String";

            var result = sut.GetManifestItem(context);

            Assert.Multiple(() =>
            {
                Assert.That(result, Is.InstanceOf<ManifestValue>(), "Correct type");
                Assert.That(result.ParentItem, Is.SameAs(parent), "Correct parent item");
                Assert.That(result, Has.Property(nameof(ManifestValue.MemberName)).EqualTo(context.MemberName), "Correct member name");
                Assert.That(result, Has.Property(nameof(ManifestValue.AccessorFromParent)).EqualTo(context.AccessorFromParent), "Correct accessor");
                Assert.That(result.ValidatedType, Is.EqualTo(context.ValidatedType), "Correct validated type");
                Assert.That(result,
                            Has.Property(nameof(ManifestValue.AccessorExceptionBehaviour)).EqualTo(((Value) context.CurrentValue).AccessorExceptionBehaviour),
                            "Correct accessor exception behaviour");
                Assert.That(parent.Children, Does.Contain(result), "Result added as one of parent's children");
            });
        }
    }
}