//
//  TabularDataBuilder.cs
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
  /// Builder type which creates <see cref="TabularData"/> instance.
  /// </summary>
  public class TabularDataBuilder
  {
    #region fields

    private IList<IList<string>> _data;
    private int _columns;
    private bool _built;

    #endregion

    #region properties

    /// <summary>
    /// Gets the count of columns for the current instance.
    /// </summary>
    /// <value>The columns.</value>
    public int ColumnCount
    {
      get {
        return _columns;
      }
    }

    #endregion

    #region methods

    /// <summary>
    /// Adds a row of data to the current builder instance.
    /// </summary>
    /// <param name="rowData">Row data.</param>
    public void AddRow(IList<string> rowData)
    {
      this.CheckNotBuilt();

      if(rowData == null)
      {
        throw new ArgumentNullException("rowData");
      }
      else if(rowData.Count != this.ColumnCount)
      {
        throw new ArgumentException("Row data must contain the same number of elements as the column count.",
                                    "rowData");
      }

      _data.Add(rowData);
    }

    /// <summary>
    /// Build a <see cref="TabularData"/> from the current builder.
    /// </summary>
    public TabularData Build()
    {
      this.CheckNotBuilt();

      _built = true;
      return new TabularListData(_data);
    }

    /// <summary>
    /// Creates a collection of <c>System.String</c> with a number of elements equal to <see cref="ColumnCount"/>.
    /// </summary>
    /// <returns>The row.</returns>
    public IList<string> CreateRow()
    {
      return new string[this.ColumnCount];
    }

    /// <summary>
    /// Checks that the current instance has not been used already to build a <see cref="TabularData"/>.
    /// </summary>
    private void CheckNotBuilt()
    {
      if(_built)
      {
        string message = String.Format("A `{0}' may be used to build an instance only once.", this.GetType().Name);
        throw new InvalidOperationException(message);
      }
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.IO.TabularDataBuilder"/> class.
    /// </summary>
    /// <param name="columns">Columns.</param>
    public TabularDataBuilder(int columns)
    {
      if(columns < 0)
      {
        throw new ArgumentOutOfRangeException("columns", "Column count must not be negative.");
      }

      _columns = columns;
      _data = new List<IList<string>>();
    }

    #endregion
  }
}

