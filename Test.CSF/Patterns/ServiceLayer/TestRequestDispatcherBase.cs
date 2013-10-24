using System;
using NUnit.Framework;
using Moq;
using CSF.Patterns.ServiceLayer;
using System.Collections.Generic;
using System.Reflection;

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
        .Setup(x => x.Register(It.IsAny<Type>(), It.IsAny<Func<IRequestHandler>>()))
        .Returns(this.MockDispatcher.Object);

      this.Dispatcher = new DummyRequestDispatcher(this.MockDispatcher.Object, 0);
    }

    #endregion

    #region tests

    [Test]
    public void TestRegister()
    {
      this.Dispatcher.Register<MockRequestType1,MockHandlerType1>();

      this.MockDispatcher
        .Verify(x => x.Register(It.Is<Type>(typ => typ == typeof(MockRequestType1)), It.IsAny<Func<IRequestHandler>>()),
                Times.Once());
    }

    [Test]
    public void TestRegisterWithFactory()
    {
      this.Dispatcher.Register<MockRequestType4>(() => new MockHandlerType4("Bar"));

      this.MockDispatcher
        .Verify(x => x.Register(It.Is<Type>(typ => typ == typeof(MockRequestType4)), It.IsAny<Func<IRequestHandler>>()),
                Times.Once());
    }

    [Test]
    public void TestRegisterFromAssemblyOf()
    {
      this.Dispatcher.RegisterFromAssemblyOf<DummyRequestDispatcher>();

      this.MockDispatcher
        .Verify(x => x.Register(It.IsAny<Type>(), It.IsAny<Func<IRequestHandler>>()),
                Times.AtLeast(2),
                "Some registrations processed");

      this.MockDispatcher
        .Verify(x => x.Register(It.Is<Type>(typ => typ == typeof(MockRequestType1)), It.IsAny<Func<IRequestHandler>>()),
                Times.Once(),
                "Mock request 1 (and handler) registered");

      this.MockDispatcher
        .Verify(x => x.Register(It.Is<Type>(typ => typ == typeof(MockRequestType2)), It.IsAny<Func<IRequestHandler>>()),
                Times.Once(),
                "Mock request 2 (and handler) registered");

      this.MockDispatcher
        .Verify(x => x.Register(It.Is<Type>(typ => typ == typeof(MockRequestType3)), It.IsAny<Func<IRequestHandler>>()),
                Times.Never(),
                "Mock request 3 (and handler) NOT registered (the handler is abstract)");
    }

    [Test]
    public void TestGetRequestHandlerTypes()
    {
      Assembly target = Assembly.GetAssembly(typeof(Test.CSF.StubAssembly.Patterns.ServiceLayer.RequestOne));
      var result = RequestDispatcherBase.GetRequestHandlerTypes(target);

      Assert.IsNotNull(result, "Nullability");
      Assert.AreEqual(3, result.Count, "Count of items");
      Assert.AreEqual(typeof(Test.CSF.StubAssembly.Patterns.ServiceLayer.RequestHandlerOne),
                      result[typeof(Test.CSF.StubAssembly.Patterns.ServiceLayer.RequestOne)],
                      "First type");
      Assert.AreEqual(typeof(Test.CSF.StubAssembly.Patterns.ServiceLayer.RequestHandlerTwo),
                      result[typeof(Test.CSF.StubAssembly.Patterns.ServiceLayer.RequestTwo)],
                      "Second type");
      Assert.AreEqual(typeof(Test.CSF.StubAssembly.Patterns.ServiceLayer.RequestHandlerThree),
                      result[typeof(Test.CSF.StubAssembly.Patterns.ServiceLayer.RequestThree)],
                      "Third type");
    }

    #endregion

    #region contained types
    
    public class MockRequestType1 : IRequest {}
    public class MockRequestType2 : IRequest {}
    public class MockRequestType3 : IRequest {}
    public class MockRequestType4 : IRequest {}

    public class MockResponseType1 : Response
    {
      public MockResponseType1() : base(null) { }
    }
    public class MockResponseType2 : Response
    {
      public MockResponseType2() : base(null) { }
    }
    public class MockResponseType3 : Response
    {
      public MockResponseType3() : base(null) { }
    }
    public class MockResponseType4 : Response
    {
      public MockResponseType4() : base(null) { }
    }

    public class MockHandlerType1 : RequestHandler<MockRequestType1,MockResponseType1> {}
    public class MockHandlerType2 : RequestHandler<MockRequestType2,MockResponseType2> {}
    public abstract class MockHandlerType3 : RequestHandler<MockRequestType3,MockResponseType3> {}
    public class MockHandlerType4 : RequestHandler<MockRequestType4,MockResponseType4>
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

      public override TResponse Dispatch<TResponse>(IRequest<TResponse> request)
      {
        return this.Implementation.Dispatch(request);
      }

      public override void Dispatch(IRequest request)
      {
        this.Implementation.Dispatch(request);
      }

      public override bool CanDispatch (Type requestType)
      {
        return this.Implementation.CanDispatch(requestType);
      }

      public override IRequestDispatcher Register (Type requestType, Func<IRequestHandler> factoryMethod)
      {
        this.Implementation.Register(requestType, factoryMethod);
        return this;
      }

      public override IRequestDispatcher Unregister (Type requestType)
      {
        this.Implementation.Unregister(requestType);
        return this;
      }

      public DummyRequestDispatcher(IRequestDispatcher implementation, ExceptionHandlingPolicy policy) : base(policy)
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

