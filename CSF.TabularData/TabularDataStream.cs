//
// TabularDataStream.cs
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
using System.Collections.Generic;

namespace CSF.TabularData
{
  /// <summary>
  /// A data stream for reading tabular data.
  /// </summary>
  public class TabularDataStream : IEnumerable<IList<string>>
  {
    #region fields
    
    private TextReader _reader;
    private ITabularDataParser _parser;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets the parser that this read operation is associated with.
    /// </summary>
    /// <value>
    /// The parser instance.
    /// </value>
    public ITabularDataParser Parser
    {
      get {
        return _parser;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _parser = value;
      }
    }
  
    /// <summary>
    /// Gets the underlying <see cref="TextReader"/> used for the read operation.
    /// </summary>
    /// <value>
    /// The text reader.
    /// </value>
    public TextReader Reader
    {
      get {
        return _reader;
      }
      private set {
        if(value == null)
        {
          throw new ArgumentNullException ("value");
        }
        
        _reader = value;
      }
    }
    
    #endregion
    
    #region methods
      
    /// <summary>
    /// Gets the next row from the underlying stream.
    /// </summary>
    /// <returns>
    /// The next row.
    /// </returns>
    public IEnumerator<IList<string>> GetEnumerator()
    {
      return new TabularDataStreamEnumerator(this);
    }
    
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return this.GetEnumerator();
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.IO.TabularDataStream"/> class.
    /// </summary>
    /// <param name='parser'>
    /// The parser associated with this read operation.
    /// </param>
    /// <param name='reader'>
    /// The underlying <see cref="TextReader"/>.
    /// </param>
    public TabularDataStream(ITabularDataParser parser, TextReader reader)
    {
      this.Parser = parser;
      this.Reader = reader;
    }
    
    #endregion
  }
}

