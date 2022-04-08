using System;
using AutoFixture.NUnit3;
using CSF.Validation.Stubs;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Bootstrap
{
    [TestFixture,Parallelizable]
    public class ServiceProviderOrActivatorResolverTests
    {
        [Test,AutoMoqData]
        public void ResolveServiceShouldReturnInstanceFromServiceProviderIfItIsNotNull([Frozen] IServiceProvider resolver,
                                                                                    ServiceProviderOrActivatorResolver sut,
                                                                                    object service,
                                                                                    Type implType)
        {
            Mock.Get(resolver).Setup(x => x.GetService(implType)).Returns(service);
            Assert.That(() => sut.ResolveService<object>(implType), Is.SameAs(service));
        }

        [Test,AutoMoqData]
        public void ResolveServiceShouldThrowAneIfImplementationTypeIsNull(ServiceProviderOrActivatorResolver sut)
        {
            Assert.That(() => sut.ResolveService<object>(null), Throws.ArgumentNullException);
        }

        [Test,AutoMoqData]
        public void ResolveServiceShouldThrowArgExceptionIfImplementationTypeDoesNotImplementGenericType(ServiceProviderOrActivatorResolver sut)
        {
            Assert.That(() => sut.ResolveService<string>(typeof(ObjectRule)), Throws.ArgumentException);
        }

        [Test,AutoMoqData]
        public void ResolveServiceShouldReturnInstanceCreatedUsingActivatorIfServiceProviderReturnsNull([Frozen] IServiceProvider resolver,
                                                                                                     ServiceProviderOrActivatorResolver sut)
        {
            Mock.Get(resolver).Setup(x => x.GetService(typeof(ObjectRule))).Returns(() => null);
            Assert.That(() => sut.ResolveService<object>(typeof(ObjectRule)), Is.InstanceOf<ObjectRule>());
        }

        [Test,AutoMoqData]
        public void ResolveServiceShouldThrowIfServiceProviderReturnsNullAndRuleHasConstructorDependencies([Frozen] IServiceProvider resolver,
                                                                                                        ServiceProviderOrActivatorResolver sut)
        {
            Mock.Get(resolver).Setup(x => x.GetService(typeof(RuleWithConstructorDependency))).Returns(() => null);
            Assert.That(() => sut.ResolveService<object>(typeof(RuleWithConstructorDependency)), Throws.InstanceOf<ResolutionException>());
        }
    }
}