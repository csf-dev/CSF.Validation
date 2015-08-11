//
//  CannotFixStackTraceException.cs
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

namespace CSF
{
  /// <summary>
  /// An exception thrown when <see cref="M:ExceptionExtensions.FixStackTrace{TException}"/> is unable to use any
  /// mechanism to fix an exception's stack trace.
  /// </summary>
  public class CannotFixStackTraceException : Exception
  {
    #region constants

    private const string MESSAGE = "An exception was thrown and attempts were made to 'fix'/preserve its stack " +
                                   "trace, these attempts failed.  Try adding a constructor of signature " +
                                   "(SerializationInfo, StreamingContext) to the inner exception type.  Otherwise do " +
                                   "not use FixStackTrace on exceptions of this type.";

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.CannotFixStackTraceException"/> class.
    /// </summary>
    /// <param name='inner'>
    /// The inner exception.
    /// </param>
    public CannotFixStackTraceException(Exception inner) : base(MESSAGE, inner) {}

    #endregion
  }
}

