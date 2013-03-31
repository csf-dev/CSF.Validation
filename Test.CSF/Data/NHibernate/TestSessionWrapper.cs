using System;
using Moq;
using NHibernate;
using NUnit.Framework;
using CSF.Data.NHibernate;
using CSF.Entities;

namespace Test.CSF.Data.NHibernate
{
  [TestFixture]
  public class TestSessionWrapper
  {
    #region tests

    [Test]
    public void TestGet()
    {
      var session = new Mock<ISession>();
      IIdentity<TestEntity,uint> identity = Identity.Create<TestEntity,uint>(5);
      var entity = new Mock<TestEntity>();

      session
        .Setup(x => x.Get<TestEntity>(It.Is<uint>(val => val == identity.Value)))
        .Returns(entity.Object);

      var wrapper = new SessionWrapper(session.Object);

      var result = wrapper.Get(identity);

      Assert.AreSame(entity.Object, result, "Correct object returned.");
      session.Verify(x => x.Get<TestEntity>(It.Is<uint>(val => val == identity.Value)), Times.Once());
    }

    [Test]
    public void TestGetNull()
    {
      var session = new Mock<ISession>();
      IIdentity<TestEntity,uint> identity = null;

      session.Setup(x => x.Get<TestEntity>(It.IsAny<uint>()));

      var wrapper = new SessionWrapper(session.Object);

      var result = wrapper.Get(identity);
      Assert.IsNull(result, "Null returned.");
      session.Verify(x => x.Get<TestEntity>(It.IsAny<uint>()), Times.Never());
    }

    [Test]
    public void TestLoad()
    {
      var session = new Mock<ISession>();
      IIdentity<TestEntity,uint> identity = Identity.Create<TestEntity,uint>(5);
      var entity = new Mock<TestEntity>();

      session
        .Setup(x => x.Load<TestEntity>(It.Is<uint>(val => val == identity.Value)))
        .Returns(entity.Object);

      var wrapper = new SessionWrapper(session.Object);

      var result = wrapper.Load(identity);

      Assert.AreSame(entity.Object, result, "Correct object returned.");
      session.Verify(x => x.Load<TestEntity>(It.Is<uint>(val => val == identity.Value)), Times.Once());
    }

    [Test]
    public void TestLoadNull()
    {
      var session = new Mock<ISession>();
      IIdentity<TestEntity,uint> identity = null;

      session.Setup(x => x.Load<TestEntity>(It.IsAny<uint>()));

      var wrapper = new SessionWrapper(session.Object);

      var result = wrapper.Load(identity);
      Assert.IsNull(result, "Null returned.");
      session.Verify(x => x.Load<TestEntity>(It.IsAny<uint>()), Times.Never());
    }

    #endregion

    #region contained type

    public class TestEntity : Entity<TestEntity,uint>
    {
      public virtual string Bar { get; set; }
    }

    #endregion
  }
}

