using System;
using NUnit.Framework;
using CSF.IO;
using System.Collections.Generic;

namespace Test.CSF.IO
{
  [TestFixture]
  public class TestTabularDataList
  {
    [Test]
    public void TestAdd()
    {
      TabularDataList list = new TabularDataList(5);
      
      list.Add(new string[] { "foo", "bar", "baz", "bloo", "blob" });
      
      Assert.AreEqual(1, list.Count, "List has correct count of rows.");
      Assert.AreEqual("bar", list[0][1], "Correct item in first row, second column.");
    }
    
    [Test]
    [ExpectedException(typeof(ArgumentException),
                       ExpectedMessage = "Row to add has an incorrect count of columns.")]
    public void TestAddException()
    {
      TabularDataList list = new TabularDataList(5);
      
      list.Add(new string[] { "foo", "bar", "baz", "bloo", "blob", "spong" });
      
      Assert.Fail("Test should not reach this point.");
    }
    
    [Test]
    public void TestInsert()
    {
      TabularDataList list = new TabularDataList(5);
      
      list.Add(new string[] { "0foo", "0bar", "0baz", "0bloo", "0blob" });
      list.Add(new string[] { "1foo", "1bar", "1baz", "1bloo", "1blob" });
      
      list.Insert(1, new string[] { "foo", "bar", "baz", "bloo", "blob" });
      
      Assert.AreEqual(3, list.Count, "List has correct count of rows.");
      Assert.AreEqual("bar", list[1][1], "Correct item in second row, second column.");
      Assert.AreEqual("1bar", list[2][1], "Correct item in third row, second column.");
    }
    
    [Test]
    [ExpectedException(typeof(ArgumentException),
                       ExpectedMessage = "Row to insert has an incorrect count of columns.")]
    public void TestInsertException()
    {
      TabularDataList list = new TabularDataList(5);
      
      list.Add(new string[] { "0foo", "0bar", "0baz", "0bloo", "0blob" });
      list.Add(new string[] { "1foo", "1bar", "1baz", "1bloo", "1blob" });
      
      list.Insert(1, new string[] { "foo", "bar", "baz", "bloo", "blob", "spong" });
      
      Assert.Fail("Test should not reach this point.");
    }
    
    [Test]
    public void TestItem()
    {
      TabularDataList list = new TabularDataList(5);
      
      list.Add(new string[] { "0foo", "0bar", "0baz", "0bloo", "0blob" });
      list.Add(new string[] { "1foo", "1bar", "1baz", "1bloo", "1blob" });
      
      list[1] = new string[] { "foo", "bar", "baz", "bloo", "blob" };
      
      Assert.AreEqual(2, list.Count, "List has correct count of rows.");
      Assert.AreEqual("0bar", list[0][1], "Correct item in first row, second column.");
      Assert.AreEqual("bar", list[1][1], "Correct item in second row, second column.");
    }
    
    [Test]
    [ExpectedException(typeof(ArgumentException),
                       ExpectedMessage = "Row has an incorrect count of columns.")]
    public void TestItemException()
    {
      TabularDataList list = new TabularDataList(5);
      
      list.Add(new string[] { "0foo", "0bar", "0baz", "0bloo", "0blob" });
      list.Add(new string[] { "1foo", "1bar", "1baz", "1bloo", "1blob" });
      
      list[1] = new string[] { "foo", "bar", "baz", "bloo", "blob", "spong" };
      
      Assert.Fail("Test should not reach this point.");
    }
    
    [Test]
    public void TestCreateRow()
    {
      TabularDataList list = new TabularDataList(5);
      
      IList<string> row = list.CreateRow();
      Assert.AreEqual(5, row.Count, "Created row has correct column count.");
    }
  }
}

