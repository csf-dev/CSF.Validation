//
// TestValidationFailureList.cs
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
  public class TestValidationTestList
  {
    #region tests
    
    [Test]
    public void TestByMember()
    {
      ValidationTestList<SampleObject> list = new ValidationTestList<SampleObject>();
      
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length > 4,
                                                        Reflect.Member<SampleObject>(x => x.PropertyOne),
                                                        "Foo"));
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length < 10,
                                                        Reflect.Member<SampleObject>(x => x.PropertyOne),
                                                        "Bar"));
      list.Add(new ValidationTest<SampleObject, int>(x => x != 3,
                                                     Reflect.Member<SampleObject>(x => x.PropertyTwo),
                                                     "Bar"));
      
      ValidationTestList<SampleObject> subset = list.ByMember(x => x.PropertyOne);
      
      Assert.AreEqual(2, subset.Count, "Correct count");
      Assert.AreEqual("PropertyOne", subset[0].Member.Name, "Correct member name 1");
      Assert.AreEqual("PropertyOne", subset[1].Member.Name, "Correct member name 2");
    }
    
    [Test]
    public void TestByIdentifier()
    {
      ValidationTestList<SampleObject> list = new ValidationTestList<SampleObject>();
      
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length > 4,
                                                        Reflect.Member<SampleObject>(x => x.PropertyOne),
                                                        "Foo"));
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length < 10,
                                                        Reflect.Member<SampleObject>(x => x.PropertyOne),
                                                        "Bar"));
      list.Add(new ValidationTest<SampleObject, int>(x => x != 3,
                                                     Reflect.Member<SampleObject>(x => x.PropertyTwo),
                                                     "Bar"));
      
      ValidationTestList<SampleObject> subset = list.ByIdentifier("Bar");
      
      Assert.AreEqual(2, subset.Count, "Correct count");
      Assert.AreEqual("Bar", subset[0].Identifier, "Correct identifier 1");
      Assert.AreEqual("Bar", subset[1].Identifier, "Correct identifier 2");
    }
    
    [Test]
    public void TestByIdentifierAndMember()
    {
      ValidationTestList<SampleObject> list = new ValidationTestList<SampleObject>();
      
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length > 4,
                                                        Reflect.Member<SampleObject>(x => x.PropertyOne),
                                                        "Foo"));
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length < 10,
                                                        Reflect.Member<SampleObject>(x => x.PropertyOne),
                                                        "Bar"));
      list.Add(new ValidationTest<SampleObject, int>(x => x != 3,
                                                     Reflect.Member<SampleObject>(x => x.PropertyTwo),
                                                     "Bar"));
      
      ValidationTestList<SampleObject> subset = list.ByIdentifier("Bar", x => x.PropertyOne);
      
      Assert.AreEqual(1, subset.Count, "Correct count");
      Assert.AreEqual("Bar", subset[0].Identifier, "Correct identifier");
      Assert.AreEqual("PropertyOne", subset[0].Member.Name, "Correct member name");
    }
    
    [Test]
    public void TestByIdentifierEmpty()
    {
      ValidationTestList<SampleObject> list = new ValidationTestList<SampleObject>();
      
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length > 4,
                                                        Reflect.Member<SampleObject>(x => x.PropertyOne),
                                                        "Foo"));
      list.Add(new ValidationTest<SampleObject, string>(x => x.Length < 10,
                                                        Reflect.Member<SampleObject>(x => x.PropertyOne),
                                                        "Bar"));
      list.Add(new ValidationTest<SampleObject, int>(x => x != 3,
                                                     Reflect.Member<SampleObject>(x => x.PropertyTwo),
                                                     "Bar"));
      
      ValidationTestList<SampleObject> subset = list.ByIdentifier("Spong");
      
      Assert.AreEqual(0, subset.Count, "Correct count");
    }
    
    #endregion
  }
}

