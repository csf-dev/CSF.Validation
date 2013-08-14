//
//  RequestHandler1.cs
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
  /// Base type for all service layer request handlers.
  /// </summary>
  /// <remarks>
  /// <para>
  /// Derived types should override and implement one of either the <c>Handle</c> or <c>HandleRequestOnly</c> methods.
  /// </para>
  /// </remarks>
  public abstract class RequestHandler<TRequest> : IRequestHandler
    where TRequest : IRequest
  {
    #region non-generic IRequestHandler implementation

    Response IRequestHandler.Handle (IRequest request)
    {
      string message = String.Format("The type `{0}' cannot handle requests that return a response.",
                                     this.GetType().FullName);
      throw new NotSupportedException(message);
    }

    void IRequestHandler.HandleRequestOnly (IRequest request)
    {
      this.HandleRequestOnly((TRequest) request);
    }

    #endregion

    #region generic IRequestHandler implementation

    /// <summary>
    ///  Handles a one-way/fire-and-forget request. This method does not return any kind of response. 
    /// </summary>
    /// <param name='request'>
    ///  The request to handle. 
    /// </param>
    public virtual void HandleRequestOnly (TRequest request)
    {
      string message = String.Format("The type `{0}' does not provide an implementation for 'HandleRequestOnly'.",
                                     this.GetType().FullName);
      throw new NotImplementedException(message);
    }

    #endregion
  }
}

