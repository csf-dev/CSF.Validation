//
// TestPosixParameterParser.cs
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
using System.Collections.Generic;
using System.Linq;
using CSF.Cli;
using CSF.Reflection;
using NUnit.Framework;

namespace Test.CSF.Cli
{
  [TestFixture]
  public class TestPosixParameterParser
  {
    #region tests relating to adding parameters
    
    [Test]
    public void TestAddParameter()
    {
      PosixParameterParser parser = new PosixParameterParser();
      
      parser
        .AddParameter("f", "foo", "test.foo")
        .AddParameter("b", null, "test.b")
        .AddParameter(null, "bar", "test.bar");
      
      Assert.AreEqual(3, parser.RegisteredParameters.Count, "Correct count of parameters");
    }
    
    [Test]
    public void TestAddParameters()
    {
      PosixParameterParser parser = new PosixParameterParser();
      
      parser.AddParameters<SampleClass>();
      
      Assert.AreEqual(2, parser.RegisteredParameters.Count, "Correct count of parameters");
      
      object identifier = Reflect.Member<SampleClass>(x => x.PropertyOne);
      IParameter testParameter = (from x in parser.RegisteredParameters
                                  where x.Identifier == identifier
                                  select x).FirstOrDefault();
      
      Assert.AreEqual(3, testParameter.ShortNames.Count, "Correct count of short names");
    }
    
    [Test]
    public void TestParse()
    {
      PosixParameterParser parser = new PosixParameterParser();
      
      parser.AddParameters<SampleClass>();
      
      SampleClass output = parser.Parse<SampleClass>(new string[] { "-n",
                                                                    "5",
                                                                    "Extra parameter",
                                                                    "--another-switch",
                                                                    "But this is an extra parameter" });
      
      Assert.IsNotNull(output, "Output not null");
      Assert.AreEqual(5, output.PropertyOne, "Property one correct value");
      Assert.IsTrue(output.PropertyTwo, "Property two correct value");
      
      IList<string> extraParams = output.RemainingArguments.ToList();
      
      Assert.AreEqual(2, extraParams.Count, "Correct count of extra arguments");
      Assert.AreEqual("Extra parameter", extraParams[0], "Correct first extra param");
      Assert.AreEqual("But this is an extra parameter", extraParams[1], "Correct second extra param");
    }
    
    #endregion
    
    #region contained type
    
    class SampleClass : ParsedParameters<SampleClass>
    {
      [Parameter(ParameterBehaviour.ValueRequired, ShortName = "n")]
      [ParameterName("l")]
      [ParameterName("m")]
      public int PropertyOne
      {
        get {
          IParameter<int> parameter = this.Get<int>(x => x.PropertyOne);
          
          return (parameter != null)? parameter.GetValue() : 0;
        }
      }
      
      [Parameter(ParameterBehaviour.Switch, ShortName = "s", LongName = "switch")]
      [ParameterName("a-switch", IsLongName = true)]
      [ParameterName("another-switch", IsLongName = true)]
      public bool PropertyTwo
      {
        get {
          return this.Contains(x => x.PropertyOne);
        }
      }
      
      public SampleClass(IDictionary<object, IParameter> parameters,
                         IList<string> remainingArguments) : base(parameters, remainingArguments) {}
    }
    
    #endregion
  }
}

