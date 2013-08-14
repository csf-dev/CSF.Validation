using System;
using NUnit.Framework;
using CSF.Validation;
using Test.CSF.Mocks;

namespace Test.CSF.Validation
{
  [TestFixture]
  public class TestValidator
  {
    #region tests
    
    [Test]
    public void TestAddTest()
    {
      IValidator<SampleObject> validator = new Validator<SampleObject>();
      
      validator.AddTest(x => x.PropertyTwo > 5);
      
      Assert.AreEqual(1, validator.Tests.Count, "Correct count of rules");
      Assert.IsNull(validator.Tests[0].Member, "Member not null");
    }
    
    [Test]
    public void TestAddTestGeneric()
    {
      IValidator<SampleObject> validator = new Validator<SampleObject>();
      
      validator.AddTest<string>(x => x.PropertyOne,
                                y => y.Length > 4);
      
      Assert.AreEqual(1, validator.Tests.Count, "Correct count of rules");
      Assert.IsNotNull(validator.Tests[0].Member, "Member is not null");
      Assert.AreEqual("PropertyOne", validator.Tests[0].Member.Name, "Correct member name");
    }
    
    [Test]
    public void TestValidate()
    {
      var validator = new Validator<SampleObject>();
      
      validator.AddTest<string>(x => x.PropertyOne,
                                y => y.Length == 3,
                                "Test one");
      validator.AddTest<string>(x => x.PropertyOne,
                                y => y.StartsWith("f"),
                                "Test two");
      
      SampleObject target = new SampleObject() {
        PropertyOne = "bar"
      };
      
      Assert.IsFalse(validator.Validate(target), "Not valid");
      
      target.PropertyOne = "foo!";
      
      Assert.IsFalse(validator.Validate(target), "Still not valid");
      
      target.PropertyOne = "foo";
      
      Assert.IsTrue(validator.Validate(target), "Is valid");
    }

    [Test]
    [ExpectedException(typeof(ValidationFailureException<SampleObject>))]
    public void TestValidateThrowOnFailure()
    {
      var validator = new Validator<SampleObject>();
      
      validator
        .AddTest<string>(x => x.PropertyOne, y => y.Length == 3, "Test one")
        .AddTest<string>(x => x.PropertyOne, y => y.StartsWith("f"), "Test two")
        .ThrowOnFailure();
      
      SampleObject target = new SampleObject() {
        PropertyOne = "bar"
      };

      validator.Validate(target);
      Assert.Fail("Test should not reach this point");
    }
    
    [Test]
    public void TestValidateResults()
    {
      var validator = new Validator<SampleObject>();
      ValidationTestList<SampleObject> list;
      
      validator.AddTest<string>(x => x.PropertyOne,
                                y => y.Length == 3,
                                "Test one");
      validator.AddTest<string>(x => x.PropertyOne,
                                y => y.StartsWith("f"),
                                "Test two");
      
      SampleObject target = new SampleObject() {
        PropertyOne = "bar"
      };
      
      Assert.IsFalse(validator.Validate(target, out list), "Not valid");
      Assert.AreEqual(1, list.Count, "Correct count of failures (1)");
      Assert.AreEqual("Test two", list[0].Identifier, "Correct identifier (1)");
      
      target.PropertyOne = "foo!";
      
      Assert.IsFalse(validator.Validate(target, out list), "Still not valid");
      Assert.AreEqual(1, list.Count, "Correct count of failures (2)");
      Assert.AreEqual("Test one", list[0].Identifier, "Correct identifier (2)");
      
      target.PropertyOne = "foo";
      
      Assert.IsTrue(validator.Validate(target, out list), "Is valid");
      Assert.AreEqual(0, list.Count, "Correct count of failures (3)");
    }
    
    [Test]
    public void TestValidateEmailAddress()
    {
      var validator = new Validator<SampleObject>();
      
      validator.AddTest<string>(x => x.PropertyOne,
                                ValidationTests.String.EmailAddress,
                                "Test one");
      
      SampleObject target = new SampleObject() {
        PropertyOne = "invalid.invalid"
      };
      
      Assert.IsFalse(validator.Validate(target), "Not valid");
      
      target.PropertyOne = "test@example.com";
      
      Assert.IsTrue(validator.Validate(target), "Is valid");
    }
    
    #endregion
  }
}

