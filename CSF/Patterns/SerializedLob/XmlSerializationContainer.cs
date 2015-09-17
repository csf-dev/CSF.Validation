//
// XmlSerializationContainer.cs
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
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml.Serialization;
using CSF.Reflection;

namespace CSF.Patterns.SerializedLob
{
  /// <summary>
  /// An implementation of <see cref="ISerializationContainer"/> for XML-serialized data.
  /// </summary>
  public class XmlSerializationContainer : ISerializationContainer
  {
    #region properties

    /// <summary>
    /// Gets or sets the <see cref="Type"/> using its name.
    /// </summary>
    /// <value>
    /// The name of the type associated with the data.
    /// </value>
    public virtual string TypeName
    {
      get;
      set;
    }

    #endregion

    #region ISerializationContainer implementation

    /// <summary>
    /// Gets or sets the serialized data. 
    /// </summary>
    /// <value>
    /// The data. 
    /// </value>
    public virtual string Data
    {
      get;
      set;
    }

    /// <summary>
    /// Gets or sets the type that this data serializes/deserializes to/from. 
    /// </summary>
    /// <value>
    /// The type associated with the data. 
    /// </value>
    public virtual Type Type
    {
      get {
        return String.IsNullOrEmpty(this.TypeName)? null : Reflect.TypeFromAppDomain(this.TypeName);
      }
      set {
        this.TypeName = (value != null)? value.FullName : null;
      }
    }

    /// <summary>
    /// Deserialize this instance into an object. 
    /// </summary>
    public virtual object Deserialize()
    {
      object output;
      XmlSerializer serializer = this.GetSerializer();

      using(TextReader reader = new StringReader(this.Data))
      {
        output = serializer.Deserialize(reader);
      }

      return output;
    }

    /// <summary>
    /// Serializes the data into the <see cref="Data"/> property.
    /// </summary>
    /// <param name='data'>
    /// The data to serialize.
    /// </param>
    public virtual void Serialize (object data)
    {
      StringBuilder newData = new StringBuilder();
      XmlSerializer serializer = this.GetSerializer();

      using(TextWriter writer = new StringWriter(newData))
      {
        serializer.Serialize(writer, data);
      }

      this.Data = newData.ToString();
    }

    #endregion

    #region private methods

    /// <summary>
    /// Creates and returns a serializer instance.
    /// </summary>
    /// <returns>
    /// A serializer.
    /// </returns>
    private XmlSerializer GetSerializer()
    {
      if(this.Type == null)
      {
        throw new InvalidOperationException("Cannot create serializer, no type has been selected.");
      }

      return new XmlSerializer(this.Type);
    }

    #endregion
  }
}

