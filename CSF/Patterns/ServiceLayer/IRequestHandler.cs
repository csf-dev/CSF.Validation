//
//  IRequestHandler.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2012 Craig Fowler
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
  /// Non-generic interface for a request handler - a type that handles requests and returns responses (or optionally
  /// does not return any form of response).
  /// </summary>
  /// <remarks>
  /// <para>
  /// Handlers MUST be stateless/thread-safe in their design, as a single handler instance is used to process many
  /// requests by the <see cref="IRequestDispatcher"/>.  Typically a handler does not need any properties/fields as they
  /// simply expose either one or two methods that handle a request (and the request itself is passed in as a method
  /// parameter).
  /// </para>
  /// <para>
  /// If you wish to take advantage of <see cref="IRequestDispatcher.RegisterFromAssembly"/> or the generic
  /// <c>IRequestDispatcher.RegisterFromAssemblyOf</c> method then your request handlers must present a default, public
  /// parameterless constructor.
  /// </para>
  /// </remarks>
  public interface IRequestHandler
  {
    /// <summary>
    /// Handles a standard/typical request, in which a response is returned to the caller.
    /// </summary>
    /// <param name='request'>
    /// The request to handle.
    /// </param>
    /// <exception cref="InvalidCastException">
    /// If the <paramref name="request"/> is not of the type that this handler is designed to deal with.
    /// </exception>
    Response Handle(IRequest request);

    /// <summary>
    /// Handles a one-way/fire-and-forget request.  This method does not return any kind of response.
    /// </summary>
    /// <param name='request'>
    /// The request to handle.
    /// </param>
    /// <exception cref="InvalidCastException">
    /// If the <paramref name="request"/> is not of the type that this handler is designed to deal with.
    /// </exception>
    void HandleRequestOnly(IRequest request);
  }
}

