using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Rules
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class MemberMustExistTests
    {
        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassResultIfValueIsNull(MemberMustExist sut, [RuleContext] RuleContext context, [ManifestModel] ManifestItem parent)
        {
            Assert.That(() => sut.GetResultAsync(null, parent, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassResultIfParentValidatedTypeIsNull(MemberMustExist sut, [RuleContext] RuleContext context, string memberName)
        {
            Assert.That(() => sut.GetResultAsync(memberName, null, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnPassResultIfValidatedTypeHasNamedMember(MemberMustExist sut,
                                                                                      [RuleContext] RuleContext context,
                                                                                      [ManifestModel] ManifestItem parent,
                                                                                        [ManifestModel] ManifestItem grandparent)
        {
            grandparent.ValidatedType = typeof(Person);
            parent.Parent = grandparent;
            Assert.That(() => sut.GetResultAsync("Name", parent, context), Is.PassingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetResultAsyncShouldReturnFailResultIfValidatedTypeHasNoNamedMember(MemberMustExist sut,
                                                                                        [RuleContext] RuleContext context,
                                                                                        [ManifestModel] ManifestItem parent,
                                                                                        [ManifestModel] ManifestItem grandparent)
        {
            grandparent.ValidatedType = typeof(Person);
            parent.Parent = grandparent;
            Assert.That(() => sut.GetResultAsync("ThisMemberDoesNotExist", parent, context), Is.FailingRuleResult);
        }

        [Test,AutoMoqData]
        public void GetFailureMessageAsyncShouldReturnCorrectMessage(MemberMustExist sut,
                                                                     [RuleResult] ValidationRuleResult result,
                                                                     [ManifestModel] ManifestItem parent)
        {
            parent.ValidatedType = typeof(Person);
            Assert.That(() => sut.GetFailureMessageAsync("ThisMemberDoesNotExist", parent, result),
                        Is.EqualTo("The ValidatedType CSF.Validation.Rules.MemberMustExistTests+Person must have a member named ThisMemberDoesNotExist, or else the MemberName property of the ManifestItem must be null."));
        }

        public class Person { public string Name { get; set; } }
    }
}