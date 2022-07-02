using System.Linq;
using NUnit.Framework;

namespace CSF.Validation.Bootstrap
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class MessageAssemblyScannerTests
    {
        [Test,AutoMoqData]
        public void GetMessageProviderTypesFromAssembliesShouldFindThreeMessageProvidersFromTheSamplePlugin()
        {
            var ruleTypes = MessageAssemblyScanner.GetMessageProviderTypesFromAssemblies(new[] { SamplePluginAssembly.GetAssembly() });
            var ruleTypeNames = ruleTypes.Select(x => x.Name).ToList();
            Assert.That(ruleTypeNames,
                        Does.Contain("SampleProvider").And.Contain("SecondSampleProvider").And.Contain("ThirdSampleProvider"),
                        "Message providers found contains all of the expected ones");
        }
    }
}