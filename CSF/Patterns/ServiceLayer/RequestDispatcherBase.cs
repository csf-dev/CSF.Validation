//
//  RequestDispatcherBase.cs
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
using System.Linq;
using System.Reflection;
using CSF.Reflection;

namespace CSF.Patterns.ServiceLayer
{
  /// <summary>
  /// Abstract base class for a request dispatcher.
  /// </summary>
  public abstract class RequestDispatcherBase : IRequestDispatcher
  {
    #region IRequestDispatcher implementation (virtual methods)

    /// <summary>
    /// Determines whether this instance can dispatch the specified request.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance can dispatch the specified request; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='request'>
    /// The request that we wish to check.
    /// </param>
    public virtual bool CanDispatch(IRequest request)
    {
      if(request == null)
      {
        throw new ArgumentNullException("request");
      }

      return this.CanDispatch(request.GetType());
    }

    /// <summary>
    /// Determines whether this instance can dispatch the specified request type.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance can dispatch requests of the specified type; otherwise, <c>false</c>.
    /// </returns>
    /// <typeparam name='TRequest'>
    /// The type of request that we wish to check.
    /// </typeparam>
    public virtual bool CanDispatch<TRequest> ()
      where TRequest : IRequest
    {
      return this.CanDispatch(typeof(TRequest));
    }

    /// <summary>
    ///  Registers that the specified handler type should be used for requests of the specified type. 
    /// </summary>
    /// <typeparam name='TRequest'>
    ///  The type of request that we are registering a handler for. 
    /// </typeparam>
    /// <typeparam name='THandler'>
    ///  The type of handler to use for requests of the given type. The handler type must expose a default/public
    /// parameterless constructor. 
    /// </typeparam>
    public virtual IRequestDispatcher Register<TRequest, THandler> ()
      where TRequest : IRequest
      where THandler : IRequestHandler, new ()
    {
      return this.Register<TRequest>(() => new THandler());
    }

    /// <summary>
    /// Registers that the specified handler type should be used for requests of the specified type.
    /// </summary>
    /// <param name='factoryMethod'>
    /// A function that creates an instance of an appropriate handler for the request.
    /// </param>
    /// <typeparam name='TRequest'>
    /// The type of request that we are registering a handler for.
    /// </typeparam>
    public virtual IRequestDispatcher Register<TRequest>(Func<IRequestHandler> factoryMethod)
      where TRequest : IRequest
    {
      return this.Register(typeof(TRequest), factoryMethod);
    }

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
    public virtual IRequestDispatcher Register (Type requestType, Type handlerType)
    {
      Func<IRequestHandler> factoryMethod = (() => (IRequestHandler) Activator.CreateInstance(handlerType));

      return this.Register(requestType, factoryMethod);
    }

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
    public virtual IRequestDispatcher RegisterFromAssembly(Assembly assembly)
    {
      IDictionary<Type,Type> containedHandlerTypes = GetRequestHandlerTypes(assembly);
      IRequestDispatcher returnValue = this;

      foreach(Type requestType in containedHandlerTypes.Keys)
      {
        returnValue = this.Register(requestType, containedHandlerTypes[requestType]);
      }

      return returnValue;
    }

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
    public virtual IRequestDispatcher RegisterFromAssemblyOf<ContainedType>()
    {
      return this.RegisterFromAssembly(Assembly.GetAssembly(typeof(ContainedType)));
    }

    /// <summary>
    ///  Removes a registration for a request handler that would handle the specified type of request. 
    /// </summary>
    /// <typeparam name='TRequest'>
    ///  The type of request for which to 'unregister' its handler. 
    /// </typeparam>
    public virtual IRequestDispatcher Unregister<TRequest>()
      where TRequest : IRequest
    {
      return this.Unregister(typeof(TRequest));
    }

    #endregion

    #region IRequestDispatcher implementation (abstract methods that must be implemented)

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
    public abstract TResponse Dispatch<TResponse>(IRequest<TResponse> request)
      where TResponse : Response;

    /// <summary>
    ///  Dispatch the specified request using a request-only mechaism, which will not return a response. 
    /// </summary>
    /// <param name='request'>
    ///  The request to dispatch. 
    /// </param>
    public abstract void Dispatch(IRequest request);

    /// <summary>
    /// Determines whether this instance can dispatch the specified request type.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance can dispatch requests of the specified type; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='requestType'>
    /// The type of request that we wish to check.
    /// </param>
    public abstract bool CanDispatch(Type requestType);

    /// <summary>
    /// Registers that the specified handler type should be used for requests of the specified type.
    /// </summary>
    /// <param name='requestType'>
    /// The type of request that we are registering a handler for.
    /// </param>
    /// <param name='factoryMethod'>
    /// A function that creates an instance of an <see cref='IRequestHandler'/>, matching the type indicated by
    /// <paramref name='handlerType'/>
    /// </param>
    public abstract IRequestDispatcher Register(Type requestType, Func<IRequestHandler> factoryMethod);

    /// <summary>
    ///  Removes a registration for a request handler that would handle the specified type of request. 
    /// </summary>
    /// <param name='requestType'>
    ///  The type of request for which to 'unregister' its handler. 
    /// </param>
    public abstract IRequestDispatcher Unregister(Type requestType);

    #endregion

    #region static methods

    /// <summary>
    /// Gets a collection of all of the <see cref="System.Type"/>s that implement <see cref="IRequestHandler"/> within
    /// the given assembly, indexed by the <see cref="IRequest"/> types that they 
    /// </summary>
    /// <returns>
    /// The request handler types.
    /// </returns>
    /// <param name='assembly'>
    /// Assembly.
    /// </param>
    public static IDictionary<Type,Type> GetRequestHandlerTypes(Assembly assembly)
    {
      if(assembly == null)
      {
        throw new ArgumentNullException("assembly");
      }

      IDictionary<Type,Type> output = new Dictionary<Type, Type>();

      Type
        handlerBase = typeof(RequestHandler<>),
        objectBase = typeof(object),
        currentBase;

      var handlerTypes = (from type in assembly.GetExportedTypes()
                          where
                            type.ImplementsInterface<IRequestHandler>()
                            && type.IsClass
                            && !type.IsAbstract
                            && type.GetConstructor(Type.EmptyTypes) != null
                          select type);

      foreach(Type handlerType in handlerTypes)
      {
        currentBase = handlerType.BaseType;

        // Walk up the inheritance hierarchy looking for the desired base type
        while(!currentBase.IsGenericType
              || currentBase.GetGenericTypeDefinition() != handlerBase)
        {
          currentBase = currentBase.BaseType;

          if(currentBase == objectBase)
          {
            currentBase = null;
            break;
          }
        }

        if(currentBase != null)
        {
          /* We need to know the 'TRequest' from RequestHandler<TRequest,TResponse> - it's the first generic type
           * parameter on that class.
           */
          Type requestType = currentBase.GetGenericArguments()[0];
          output.Add(requestType, handlerType);
        }
      }

      return output;
    }

    #endregion
  }
}

