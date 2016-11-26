//
// TestIDbCommandExtensions.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

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

