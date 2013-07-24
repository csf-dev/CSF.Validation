//
//  RequestExtensions.cs
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
  /// Extension methods for IRequest types.
  /// </summary>
  [Obsolete("This entire type is obsolete - it is not needed.")]
  public static class RequestExtensions
  {
    /// <summary>
    /// Dispatch the specified request to the given request dispatcher.
    /// </summary>
    /// <param name='request'>
    /// The request to dispatch.
    /// </param>
    /// <param name='dispatcher'>
    /// The request dispatcher to use.
    /// </param>
    /// <typeparam name='TResponse'>
    /// The expected type of the response.
    /// </typeparam>
    public static TResponse Dispatch<TResponse>(this IRequest<TResponse> request, IRequestDispatcher dispatcher)
      where TResponse : Response
    {
      if(request == null)
      {
        throw new ArgumentNullException("request");
      }
      else if(dispatcher == null)
      {
        throw new ArgumentNullException("dispatcher");
      }

      return dispatcher.Dispatch<TResponse>(request);
    }
  }
}

