//
//  ExceptionHandlingPolicy.cs
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

namespace CSF.Patterns.ServiceLayer
{
  /// <summary>
  /// Enumerates the possible exception handling policies that a service layer dispatcher may implement.
  /// </summary>
  [Flags]
  public enum ExceptionHandlingPolicy
  {
    /// <summary>
    /// Where possible, the exception should be 'rolled into' the service layer response.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This means that a response will be returned from the service layer dispatcher, but it will simply indicate that
    /// an exception has been thrown.  This requires that the appropriate response type has a constructor that takes a
    /// single exception instance.
    /// </para>
    /// </remarks>
    IncludeWithWithResponseIfPossible             = 1,

    /// <summary>
    /// The exception thrown from the service layer handler should be wrapped with
    /// <see cref="RequestDispatchException"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This means that the real exception is the <c>.InnerException</c> property of the exception thrown.
    /// </para>
    /// </remarks>
    WrapWithRequestDispatchException              = 2
  }
}

