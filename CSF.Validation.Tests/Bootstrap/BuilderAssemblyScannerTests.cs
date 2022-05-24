using System.Linq;
using NUnit.Framework;

namespace CSF.Validation.Bootstrap
{
    [TestFixture,Parallelizable]
    public class BuilderAssemblyScannerTests
    {
        [Test,AutoMoqData]
        public void GetValidatorBuilderTypesFromAssembliesShouldFindTwoBuilderTypesFromTheSamplePlugin()
        {
            var ruleTypes = BuilderAssemblyScanner.GetValidatorBuilderTypesFromAssemblies(new[] { SamplePluginAssembly.GetAssembly() });
            var ruleTypeNames = ruleTypes.Select(x => x.Name).ToList();
            Assert.That(ruleTypeNames,
                        Does.Contain("FirstBuilderType").And.Contain("SecondBuilderType"),
                        "Builders found contains all of the expected ones");
        }
    }
}