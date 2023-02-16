using System.Collections.Generic;
using CSF.Validation.ManifestModel;
using NUnit.Framework;

namespace CSF.Validation.Xml
{
    [TestFixture,Parallelizable]
    public class XmlManifestModelSerializerTests
    {
        static readonly Value model = new Value
            {
                Id = "1",
                Children = new Dictionary<string, Value>
                {
                    { "Foo", new Value
                        {
                            Id = "2",
                            Children = new Dictionary<string,Value>
                            {
                                { "Foo", new Value
                                    {
                                        AccessorExceptionBehaviour = Manifest.ValueAccessExceptionBehaviour.Throw,
                                        Id = "3",
                                        IdentityMemberName = "TheIdentity",
                                        Rules = new List<Rule>
                                        {
                                            new Rule
                                            {
                                                RuleName = "IAmARule",
                                                RuleTypeName = "IAmARuleType",
                                                RulePropertyValues = new Dictionary<string,object>
                                                {
                                                    { "One", 1 },
                                                    { "Two", "Two" },
                                                    { "False", false },
                                                },
                                            },
                                        },
                                    }
                                },
                                { "Bar", new Value { ValidateRecursivelyAsAncestor = 1, Id = "4" } },
                                { "Baz", new Value
                                    {
                                        Id = "5",
                                        CollectionItemValue = new Value { Id = "6" },
                                    }
                                },
                            },
                            PolymorphicValues = new Dictionary<string,Value>
                            {
                                { "System.String", new Value { Id = "7" } },
                            }
                        }
                    },
                    { "Bar", new Value { Id = "8" } },
                }
            };
        static readonly string serialized = "{\"Id\":\"1\",\"PolymorphicValues\":{},\"Children\":{\"Foo\":{\"Id\":\"2\",\"PolymorphicValues\":{\"System.String\":{\"Id\":\"7\",\"PolymorphicValues\":{},\"Children\":{},\"Rules\":[]}},\"Children\":{\"Foo\":{\"Id\":\"3\",\"AccessorExceptionBehaviour\":\"Throw\",\"PolymorphicValues\":{},\"Children\":{},\"Rules\":[{\"RuleTypeName\":\"IAmARuleType\",\"RuleName\":\"IAmARule\",\"RulePropertyValues\":{\"One\":1,\"Two\":\"Two\",\"False\":false},\"Dependencies\":[]}],\"IdentityMemberName\":\"TheIdentity\"},\"Bar\":{\"Id\":\"4\",\"PolymorphicValues\":{},\"Children\":{},\"Rules\":[],\"ValidateRecursivelyAsAncestor\":1},\"Baz\":{\"Id\":\"5\",\"PolymorphicValues\":{},\"Children\":{},\"Rules\":[],\"CollectionItemValue\":{\"Id\":\"6\",\"PolymorphicValues\":{},\"Children\":{},\"Rules\":[]}}},\"Rules\":[]},\"Bar\":{\"Id\":\"8\",\"PolymorphicValues\":{},\"Children\":{},\"Rules\":[]}},\"Rules\":[]}";

        [Test,AutoMoqData]
        public void SerializeManifestModelShouldProduceExpectedXml(XmlManifestModelSerializer sut)
        {
            Assert.That(() => {
                var result = sut.SerializeManifestModel(model);
                return result;
            }, Is.EqualTo(serialized));
        }

        [Test,AutoMoqData]
        public void DeserializeManifestModelShouldProduceExpectedModel(XmlManifestModelSerializer sut)
        {
            var result = sut.DeserializeManifestModel(serialized);
            Assert.That(result?.Children?["Foo"]?.Children?["Foo"]?.AccessorExceptionBehaviour, Is.EqualTo(Manifest.ValueAccessExceptionBehaviour.Throw));
        }
    }
}