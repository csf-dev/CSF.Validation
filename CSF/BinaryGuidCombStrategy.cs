//
//  GuidCombGenerator.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2013 Craig Fowler
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.
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

