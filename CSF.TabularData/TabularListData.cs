//
// TabularListData.cs
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
  /// Immutable type representing a <see cref="TabularData"/> built from a jagged array.
  /// </summary>
  public class TabularListData : TabularData
  {
    #region fields

    private IList<IList<string>> _data;

    #endregion

    #region properties

    /// <summary>
    /// Gets a collection of <c>System.String</c> representing a given row of data.
    /// </summary>
    /// <param name="row">Row.</param>
    public override IList<string> this [int row]
    {
      get {
        return _data[row];
      }
    }

    #endregion

    #region implemented abstract members of TabularData

    /// <summary>
    /// Gets the count of rows in the data.
    /// </summary>
    /// <returns>The row count.</returns>
    public override int GetRowCount()
    {
      return _data.Count;
    }

    /// <summary>
    /// Gets the count of columns in the data.
    /// </summary>
    /// <returns>The column count.</returns>
    public override int GetColumnCount()
    {
      return (this.GetRowCount() > 0)? _data[0].Count : 0;
    }

    /// <summary>
    /// Gets the data item at the given row and column.
    /// </summary>
    /// <returns>The data.</returns>
    /// <param name="row">Row.</param>
    /// <param name="column">Column.</param>
    public override string GetData(int row, int column)
    {
      return _data[row][column];
    }

    #endregion

    #region constructor

    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.IO.TabularArrayData"/> class.
    /// </summary>
    /// <param name="data">Data.</param>
    public TabularListData(IList<IList<string>> data)
    {
      if(data == null)
      {
        throw new ArgumentNullException("data");
      }

      _data = data;
    }

    #endregion
  }
}

