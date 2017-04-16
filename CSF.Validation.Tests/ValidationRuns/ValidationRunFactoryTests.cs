//
// ValidationRunFactoryTests.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2017 Craig Fowler
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
using NUnit.Framework;
using System;
using CSF.Validation.Rules;
using CSF.Validation.Manifest;
using CSF.Validation.ValidationRuns;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Validation.Tests.ValidationRuns
{
  [TestFixture]
  public class ValidationRunFactoryTests
  {
    #region tests

    [Test,AutoMoqData]
    public void CreateRunnableRule_uses_resolver(IRuleResolver resolver,
                                                 IManifestRule manifestRule,
                                                 IRule rule)
    {
      // Arrange
      var sut = CreateSut(resolver: resolver);
      Mock.Get(resolver)
          .Setup(x => x.Resolve(manifestRule))
          .Returns(rule);

      // Act
      sut.CreateRunnableRule(manifestRule);

      // Assert
      Mock.Get(resolver).Verify(x => x.Resolve(manifestRule), Times.Once());
    }

    [Test,AutoMoqData]
    public void CreateRunnableRule_configures_rule(IRuleResolver resolver,
                                                   IManifestRule manifestRule,
                                                   IRule rule)
    {
      // Arrange
      var sut = CreateSut(resolver: resolver);
      Mock.Get(resolver)
          .Setup(x => x.Resolve(manifestRule))
          .Returns(rule);
      Mock.Get(manifestRule).Setup(x => x.Configure(rule));

      // Act
      sut.CreateRunnableRule(manifestRule);

      // Assert
      Mock.Get(manifestRule).Verify(x => x.Configure(rule), Times.Once());
    }

    [Test,AutoMoqData]
    public void CreateRunnableRule_uses_generator_when_identity_is_missing(IManifestIdentityGenerator generator,
                                                                           IManifestRule manifestRule,
                                                                           object identity)
    {
      // Arrange
      var sut = CreateSut(identityGenerator: generator);
      Mock.Get(manifestRule)
          .SetupGet(x => x.Identity)
          .Returns((object) null);
      Mock.Get(generator)
          .Setup(x => x.GetIdentity(manifestRule))
          .Returns(identity);

      // Act
      sut.CreateRunnableRule(manifestRule);

      // Assert
      Mock.Get(generator).Verify(x => x.GetIdentity(manifestRule), Times.Once());
    }

    [Test,AutoMoqData]
    public void CreateRunnableRule_does_not_use_generator_when_identity_is_provided(IManifestIdentityGenerator generator,
                                                                                    IManifestRule manifestRule,
                                                                                    object identity)
    {
      // Arrange
      var sut = CreateSut(identityGenerator: generator);
      Mock.Get(manifestRule)
          .SetupGet(x => x.Identity)
          .Returns(identity);
      Mock.Get(generator)
          .Setup(x => x.GetIdentity(manifestRule))
          .Returns(identity);

      // Act
      sut.CreateRunnableRule(manifestRule);

      // Assert
      Mock.Get(generator).Verify(x => x.GetIdentity(manifestRule), Times.Never());
    }

    [Test,AutoMoqData]
    public void CreateRunnableRule_returns_runnable_rule_instance(IManifestRule manifestRule)
    {
      // Arrange
      var sut = CreateSut();

      // Act
      var result = sut.CreateRunnableRule(manifestRule);

      // Assert
      Assert.NotNull(result);
    }

    [Test,AutoMoqData]
    public void FindAndProvideDependencies_finds_matching_dependencies_when_one_exists(IRunnableRule currentRule,
                                                                                       IManifestRule manifest,
                                                                                       IRunnableRule dependency,
                                                                                       IRunnableRule otherRule,
                                                                                       object dependencyId,
                                                                                       object otherId)
    {
      // Arrange
      var sut = CreateSut();
      Mock.Get(manifest)
          .SetupGet(x => x.DependencyIdentifiers)
          .Returns(new [] { dependencyId });
      Mock.Get(currentRule)
          .Setup(x => x.ProvideDependencies(It.IsAny<IEnumerable<IRunnableRule>>()));
      Mock.Get(dependency)
          .SetupGet(x => x.Identity)
          .Returns(dependencyId);
      Mock.Get(otherRule)
          .SetupGet(x => x.Identity)
          .Returns(otherId);
      var allRules = new [] { currentRule, dependency, otherRule };

      // Act
      sut.FindAndProvideDependencies(currentRule, manifest, allRules);

      // Assert
      Mock.Get(currentRule)
          .Verify(x => x.ProvideDependencies(It.Is<IEnumerable<IRunnableRule>>(rules => rules.Count() == 1 && rules.Single() == dependency)),
                  Times.Once());
    }

    [Test,AutoMoqData]
    public void FindAndProvideDependencies_finds_no_dependencies_when_none_exist(IRunnableRule currentRule,
                                                                                 IManifestRule manifest,
                                                                                 IRunnableRule otherRule,
                                                                                 object otherId)
    {
      // Arrange
      var sut = CreateSut();
      Mock.Get(manifest)
          .SetupGet(x => x.DependencyIdentifiers)
          .Returns(Enumerable.Empty<object>());
      Mock.Get(currentRule)
          .Setup(x => x.ProvideDependencies(It.IsAny<IEnumerable<IRunnableRule>>()));
      Mock.Get(otherRule)
          .SetupGet(x => x.Identity)
          .Returns(otherId);
      var allRules = new [] { currentRule, otherRule };

      // Act
      sut.FindAndProvideDependencies(currentRule, manifest, allRules);

      // Assert
      Mock.Get(currentRule)
          .Verify(x => x.ProvideDependencies(It.Is<IEnumerable<IRunnableRule>>(rules => rules.Count() == 0)),
                  Times.Once());
    }

    #endregion

    #region methods

    private ValidationRunFactory CreateSut(IRuleResolver resolver = null,
                                           IManifestIdentityGenerator identityGenerator = null)
    {
      if(resolver == null)
      {
        var rule = Mock.Of<IRule>();
        resolver = Mock.Of<IRuleResolver>(x => x.Resolve(It.IsAny<IManifestRule>()) == rule);
      }

      if(identityGenerator == null)
      {
        var identity = new object();
        identityGenerator = Mock.Of<IManifestIdentityGenerator>(x => x.GetIdentity(It.IsAny<IManifestRule>()) == identity);
      }

      return new ValidationRunFactory(resolver, identityGenerator);
    }

    #endregion
  }
}
