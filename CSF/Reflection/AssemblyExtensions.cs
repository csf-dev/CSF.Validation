//
//  AssemblyExtensions.cs
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
using System.Reflection;
using System.IO;
using System.Collections.Generic;
using System.Linq;

namespace CSF.Reflection
{
  /// <summary>
  /// Extension methods for the Assembly type.
  /// </summary>
  public static class AssemblyExtensions
  {
    #region embedded resources

    /// <summary>
    /// Extension method reads a text-based resource stored within an assembly.
    /// </summary>
    /// <returns>
    /// The manifest resource text.
    /// </returns>
    /// <param name='assembly'>
    /// The assembly
    /// </param>
    /// <param name='resourceName'>
    /// Resource name.
    /// </param>
    public static string GetManifestResourceText(this Assembly assembly, string resourceName)
    {
      string output;

      if(assembly == null)
      {
        throw new ArgumentNullException("assembly");
      }

      using(Stream resourceStream = assembly.GetManifestResourceStream(resourceName))
      {
        if(resourceStream == null)
        {
          string message = String.Format("Manifest resource '{0}' was not found in the assembly '{1}'",
                                         resourceName,
                                         assembly.FullName);
          throw new InvalidOperationException(message);
        }

        output = GetResourceText(resourceStream);
      }

      return output;
    }

    /// <summary>
    /// Extension method reads a text-based resource stored within an assembly.
    /// </summary>
    /// <returns>
    /// The manifest resource text.
    /// </returns>
    /// <param name='assembly'>
    /// The assembly.
    /// </param>
    /// <param name='type'>
    /// A type by which to namespace the resource.
    /// </param>
    /// <param name='resourceName'>
    /// Resource name.
    /// </param>
    public static string GetManifestResourceText(this Assembly assembly, Type type, string resourceName)
    {
      string output;

      if(assembly == null)
      {
        throw new ArgumentNullException("assembly");
      }
      else if(type == null)
      {
        throw new ArgumentNullException("type");
      }

      using(Stream resourceStream = assembly.GetManifestResourceStream(type, resourceName))
      {
        if(resourceStream == null)
        {
          string message = String.Format("Manifest resource '{2}.{0}' was not found in the assembly '{1}'",
                                         resourceName,
                                         assembly.FullName,
                                         type.Namespace);
          throw new InvalidOperationException(message);
        }

        output = GetResourceText(resourceStream);
      }

      return output;
    }

    /// <summary>
    /// Private helper method gets the textual content of a stream.
    /// </summary>
    /// <returns>
    /// The text content of the stream.
    /// </returns>
    /// <param name='resourceStream'>
    /// A resource stream.
    /// </param>
    private static string GetResourceText(Stream resourceStream)
    {
      string output;

      using(TextReader reader = new StreamReader(resourceStream))
      {
        output = reader.ReadToEnd();
      }

      return output;
    }

    #endregion

    #region assembly attributes
    
    /// <summary>
    /// Gets a single <see cref="Attribute"/> that decorates the given assembly.
    /// </summary>
    /// <returns>
    /// The attribute present on the assembly, or null if no attribute was present.
    /// </returns>
    /// <param name='assembly'>
    /// An assembly.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The type of attribute desired.
    /// </typeparam>
    public static TAttribute GetAttribute<TAttribute>(this Assembly assembly) where TAttribute : Attribute
    {
      return assembly.GetAttribute<TAttribute>(false);
    }
    
    /// <summary>
    /// Gets a collection of <see cref="Attribute"/> that decorates the given assembly.
    /// </summary>
    /// <returns>
    /// A collection of attributes for the assembly, which may be empty.
    /// </returns>
    /// <param name='assembly'>
    /// An assembly.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The type of attribute desired.
    /// </typeparam>
    public static IList<TAttribute> GetAttributes<TAttribute>(this Assembly assembly) where TAttribute : Attribute
    {
      return assembly.GetAttributes<TAttribute>(false);
    }
    
    /// <summary>
    /// Gets a single <see cref="Attribute"/> that decorates the given assembly.
    /// </summary>
    /// <returns>
    /// The attribute present on the assembly, or null if no attribute was present.
    /// </returns>
    /// <param name='assembly'>
    /// An assembly.
    /// </param>
    /// <param name='inherit'>
    /// Whether or not to use inheritance when searching for the attribute.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The type of attribute desired.
    /// </typeparam>
    /// <exception cref='InvalidOperationException'>
    /// Is thrown if the assembly is decorated with multiple attributes of the desired type.
    /// </exception>
    public static TAttribute GetAttribute<TAttribute>(this Assembly assembly,
                                                      bool inherit) where TAttribute : Attribute
    {
      IList<TAttribute> attributes = assembly.GetAttributes<TAttribute>(inherit);
      
      if(attributes.Count > 1)
      {
        throw new InvalidOperationException(String.Format("Assembly `{0}' is decorated with `{1}' multiple times.",
                                                          assembly.GetName().Name,
                                                          typeof(TAttribute).FullName));
      }
      
      return (attributes.Count != 0)? attributes.First() : (TAttribute) null;
    }
    
    /// <summary>
    /// Gets a collection of <see cref="Attribute"/> that decorates the given assembly.
    /// </summary>
    /// <returns>
    /// A collection of attributes for the assembly, which may be empty.
    /// </returns>
    /// <param name='assembly'>
    /// An assembly.
    /// </param>
    /// <param name='inherit'>
    /// Whether or not to use inheritance when searching for the attribute.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The type of attribute desired.
    /// </typeparam>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" />.
    /// </exception>
    public static IList<TAttribute> GetAttributes<TAttribute>(this Assembly assembly,
                                                              bool inherit) where TAttribute : Attribute
    {
      if(assembly == null)
      {
        throw new ArgumentNullException ("assembly");
      }
      
      return assembly.GetCustomAttributes(typeof(TAttribute), inherit).Cast<TAttribute>().ToList();
    }

    /// <summary>
    /// Determines whether this instance is decorated with the specified attribute.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is decorated with the specified attribute; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='assembly'>
    /// The assembly to inspect.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The attribute type to look for.
    /// </typeparam>
    public static bool HasAttribute<TAttribute>(this Assembly assembly) where TAttribute : Attribute
    {
      return assembly.HasAttribute<TAttribute>(false);
    }

    /// <summary>
    /// Determines whether this instance is decorated with the specified attribute.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance is decorated with the specified attribute; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='assembly'>
    /// The assembly to inspect.
    /// </param>
    /// <param name='inherit'>
    /// Whether or not to search the <paramref name="assembly"/>'s inheritance chain to find the attribute or not.
    /// </param>
    /// <typeparam name='TAttribute'>
    /// The attribute type to look for.
    /// </typeparam>
    public static bool HasAttribute<TAttribute>(this Assembly assembly, bool inherit) where TAttribute : Attribute
    {
      return (assembly.GetAttributes<TAttribute>(inherit).Count > 0);
    }
    
    #endregion
  }
}

