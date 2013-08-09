using System;
using NUnit.Framework;
using CSF.Entities;

namespace Test.CSF.Entities
{
  [TestFixture]
  public class TestIdentityParser
  {
    #region test static methods

    [Test]
    public void TestCreate()
    {
      var parser = IdentityParser.Create<DummyEntityOne>();

      Assert.IsNotNull(parser, "Nullability");
      Assert.IsInstanceOfType(typeof(IdentityParser<DummyEntityOne,int>), parser, "Correct type");
    }

    [Test]
    [Description("This entity type is bad - it does not match the root entity type in the hierarchy.")]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestCreateEntityMismatch()
    {
      IdentityParser.Create<DummyEntityThree>();
    }

    [Test]
    [Description("This entity type is bad - it does not implement the correct entity interface.")]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestCreateDoesNotImplementGenericIEntity()
    {
      IdentityParser.Create<DummyEntityFour>();
    }

    [Test]
    [Description("This entity type is bad - it implements the desired entity interface too many times.")]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestCreateImplementsGenericIEntityTooManyTimes()
    {
      IdentityParser.Create<DummyEntityFive>();
    }

    #endregion

    #region instance methods

    [Test]
    public void TestParse()
    {
      var parser = new IdentityParser<DummyEntityOne,int>();
      var result = parser.Parse(5);

      Assert.IsNotNull(result, "Nullability");
      Assert.AreEqual(typeof(DummyEntityOne), result.EntityType, "Entity type");
      Assert.AreEqual(typeof(Int32), result.IdentifierType, "Identifier type");
      Assert.AreEqual(5, result.Value, "Parsed value");
    }

    [Test]
    public void TestTryParse()
    {
      var parser = new IdentityParser<DummyEntityOne,int>();
      IIdentity<DummyEntityOne> result;
      var success = parser.TryParse(5, out result);

      Assert.IsTrue(success, "Success");

      Assert.IsNotNull(result, "Nullability");
      Assert.AreEqual(typeof(DummyEntityOne), result.EntityType, "Entity type");
      Assert.AreEqual(typeof(Int32), result.IdentifierType, "Identifier type");
      Assert.AreEqual(5, result.Value, "Parsed value");
    }

    [Test]
    public void TestTryParseFailure()
    {
      var parser = new IdentityParser<DummyEntityOne,int>();
      IIdentity<DummyEntityOne> result;
      var success = parser.TryParse("foo", out result);

      Assert.IsFalse(success, "Success");

      Assert.IsNull(result, "Nullability");
    }

    #endregion

    #region contained types

    public class DummyEntityOne : Entity<DummyEntityOne,int> {}

    public class DummyEntityTwo : Entity<DummyEntityTwo,string> {}

    public class DummyEntityThree : DummyEntityTwo {}

    public class DummyEntityFour : IEntity
    {
      public event EventHandler Dirty;

      public event EventHandler Deleted;

      public event EventHandler Created;

#pragma warning disable 618
      public IIdentity GetIdentity ()
      {
        throw new System.NotImplementedException ();
      }
#pragma warning restore 618

      public void SetIdentity (object identityValue)
      {
        throw new System.NotImplementedException ();
      }

      public bool ValidateIdentity (object identityValue)
      {
        throw new System.NotImplementedException ();
      }

      public void ClearIdentity ()
      {
        throw new System.NotImplementedException ();
      }

      public bool HasIdentity {
        get {
          throw new System.NotImplementedException ();
        }
      }

      public object Id {
        get {
          throw new System.NotImplementedException ();
        }
        set {
          throw new System.NotImplementedException ();
        }
      }
    }

    public class DummyEntityFive : DummyEntityTwo, IEntity<DummyEntityTwo,ulong>
    {
#pragma warning disable 618
      IIdentity IEntity.GetIdentity ()
      {
        throw new System.NotImplementedException ();
      }
#pragma warning restore 618

      void IEntity.SetIdentity (object identityValue)
      {
        throw new System.NotImplementedException ();
      }

      bool IEntity.ValidateIdentity (object identityValue)
      {
        throw new System.NotImplementedException ();
      }

      void IEntity.ClearIdentity ()
      {
        throw new System.NotImplementedException ();
      }

      bool IEntity.HasIdentity {
        get {
          throw new System.NotImplementedException ();
        }
      }

      object IEntity.Id {
        get {
          throw new System.NotImplementedException ();
        }
        set {
          throw new System.NotImplementedException ();
        }
      }

      bool IEquatable<IEntity<DummyEntityTwo, ulong>>.Equals (IEntity<DummyEntityTwo, ulong> other)
      {
        throw new System.NotImplementedException ();
      }

      Identity<DummyEntityTwo, ulong> IEntity<DummyEntityTwo, ulong>.GetIdentity ()
      {
        throw new System.NotImplementedException ();
      }

      public void SetIdentity (ulong identityValue)
      {
        throw new System.NotImplementedException ();
      }

      public bool ValidateIdentity (ulong identityValue)
      {
        throw new System.NotImplementedException ();
      }

      ulong IEntity<TestIdentityParser.DummyEntityTwo, ulong>.Id {
        get {
          throw new System.NotImplementedException ();
        }
        set {
          throw new System.NotImplementedException ();
        }
      }
    }

    #endregion
  }
}

