using System;
using CSF;
using NUnit.Framework;
using System.Threading;

namespace Test.CSF
{
  [TestFixture]
  public class TestBinaryGuidCombStrategy
  {
    #region tests

    [Test]
    public void TestGenerate()
    {
      BinaryGuidCombStrategy strategy = new BinaryGuidCombStrategy(6, true, true);
      strategy.Generate();
    }

    [Test]
    [Explicit("This is not really a test, just a way of seeing the strategy in action.")]
    public void TestGenerateManyGuids()
    {
      BinaryGuidCombStrategy strategy = new BinaryGuidCombStrategy(6, true, true);

      DateTime now = DateTime.Now;

      for(int i = 0; i < 200; i++)
      {
        Console.WriteLine(strategy.Generate(now.AddSeconds(i)));
      }
    }

    #endregion
  }
}

