//
// TestGuidExtensions.cs
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
  public class TestGuidExtensions
  {
    #region tests

    [Test]
    [Description("Tests that the reordering of GUID bytes occurs correctly, regardless of the endianness of the " +
                 "current machine.")]
    public void TestReorderBytes()
    {
      byte[]
        original        = new byte[] { 14, 182, 97, 71, 156, 226, 185, 65, 191, 150, 152, 236, 122, 84, 151, 18 },
        expectedResult  = new byte[] { 71, 97, 182, 14, 226, 156, 65, 185, 191, 150, 152, 236, 122, 84, 151, 18 };

      Assert.AreEqual(expectedResult, GuidExtensions.ReorderBytes(original), "Reversed order");
      Assert.AreEqual(original,
                      GuidExtensions.ReorderBytes(GuidExtensions.ReorderBytes(original)),
                      "Back to original order after reordering twice");
    }

    [Test]
    public void TestToRFC4122ByteArray()
    {
      /* Note - this test has only been executed on a little-endian machine, but it should work regardless of the
       * endianness of the machine.  If the machine is big-endian then the byte order for the given GUID should be
       * correct without needing to reverse the order (and the extension method will not reverse the order).  If it is
       * little-endian then it should correctly reverse the byte-order.
       */
      Guid guid = new Guid("4761b60e-e29c-41b9-bf96-98ec7a549712");

      byte[]
        actualResult          = guid.ToRFC4122ByteArray(),
        expectedResult  = new byte[] { 71, 97, 182, 14, 226, 156, 65, 185, 191, 150, 152, 236, 122, 84, 151, 18 };

      Assert.AreEqual(expectedResult, actualResult);
    }

    [Test]
    public void TestFromRFC4122ByteArray()
    {
      /* Note - this test has only been executed on a little-endian machine, but it should work regardless of the
       * endianness of the machine.  If the machine is big-endian then the byte order for the given GUID should be
       * correct without needing to reverse the order (and the extension method will not reverse the order).  If it is
       * little-endian then it should correctly reverse the byte-order.
       */
      Guid
        actualResult    = (new byte[] { 71, 97, 182, 14, 226, 156, 65, 185, 191, 150, 152, 236, 122, 84, 151, 18 })
                          .FromRFC4122ByteArray(),
        expectedResult  = new Guid("4761b60e-e29c-41b9-bf96-98ec7a549712");

      Assert.AreEqual(expectedResult, actualResult);
    }

    #endregion
  }
}

