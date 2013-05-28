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
    ///  Dispatch the specified request and get the response. 
    /// </summary>
    /// <param name='request'>
    ///  The request to dispatch. 
    /// </param>
    /// <typeparam name='TResponse'>
    /// The expected type of the response.  The returned response will be cast to this type.
    /// </typeparam>
    /// <exception cref="RequestDispatchException">
    /// If the response returned by the request handler is not of the specifed type.
    /// </exception>
    public virtual TResponse Dispatch<TResponse>(IRequest request)
      where TResponse : IResponse
    {
      TResponse output;

      try
      {
        output = (TResponse) this.Dispatch(request);
      }
      catch(InvalidCastException ex)
      {
        throw new RequestDispatchException(request, "Cannot return response, it was not of the requested type.", ex);
      }

      return output;
    }

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
    ///  Registers that the specified handler should be used for requests of the specified type. 
    /// </summary>
    /// <param name='handler'>
    ///  The handler to use for requests of the given type. 
    /// </param>
    /// <typeparam name='TRequest'>
    ///  The type of request that we are registering a handler for. 
    /// </typeparam>
    public virtual IRequestDispatcher Register<TRequest>(IRequestHandler handler)
      where TRequest : IRequest
    {
      return this.Register(typeof(TRequest), handler);
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
      return this.Register<TRequest,THandler>(() => new THandler());
    }

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
    public virtual IRequestDispatcher Register<TRequest, THandler>(Func<IRequestHandler> factoryMethod)
      where TRequest : IRequest
      where THandler : IRequestHandler
    {
      if(factoryMethod == null)
      {
        throw new ArgumentNullException("factoryMethod");
      }

      return this.Register(typeof(TRequest), typeof(THandler), factoryMethod);
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
      Func<IRequestHandler> factoryMethod = () => {
        ConstructorInfo constructor = handlerType.GetConstructor(Type.EmptyTypes);
        if(constructor == null)
        {
          throw new ArgumentException("The specified handler type does not expose a public parameterless constructor.");
        }

        return (IRequestHandler) constructor.Invoke(null);
      };

      return this.Register(requestType, handlerType, factoryMethod);
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
    /// <param name='factoryMethod'>
    /// A function that creates an instance of an <see cref='IRequestHandler'/>, matching the type indicated by
    /// <paramref name='handlerType'/>
    /// </param>
    public virtual IRequestDispatcher Register(Type requestType, Type handlerType, Func<IRequestHandler> factoryMethod)
    {
      if(handlerType == null)
      {
        throw new ArgumentNullException("handlerType");
      }
      else if(!handlerType.ImplementsInterface<IRequestHandler>())
      {
        throw new ArgumentException("The specified type does not indicate IRequestHandler", "handlerType");
      }
      else if(factoryMethod == null)
      {
        throw new ArgumentNullException("factoryMethod");
      }

      IRequestHandler handler = factoryMethod();
      if(handler.GetType() != handlerType)
      {
        string message = String.Format("Type mismatch: Handler of type `{0}' being registered but factory method " +
                                       "created a handler of type `{1}'.",
                                       handlerType.FullName,
                                       handler.GetType().FullName);
        throw new InvalidOperationException(message);
      }

      return this.Register(requestType, handler);
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
      if(assembly == null)
      {
        throw new ArgumentNullException("assembly");
      }

      IRequestDispatcher returnValue = this;

      var handlerTypes = (from type in assembly.GetExportedTypes()
                          where
                            type.ImplementsInterface<IRequestHandler>()
                            && type.IsClass
                            && !type.IsAbstract
                            && type.GetConstructor(Type.EmptyTypes) != null
                          select type);

      foreach(Type handlerType in handlerTypes)
      {
        Type genericHandlerInterface = (from i in handlerType.GetInterfaces()
                                        where
                                          i.IsGenericType
                                          && i.GetGenericTypeDefinition() == typeof(IRequestHandler<,>)
                                        select i).FirstOrDefault();

        if(genericHandlerInterface != null)
        {
          /* We need to know the 'TRequest' from IRequestHandler<TRequest,TResponse> - it's the first generic type
           * parameter on that interface.
           */
          Type requestType = genericHandlerInterface.GetGenericArguments()[0];
          returnValue = this.Register(requestType, handlerType);
        }
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
    ///  Dispatch the specified request and get the response. 
    /// </summary>
    /// <param name='request'>
    ///  The request to dispatch. 
    /// </param>
    public abstract IResponse Dispatch(IRequest request);

    /// <summary>
    ///  Dispatch the specified request using a request-only mechaism, which will not return a response. 
    /// </summary>
    /// <param name='request'>
    ///  The request to dispatch. 
    /// </param>
    public abstract void DispatchRequestOnly(IRequest request);

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
    ///  Gets a read-only indexed collection of the registered request types and the request handlers that requests of
    /// that type would be dispatched to. 
    /// </summary>
    /// <returns>
    ///  The registered handlers. 
    /// </returns>
    public abstract IDictionary<Type, IRequestHandler> GetRegisteredHandlers();

    /// <summary>
    ///  Registers that the specified handler should be used for requests of the specified type. 
    /// </summary>
    /// <param name='requestType'>
    ///  The type of request that we are registering a handler for. 
    /// </param>
    /// <param name='handler'>
    ///  The handler to use for requests of the given type. 
    /// </param>
    public abstract IRequestDispatcher Register(Type requestType, IRequestHandler handler);

    /// <summary>
    ///  Removes a registration for a request handler that would handle the specified type of request. 
    /// </summary>
    /// <param name='requestType'>
    ///  The type of request for which to 'unregister' its handler. 
    /// </param>
    public abstract IRequestDispatcher Unregister(Type requestType);

    #endregion
  }
}

