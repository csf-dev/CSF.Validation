//
// BinaryGuidCombStrategy.cs
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
using System.Text;

namespace CSF
{
  /// <summary>
  /// GUID generator that uses the COMB strategy.
  /// </summary>
  public class BinaryGuidCombStrategy : IGuidGenerationStrategy
  {
    #region constants

    private const int
      MIN_NONRANDOM_BYTES               = 3,
      MAX_NONRANDOM_BYTES               = 6,
      GUID_BYTES                        = 16,
      TICKS_BYTES                       = 8;

    #endregion

    #region fields

    private int _firstNonRandomByte, _nonRandomByteCount;
    private bool _nonRandomBytesBigEndian;

    #endregion

    #region methods

    /// <summary>
    /// Generate a new COMB-based GUID.
    /// </summary>
    public virtual Guid Generate()
    {
      return this.Generate(DateTime.Now);
    }

    /// <summary>
    /// Generate a GUID using this strategy, for the given timestamp.
    /// </summary>
    /// <param name='forTimestamp'>
    /// The timestamp for which to generate a GUID.
    /// </param>
    public virtual Guid Generate(DateTime forTimestamp)
    {
      byte[]
        timestampBytes = this.GetMostSignificantBytes(forTimestamp.Ticks,
                                                      _nonRandomByteCount,
                                                      _nonRandomBytesBigEndian),
        outputBytes = Guid.NewGuid().ToByteArray();

      Array.Copy(timestampBytes, 0, outputBytes, _firstNonRandomByte, _nonRandomByteCount);

      return new Guid(outputBytes);
    }

    private byte[] GetMostSignificantBytes(long ticks, int byteCount, bool bigEndianDesired)
    {
      byte[] tickBytes = BitConverter.GetBytes(ticks);
      byte[] output = new byte[byteCount];

      int startPosition = BitConverter.IsLittleEndian? (TICKS_BYTES - byteCount) : 0;
      Array.Copy(tickBytes, startPosition, output, 0, byteCount);

      if((bigEndianDesired && BitConverter.IsLittleEndian)
         || (!bigEndianDesired && !BitConverter.IsLittleEndian))
      {
        Array.Reverse(output);
      }

      return output;
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.BinaryGuidCombStrategy"/> class.
    /// </summary>
    /// <param name='nonRandomByteCount'>
    /// The count of non-random bytes to take from the timestamp.
    /// </param>
    /// <param name='placeNonRandomBytesFirst'>
    /// A value indicating whether non-random bytes should be placed first or last in the resulting GUID.
    /// </param>
    /// <param name='nonRandomBytesBigEndian'>
    /// A value indicating whether or not the random bytes should be stored in a big-endian fashion.
    /// </param>
    public BinaryGuidCombStrategy(int nonRandomByteCount,
                                  bool placeNonRandomBytesFirst,
                                  bool nonRandomBytesBigEndian)
    {
      if(nonRandomByteCount > MAX_NONRANDOM_BYTES
         || nonRandomByteCount < MIN_NONRANDOM_BYTES)
      {
        string message = String.Format("Count of non-random bytes must be between {0} and {1} (inclusive).",
                                       MIN_NONRANDOM_BYTES,
                                       MAX_NONRANDOM_BYTES);
        throw new ArgumentOutOfRangeException(message, "nonRandomByteCount");
      }

      _nonRandomByteCount = nonRandomByteCount;
      _firstNonRandomByte = placeNonRandomBytesFirst? 0 : GUID_BYTES - nonRandomByteCount;
      _nonRandomBytesBigEndian = nonRandomBytesBigEndian;
    }

    #endregion
  }
}

