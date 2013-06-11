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
    private Mock<MockResponseType3> Response3;
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
      this.Response3 = new Mock<MockResponseType3>();

      this.Response1.As<IResponse>();
      this.Response2.As<IResponse>();
      this.Response3.As<IResponse>();

      this.Handler1 = new Mock<IRequestHandler>(MockBehavior.Strict);
      this.Handler2 = new Mock<IRequestHandler>(MockBehavior.Strict);

      this.Handler1
        .Setup(x => x.Handle(It.IsAny<IRequest>()))
        .Returns((IResponse) this.Response1.Object);
      this.Handler2
        .Setup(x => x.Handle(It.IsAny<IRequest>()))
        .Returns((IResponse) this.Response2.Object);
      this.Handler1
        .Setup(x => x.HandleRequestOnly(It.IsAny<IRequest>()));
      this.Handler2
        .Setup(x => x.HandleRequestOnly(It.IsAny<IRequest>()));

      this.Dispatcher = new LocalRequestDispatcher();

#pragma warning disable 618
      this.Dispatcher
        .Register(typeof(MockRequestType1), this.Handler1.Object)
        .Register(typeof(MockRequestType2), this.Handler2.Object);
#pragma warning restore 618

      DisposeableRequestHandler.DisposedOnce = false;
    }

    #endregion

    #region tests

    [Test]
    public void TestRegister()
    {
#pragma warning disable 618
      var handlers = this.Dispatcher.GetRegisteredHandlers();
#pragma warning restore 618

      Assert.AreEqual(2, handlers.Count);

      Assert.IsTrue(handlers.ContainsKey(typeof(MockRequestType1)), "Contains a definition for request type 1");
      Assert.IsTrue(handlers.ContainsKey(typeof(MockRequestType2)), "Contains a definition for request type 2");

      Assert.AreSame(this.Handler1.Object, handlers[typeof(MockRequestType1)], "Correct handler 1");
      Assert.AreSame(this.Handler2.Object, handlers[typeof(MockRequestType2)], "Correct handler 2");
    }

    [Test]
    public void TestDispatch()
    {
      var response = this.Dispatcher.Dispatch<MockResponseType1>(this.Request1.Object);

      Assert.AreSame(this.Response1.Object, response);

      this.Handler1
        .Verify(x => x.Handle(It.Is<IRequest>(req => req == this.Request1.Object)), Times.Once());
      this.Handler1
        .Verify(x => x.HandleRequestOnly(It.IsAny<IRequest>()), Times.Never());
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(RequestDispatchException),
                       ExpectedMessage = "Could not dispatch the request as there is no registered handler. " +
                                         "Request type: `Test.CSF.Patterns.ServiceLayer.TestLocalRequestDispatcher+MockRequestType3'")]
    public void TestDispatchNotRegistered()
    {
      this.Dispatcher.Dispatch<MockResponseType3>(this.Request3.Object);
    }

    [Test]
    [ExpectedException(ExceptionType = typeof(RequestDispatchException),
                       ExpectedMessage = "Could not dispatch the request; it is null.")]
    public void TestDispatchNullRequest()
    {
      this.Dispatcher.Dispatch<MockResponseType1>(null);
    }

    [Test]
    public void TestDispatchRequestOnly()
    {
      this.Dispatcher.DispatchRequestOnly(this.Request1.Object);

      this.Handler1
        .Verify(x => x.Handle(It.IsAny<IRequest>()), Times.Never());
      this.Handler1
        .Verify(x => x.HandleRequestOnly(It.Is<IRequest>(req => req == this.Request1.Object)), Times.Once());
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
    public class MockRequestType3 : IRequest<MockResponseType3>
    {
      public new Type GetType() { return typeof(MockRequestType3); }
    }

    public class MockResponseType1 : Response {}
    public class MockResponseType2 : Response {}
    public class MockResponseType3 : Response {}

    public class DisposeableRequestHandler : RequestHandlerBase<MockRequestType1,MockResponseType1>, IDisposable
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

