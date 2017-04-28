//
// IManifestMetadata.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2017 Craig Fowler
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

namespace CSF.Validation.Manifest
{
  /// <summary>
  /// Describes a type which provides access to arbitrary information about an <see cref="IManifestRule"/>.
  /// </summary>
  public interface IManifestMetadata
  {
    /// <summary>
    /// Gets the value with the specified name, or a <c>null</c> reference if it does not exist.
    /// </summary>
    /// <param name="name">The metadata name.</param>
    object Get(string name);

    /// <summary>
    /// Gets the value with the specified name, of the associated type, or a <c>null</c> reference if it does not exist.
    /// Note that this method will return <c>null</c> if called with an incorrect type, even if the item did exist as
    /// an instance of a different type.
    /// </summary>
    /// <param name="name">The metadata name.</param>
    /// <typeparam name="T">The desired metadata object type.</typeparam>
    T Get<T>(string name) where T : class;
  }
}
