//
//  LocalRequestDispatcher.cs
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
using System.Collections.Generic;

namespace CSF.Patterns.ServiceLayer
{
  /// <summary>
  /// Implementation of <see cref="IRequestDispatcher"/> that is suited to handle local/in-process requests and
  /// responses.
  /// </summary>
  /// <remarks>
  /// <para>
  /// This implementation is best-used when there is no remoting going on and the requests are all coming from the same
  /// process as will be hosting the request dispatcher itself.
  /// </para>
  /// </remarks>
  public class LocalRequestDispatcher : RequestDispatcherBase
  {
    #region properties

    /// <summary>
    /// Gets the registered handler factories, indexed by the request types that they handle.
    /// </summary>
    /// <value>
    /// The handlers.
    /// </value>
    protected virtual IDictionary<Type, Func<IRequestHandler>> HandlerFactories
    {
      get;
      private set;
    }

    #endregion

    #region implemented abstract members of CSF.Patterns.ServiceLayer.RequestDispatcherBase

    /// <summary>
    ///  Dispatch the specified request and get the response. 
    /// </summary>
    /// <param name='request'>
    ///  The request to dispatch. 
    /// </param>
    public override TResponse Dispatch<TResponse>(IRequest<TResponse> request)
    {
      if(request == null)
      {
        throw new RequestDispatchException();
      }

      TResponse output = null;
      Response untypedOutput;
      bool releaseable;
      IRequestHandler handler = this.GetHandlerFor(request, out releaseable);

      try
      {
        untypedOutput = handler.Handle(request);
      }
      catch(Exception ex)
      {
        Type responseType = typeof(TResponse);

        if(responseType.GetConstructor(new Type[] { typeof(Exception) }) != null)
        {
          untypedOutput = (Response) Activator.CreateInstance(responseType, ex);
        }
        else
        {
          throw new RequestDispatchException(request, ex);
        }
      }
      finally
      {
        if(releaseable)
        {
          this.ReleaseHandler(handler);
        }
      }

      if(untypedOutput == null)
      {
        string message = String.Format("Request handler (type: `{0}') must return an instance that derives from " +
                                       "`CSF.Patterns.ServiceLayer.Response' and not return a null reference.",
                                       handler.GetType().FullName);
        throw new RequestDispatchException(request, message);
      }

      try
      {
        output = (TResponse) untypedOutput;
      }
      catch(InvalidCastException)
      {
        string message = String.Format("Request handler (type: `{0}') must return an instance of `{1}'.  Instead it " +
                                       "returned an instance of `{2}'.",
                                       handler.GetType().FullName,
                                       typeof(TResponse).FullName,
                                       untypedOutput.GetType().FullName);
        throw new RequestDispatchException(request, message);
      }

      return output;
    }

    /// <summary>
    ///  Dispatch the specified request using a request-only mechaism, which will not return a response. 
    /// </summary>
    /// <param name='request'>
    ///  The request to dispatch. 
    /// </param>
    public override void Dispatch(IRequest request)
    {
      if(request == null)
      {
        throw new RequestDispatchException();
      }

      bool releaseable;
      IRequestHandler handler = this.GetHandlerFor(request, out releaseable);

      try
      {
        handler.HandleRequestOnly(request);
      }
      catch(Exception ex)
      {
        throw new RequestDispatchException(request, ex);
      }
      finally
      {
        if(releaseable)
        {
          this.ReleaseHandler(handler);
        }
      }
    }

    /// <summary>
    ///  Determines whether this instance can dispatch the specified request. 
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance can dispatch the specified requestType; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='requestType'>
    /// The request type to test.
    /// </param>
    public override bool CanDispatch(Type requestType)
    {
      return (this.HandlerFactories.ContainsKey(requestType)
              && this.HandlerFactories[requestType] != null);
    }

    /// <summary>
    ///  Registers that the specified handler should be used for requests of the specified type. 
    /// </summary>
    /// <param name='requestType'>
    ///  The type of request that we are registering a handler for. 
    /// </param>
    /// <param name='factoryMethod'>
    /// The factory method used to create handlers.
    /// </param>
    public override IRequestDispatcher Register (Type requestType, Func<IRequestHandler> factoryMethod)
    {
      if(requestType == null)
      {
        throw new ArgumentNullException("requestType");
      }
      if(factoryMethod == null)
      {
        throw new ArgumentNullException("factoryMethod");
      }

      this.HandlerFactories.Add(requestType, factoryMethod);
      return this;
    }

    /// <summary>
    ///  Removes a registration for a request handler that would handle the specified type of request. 
    /// </summary>
    /// <param name='requestType'>
    ///  The type of request for which to 'unregister' its handler. 
    /// </param>
    public override IRequestDispatcher Unregister(Type requestType)
    {
      if(requestType == null)
      {
        throw new ArgumentNullException("requestType");
      }

      this.HandlerFactories.Remove(requestType);
      return this;
    }

    #endregion

    #region methods

    /// <summary>
    /// Gets the appropriate request handler for a given request.
    /// </summary>
    /// <returns>
    /// A request handler instance.
    /// </returns>
    /// <param name='request'>
    /// The request.
    /// </param>
    /// <param name='releaseable'>
    /// A value that indicates whether or not the returned handler may be released.
    /// </param>
    protected virtual IRequestHandler GetHandlerFor(IRequest request, out bool releaseable)
    {
      if(request == null)
      {
        throw new ArgumentNullException("request");
      }

      Type requestType = request.GetType();
      IRequestHandler output;

      if(this.HandlerFactories.ContainsKey(requestType))
      {
        var factory = this.HandlerFactories[requestType];
        output = factory();
        releaseable = true;
      }
      else
      {
        throw new RequestDispatchException(request);
      }

      return output;
    }

    /// <summary>
    /// Releases a request handler.
    /// </summary>
    /// <param name='handler'>
    /// The handler to release.
    /// </param>
    protected virtual void ReleaseHandler(IRequestHandler handler)
    {
      IDisposable disposeableHandler = handler as IDisposable;

      if(disposeableHandler != null)
      {
        disposeableHandler.Dispose();
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.Patterns.ServiceLayer.LocalRequestDispatcher"/> class.
    /// </summary>
    public LocalRequestDispatcher()
    {
      this.HandlerFactories = new Dictionary<Type, Func<IRequestHandler>>();
    }

    #endregion
  }
}

