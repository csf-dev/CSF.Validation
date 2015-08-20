//
// TestDateTimeExtensions.cs
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
using CSF;

namespace Test.CSF
{
  [TestFixture]
  public class TestDateTimeExtensions
  {
    #region tests

    [TestCase(1976, 09, 18, 2012, 10, 03, 36)]
    [TestCase(1976, 09, 18, 2012, 10, 20, 36)]
    [TestCase(1976, 09, 18, 2012, 09, 03, 35)]
    [TestCase(1976, 09, 18, 2012, 09, 20, 36)]
    [TestCase(1976, 09, 18, 2012, 08, 03, 35)]
    [TestCase(1976, 09, 18, 2012, 08, 20, 35)]
    public void TestAsAgeInYears(int birthYear, int birthMonth, int birthDay,
                                 int referenceYear, int referenceMonth, int referenceDay,
                                 int expectedAge)
    {
      // Arrange
      DateTime
        birth = new DateTime(birthYear, birthMonth, birthDay),
        reference = new DateTime(referenceYear, referenceMonth, referenceDay);

      // Act and assert together
      Assert.AreEqual(expectedAge,
                      birth.AsAgeInYears(reference),
                      "Birthday {0}: Age {1} on {2}",
                      birth.ToShortDateString(),
                      expectedAge,
                      reference.ToShortDateString());
    }

    [TestCase(2015,02,12, 2015,02,28)]
    [TestCase(2015,02,01, 2015,02,28)]
    [TestCase(2015,02,28, 2015,02,28)]
    [TestCase(2015,03,01, 2015,03,31)]
    [TestCase(2015,03,04, 2015,03,31)]
    [TestCase(2015,03,31, 2015,03,31)]
    public void TestGetLastDayOfMonth(int givenYear, int givenMonth, int givenDay,
                                      int expectedYear, int expectedMonth, int expectedDay)
    {
      // Arrange
      DateTime
        given = new DateTime(givenYear, givenMonth, givenDay),
        expected = new DateTime(expectedYear, expectedMonth, expectedDay);

      // Act and assert together
      Assert.AreEqual(expected,
                      given.GetLastDayOfMonth(),
                      "Date: {0}, expected: {1}",
                      given.ToShortDateString(),
                      expected.ToShortDateString());
    }

    #endregion
  }
}

