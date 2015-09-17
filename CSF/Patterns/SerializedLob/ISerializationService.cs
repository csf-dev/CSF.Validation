//
// ISerializationService.cs
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
  /// Interface for a service that is capable of serializing and persisting objects in a non-relational manner.
  /// </summary>
  public interface ISerializationService
  {
    #region serialization and deserialization

    /// <summary>
    /// Serialize and persist the specified data.
    /// </summary>
    /// <param name='data'>
    /// The 'payload' data to serialize.
    /// </param>
    /// <typeparam name='TData'>
    /// The type of the data.
    /// </typeparam>
    void Serialize<TData>(TData data);

    /// <summary>
    /// Serialize and persist the specified data.
    /// </summary>
    /// <param name='dataType'>
    /// The type of the data.
    /// </param>
    /// <param name='data'>
    /// The 'payload' data to serialize.
    /// </param>
    void Serialize(Type dataType, object data);

    /// <summary>
    /// Deserialize and return data of the specified type.
    /// </summary>
    /// <typeparam name='TData'>
    /// The type of the data.
    /// </typeparam>
    TData Deserialize<TData>();

    /// <summary>
    /// Deserialize and return data of the specified type.
    /// </summary>
    /// <param name='dataType'>
    /// The type of the data.
    /// </param>
    object Deserialize(Type dataType);

    #endregion

    #region persistance of raw data

    /// <summary>
    /// Persist the specified raw serialized data.
    /// </summary>
    /// <param name='data'>
    /// The raw/serialized data to persist.
    /// </param>
    void Persist(ISerializationContainer data);

    /// <summary>
    /// Retrieves raw/serialized data from the persistence backend.
    /// </summary>
    /// <typeparam name='TData'>
    /// The serialization-type of the data.
    /// </typeparam>
    ISerializationContainer Retrieve<TData>();

    /// <summary>
    /// Retrieves raw/serialized data from the persistence backend.
    /// </summary>
    /// <param name='serializationType'>
    /// The serialization-type of the data.
    /// </param>
    ISerializationContainer Retrieve(Type serializationType);

    #endregion
  }
}

