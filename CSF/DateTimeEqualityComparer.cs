//
// DateTimeEqualityComparer.cs
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
using System.Collections;

namespace CSF
{
  /// <summary>
  /// Custom comparison type for comparing equality between <see cref="DateTime"/> instances.
  /// </summary>
  public class DateTimeEqualityComparer : IEqualityComparer
  {
    #region properties

    /// <summary>
    /// Gets the permitted difference between dates to be compared.
    /// </summary>
    /// <value>
    /// The permitted difference.
    /// </value>
    public TimeSpan PermittedDifference
    {
      get;
      private set;
    }

    #endregion

    #region methods

    /// <summary>
    /// Tests for equality between the two objects (which should both be instances of <see cref="DateTime"/>) within the
    /// <see cref="PermittedDifference"/>.
    /// </summary>
    /// <param name='obj1'>
    /// A date/time instance
    /// </param>
    /// <param name='obj2'>
    /// A date/time instance
    /// </param>
    public bool AreEqual(object obj1, object obj2)
    {
      bool output;

      if(Object.ReferenceEquals(obj1, obj2))
      {
        output = true;
      }
      else if(obj1 == null)
      {
        output = false;
      }
      else
      {
        try
        {
          DateTime
            dateTime1 = (DateTime) obj1,
            dateTime2 = (DateTime) obj2;

          var difference = (dateTime1 - dateTime2).Duration();
          output = (difference <= this.PermittedDifference);
        }
        catch(InvalidCastException)
        {
          output = obj1.Equals(obj2);
        }
      }

      return output;
    }

    /// <summary>
    /// Unsupported method, would get the hash code of a DateTime.
    /// </summary>
    /// <param name='obj'>
    /// An object
    /// </param>
    public int GetHashCode(object obj)
    {
      throw new NotImplementedException("This method is unsupported");
    }

    #endregion

    #region explicit interface implementation

    bool IEqualityComparer.Equals(object obj1, object obj2)
    {
      return this.AreEqual(obj1, obj2);
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.DateTimeEqualityComparer"/> class.
    /// </summary>
    /// <param name='permittedDifference'>
    /// Permitted difference.
    /// </param>
    public DateTimeEqualityComparer(TimeSpan permittedDifference)
    {
      this.PermittedDifference = permittedDifference;
    }

    #endregion
  }
}

