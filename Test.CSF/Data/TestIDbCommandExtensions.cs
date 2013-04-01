using System;
using NUnit.Framework;
using System.Data;
using CSF.Data;
using Moq;

namespace Test.CSF.Data
{
  [TestFixture]
  public class TestIDbCommandExtensions
  {
    [Test]
    public void TestAddParameter()
    {
      var command = new Mock<IDbCommand>(MockBehavior.Strict);
      var parameter = new Mock<IDbDataParameter>(MockBehavior.Strict);
      var paramsCollection = new Mock<IDataParameterCollection>(MockBehavior.Strict);

      command.Setup(x => x.CreateParameter()).Returns(parameter.Object);
      parameter.SetupSet(x => x.ParameterName = It.IsAny<string>());
      parameter.SetupSet(x => x.Value = It.IsAny<object>());
      paramsCollection
        .As<System.Collections.IList>()
        .Setup(x => x.Add(It.Is<IDbDataParameter>(val => val == parameter.Object)))
        .Returns(1);
      command.SetupGet(x => x.Parameters).Returns(paramsCollection.Object);

      command.Object.AddParameter("TheMeaning", 42);

      command.Verify(x => x.CreateParameter(), Times.Once());
      parameter.VerifySet(x => x.ParameterName = It.Is<string>(val => val == "TheMeaning"), Times.Once());
      parameter.VerifySet(x => x.Value = It.Is<int>(val => val == 42), Times.Once());
      paramsCollection
        .As<System.Collections.IList>()
        .Verify(x => x.Add(It.Is<IDbDataParameter>(val => val == parameter.Object)), Times.Once());
    }
  }
}

