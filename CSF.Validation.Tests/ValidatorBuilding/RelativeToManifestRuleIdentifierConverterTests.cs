using System;
using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.ValidatorBuilding
{
    [TestFixture,Parallelizable,Ignore("Temporarily broken, to be restored")]
    public class RelativeToManifestRuleIdentifierConverterTests
    {
        [Test,AutoMoqData]
        public void GetManifestRuleIdentifierShouldGetCorrectIdentifierForASiblingRule(RelativeToManifestRuleIdentifierConverter sut,
                                                                                       [ManifestModel] ManifestValue value,
                                                                                       Type ruleType)
        {
            var expected = new ManifestRuleIdentifier(value, ruleType);
            var result = sut.GetManifestRuleIdentifier(value, new RelativeRuleIdentifier(ruleType));
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test,AutoMoqData]
        public void GetManifestRuleIdentifierShouldGetCorrectIdentifierForASiblingRuleWitAName(RelativeToManifestRuleIdentifierConverter sut,
                                                                                               [ManifestModel] ManifestValue value,
                                                                                               Type ruleType,
                                                                                               string ruleName)
        {
            var expected = new ManifestRuleIdentifier(value, ruleType, ruleName);
            var result = sut.GetManifestRuleIdentifier(value, new RelativeRuleIdentifier(ruleType, ruleName: ruleName));
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test,AutoMoqData]
        public void GetManifestRuleIdentifierShouldGetCorrectIdentifierForAParentRule(RelativeToManifestRuleIdentifierConverter sut,
                                                                                      [ManifestModel] ManifestValue value,
                                                                                      [ManifestModel] ManifestValue parent,
                                                                                      Type ruleType)
        {
            value.Parent = parent;
            var expected = new ManifestRuleIdentifier(parent, ruleType);
            var result = sut.GetManifestRuleIdentifier(value, new RelativeRuleIdentifier(ruleType, ancestorLevels: 1));
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test,AutoMoqData]
        public void GetManifestRuleIdentifierShouldGetCorrectIdentifierForAParentRuleWithAName(RelativeToManifestRuleIdentifierConverter sut,
                                                                                      [ManifestModel] ManifestValue value,
                                                                                      [ManifestModel] ManifestValue parent,
                                                                                      Type ruleType,
                                                                                      string ruleName)
        {
            value.Parent = parent;
            var expected = new ManifestRuleIdentifier(parent, ruleType, ruleName);
            var result = sut.GetManifestRuleIdentifier(value, new RelativeRuleIdentifier(ruleType, ancestorLevels: 1, ruleName: ruleName));
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test,AutoMoqData]
        public void GetManifestRuleIdentifierShouldGetCorrectIdentifierForACousinRuleForAMember(RelativeToManifestRuleIdentifierConverter sut,
                                                                                                [ManifestModel] ManifestValue value,
                                                                                                [ManifestModel] ManifestValue parent,
                                                                                                [ManifestModel] ManifestValue cousin,
                                                                                                Type ruleType,
                                                                                                string memberName)
        {
            // A "Cousin" rule requires us to descend to the parent manifest
            // value, then enter a child based upon a member name
            value.Parent = parent;
            parent.Children.Add(cousin);
            cousin.MemberName = memberName;
            var expected = new ManifestRuleIdentifier(cousin, ruleType);
            var result = sut.GetManifestRuleIdentifier(value, new RelativeRuleIdentifier(ruleType, memberName, ancestorLevels: 1));
            Assert.That(result, Is.EqualTo(expected));
        }

        [Test,AutoMoqData]
        public void GetManifestRuleIdentifierShouldGetCorrectIdentifierForACousinRuleForAMemberWithARuleName(RelativeToManifestRuleIdentifierConverter sut,
                                                                                                             [ManifestModel] ManifestValue value,
                                                                                                             [ManifestModel] ManifestValue parent,
                                                                                                             [ManifestModel] ManifestValue cousin,
                                                                                                             Type ruleType,
                                                                                                             string memberName,
                                                                                                             string ruleName)
        {
            // A "Cousin" rule requires us to descend to the parent manifest
            // value, then enter a child based upon a member name
            value.Parent = parent;
            parent.Children.Add(cousin);
            cousin.MemberName = memberName;
            var expected = new ManifestRuleIdentifier(cousin, ruleType, ruleName);
            var result = sut.GetManifestRuleIdentifier(value, new RelativeRuleIdentifier(ruleType, memberName, ruleName, 1));
            Assert.That(result, Is.EqualTo(expected));
        }
        
        [Test,AutoMoqData]
        public void GetManifestRuleIdentifierShouldThrowIfExcessAncestorLevelsAreSpecified(RelativeToManifestRuleIdentifierConverter sut,
                                                                                           [ManifestModel] ManifestValue value,
                                                                                           [ManifestModel] ManifestValue parent,
                                                                                           Type ruleType)
        {
            value.Parent = parent;
            Assert.That(() => sut.GetManifestRuleIdentifier(value, new RelativeRuleIdentifier(ruleType, ancestorLevels: 2)),
                        Throws.ArgumentException.And.Message.StartWith("The ManifestValue must have at least 2 level(s) of ancestors."));
        }
        
        [Test,AutoMoqData]
        public void GetManifestRuleIdentifierShouldThrowIfNoMatchingMemberIsFound(RelativeToManifestRuleIdentifierConverter sut,
                                                                                  [ManifestModel] ManifestValue value,
                                                                                  [ManifestModel] ManifestValue parent,
                                                                                  Type ruleType)
        {
            value.Parent = parent;
            Assert.That(() => sut.GetManifestRuleIdentifier(value, new RelativeRuleIdentifier(ruleType, "NoMember", ancestorLevels: 1)),
                        Throws.ArgumentException.And.Message.StartWith("The ManifestValue must have configuration for a member named \"NoMember\"."));
        }

    }
}