using System;
using System.Collections.Generic;
using AutoFixture.NUnit3;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.ManifestModel
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class RuleConfigurationFactoryTests
    {
        [Test,AutoMoqData]
        public void GetRuleConfigurationActionShouldGetActionWhichExecutesCorrectSetters([Frozen] IGetsPropertySetterAction setterFactory,
                                                                                         RuleConfigurationFactory sut,
                                                                                         Type type,
                                                                                         string name1,
                                                                                         string name2,
                                                                                         object val1,
                                                                                         object val2,
                                                                                         object target)
        {
            bool action1Executed = false, action2Executed = false;
            void Action1(object param1, object param2) => action1Executed = (param1 == target && param2 == val1);
            void Action2(object param1, object param2) => action2Executed = (param1 == target && param2 == val2);
            Mock.Get(setterFactory).Setup(x => x.GetSetterAction(type, name1)).Returns(Action1);
            Mock.Get(setterFactory).Setup(x => x.GetSetterAction(type, name2)).Returns(Action2);
            var rulePropertyValues = new Dictionary<string, object>
            {
                { name1, val1 },
                { name2, val2 },
            };

            var result = sut.GetRuleConfigurationAction(type, rulePropertyValues);

            result(target);
            Assert.That(action1Executed && action2Executed, Is.True, "Both actions have been executed");
        }
    }
}