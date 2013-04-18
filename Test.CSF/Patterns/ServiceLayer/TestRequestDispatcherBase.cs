using System;
using NUnit.Framework;
using Moq;
using CSF.Patterns.ServiceLayer;
using System.Collections.Generic;

namespace Test.CSF.Patterns.ServiceLayer
{
  [TestFixture]
  public class TestRequestDispatcherBase
  {
    #region fields

    private Mock<IRequestDispatcher> MockDispatcher;
    private IRequestDispatcher Dispatcher;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      this.MockDispatcher = new Mock<IRequestDispatcher>();
      this.MockDispatcher
        .Setup(x => x.Register(It.IsAny<Type>(), It.IsAny<IRequestHandler>()))
        .Returns(this.MockDispatcher.Object);

      this.Dispatcher = new DummyRequestDispatcher(this.MockDispatcher.Object);
    }

    #endregion

    #region tests

    [Test]
    public void TestRegister()
    {
      this.Dispatcher.Register<MockRequestType1,MockHandlerType1>();

      this.MockDispatcher
        .Verify(x => x.Register(It.Is<Type>(typ => typ == typeof(MockRequestType1)), It.IsAny<MockHandlerType1>()),
                Times.Once());
    }

    [Test]
    public void TestRegisterWithFactory()
    {
      this.Dispatcher.Register<MockRequestType4,MockHandlerType4>(() => new MockHandlerType4("Bar"));

      this.MockDispatcher
        .Verify(x => x.Register(It.Is<Type>(typ => typ == typeof(MockRequestType4)), It.IsAny<MockHandlerType4>()),
                Times.Once());
    }

    [Test]
    public void TestRegisterFromAssemblyOf()
    {
      this.Dispatcher.RegisterFromAssemblyOf<DummyRequestDispatcher>();

      this.MockDispatcher
        .Verify(x => x.Register(It.IsAny<Type>(), It.IsAny<IRequestHandler>()),
                Times.AtLeast(2),
                "Some registrations processed");

      this.MockDispatcher
        .Verify(x => x.Register(It.Is<Type>(typ => typ == typeof(MockRequestType1)), It.IsAny<MockHandlerType1>()),
                Times.Once(),
                "Mock request 1 (and handler) registered");

      this.MockDispatcher
        .Verify(x => x.Register(It.Is<Type>(typ => typ == typeof(MockRequestType2)), It.IsAny<MockHandlerType2>()),
                Times.Once(),
                "Mock request 2 (and handler) registered");

      this.MockDispatcher
        .Verify(x => x.Register(It.Is<Type>(typ => typ == typeof(MockRequestType3)), It.IsAny<MockHandlerType3>()),
                Times.Never(),
                "Mock request 3 (and handler) NOT registered (the handler is abstract)");
    }

    #endregion

    #region contained types
    
    public class MockRequestType1 : IRequest {}
    public class MockRequestType2 : IRequest {}
    public class MockRequestType3 : IRequest {}
    public class MockRequestType4 : IRequest {}

    public class MockResponseType1 : Response {}
    public class MockResponseType2 : Response {}
    public class MockResponseType3 : Response {}
    public class MockResponseType4 : Response {}

    public class MockHandlerType1 : RequestHandlerBase<MockRequestType1,MockResponseType1> {}
    public class MockHandlerType2 : RequestHandlerBase<MockRequestType2,MockResponseType2> {}
    public abstract class MockHandlerType3 : RequestHandlerBase<MockRequestType3,MockResponseType3> {}
    public class MockHandlerType4 : RequestHandlerBase<MockRequestType4,MockResponseType4>
    {
      public string Foo;

      public MockHandlerType4(string foo)
      {
        this.Foo = foo;
      }
    }

    #endregion

    #region contained type (stub implementation of the base class we are testing, hands off to a mock)

    public class DummyRequestDispatcher : RequestDispatcherBase
    {
      private IRequestDispatcher Implementation;

      public override IResponse Dispatch (IRequest request)
      {
        return this.Implementation.Dispatch(request);
      }

      public override void DispatchRequestOnly (IRequest request)
      {
        this.Implementation.DispatchRequestOnly(request);
      }

      public override bool CanDispatch (Type requestType)
      {
        return this.Implementation.CanDispatch(requestType);
      }

      public override IDictionary<Type, IRequestHandler> GetRegisteredHandlers ()
      {
        return this.Implementation.GetRegisteredHandlers();
      }

      public override IRequestDispatcher Register (Type requestType, IRequestHandler handler)
      {
        this.Implementation.Register(requestType, handler);
        return this;
      }

      public override IRequestDispatcher Unregister (Type requestType)
      {
        this.Implementation.Unregister(requestType);
        return this;
      }

      public DummyRequestDispatcher(IRequestDispatcher implementation)
      {
        if(implementation == null)
        {
          throw new ArgumentNullException("implementation");
        }

        this.Implementation = implementation;
      }
    }

    #endregion
  }
}

