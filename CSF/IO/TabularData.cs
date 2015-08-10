//
//  TabularDataWrapper.cs
//
//  Author:
//       Craig Fowler <craig@craigfowler.me.uk>
//
//  Copyright (c) 2015 Craig Fowler
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
using System.Collections.Generic;

namespace CSF.IO
{
  /// <summary>
  /// Immutable type representing tabular data.
  /// </summary>
  public abstract class TabularData
  {
    #region properties

    /// <summary>
    /// Gets a collection of <c>System.String</c> representing a given row of data.
    /// </summary>
    /// <param name="row">Row.</param>
    public abstract IList<string> this [int row] { get; }

    #endregion

    #region methods

    /// <summary>
    /// Gets the count of rows in the data.
    /// </summary>
    /// <returns>The row count.</returns>
    public abstract int GetRowCount();

    /// <summary>
    /// Gets the count of columns in the data.
    /// </summary>
    /// <returns>The column count.</returns>
    public abstract int GetColumnCount();

    /// <summary>
    /// Gets the data item at the given row and column.
    /// </summary>
    /// <returns>The data.</returns>
    /// <param name="row">Row.</param>
    /// <param name="column">Column.</param>
    public abstract string GetData(int row, int column);

    #endregion
  }
}

