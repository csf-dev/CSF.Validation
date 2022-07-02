using System.Linq;
using NUnit.Framework;

namespace CSF.Validation.Bootstrap
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class RuleAssemblyScannerTests
    {
        [Test,AutoMoqData]
        public void GetRuleTypesFromAssembliesShouldFindThreeRulesFromTheSamplePlugin()
        {
            var ruleTypes = RuleAssemblyScanner.GetRuleTypesFromAssemblies(new[] { SamplePluginAssembly.GetAssembly() });
            var ruleTypeNames = ruleTypes.Select(x => x.Name).ToList();
            Assert.That(ruleTypeNames,
                        Does.Contain("SampleRule").And.Contain("SecondSampleRule").And.Contain("SampleRuleWithParent"),
                        "Rules found contains all of the expected ones");
        }
    }
}