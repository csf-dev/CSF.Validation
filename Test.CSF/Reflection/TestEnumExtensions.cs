//
// TestEnumExtensions.cs
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
using CSF.Reflection;
using System.Reflection;

namespace Test.CSF.Reflection
{
  [TestFixture]
  public class TestEnumExtensions
  {
    #region tests
    
    [Test]
    public void TestGetUIText()
    {
      Assert.AreEqual("First", SampleEnum.One.GetUIText(), "Correct description");
    }
    
    [Test]
    public void TestGetUITextNull()
    {
      Assert.IsNull(SampleEnum.Three.GetUIText(), "Null description");
    }
    
    [Test]
    [ExpectedException(typeof(ArgumentException))]
    public void TestGetUITextInvalid()
    {
      ((SampleEnum) 5).GetUIText();
      Assert.Fail("Test should not reach this point");
    }
    
    [Test]
    public void TestGetFieldInfo()
    {
      FieldInfo field = SampleEnum.Three.GetFieldInfo();
      Assert.IsNotNull(field, "Field info not null");
      Assert.AreEqual(SampleEnum.Three.ToString(), field.Name, "Field hsa correct name");
    }

    #endregion
    
    #region test enumeration
    
    enum SampleEnum : int
    {
      [UIText("First")]
      One   = 1,
      
      [UIText("Second")]
      Two   = 2,
      
      Three = 3
    }
    
    #endregion
  }
}

