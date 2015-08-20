//
// TestBinaryGuidCombStrategy.cs
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

