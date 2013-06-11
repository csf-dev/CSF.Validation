using System;
using Moq;
using NUnit.Framework;
using CSF.Patterns.ServiceLayer;

namespace Test.CSF.Patterns.ServiceLayer
{
  [TestFixture]
  public class TestRequestExtensions
  {
    #region tests

    [Test]
    public void TestDispatch()
    {
      var dispatcher = new Mock<IRequestDispatcher>(MockBehavior.Strict);
      var request = new DummyRequest();

      dispatcher
        .Setup(x => x.Dispatch(It.Is<IRequest<DummyResponse>>(req => req == request)))
        .Returns(new DummyResponse());

      var response = request.Dispatch(dispatcher.Object);

      Assert.IsNotNull(response, "Response nullability");
      Assert.IsInstanceOfType(typeof(DummyResponse), response, "Response correct type");
      dispatcher
        .Verify(x => x.Dispatch(It.Is<IRequest<DummyResponse>>(req => req == request)),
                Times.Once(),
                "Dispatcher called correctly");
    }

    #endregion

    #region contained types

    private class DummyResponse : Response {}
    private class DummyRequest : IRequest<DummyResponse> {}

    #endregion
  }
}

