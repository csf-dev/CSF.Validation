//
// DefaultManifestIdentityTests.cs
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
using CSF.Reflection;
using CSF.Validation.Manifest;
using NUnit.Framework;

namespace CSF.Validation.Tests.Manifest
{
  [TestFixture]
  public class DefaultManifestIdentityTests
  {
    [Test]
    public void Equals_returns_true_for_equal_instances_without_any_optional_data()
    {
      // Arrange
      Type validated =  typeof(string), rule = typeof(int);
      var first = new DefaultManifestIdentity(validated, rule);
      var second = new DefaultManifestIdentity(validated, rule);

      // Act
      var result = first.Equals(second);

      // Assert
      Assert.IsTrue(result);
    }

    [Test]
    public void Equals_returns_true_for_equal_instances_with_names()
    {
      // Arrange
      Type validated =  typeof(string), rule = typeof(int);
      var name = "foobar";
      var first = new DefaultManifestIdentity(validated, rule, name: name);
      var second = new DefaultManifestIdentity(validated, rule, name: name);

      // Act
      var result = first.Equals(second);

      // Assert
      Assert.IsTrue(result);
    }

    [Test]
    public void Equals_returns_true_for_equal_instances_with_members()
    {
      // Arrange
      Type validated =  typeof(string), rule = typeof(int);
      var member = Reflect.Property<StubValidatedObject>(x => x.IntegerProperty);
      var first = new DefaultManifestIdentity(validated, rule, validatedMember: member);
      var second = new DefaultManifestIdentity(validated, rule, validatedMember: member);

      // Act
      var result = first.Equals(second);

      // Assert
      Assert.IsTrue(result);
    }

    [Test]
    public void Equals_returns_true_for_equal_instances_with_parent_rules()
    {
      // Arrange
      Type validated =  typeof(string), rule = typeof(int);
      var parent = new DefaultManifestIdentity(validated, rule);
      var first = new DefaultManifestIdentity(validated, rule, parentIdentity: parent);
      var second = new DefaultManifestIdentity(validated, rule, parentIdentity: parent);

      // Act
      var result = first.Equals(second);

      // Assert
      Assert.IsTrue(result);
    }

    [Test]
    public void Equals_returns_false_for_instances_with_different_validated_types()
    {
      // Arrange
      Type validatedOne = typeof(string), validatedTwo = typeof(DateTime), rule = typeof(int);
      var first = new DefaultManifestIdentity(validatedOne, rule);
      var second = new DefaultManifestIdentity(validatedTwo, rule);

      // Act
      var result = first.Equals(second);

      // Assert
      Assert.IsFalse(result);
    }

    [Test]
    public void ToString_gets_expected_result_for_type_and_rule()
    {
      // Arrange
      Type validated =  typeof(string), rule = typeof(int);
      var sut = new DefaultManifestIdentity(validated, rule);

      // Act
      var result = sut.ToString();

      // Assert
      Assert.AreEqual("String.Int32", result);
    }

    [Test]
    public void ToString_gets_expected_result_for_generic_type_and_rule()
    {
      // Arrange
      Type validated =  typeof(int?), rule = typeof(Func<DateTime?,string>);
      var sut = new DefaultManifestIdentity(validated, rule);

      // Act
      var result = sut.ToString();

      // Assert
      Assert.AreEqual("Nullable<Int32>.Func<Nullable<DateTime>,String>", result);
    }

    [Test]
    public void ToString_gets_expected_result_for_generic_type_and_rule_with_member()
    {
      // Arrange
      Type validated =  typeof(int?), rule = typeof(Func<DateTime?,string>);
      var member = Reflect.Property<StubValidatedObject>(x => x.IntegerProperty);
      var sut = new DefaultManifestIdentity(validated, rule, validatedMember: member);

      // Act
      var result = sut.ToString();

      // Assert
      Assert.AreEqual("Nullable<Int32>.IntegerProperty.Func<Nullable<DateTime>,String>", result);
    }

    [Test]
    public void ToString_gets_expected_result_for_generic_type_and_rule_with_member_and_name()
    {
      // Arrange
      Type validated =  typeof(int?), rule = typeof(Func<DateTime?,string>);
      var member = Reflect.Property<StubValidatedObject>(x => x.IntegerProperty);
      var name = "FooBarBaz";
      var sut = new DefaultManifestIdentity(validated, rule, validatedMember: member, name: name);

      // Act
      var result = sut.ToString();

      // Assert
      Assert.AreEqual("Nullable<Int32>.IntegerProperty.Func<Nullable<DateTime>,String>.FooBarBaz", result);
    }

    [Test]
    public void ToString_gets_expected_result_for_generic_type_and_rule_with_member_name_and_parent()
    {
      // Arrange
      Type validated =  typeof(int?), rule = typeof(Func<DateTime?,string>);
      var member = Reflect.Property<StubValidatedObject>(x => x.IntegerProperty);
      var name = "FooBarBaz";
      var parent = new DefaultManifestIdentity(typeof(string), typeof(int));
      var sut = new DefaultManifestIdentity(validated, rule, validatedMember: member, name: name, parentIdentity: parent);

      // Act
      var result = sut.ToString();

      // Assert
      Assert.AreEqual("String.Int32 ==> Nullable<Int32>.IntegerProperty.Func<Nullable<DateTime>,String>.FooBarBaz",
                      result);
    }

    [Test]
    [Description("This isn't a great test, but equally, I can't rely on these hash codes being stable cross-platform " +
                 "and across framework versions.  I really just care that it produces a number and doesn't crash.")]
    public void GetHashCode_produces_a_number()
    {
      // Arrange
      Type validated =  typeof(string), rule = typeof(int);
      var sut = new DefaultManifestIdentity(validated, rule);

      // Act
      var result = sut.GetHashCode();

      // Assert
      Assert.IsInstanceOf<int>(result);
      Assert.AreNotEqual(default(int), result);
    }
  }
}
