//
// TestInMemoryDataReader.cs
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
using System.Data;
using System.Linq;
using NUnit.Framework;
using CSF.Data;

namespace Test.CSF.Data
{
  [TestFixture]
  public class TestInMemoryDataReader
  {
    #region fields

    private string[,] Data1 = { { "R1C1", "R1C2", "R1C3" },
                                { "R2C1", "R2C2", "R2C3" },
                                { "R3C1", "R3C2", "R3C3" } };

    private string[,] Data2 = { { "TWO R1C1", "TWO R1C2" },
                                { "TWO R2C1", "TWO R2C2" } };

    private string[,] Data3 = { { "True",     "False"    },
                                { "False",    "True"     } };

    private string[,] Data4 = { { "R1C1", null,   "R1C3" },
                                { null,   "R2C2", null   },
                                { null,   null,   null   } };

    #endregion

    #region tests

    [Test]
    public void TestConstructor()
    {
      using(IDataReader reader = new InMemoryDataReader(false, this.Data1, this.Data2))
      {
        Assert.IsNotNull(reader, "Reader nullability");
        Assert.AreEqual(3, reader.FieldCount, "Reader field count");
        Assert.AreEqual("Column2", reader.GetName(2), "Column name");
      }
    }

    [Test]
    public void TestConstructorWithHeaders()
    {
      using(IDataReader reader = new InMemoryDataReader(true, this.Data1, this.Data2))
      {
        Assert.IsNotNull(reader, "Reader nullability");
        Assert.AreEqual(3, reader.FieldCount, "Reader field count");
        Assert.AreEqual("R1C3", reader.GetName(2), "Column name");
      }
    }

    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestGetValueNotReadYet()
    {
      using(IDataReader reader = new InMemoryDataReader(false, this.Data1))
      {
        reader.GetString(0);
      }
    }

    [Test]
    public void TestRead()
    {
      using(IDataReader reader = new InMemoryDataReader(false, this.Data1))
      {
        Assert.IsTrue(reader.Read(), "Read 1");
        Assert.AreEqual("R1C2", reader.GetString(1));

        Assert.IsTrue(reader.Read(), "Read 2");
        Assert.AreEqual("R2C1", reader.GetString(0));

        Assert.IsTrue(reader.Read(), "Read 3");
        Assert.AreEqual("R3C3", reader.GetString(2));

        Assert.IsFalse(reader.Read(), "Read 4");
      }
    }

    [Test]
    public void TestGetBoolean()
    {
      using(IDataReader reader = new InMemoryDataReader(false, this.Data3))
      {
        Assert.IsTrue(reader.Read(), "Read");
        Assert.IsTrue(reader.GetBoolean(0));
        Assert.IsFalse(reader.GetBoolean(1));
      }
    }

    [Test]
    public void TestGetValues()
    {
      using(IDataReader reader = new InMemoryDataReader(false, this.Data1))
      {
        Assert.IsTrue(reader.Read(), "Read");

        object[] buffer = new object[3];
        Assert.AreEqual(3, reader.GetValues(buffer), "Number of values");
        var strings = buffer.Cast<string>().ToArray();
        Assert.AreEqual(new string[]{ "R1C1", "R1C2", "R1C3" }, strings, "Data equal");
      }
    }

    [Test]
    [ExpectedException(typeof(InvalidOperationException))]
    public void TestGetStringNull()
    {
      using(IDataReader reader = new InMemoryDataReader(false, this.Data4))
      {
        Assert.IsTrue(reader.Read(), "Read");
        reader.GetString(1);
      }
    }

    [Test]
    public void TestGetValuesWithNull()
    {
      using(IDataReader reader = new InMemoryDataReader(false, this.Data4))
      {
        Assert.IsTrue(reader.Read(), "Read");

        object[] buffer = new object[3];
        Assert.AreEqual(2, reader.GetValues(buffer), "Number of values");
        var strings = buffer.Cast<string>().ToArray();
        Assert.AreEqual(new string[]{ "R1C1", null, "R1C3" }, strings, "Data equal");
      }
    }

    [Test]
    public void TestNextResult()
    {
      using(IDataReader reader = new InMemoryDataReader(false, this.Data1, this.Data2))
      {
        Assert.AreEqual(3, reader.FieldCount, "Field count.");
        Assert.IsTrue(reader.NextResult(), "Next result");
        Assert.AreEqual(2, reader.FieldCount, "Field count has changed.");
        Assert.IsTrue(reader.Read(), "Read");
        Assert.AreEqual("TWO R1C2", reader.GetString(1), "Row 1 column 2.");
      }
    }

    [Test]
    public void TestNextResultPastEnd()
    {
      using(IDataReader reader = new InMemoryDataReader(false, this.Data1, this.Data2))
      {
        Assert.IsTrue(reader.NextResult(), "Next result 1");
        Assert.IsFalse(reader.NextResult(), "Next result 2");
      }
    }

    #endregion
  }
}

