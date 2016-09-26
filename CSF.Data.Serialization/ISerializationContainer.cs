//
// ISerializationContainer.cs
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

namespace CSF.Data.Serialization
{
  /// <summary>
  /// Interface represents a container for data that can be serialized into string form.
  /// </summary>
  public interface ISerializationContainer
  {
    #region properties

    /// <summary>
    /// Gets or sets the serialized data.
    /// </summary>
    /// <value>
    /// The data.
    /// </value>
    string Data { get; set; }

    /// <summary>
    /// Gets or sets the type that this data serializes/deserializes to/from.
    /// </summary>
    /// <value>
    /// The type associated with the data.
    /// </value>
    Type Type { get; set; }

    #endregion

    #region methods

    /// <summary>
    /// Deserialize this instance into an object.
    /// </summary>
    object Deserialize();

    /// <summary>
    /// Serializes the configuration into the <see cref="Data"/> property.
    /// </summary>
    /// <param name='data'>
    /// The data to serialize.
    /// </param>
    void Serialize(object data);

    #endregion
  }
}

