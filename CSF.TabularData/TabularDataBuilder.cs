//
// TabularDataBuilder.cs
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
using System.Collections.Generic;

namespace CSF.TabularData
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

