//
// ConnectionStringSettingsExtensions.cs
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
using System.Configuration;
using System.Data.Common;

namespace CSF.Configuration.Data
{
  /// <summary>
  /// Extension methods for the <see cref="ConnectionStringSettings"/> type.
  /// </summary>
  public static class ConnectionStringSettingsExtensions
  {
    /// <summary>
    /// Opens the connection.
    /// </summary>
    /// <returns>
    /// The connection.
    /// </returns>
    /// <param name='settings'>
    /// Settings.
    /// </param>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown if <paramref name="settings"/> is null.
    /// </exception>
    /// <exception cref="ConfigurationErrorsException">
    /// If <paramref name="settings"/> refers to a provider invariant name that is not registered with the .NET
    /// framework.
    /// </exception>
    public static IDbConnection CreateAndOpenConnection(this ConnectionStringSettings settings)
    {
      IDbConnection output;
      DbProviderFactory factory;
      
      if(settings == null)
      {
        throw new ArgumentNullException ("settings");
      }
      
      factory = DbProviderFactories.GetFactory(settings.ProviderName);
      output = factory.CreateConnection();
      output.ConnectionString = settings.ConnectionString;
      output.Open();
      
      return output;      
    }
  }
}

