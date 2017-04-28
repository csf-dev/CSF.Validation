//
// MessageProviderIntegrationTests.cs
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
using System;
using System.Collections.Generic;
using CSF.Reflection;
using CSF.Validation.Manifest;
using CSF.Validation.Messages;
using CSF.Validation.StockRules;
using CSF.Validation.Tests.Resources;
using CSF.Validation.ValidationRuns;
using Moq;
using NUnit.Framework;

namespace CSF.Validation.Tests.Messages
{
  [TestFixture]
  public class MessageProviderIntegrationTests
  {
    [Test]
    public void GetFailureMessage_returns_expected_formatted_message_with_constant_replacements()
    {
      // Arrange
      var sut = GetSut();
      var failure = GetFailures()[0];

      // Act
      var result = sut.GetFailureMessage(failure);

      // Assert
      Assert.AreEqual("Number must be between 5 and 20.", result);
    }

    [Test]
    public void GetFailureMessage_returns_expected_formatted_message_with_dynamic_replacement()
    {
      // Arrange
      var sut = GetSut();
      var failure = GetFailures()[1];

      // Act
      var result = sut.GetFailureMessage(failure);

      // Assert
      Assert.AreEqual("The year 2001 is too early.", result);
    }

    [Test]
    public void GetFailureMessage_returns_expected_formatted_message_with_no_formatter()
    {
      // Arrange
      var sut = GetSut();
      var failure = GetFailures()[2];

      // Act
      var result = sut.GetFailureMessage(failure);

      // Assert
      Assert.AreEqual("Value must not be null.", result);
    }

    IMessageProvider GetSut()
    {
      var templateProvider = new FailureMessageTemplates();
      var provider = new ByIdentityFormatterProvider();

      provider.RegisterPlaceholderFormatter<StubValidatedObject>(typeof(NumericRangeValueRule),
                                                                 x => x.IntegerProperty,
                                                                 r => {
        r.Placeholder("{min}", 5);
        r.Placeholder("{max}", 20);
      });

      provider.RegisterPlaceholderFormatter<StubValidatedObject>(typeof(DateTimeRangeValueRule),
                                                                 x => x.DateTimeProperty,
                                                                 r => {
        r.Placeholder("{actual}", x => x.DateTimeProperty.Year);
      });

      return new MessageProvider(templateProvider, provider);
    }

    IRunnableRuleResult[] GetFailures()
    {
      var identityOne = new DefaultManifestIdentity(typeof(StubValidatedObject),
                                                    typeof(NumericRangeValueRule),
                                                    validatedMember: Reflect.Member<StubValidatedObject>(x => x.IntegerProperty));
      var identityTwo = new DefaultManifestIdentity(typeof(StubValidatedObject),
                                                    typeof(DateTimeRangeValueRule),
                                                    validatedMember: Reflect.Member<StubValidatedObject>(x => x.DateTimeProperty));
      var identityThree = new DefaultManifestIdentity(typeof(StubValidatedObject),
                                                      typeof(NotNullValueRule),
                                                      validatedMember: Reflect.Member<StubValidatedObject>(x => x.StringProperty));

      var validated = new StubValidatedObject { DateTimeProperty = new DateTime(2001, 1, 1) };

      return new [] {
        Mock.Of<IRunnableRuleResult>(x => x.ManifestIdentity == identityOne && x.Validated == validated),
        Mock.Of<IRunnableRuleResult>(x => x.ManifestIdentity == identityTwo && x.Validated == validated),
        Mock.Of<IRunnableRuleResult>(x => x.ManifestIdentity == identityThree && x.Validated == validated),
      };
    }
  }
}
