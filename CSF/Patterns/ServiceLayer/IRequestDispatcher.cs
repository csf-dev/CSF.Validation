//
//  IRequestDispatcher.cs
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
using System.Reflection;
using System.Collections.Generic;

namespace CSF.Patterns.ServiceLayer
{
  /// <summary>
  /// Interface for a type that dispatches <see cref="IRequest"/> instances to the correct handler type, executes them
  /// and then returns their relevant responses.
  /// </summary>
  public interface IRequestDispatcher
  {
    #region request dispatching

    /// <summary>
    /// Dispatch the specified request and get the response.
    /// </summary>
    /// <param name='request'>
    /// The request to dispatch.
    /// </param>
    IResponse Dispatch(IRequest request);

    /// <summary>
    /// Dispatch the specified request and get a strongly-typed response.
    /// </summary>
    /// <param name='request'>
    /// The request to dispatch.
    /// </param>
    /// <typeparam name='TResponse'>
    /// The expected type of the response.  The returned response will be cast to this type.
    /// </typeparam>
    /// <exception cref="RequestDispatchException">
    /// If the response returned by the request handler is not of the specifed type.
    /// </exception>
    TResponse Dispatch<TResponse>(IRequest request)
      where TResponse : IResponse;

    /// <summary>
    /// Dispatch the specified request using a request-only mechaism, which will not return a response.
    /// </summary>
    /// <param name='request'>
    /// The request to dispatch.
    /// </param>
    void DispatchRequestOnly(IRequest request);

    #endregion

    #region inspecting what this dispatcher can do

    /// <summary>
    /// Determines whether this instance can dispatch the specified request.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance can dispatch the specified request; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='request'>
    /// The request that we wish to check.
    /// </param>
    bool CanDispatch(IRequest request);

    /// <summary>
    /// Determines whether this instance can dispatch the specified request type.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance can dispatch requests of the specified type; otherwise, <c>false</c>.
    /// </returns>
    /// <typeparam name='TRequest'>
    /// The type of request that we wish to check.
    /// </typeparam>
    bool CanDispatch<TRequest>() where TRequest : IRequest;

    /// <summary>
    /// Determines whether this instance can dispatch the specified request type.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance can dispatch requests of the specified type; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='requestType'>
    /// The type of request that we wish to check.
    /// </param>
    bool CanDispatch(Type requestType);

    /// <summary>
    /// Gets a read-only indexed collection of the registered request types and the request handlers that requests of
    /// that type would be dispatched to.
    /// </summary>
    /// <returns>
    /// The registered handlers.
    /// </returns>
    IDictionary<Type, IRequestHandler> GetRegisteredHandlers();

    #endregion

    #region registering handlers

    /// <summary>
    /// Registers that the specified handler should be used for requests of the specified type.
    /// </summary>
    /// <param name='handler'>
    /// The handler to use for requests of the given type.
    /// </param>
    /// <typeparam name='TRequest'>
    /// The type of request that we are registering a handler for.
    /// </typeparam>
    IRequestDispatcher Register<TRequest>(IRequestHandler handler)
      where TRequest : IRequest;

    /// <summary>
    /// Registers that the specified handler should be used for requests of the specified type.
    /// </summary>
    /// <param name='requestType'>
    /// The type of request that we are registering a handler for.
    /// </param>
    /// <param name='handler'>
    /// The handler to use for requests of the given type.
    /// </param>
    IRequestDispatcher Register(Type requestType, IRequestHandler handler);

    /// <summary>
    /// Registers that the specified handler type should be used for requests of the specified type.
    /// </summary>
    /// <typeparam name='TRequest'>
    /// The type of request that we are registering a handler for.
    /// </typeparam>
    /// <typeparam name='THandler'>
    /// The type of handler to use for requests of the given type.  The handler type must expose a default/public
    /// parameterless constructor.
    /// </typeparam>
    IRequestDispatcher Register<TRequest, THandler>()
      where TRequest : IRequest
      where THandler : IRequestHandler, new();

    /// <summary>
    /// Registers that the specified handler type should be used for requests of the specified type.
    /// </summary>
    /// <param name='factoryMethod'>
    /// A function that creates an instance of <typeparamref name='THandler' />.
    /// </param>
    /// <typeparam name='TRequest'>
    /// The type of request that we are registering a handler for.
    /// </typeparam>
    /// <typeparam name='THandler'>
    /// The type of handler to use for requests of the given type.  The handler type must expose a default/public
    /// parameterless constructor.
    /// </typeparam>
    IRequestDispatcher Register<TRequest, THandler>(Func<IRequestHandler> factoryMethod)
      where TRequest : IRequest
      where THandler : IRequestHandler;

    /// <summary>
    /// Registers that the specified handler type should be used for requests of the specified type.
    /// </summary>
    /// <param name='requestType'>
    /// The type of request that we are registering a handler for.
    /// </param>
    /// <param name='handlerType'>
    /// The type of handler to use for requests of the given type.  The handler type must expose a default/public
    /// parameterless constructor.
    /// </param>
    IRequestDispatcher Register(Type requestType, Type handlerType);

    /// <summary>
    /// Registers that the specified handler type should be used for requests of the specified type.
    /// </summary>
    /// <param name='requestType'>
    /// The type of request that we are registering a handler for.
    /// </param>
    /// <param name='handlerType'>
    /// The type of handler to use for requests of the given type.  The handler type must expose a default/public
    /// parameterless constructor.
    /// </param>
    /// <param name='factoryMethod'>
    /// A function that creates an instance of an <see cref='IRequestHandler'/>, matching the type indicated by
    /// <paramref name='handlerType'/>
    /// </param>
    IRequestDispatcher Register(Type requestType, Type handlerType, Func<IRequestHandler> factoryMethod);

    /// <summary>
    /// Searches an <see cref="Assembly"/> and registers all request handlers found within.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This search discovers all types within the assembly that:
    /// <list type="bullet">
    /// <item>Implement the generic <c>IRequestHandler&lt;TRequest,TResponse&gt;</c> interface AND</item>
    /// <item>Have a default/public parameterless constructor</item>
    /// </list>
    /// All of these types are registered using their designated request type.
    /// </para>
    /// </remarks>
    /// <param name='assembly'>
    /// The assembly to search for request handlers.
    /// </param>
    IRequestDispatcher RegisterFromAssembly(Assembly assembly);

    /// <summary>
    /// Searches an <see cref="Assembly"/> and registers all request handlers found within.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This search discovers all types within the assembly that:
    /// <list type="bullet">
    /// <item>Implement the generic <c>IRequestHandler&lt;TRequest,TResponse&gt;</c> interface AND</item>
    /// <item>Have a default/public parameterless constructor</item>
    /// </list>
    /// All of these types are registered using their designated request type.
    /// </para>
    /// </remarks>
    /// <typeparam name='ContainedType'>
    /// A reference to a type, used to indicate the <see cref="Assembly"/> to search.
    /// </typeparam>
    IRequestDispatcher RegisterFromAssemblyOf<ContainedType>();

    /// <summary>
    /// Removes a registration for a request handler that would handle the specified type of request.
    /// </summary>
    /// <typeparam name='TRequest'>
    /// The type of request for which to 'unregister' its handler.
    /// </typeparam>
    IRequestDispatcher Unregister<TRequest>()
      where TRequest : IRequest;

    /// <summary>
    /// Removes a registration for a request handler that would handle the specified type of request.
    /// </summary>
    /// <param name='requestType'>
    /// The type of request for which to 'unregister' its handler.
    /// </param>
    IRequestDispatcher Unregister(Type requestType);

    #endregion
  }
}

