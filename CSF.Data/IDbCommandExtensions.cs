//
// IDbCommandExtensions.cs
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
using System.Data;

namespace CSF.Data
{
  /// <summary>
  /// Extension methods for <c>IDbCommand</c> instances.
  /// </summary>
  public static class IDbCommandExtensions
  {
    /// <summary>
    /// Convenience extension method adds a named parameter to a <c>IDbCommand</c> instance.
    /// </summary>
    /// <param name='command'>
    /// The command to which the parameter is to be added.
    /// </param>
    /// <param name='name'>
    /// The name of the parameter.
    /// </param>
    /// <param name='value'>
    /// The parameter's value.
    /// </param>
    public static void AddParameter(this IDbCommand command, string name, object value)
    {
      if(command == null)
      {
        throw new ArgumentNullException("command");
      }

      var param = command.CreateParameter();
      param.ParameterName = name;
      param.Value = value;
      command.Parameters.Add(param);
    }
  }
}

