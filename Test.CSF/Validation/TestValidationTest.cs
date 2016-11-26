//
// TestValidationTest.cs
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
using CSF.Validation;
using Test.CSF.Mocks;
using CSF.Reflection;

namespace Test.CSF.Validation
{
  [TestFixture]
  public class TestValidationTest
  {
    #region tests
    
    [Test]
    public void TestExecute()
    {
      ValidationTest<SampleObject> test;
      test = new ValidationTest<SampleObject>(x => x.PropertyOne == "foo", null);
      
      SampleObject target = new SampleObject() {
        PropertyOne = "bar"
      };
      
      Assert.IsFalse(test.Execute(target), "Execute indicates false");
      
      target.PropertyOne = "foo";
      
      Assert.IsTrue(test.Execute(target), "Execute indicates true");
    }
    
    [Test]
    public void TestExecuteProperty()
    {
      ValidationTest<SampleObject, string> test;
      test = new ValidationTest<SampleObject, string>(x => x == "foo",
                                                      Reflect.Member<SampleObject>(x => x.PropertyOne),
                                                      null);
      
      SampleObject target = new SampleObject() {
        PropertyOne = "bar"
      };
      
      Assert.IsFalse(test.Execute(target), "Execute indicates false");
      
      target.PropertyOne = "foo";
      
      Assert.IsTrue(test.Execute(target), "Execute indicates true");
    }
    
    [Test]
    public void TestExecuteField()
    {
      ValidationTest<SampleObject, int> test;
      test = new ValidationTest<SampleObject, int>(x => x == 4,
                                                   Reflect.Member<SampleObject>(x => x.FieldTwo),
                                                   null);
      
      SampleObject target = new SampleObject() {
        FieldTwo = 2
      };
      
      Assert.IsFalse(test.Execute(target), "Execute indicates false");
      
      target.FieldTwo = 4;
      
      Assert.IsTrue(test.Execute(target), "Execute indicates true");
    }
    
    #endregion
  }
}

