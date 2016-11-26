//
// GuidExtensions.cs
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

namespace CSF
{
  /// <summary>
  /// Extension methods for the <c>System.Guid</c> type.
  /// </summary>
  public static class GuidExtensions
  {
    #region constants

    private const int GUID_BYTE_COUNT = 16;
    private static readonly int[] REORDERING_MAP = new[] { 3, 2, 1, 0, 5, 4, 7, 6, 8, 9, 10, 11, 12, 13, 14, 15 };

    #endregion

    #region extension methods

    /// <summary>
    /// Returns a byte array representing the given <c>System.Guid</c> in an RFC-4122 compliant format.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The rationale for this method (and the reason for requiring it) is because Microsoft internally represent the
    /// GUID structure in a manner which does not comply with RFC-4122's definition of a UUID.  The first three blocks
    /// of data (out of 4 total) are stored using the machine's native endianness.  The RFC defines that these three
    /// blocks should be represented in big-endian format.  This does not cause a problem when getting a
    /// string-representation of the GUID, since the framework automatically converts to big-endian format before
    /// formatting as a string.  When getting a byte array equivalent of the GUID though, it can cause issues if the
    /// recipient of that byte array expects a standards-compliant UUID.
    /// </para>
    /// <para>
    /// This method checks the architecture of the current machine.  If it is little-endian then - before returning a
    /// value - the byte-order of the first three blocks of data are reversed.  If the machine is big-endian then the
    /// bytes are left untouched (since they are already correct).
    /// </para>
    /// <para>
    /// For more information, see https://en.wikipedia.org/wiki/Globally_unique_identifier#Binary_encoding
    /// </para>
    /// </remarks>
    /// <returns>
    /// A byte array representation of the GUID, in RFC-4122 compliant form.
    /// </returns>
    /// <param name='guid'>
    /// The GUID for which to get the byte array.
    /// </param>
    public static byte[] ToRFC4122ByteArray(this Guid guid)
    {
      return BitConverter.IsLittleEndian? ReorderBytes(guid.ToByteArray()) : guid.ToByteArray();
    }

    /// <summary>
    /// Returns a <c>System.Guid</c>, created from the given RFC-4122 compliant byte array.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The rationale for this method (and the reason for requiring it) is because Microsoft internally represent the
    /// GUID structure in a manner which does not comply with RFC-4122's definition of a UUID.  The first three blocks
    /// of data (out of 4 total) are stored using the machine's native endianness.  The RFC defines that these three
    /// blocks should be represented in big-endian format.  This does not cause a problem when getting a
    /// string-representation of the GUID, since the framework automatically converts to big-endian format before
    /// formatting as a string.  When getting a byte array equivalent of the GUID though, it can cause issues if the
    /// recipient of that byte array expects a standards-compliant UUID.
    /// </para>
    /// <para>
    /// This method checks the architecture of the current machine.  If it is little-endian then - before returning a
    /// value - the byte-order of the first three blocks of data are reversed.  If the machine is big-endian then the
    /// bytes are left untouched (since they are already correct).
    /// </para>
    /// <para>
    /// For more information, see https://en.wikipedia.org/wiki/Globally_unique_identifier#Binary_encoding
    /// </para>
    /// </remarks>
    /// <returns>
    /// A GUID, created from the given byte array.
    /// </returns>
    /// <param name='guidBytes'>
    /// A byte array representing a GUID, in RFC-4122 compliant form.
    /// </param>
    public static Guid FromRFC4122ByteArray(this byte[] guidBytes)
    {
      return new Guid(BitConverter.IsLittleEndian? ReorderBytes(guidBytes) : guidBytes);
    }

    #endregion

    #region static methods

    /// <summary>
    /// Copies a byte array that represents a GUID, reversing the order of the bytes in data-blocks one to three.
    /// </summary>
    /// <returns>
    /// A copy of the original byte array, with the modifications.
    /// </returns>
    /// <param name='guidBytes'>
    /// A byte array representing a GUID.
    /// </param>
    public static byte[] ReorderBytes(byte[] guidBytes)
    {
      if(guidBytes == null)
      {
        throw new ArgumentNullException("guidBytes");
      }
      else if(guidBytes.Length != GUID_BYTE_COUNT)
      {
        throw new ArgumentException("Byte arrays representing GUIDs must contain exactly 16 bytes.", "guidBytes.");
      }

      byte[] output = new byte[GUID_BYTE_COUNT];
      for(int i = 0; i < GUID_BYTE_COUNT; i++)
      {
        output[i] = guidBytes[REORDERING_MAP[i]];
      }

      return output;
    }

    #endregion
  }
}

