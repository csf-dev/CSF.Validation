using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using NUnit.Framework;

namespace CSF.Validation
{
    [TestFixture,Parallelizable]
    public class SerializableValidationResultTests
    {
        [Test,AutoMoqData]
        public void SerialiseShouldBeAbleToRoundtripAResultObject(SerializableValidationRuleResult ruleResult1,
                                                                  SerializableValidationRuleResult ruleResult2)
        {
            var sut = new SerializableValidationResult { RuleResults = new[] { ruleResult1, ruleResult2 } };
            SerializableValidationResult clone;
            var formatter = new BinaryFormatter();

            using(var stream = new MemoryStream())
            {
#pragma warning disable SYSLIB0011
                formatter.Serialize(stream, sut);

                stream.Flush();
                stream.Position = 0;

                clone = (SerializableValidationResult) formatter.Deserialize(stream);
#pragma warning restore SYSLIB0011
            }

            Assert.Multiple(() =>
            {
                Assert.That(clone.RuleResults, Has.Length.EqualTo(sut.RuleResults.Length), "Rule results have same length.");
                Assert.That(clone.RuleResults.First().MemberName, Is.EqualTo(sut.RuleResults.First().MemberName), "First rule result has same member name.");
            });
        }
    }
}