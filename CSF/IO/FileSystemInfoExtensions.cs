//
// FileSystemInfoExtensions.cs
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

namespace CSF.IO
{
  /// <summary>
  /// Extension methods for <see cref="FileSystemInfo"/>.
  /// </summary>
  public static class FileSystemInfoExtensions
  {
    #region extension methods

    /// <summary>
    /// Determines whether this instance is child of the specified directory.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is child of the specified directory; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='info'>
    /// The current <see cref="FileSystemInfo"/> instance.
    /// </param>
    /// <param name='directory'>
    /// The directory to test against.
    /// </param>
    public static bool IsChildOf(this FileSystemInfo info, DirectoryInfo directory)
    {
      return info.FullName.StartsWith(directory.FullName);
    }
    
    /// <summary>
    /// Gets a relative path that represents the current instance's relative path from a given <paramref name="root"/>.
    /// </summary>
    /// <returns>
    /// The relative path component.
    /// </returns>
    /// <param name='info'>
    /// The current <see cref="FileSystemInfo"/> instance.
    /// </param>
    /// <param name='root'>
    /// The root directory from which to create the output
    /// </param>
    /// <exception cref='ArgumentException'>
    /// Is thrown if the current instance is not a child of the root directory.
    /// </exception>
    public static string GetRelative(this FileSystemInfo info, DirectoryInfo root)
    {
      if(!info.IsChildOf(root))
      {
        throw new ArgumentException("Parameter 'info' is not a child of the root directory.");
      }
      
      return info.FullName.Substring(root.FullName.Length);
    }
    
    /// <summary>
    /// Gets the parent of the current instance, or null if the current instance is the root of its filesystem.
    /// </summary>
    /// <returns>
    /// The parent directory.
    /// </returns>
    /// <param name='info'>
    /// The current <see cref="FileSystemInfo"/> instance.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public static DirectoryInfo GetParent(this FileSystemInfo info)
    {
      DirectoryInfo output;
      DirectoryInfo directory = info as DirectoryInfo;
      FileInfo file = info as FileInfo;
      
      if(directory != null)
      {
        output = directory.Parent;
      }
      else if(file != null)
      {
        output = file.Directory;
      }
      else
      {
        throw new ArgumentNullException("info");
      }
      
      return output;
    }

    /// <summary>
    /// Creates a directory recursively, creating parent directories if required.
    /// </summary>
    /// <param name="info">Info.</param>
    public static void CreateRecursive(this DirectoryInfo info)
    {
      if(info == null)
      {
        throw new ArgumentNullException("info");
      }

      if(info == info.Root)
      {
        var message = String.Format("Cannot create the root of a file system: {0}", info.Root.FullName);
        throw new IOException(message);
      }

      if(!info.Exists)
      {
        if(info.Parent != info.Root)
        {
          info.Parent.CreateRecursive();
        }

        info.Create();
        info.Refresh();
      }
    }

    #endregion
  }
}

