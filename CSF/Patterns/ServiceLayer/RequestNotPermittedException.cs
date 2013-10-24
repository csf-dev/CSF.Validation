//
//  RequestNotPermittedException.cs
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
  /// Exception type raised when a <see cref="IRequest"/> is denied due to the current circumstances (such as user
  /// permissions).
  /// </summary>
  /// <remarks>
  /// <para>
  /// This exception indicates that the given request could be valid and that it may be retried if the reasons for which
  /// it has been denied are resolved.  If the request could never be valid (and no amount of retrying will help,
  /// without changing the request itself) then instead use <see cref="InvalidRequestException"/>.
  /// </para>
  /// </remarks>
  public class RequestNotPermittedException : Exception
  {
    #region constants

    private const string
      DEFAULT_MESSAGE         = "The request was not permitted.",
      REQUEST_KEY             = "Request";

    #endregion

    #region properties

    /// <summary>
    /// Gets the request that caused this exception.
    /// </summary>
    /// <value>
    /// The request.
    /// </value>
    public IRequest Request
    {
      get {
        return (IRequest) this.Data[REQUEST_KEY];
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.RequestNotPermittedException"/> class.
    /// </summary>
    /// <param name='request'>
    /// Request.
    /// </param>
    public RequestNotPermittedException(IRequest request) : this(DEFAULT_MESSAGE, null, request) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.RequestNotPermittedException"/> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    /// <param name='request'>
    /// Request.
    /// </param>
    public RequestNotPermittedException(string message, IRequest request) : this(message, null, request) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.RequestNotPermittedException"/> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    /// <param name='inner'>
    /// Inner.
    /// </param>
    public RequestNotPermittedException(string message, Exception inner) : this(message, inner, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.RequestNotPermittedException"/> class.
    /// </summary>
    /// <param name='message'>
    /// Message.
    /// </param>
    /// <param name='inner'>
    /// Inner.
    /// </param>
    /// <param name='request'>
    /// Request.
    /// </param>
    public RequestNotPermittedException(string message, Exception inner, IRequest request) : base(message, inner)
    {
      this.Data[REQUEST_KEY] = request;
    }

    #endregion
  }
}

