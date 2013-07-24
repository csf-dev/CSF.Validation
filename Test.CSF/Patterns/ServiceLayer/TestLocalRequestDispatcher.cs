using System;
using NUnit.Framework;
using CSF.Patterns.ServiceLayer;
using Moq;

namespace Test.CSF.Patterns.ServiceLayer
{
  [TestFixture]
  public class TestLocalRequestDispatcher
  {
    #region fields

#pragma warning disable 414
    private Mock<MockRequestType1> Request1;
    private Mock<MockRequestType2> Request2;
    private Mock<MockRequestType3> Request3;
    private Mock<MockResponseType1> Response1;
    private Mock<MockResponseType2> Response2;
    private Mock<IRequestHandler> Handler1;
    private Mock<IRequestHandler> Handler2;
#pragma warning restore 414

    private LocalRequestDispatcher Dispatcher;

    #endregion

    #region setup

    [SetUp]
    public void Setup()
    {
      this.Request1 = new Mock<MockRequestType1>();
      this.Request2 = new Mock<MockRequestType2>();
      this.Request3 = new Mock<MockRequestType3>();

      this.Request1
        .As<IRequest>()
        .Setup(x => x.GetType()).Returns(typeof(MockRequestType1));
      this.Request2
        .As<IRequest>()
        .Setup(x => x.GetType()).Returns(typeof(MockRequestType2));
      this.Request3
        .As<IRequest>()
        .Setup(x => x.GetType()).Returns(typeof(MockRequestType3));

      this.Response1 = new Mock<MockResponseType1>();
      this.Response2 = new Mock<MockResponseType2>();

      this.Handler1 = new Mock<IRequestHandler>(MockBehavior.Strict);
      this.Handler2 = new Mock<IRequestHandler>(MockBehavior.Strict);

      this.Handler1
        .Setup(x => x.Handle(It.IsAny<IRequest>()))
        .Returns((Response) this.Response1.Object);
      this.Handler2
        .Setup(x => x.Handle(It.IsAny<IRequest>()))
        .Returns((Response) this.Response2.Object);
      this.Handler1
        .Setup(x => x.HandleRequestOnly(It.IsAny<IRequest>()));
      this.Handler2
        .Setup(x => x.HandleRequestOnly(It.IsAny<IRequest>()));

      this.Dispatcher = new LocalRequestDispatcher();

      this.Dispatcher
        .Register(typeof(MockRequestType1), () => this.Handler1.Object)
        .Register(typeof(MockRequestType2), () => this.Handler2.Object);

      DisposeableRequestHandler.DisposedOnce = false;
    }

    #endregion

    #region tests

    [Test]
    public void TestRegister()
    {
      Assert.IsTrue(this.Dispatcher.CanDispatch<MockRequestType1>(), "Can dispatch type one");
      Assert.IsTrue(this.Dispatcher.CanDispatch<MockRequestType2>(), "Can dispatch type two");
    }

    [Test]
    public void TestDispatch()
    {
      var response = this.Dispatcher.Dispatch(this.Request1.Object);

      Assert.AreSame(this.Response1.Object, response);

      this.Handler1
        .Verify(x => x.Handle(It.Is<IRequest>(req => req == this.Request1.Object)), Times.Once());
      this.Handler1
        .Verify(x => x.HandleRequestOnly(It.IsAny<IRequest>()), Times.Never());
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(RequestDispatchException),
                       ExpectedMessage = "There must be a type (that derives from " +
                                         "RequestHandler<TRequest,TResponse>) registered with the request " +
                                         "dispatcher in order to dispatch this request; no such type was found.\n" +
                                         "Request type: `Test.CSF.Patterns.ServiceLayer.TestLocalRequestDispatcher+MockRequestType2'")]
    public void TestDispatchNotRegistered()
    {
      this.Dispatcher.Unregister<MockRequestType2>();
      this.Dispatcher.Dispatch(this.Request2.Object);
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(RequestDispatchException),
                       ExpectedMessage = "The request to dispatch must not be null.")]
    public void TestDispatchNullRequest()
    {
      this.Dispatcher.Dispatch<MockResponseType1>(null);
    }

    [Test]
    public void TestDispatchRequestOnly()
    {
      var handler3 = new Mock<IRequestHandler>();
      this.Dispatcher.Register<MockRequestType3>(() => handler3.Object);
      this.Dispatcher.Dispatch(this.Request3.Object);

      handler3
        .Verify(x => x.Handle(It.IsAny<IRequest>()), Times.Never());
      handler3
        .Verify(x => x.HandleRequestOnly(It.Is<IRequest>(req => req == this.Request3.Object)), Times.Once());
    }

    [Test]
    public void TestDispatchAndRelease()
    {
      this.Dispatcher.Unregister<MockRequestType1>();
      this.Dispatcher.Register<MockRequestType1,DisposeableRequestHandler>();

      Assert.IsFalse(DisposeableRequestHandler.DisposedOnce, "Disposed before handling");

      this.Dispatcher.Dispatch(new MockRequestType1());

      Assert.IsTrue(DisposeableRequestHandler.DisposedOnce, "Disposed after handling");
    }

    #endregion

    #region contained types
    
    public class MockRequestType1 : IRequest<MockResponseType1>
    {
      public new Type GetType() { return typeof(MockRequestType1); }
    }
    public class MockRequestType2 : IRequest<MockResponseType2>
    {
      public new Type GetType() { return typeof(MockRequestType2); }
    }
    public class MockRequestType3 : IRequest
    {
      public new Type GetType() { return typeof(MockRequestType3); }
    }

    public class MockResponseType1 : Response
    {
      public MockResponseType1() : base(null) { }
    }
    public class MockResponseType2 : Response
    {
      public MockResponseType2() : base(null) { }
    }

    public class DisposeableRequestHandler : RequestHandler<MockRequestType1,MockResponseType1>, IDisposable
    {
      public static bool DisposedOnce;

      public override MockResponseType1 Handle (MockRequestType1 request)
      {
        return new MockResponseType1();
      }

      public void Dispose()
      {
        DisposedOnce = true;
      }
    }

    #endregion
  }
}

