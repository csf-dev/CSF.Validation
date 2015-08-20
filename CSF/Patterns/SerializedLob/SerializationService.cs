//
// SerializationService.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2015 CSF Software Limited
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.

using System;

namespace CSF.Patterns.SerializedLob
{
  /// <summary>
  /// Abstract base class implementing <see cref="ISerializationService"/> and providing some of the framework for
  /// serialization and deserialization.
  /// </summary>
  public abstract class SerializationService : ISerializationService
  {
    #region ISerializationService implementation

    /// <summary>
    /// Serialize and persist the specified data. 
    /// </summary>
    /// <param name='data'>
    /// The 'payload' data to serialize. 
    /// </param>
    /// <typeparam name='TData'>
    /// The type of the data. 
    /// </typeparam>
    public virtual void Serialize<TData>(TData data)
    {
      this.Serialize(typeof(TData), data);
    }

    /// <summary>
    /// Serialize and persist the specified data. 
    /// </summary>
    /// <param name='dataType'>
    /// The type of the data. 
    /// </param>
    /// <param name='data'>
    /// The 'payload' data to serialize. 
    /// </param>
    public virtual void Serialize(Type dataType, object data)
    {
      if(dataType == null)
      {
        throw new ArgumentNullException("dataType");
      }

      Type serializationType = this.SelectSerializationType(dataType);

      ISerializationContainer container = this.CreateSerializationContainer(serializationType);
      container.Type = serializationType;

      container.Serialize(data);
      this.Persist(container);
    }

    /// <summary>
    /// Deserialize and return data of the specified type. 
    /// </summary>
    /// <typeparam name='TData'>
    /// The type of the data. 
    /// </typeparam>
    public virtual TData Deserialize<TData>()
    {
      return (TData) this.Deserialize(typeof(TData));
    }

    /// <summary>
    /// Deserialize and return data of the specified type. 
    /// </summary>
    /// <param name='dataType'>
    /// The type of the data. 
    /// </param>
    public virtual object Deserialize(Type dataType)
    {
      if(dataType == null)
      {
        throw new ArgumentNullException("dataType");
      }

      Type serializationType = this.SelectSerializationType(dataType);
      ISerializationContainer container = this.Retrieve(serializationType);

      return container.Deserialize();
    }

    /// <summary>
    /// Retrieves raw/serialized data from the persistence backend. 
    /// </summary>
    /// <typeparam name='TData'>
    /// The serialization-type of the data. 
    /// </typeparam>
    public virtual ISerializationContainer Retrieve<TData>()
    {
      return this.Retrieve(typeof(TData));
    }

    #endregion

    #region virtual methods

    /// <summary>
    /// Selects the serialization/deserialization type for a given 'desired' type.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This basic/default implementation simply returns the desired type directly but in a more advanced scenario the
    /// desired type could easily be an interface.  In that case we will want to perform some more advanced selection of
    /// the exact concrete type that is used for the serialization/deserialization process.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The serialization type.
    /// </returns>
    /// <param name='desiredType'>
    /// The type for which a serialization type is desired.
    /// </param>
    protected virtual Type SelectSerializationType(Type desiredType)
    {
      return desiredType;
    }

    #endregion

    #region abstract methods

    /// <summary>
    /// Persist the specified raw serialized data. 
    /// </summary>
    /// <param name='data'>
    /// The raw/serialized data to persist. 
    /// </param>
    public abstract void Persist(ISerializationContainer data);

    /// <summary>
    /// Retrieves raw/serialized data from the persistence backend. 
    /// </summary>
    /// <param name='serializationType'>
    /// The serialization-type of the data. 
    /// </param>
    public abstract ISerializationContainer Retrieve(Type serializationType);

    /// <summary>
    /// Creates a new serialization container (appropriate to this service) for the given type.
    /// </summary>
    /// <returns>
    /// The serialization container.
    /// </returns>
    /// <param name='serializationType'>
    /// The serialization type of the data that the container will be created for.
    /// </param>
    public abstract ISerializationContainer CreateSerializationContainer(Type serializationType);

    #endregion
  }
}

