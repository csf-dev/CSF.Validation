﻿//
// StubValidatedObject.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2017 Craig Fowler
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
namespace CSF.Validation.Tests
{
  public class StubValidatedObject
  {
    public string StringProperty { get; set; }

    public int? NullableIntegerProperty { get; set; }

    public int IntegerProperty { get; set; }

    public decimal? NullableDecimalProperty { get; set; }

    public decimal DecimalProperty { get; set; }

    public DateTime DateTimeProperty { get; set; }

    public DateTime? NullableDateTimeProperty { get; set; }

    public StubEnum EnumProperty { get; set; }

    public StubEnum? NullableEnumProperty { get; set; }
  }

  public enum StubEnum
  {
    One = 1,

    Two = 2,

    Three = 3
  }
}
