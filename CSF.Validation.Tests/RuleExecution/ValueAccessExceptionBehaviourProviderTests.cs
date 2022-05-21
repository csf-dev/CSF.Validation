using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.RuleExecution
{
    [TestFixture,Parallelizable]
    public class ValueAccessExceptionBehaviourProviderTests
    {
        [Test,AutoMoqData]
        public void GetBehaviourShouldReturnManifestBehaviourIfItIsNotNull(ValueAccessExceptionBehaviourProvider sut,
                                                                           [ManifestModel] ManifestValue manifestValue,
                                                                           ValidationOptions validationOptions,
                                                                           ValueAccessExceptionBehaviour valueBehaviour,
                                                                           ValueAccessExceptionBehaviour optionsBehaviour)
        {
            manifestValue.AccessorExceptionBehaviour = valueBehaviour;
            validationOptions.AccessorExceptionBehaviour = optionsBehaviour;
            Assert.That(() => sut.GetBehaviour(manifestValue, validationOptions), Is.EqualTo(valueBehaviour));
        }

        [Test,AutoMoqData]
        public void GetBehaviourShouldReturnOptionsBehaviourIfManifestBehaviourIsNull(ValueAccessExceptionBehaviourProvider sut,
                                                                                      [ManifestModel] ManifestValue manifestValue,
                                                                                      ValidationOptions validationOptions,
                                                                                      ValueAccessExceptionBehaviour optionsBehaviour)
        {
            manifestValue.AccessorExceptionBehaviour = null;
            validationOptions.AccessorExceptionBehaviour = optionsBehaviour;
            Assert.That(() => sut.GetBehaviour(manifestValue, validationOptions), Is.EqualTo(optionsBehaviour));
        }
    }
}