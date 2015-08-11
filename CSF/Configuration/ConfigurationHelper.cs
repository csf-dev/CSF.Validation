//
//  ConfigurationManagerExtensions.cs
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
using System.Configuration;
using CSF.Reflection;

namespace CSF.Configuration
{
  /// <summary>
  /// Type providing convenience methods relating to configuration.
  /// </summary>
  public static class ConfigurationHelper
  {
    /// <summary>
    /// Gets a type that represents a <see cref="ConfigurationSection"/> from the current
    /// <see cref="ConfigurationManager"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method uses the default configuration path, retrieved using <see cref="M:GetDefaultConfigurationPath{TSection}"/>.
    /// </para>
    /// <para>
    /// If the section is not found at the appropriate configuration path, or if the configuration section found at that
    /// path is not of the requested type then a null reference is returned instead.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The section, or a null reference.
    /// </returns>
    /// <typeparam name='TSection'>
    /// The type of configuration section to retrieve.
    /// </typeparam>
    public static TSection GetSection<TSection>() where TSection : ConfigurationSection
    {
      string sectionPath = GetDefaultConfigurationPath<TSection>();
      return GetSection<TSection>(sectionPath);
    }

    /// <summary>
    /// Gets a type that represents a <see cref="ConfigurationSection"/> from the current
    /// <see cref="ConfigurationManager"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This method uses a user-supplied path within the configuration file.
    /// </para>
    /// <para>
    /// If the section is not found at the appropriate configuration path, or if the configuration section found at that
    /// path is not of the requested type then a null reference is returned instead.
    /// </para>
    /// </remarks>
    /// <returns>
    /// The section, or a null reference.
    /// </returns>
    /// <param name='sectionPath'>
    /// Section path.
    /// </param>
    /// <typeparam name='TSection'>
    /// The type of configuration section to retrieve.
    /// </typeparam>
    public static TSection GetSection<TSection>(string sectionPath) where TSection : ConfigurationSection
    {
      object configObject = ConfigurationManager.GetSection(sectionPath);
      return configObject as TSection;
    }

    /// <summary>
    /// Gets the default configuration path for a type that implements <see cref="ConfigurationSection"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The default configiuration path returned is equivalent to the full name of the specified type, substituting the
    /// period <c>'.'</c> character with a forward-slash character <c>'/'</c>
    /// </para>
    /// </remarks>
    /// <returns>
    /// The default configuration path.
    /// </returns>
    /// <typeparam name='TSection'>
    /// The type of configuration section for which we want to generate a default path.
    /// </typeparam>
    public static string GetDefaultConfigurationPath<TSection>() where TSection : ConfigurationSection
    {
      Type sectionType = typeof(TSection);
      return sectionType.FullName.Replace('.', '/');
    }
  }
}

