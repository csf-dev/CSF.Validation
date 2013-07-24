//
//  RequestDispatchException.cs
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
  /// Represents an error encountered whilst dispatching a <see cref="IRequest"/>, usually because there was no
  /// associated handler.
  /// </summary>
  public class RequestDispatchException : Exception
  {
    #region constants

    private const string
      DefaultMessageNull                = "The request to dispatch must not be null.",
      DefaultMessageUncaught            = "An unhandled exception was raised by the request handler.  Request " +
                                          "handler implementations should catch exceptions and return them as part " +
                                          "of the response object.  The original exception is stored within the " +
                                          "InnerException property of this exception.",
      DefaultMessageFormatNoHandler     = "There must be a type (that derives from " +
                                          "RequestHandler<TRequest,TResponse>) registered with the request " +
                                          "dispatcher in order to dispatch this request; no such type was found.\n" +
                                          "Request type: `{0}'";

    #endregion

    #region properties

    /// <summary>
    /// Gets or sets the request that triggered this exception.
    /// </summary>
    /// <value>
    /// The request.
    /// </value>
    public virtual IRequest Request
    {
      get {
        return this.Data.Contains("Request")? this.Data["Request"] as IRequest : (IRequest) null;
      }
      set {
        this.Data["Request"] = value;
      }
    }

    #endregion

    #region constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.RequestDispatchException"/> class for an
    /// exception due to a null request passed to the dispatcher.
    /// </summary>
    public RequestDispatchException() : this(null, DefaultMessageNull, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.RequestDispatchException"/> class with a
    /// custom message.
    /// </summary>
    /// <param name='message'>
    /// The exception message
    /// </param>
    public RequestDispatchException(string message) : this(null, message, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.RequestDispatchException"/> class with a
    /// custom message and inner exception information.
    /// </summary>
    /// <param name='message'>
    /// The exception message
    /// </param>
    /// <param name='innner'>
    /// The inner exception.
    /// </param>
    public RequestDispatchException(string message, Exception innner) : this(null, message, innner) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.RequestDispatchException"/> class for a
    /// request that cannot be dispatched because the dispatcher has no registered handler for the request type.
    /// </summary>
    /// <param name='request'>
    /// The request.
    /// </param>
    public RequestDispatchException(IRequest request) : this(request,
                                                             String.Format(DefaultMessageFormatNoHandler,
                                                                           request.GetType().FullName),
                                                             null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.RequestDispatchException"/> class for a
    /// request that raised an uncaught exception during its handling.
    /// </summary>
    /// <param name='request'>
    /// The request.
    /// </param>
    /// <param name='inner'>
    /// The uncaught exception raised by the request handler.
    /// </param>
    public RequestDispatchException(IRequest request, Exception inner) : this(request,
                                                                              DefaultMessageUncaught,
                                                                              inner) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.RequestDispatchException"/> class with
    /// a custom exception message.
    /// </summary>
    /// <param name='request'>
    /// The request.
    /// </param>
    /// <param name='message'>
    /// The exception message.
    /// </param>
    public RequestDispatchException(IRequest request, string message) : this(request, message, null) {}

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.RequestDispatchException"/> class with
    /// a custom exception message and inner exception.
    /// </summary>
    /// <param name='request'>
    /// The request.
    /// </param>
    /// <param name='message'>
    /// The exception message
    /// </param>
    /// <param name='inner'>
    /// The inner exception.
    /// </param>
    public RequestDispatchException(IRequest request, string message, Exception inner) : base(message, inner)
    {
      this.Request = request;
    }

    #endregion
  }
}

