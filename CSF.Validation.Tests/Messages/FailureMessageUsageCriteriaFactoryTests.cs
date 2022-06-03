using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using NUnit.Framework;

namespace CSF.Validation.Messages
{
    [TestFixture, NUnit.Framework.Parallelizable]
    public class FailureMessageUsageCriteriaFactoryTests
    {
        [Test,AutoMoqData]
        public void GetNonGenericMessageCriteriaShouldReturnADoubleGenericAdapterForAProviderTypeWhichMatchesADoubleGenericRule(FailureMessageUsageCriteriaFactory sut,
                                                                                                                                TwoGenericCriteria criteria)
        {
            var providerInfo = new TestingMessageProviderInfo(criteria);
            Assert.That(() => sut.GetNonGenericMessageCriteria(providerInfo, typeof(IRule<string, int>)), Is.InstanceOf<FailureMessageCriteriaAdapter<string,int>>());
        }

        [Test,AutoMoqData]
        public void GetNonGenericMessageCriteriaShouldReturnASingleGenericAdapterForAProviderTypeWhichMatchesASingleGenericRule(FailureMessageUsageCriteriaFactory sut,
                                                                                                                                OneGenericCriteria criteria)
        {
            var providerInfo = new TestingMessageProviderInfo(criteria);
            Assert.That(() => sut.GetNonGenericMessageCriteria(providerInfo, typeof(IRule<string>)), Is.InstanceOf<FailureMessageCriteriaAdapter<string>>());
        }

        [Test,AutoMoqData]
        public void GetNonGenericMessageCriteriaShouldReturnASingleGenericAdapterForAProviderTypeWhichMatchesTheFirstTypeOfADoubleGenericRule(FailureMessageUsageCriteriaFactory sut,
                                                                                                                                              OneGenericCriteria criteria)
        {
            var providerInfo = new TestingMessageProviderInfo(criteria);
            Assert.That(() => sut.GetNonGenericMessageCriteria(providerInfo, typeof(IRule<string,int>)), Is.InstanceOf<FailureMessageCriteriaAdapter<string>>());
        }

        [Test,AutoMoqData]
        public void GetNonGenericMessageCriteriaShouldReturnTheSameInstanceIfItImplementsTheNonGenericCriteriaInterface(FailureMessageUsageCriteriaFactory sut,
                                                                                                                        NonGenericCriteria criteria)
        {
            var providerInfo = new TestingMessageProviderInfo(criteria);
            Assert.That(() => sut.GetNonGenericMessageCriteria(providerInfo, typeof(IRule<string,int>)), Is.SameAs(criteria));
        }

        [Test,AutoMoqData]
        public void GetNonGenericMessageCriteriaShouldReturnAUniversalMatchForAProviderTypeWhichHasNoMatchingCriteriaInterface(FailureMessageUsageCriteriaFactory sut,
                                                                                                                               TwoGenericCriteria criteria)
        {
            var providerInfo = new TestingMessageProviderInfo(criteria);
            Assert.That(() => sut.GetNonGenericMessageCriteria(providerInfo, typeof(IRule<string,bool>)), Is.InstanceOf<AllowAllUsageCriteriaProvider>());
        }

        [Test,AutoMoqData]
        public void GetNonGenericMessageCriteriaShouldReturnADoubleGenericAdapterIfTheProviderTypeIsAmbiguousButMatchesADoubleGenericRule(FailureMessageUsageCriteriaFactory sut,
                                                                                                                                          AmbiguousGenericCriteria criteria)
        {
            var providerInfo = new TestingMessageProviderInfo(criteria);
            Assert.That(() => sut.GetNonGenericMessageCriteria(providerInfo, typeof(IRule<string,int>)), Is.InstanceOf<FailureMessageCriteriaAdapter<string,int>>());
        }

        public class TwoGenericCriteria : IGetsFailureMessage, IHasFailureMessageUsageCriteria<string, int>
        {
            bool IHasFailureMessageUsageCriteria<string, int>.CanGetFailureMessage(string value, int parentValue, ValidationRuleResult result)
            {
                throw new System.NotImplementedException();
            }

            Task<string> IGetsFailureMessage.GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }
        }

        public class OneGenericCriteria : IGetsFailureMessage, IHasFailureMessageUsageCriteria<string>
        {
            bool IHasFailureMessageUsageCriteria<string>.CanGetFailureMessage(string value, ValidationRuleResult result)
            {
                throw new System.NotImplementedException();
            }

            Task<string> IGetsFailureMessage.GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }
        }

        public class NonGenericCriteria : IGetsFailureMessage, IHasFailureMessageUsageCriteria
        {
            bool IHasFailureMessageUsageCriteria.CanGetFailureMessage(ValidationRuleResult result)
            {
                throw new System.NotImplementedException();
            }

            Task<string> IGetsFailureMessage.GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }
        }

        public class AmbiguousGenericCriteria : IGetsFailureMessage, IHasFailureMessageUsageCriteria, IHasFailureMessageUsageCriteria<string,int>
        {
            bool IHasFailureMessageUsageCriteria.CanGetFailureMessage(ValidationRuleResult result)
            {
                throw new System.NotImplementedException();
            }

            bool IHasFailureMessageUsageCriteria<string, int>.CanGetFailureMessage(string value, int parentValue, ValidationRuleResult result)
            {
                throw new System.NotImplementedException();
            }

            Task<string> IGetsFailureMessage.GetFailureMessageAsync(ValidationRuleResult result, CancellationToken token)
            {
                throw new System.NotImplementedException();
            }
        }
    }
}