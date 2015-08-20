//
//  TabularDataReadException.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2015 Craig Fowler
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
using System.Runtime.Serialization;

namespace CSF.IO
{
  /// <summary>
  /// Represents an exception encountered when an <see cref="ITabularDataParser"/> fails to read data.
  /// </summary>
  [Serializable]
  public class TabularDataReadException : Exception
  {
    /// <summary>
    /// Initializes a new instance of the <see cref="T:TabularDataReadException"/> class
    /// </summary>
    public TabularDataReadException()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:TabularDataReadException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    public TabularDataReadException(string message) : base(message)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:TabularDataReadException"/> class
    /// </summary>
    /// <param name="message">A <see cref="T:System.String"/> that describes the exception. </param>
    /// <param name="inner">The exception that is the cause of the current exception. </param>
    public TabularDataReadException(string message, Exception inner) : base(message, inner)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:TabularDataReadException"/> class
    /// </summary>
    /// <param name="context">The contextual information about the source or destination.</param>
    /// <param name="info">The object that holds the serialized object data.</param>
    protected TabularDataReadException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
  }
}

