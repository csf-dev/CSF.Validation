using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class ContextToRecursiveManifestItemConverterTests
    {
        [Test,AutoMoqData]
        public void GetManifestItemShouldReturnARecuriveManifestItem([ManifestModel] ModelToManifestConversionContext context,
                                                                     ContextToRecursiveManifestItemConverter sut,
                                                                     [ManifestModel] ManifestItem parent,
                                                                     [ManifestModel] ManifestItem grandparent)
        {
            parent.Parent = grandparent;
            context.ParentManifestValue = parent;
            context.ConversionType = ModelToManifestConversionType.RecursiveManifestValue;
            context.CurrentValue.ValidateRecursivelyAsAncestor = 2;

            var result = sut.GetManifestItem(context);

            Assert.Multiple(() =>
            {
                Assert.That(result.IsRecursive, Is.True, "Converted to a recursive value");
                Assert.That(result, Has.Property(nameof(ManifestItem.RecursiveAncestor)).SameAs(grandparent), "Correct wrapped value");
                Assert.That(result, Has.Property(nameof(ManifestItem.MemberName)).EqualTo(context.MemberName), "Correct member name");
                Assert.That(result, Has.Property(nameof(ManifestItem.AccessorFromParent)).EqualTo(context.AccessorFromParent), "Correct accessor");
                Assert.That(parent.Children, Does.Contain(result), "Parent contains the result");
            });
        }

        [Test,AutoMoqData]
        public void GetManifestItemShouldThrowIfAncestorIsNegative([ManifestModel] ModelToManifestConversionContext context,
                                                                   ContextToRecursiveManifestItemConverter sut)
        {
            context.ConversionType = ModelToManifestConversionType.RecursiveManifestValue;
            context.CurrentValue.ValidateRecursivelyAsAncestor = -1;

            Assert.That(() => sut.GetManifestItem(context), Throws.InstanceOf<ValidationException>());
        }

        [Test,AutoMoqData]
        public void GetManifestItemShouldThrowIfNotEnoughAncestors([ManifestModel] ModelToManifestConversionContext context,
                                                                   ContextToRecursiveManifestItemConverter sut,
                                                                   [ManifestModel] ManifestItem parent,
                                                                   [ManifestModel] ManifestItem grandparent)
        {
            parent.Parent = grandparent;
            context.ParentManifestValue = parent;
            grandparent.Parent = null;
            context.ConversionType = ModelToManifestConversionType.RecursiveManifestValue;
            context.CurrentValue.ValidateRecursivelyAsAncestor = 3;

            Assert.That(() => sut.GetManifestItem(context), Throws.InstanceOf<ValidationException>());
        }
    }
}