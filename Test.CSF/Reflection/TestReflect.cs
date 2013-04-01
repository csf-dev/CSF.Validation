using System;
using NUnit.Framework;
using CSF.Reflection;
using System.Reflection;
using Test.CSF.Mocks;

namespace Test.CSF.Reflection
{
  [TestFixture]
  public class TestReflect
  {
    #region fields

    private StubType TestStub;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      this.TestStub = new StubType();
    }

    #endregion

    #region static reflection tests

    [Test]
    public void TestReferenceTypeField()
    {
      FieldInfo member;
      member = Reflect.Field<StubType>(x => x.ReferenceTypeField);

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ReferenceTypeField", member.Name, "Member name");
    }

    [Test]
    public void TestValueTypeField()
    {
      FieldInfo member;
      member = Reflect.Field<StubType>(x => x.ValueTypeField);

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ValueTypeField", member.Name, "Member name");
    }

    [Test]
    public void TestReferenceTypeProperty()
    {
      PropertyInfo member;
      member = Reflect.Property<StubType>(x => x.ReferenceTypeProperty);

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ReferenceTypeProperty", member.Name, "Member name");
    }

    [Test]
    public void TestValueTypeProperty()
    {
      PropertyInfo member;
      member = Reflect.Property<StubType>(x => x.ValueTypeProperty);

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ValueTypeProperty", member.Name, "Member name");
    }

    [Test]
    public void TestReferenceTypeMethod()
    {
      MethodInfo member;
      member = Reflect.Method<StubType>(x => x.ReferenceTypeMethod());

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ReferenceTypeMethod", member.Name, "Member name");
    }

    [Test]
    public void TestValueTypeMethod()
    {
      MethodInfo member;
      member = Reflect.Method<StubType>(x => x.ValueTypeMethod());

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ValueTypeMethod", member.Name, "Member name");
    }

    [Test]
    public void TestActionMethod()
    {
      MethodInfo member;
      member = Reflect.Method<StubType>(x => x.Action());

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("Action", member.Name, "Member name");
    }

    [Test]
    public void TestMember()
    {
      MemberInfo member;
      member = Reflect.Member<StubType>(x => x.Action());

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("Action", member.Name, "Member name");
    }

    #endregion

    #region static reflection extension method tests

    [Test]
    public void TestReferenceTypeFieldExtension()
    {
      FieldInfo member;
      member = this.TestStub.GetField(x => x.ReferenceTypeField);

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ReferenceTypeField", member.Name, "Member name");
    }

    [Test]
    public void TestValueTypeFieldExtension()
    {
      FieldInfo member;
      member = this.TestStub.GetField(x => x.ValueTypeField);

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ValueTypeField", member.Name, "Member name");
    }

    [Test]
    public void TestReferenceTypePropertyExtension()
    {
      PropertyInfo member;
      member = this.TestStub.GetProperty(x => x.ReferenceTypeProperty);

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ReferenceTypeProperty", member.Name, "Member name");
    }

    [Test]
    public void TestValueTypePropertyExtension()
    {
      PropertyInfo member;
      member = this.TestStub.GetProperty(x => x.ValueTypeProperty);

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ValueTypeProperty", member.Name, "Member name");
    }

    [Test]
    public void TestReferenceTypeMethodExtension()
    {
      MethodInfo member;
      member = this.TestStub.GetMethod(x => x.ReferenceTypeMethod());

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ReferenceTypeMethod", member.Name, "Member name");
    }

    [Test]
    public void TestValueTypeMethodExtension()
    {
      MethodInfo member;
      member = this.TestStub.GetMethod(x => x.ValueTypeMethod());

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("ValueTypeMethod", member.Name, "Member name");
    }

    [Test]
    public void TestActionMethodExtension()
    {
      MethodInfo member;
      member = this.TestStub.GetMethod(x => x.Action());

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("Action", member.Name, "Member name");
    }

    [Test]
    public void TestMemberExtension()
    {
      MemberInfo member;
      member = this.TestStub.GetMember(x => x.Action());

      Assert.IsNotNull(member, "Member nullability");
      Assert.AreEqual("Action", member.Name, "Member name");
    }

    #endregion

    #region other tests

    [Test]
    public void TestGetTypeFromAppDomain()
    {
      Type targetType = Reflect.TypeFromAppDomain("System.Configuration.ConnectionStringsSection");
      Assert.IsNotNull(targetType);
      Assert.AreEqual(typeof(System.Configuration.ConnectionStringsSection), targetType, "Correct type");
    }

    #endregion

    #region mocks

    public class StubType
    {
      public string ReferenceTypeField;

      public int ValueTypeField;

      public string ReferenceTypeProperty
      {
        get;
        set;
      }

      public int ValueTypeProperty
      {
        get;
        set;
      }

      public string ReferenceTypeMethod()
      {
        return this.ReferenceTypeProperty;
      }

      public int ValueTypeMethod()
      {

        return this.ValueTypeProperty;
      }

      public void Action()
      {
        // Intentional no-op
      }

      public StubType()
      {
        this.ReferenceTypeProperty = "Foo bar baz";

        // Chosen by fair dice roll, guaranteed to be random!
        this.ValueTypeProperty = 4;
      }
    }

    #endregion
  }
}

