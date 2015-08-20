//  
//  TabularDataFormat.cs
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
using System.Collections.Generic;
using System.Linq;

namespace CSF.IO
{
  /// <summary>
  /// Immutable type describes the formatting rules for a tabular data format.
  /// </summary>
  public class TabularDataFormat
  {
    #region constants
    
    private char[] Whitespace = new char[] { ' ', '\t' };
    
    #endregion
    
    #region fields
    
    private char _columnDelimiter;
    private Nullable<char> _quotationCharacter, _quotationEscapeCharacter;
    private string _rowDelimiter;
    private TabularDataWriteOptions _defaultWriteOptions;
    private ISet<char> _disallowedCharacters;
    private bool _trimWhitespace;

    private IList<char> CachedCharactersRequiringQuotation;
    
    #endregion
    
    #region properties
  
    /// <summary>
    /// Gets or sets the column delimiter character.
    /// </summary>
    /// <value>
    /// The column delimiter.
    /// </value>
    public virtual char ColumnDelimiter
    {
      get {
        return _columnDelimiter;
      }
    }
  
    /// <summary>
    /// Gets or sets the row delimiter (to indicate the end of a row and the beginning of the next).
    /// </summary>
    /// <value>
    /// The row delimiter.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    /// <exception cref='ArgumentException'>
    /// Is thrown when an argument passed to a method is invalid.
    /// </exception>
    public virtual string RowDelimiter
    {
      get {
        return _rowDelimiter;
      }
    }
  
    /// <summary>
    /// Gets or sets an optional character used to quote data values.
    /// </summary>
    /// <value>
    /// The quotation character.
    /// </value>
    public virtual Nullable<char> QuotationCharacter
    {
      get {
        return _quotationCharacter;
      }
    }
  
    /// <summary>
    /// Gets or sets an optional character used to escape quotation characters (to represent the quotation character
    /// inside data values).
    /// </summary>
    /// <value>
    /// The quotation escape character.
    /// </value>
    public virtual Nullable<char> QuotationEscapeCharacter
    {
      get {
        return _quotationEscapeCharacter;
      }
    }
  
    /// <summary>
    /// Gets or sets the default write options.
    /// </summary>
    /// <value>
    /// The default write options.
    /// </value>
    public virtual TabularDataWriteOptions DefaultWriteOptions
    {
      get {
        return _defaultWriteOptions;
      }
    }
  
    /// <summary>
    /// Gets or sets a collection of the characters that are disallowed in data values.
    /// </summary>
    /// <value>
    /// The disallowed characters.
    /// </value>
    /// <exception cref='ArgumentNullException'>
    /// Is thrown when an argument passed to a method is invalid because it is <see langword="null" /> .
    /// </exception>
    public virtual IEnumerable<char> DisallowedCharacters
    {
      get {
        return _disallowedCharacters.AsEnumerable();
      }
    }
  
    /// <summary>
    /// Gets or sets a value indicating whether unquoted leading/trailing whitespace should be trimmed from data when
    /// reading.
    /// </summary>
    /// <value>
    /// <c>true</c> if unquoted whitespace should be trimmed; otherwise, <c>false</c>.
    /// </value>
    public virtual bool TrimWhitespace
    {
      get {
        return _trimWhitespace;
      }
    }
    
    #endregion
    
    #region methods
    
    /// <summary>
    /// Determines whether the specified character is disallowed.
    /// </summary>
    /// <returns>
    /// <c>true</c> if the specified character is disallowed; otherwise, <c>false</c>.
    /// </returns>
    /// <param name='character'>
    /// The character to test.
    /// </param>
    public virtual bool IsDisallowed(char character)
    {
      return this.DisallowedCharacters.Contains(character);
    }
    
    /// <summary>
    /// Gets a collection of the characters requiring quotation.
    /// </summary>
    /// <returns>
    /// The characters requiring quotation.
    /// </returns>
    public virtual IList<char> GetCharactersRequiringQuotation()
    {
      if(CachedCharactersRequiringQuotation == null)
      {
        CachedCharactersRequiringQuotation = new List<char>();
        
        // The column delimiter needs quoting if it is not disallowed
        if(!this.IsDisallowed(this.ColumnDelimiter))
        {
          CachedCharactersRequiringQuotation.Add(this.ColumnDelimiter);
        }
        
        // The characters that form the row delimiter need quoting if they are not disallowed
        foreach(char character in this.RowDelimiter.ToCharArray())
        {
          if(!this.IsDisallowed(character))
          {
            CachedCharactersRequiringQuotation.Add(character);
          }
        }
        
        // The quotation character needs quoting if it is not disallowed
        if(this.QuotationCharacter.HasValue
           && !this.IsDisallowed(this.QuotationCharacter.Value))
        {
          CachedCharactersRequiringQuotation.Add(this.QuotationCharacter.Value);
        }
        
        // Whitespace characters need quoting if they are not disallowed
        if(this.TrimWhitespace)
        {
          foreach(char character in Whitespace)
          {
            if(!this.IsDisallowed(character))
            {
              CachedCharactersRequiringQuotation.Add(character);
            }
          }
        }
      }
      
      return CachedCharactersRequiringQuotation;
    }
    
    /// <summary>
    /// Determines whether the given <paramref name="value"/> should be quoted when writing data.
    /// </summary>
    /// <returns>
    /// Whether or not to quote the value
    /// </returns>
    /// <param name='value'>
    /// The value to test against.
    /// </param>
    /// <param name='additionalOptions'>
    /// Additional options for writing data.
    /// </param>
    public virtual bool QuoteWhenWriting(string value, TabularDataWriteOptions additionalOptions)
    {
      bool output = false;
      TabularDataWriteOptions effectiveOptions = this.Merge(this.DefaultWriteOptions, additionalOptions);
      
      if(value == null)
      {
        throw new ArgumentNullException ("value");
      }
      
      if((effectiveOptions & TabularDataWriteOptions.AlwaysQuote) == TabularDataWriteOptions.AlwaysQuote)
      {
        // We are configured to always quote output
        output = true;
      }
      else if(this.TrimWhitespace
              && value.Trim() != value)
      {
        output = true;
      }
      else if(this.GetCharactersRequiringQuotation().Intersect(value.ToCharArray()).Count() > 0)
      {
        // The value contains one or more characters that require quotation
        output = true;
      }
      
      return output;
    }
    
    /// <summary>
    /// Merge the two sets of <see cref="TabularDataWriteOptions"/> together to produce a combined set of options.
    /// </summary>
    /// <param name='first'>
    /// The first set of options.
    /// </param>
    /// <param name='second'>
    /// The second set of options.
    /// </param>
    public virtual TabularDataWriteOptions Merge(TabularDataWriteOptions first, TabularDataWriteOptions second)
    {
      return first | second;
    }
    
    /// <summary>
    /// Invariant method checks for validity conditions within the current instance and enforces them.
    /// </summary>
    /// <exception cref='InvalidOperationException'>
    /// Is thrown when an operation cannot be performed.
    /// </exception>
    protected virtual void EnsureValidity()
    {
      if(this.QuotationCharacter.HasValue)
      {
        if(!this.IsDisallowed(this.QuotationCharacter.Value) && !this.QuotationEscapeCharacter.HasValue)
        {
          throw new InvalidOperationException("The current instance is invalid, if a quotation character is defined " +
                                              "then it must have either an escape character also defined or it must " +
                                              "be disallowed.");
        }
      }
    }
    
    #endregion
    
    #region constructor
    
    /// <summary>
    /// Initializes a new instance of the <see cref="CSF.IO.TabularDataFormat"/> class.
    /// </summary>
    /// <param name="rowDelimiter">Row delimiter.</param>
    /// <param name="columnDelimiter">Column delimiter.</param>
    /// <param name="quotationCharacter">Quotation character.</param>
    /// <param name="quotationEscapeCharacter">Quotation escape character.</param>
    /// <param name="defaultWriteOptions">Default write options.</param>
    /// <param name="disallowedCharacters">Disallowed characters.</param>
    /// <param name="trimWhitespace">If set to <c>true</c> trim whitespace.</param>
    public TabularDataFormat(string rowDelimiter,
                             char columnDelimiter = '\t',
                             char? quotationCharacter = null,
                             char? quotationEscapeCharacter = null,
                             TabularDataWriteOptions defaultWriteOptions = TabularDataWriteOptions.None,
                             ISet<char> disallowedCharacters = null,
                             bool trimWhitespace = false)
    {
      if(rowDelimiter == null)
      {
        throw new ArgumentNullException ("rowDelimiter");
      }
      else if(rowDelimiter.Length < 1 || rowDelimiter.Length > 2)
      {
        throw new ArgumentException("Row delimiter must be precisely 1 or two characters.", "rowDelimiter");
      }

      _rowDelimiter = rowDelimiter;
      _columnDelimiter = columnDelimiter;
      _quotationCharacter = quotationCharacter;
      _quotationEscapeCharacter = quotationEscapeCharacter;
      _defaultWriteOptions = defaultWriteOptions;
      _disallowedCharacters = disallowedCharacters?? new HashSet<char>();
      _trimWhitespace = trimWhitespace;

      this.EnsureValidity();
    }
    
    #endregion
    
    #region static properties
    
    /// <summary>
    /// Gets a <see cref="TabularDataFormat"/> instance that represents the popular CSV data format.
    /// </summary>
    /// <value>
    /// A format instance for CSV data.
    /// </value>
    [Obsolete("Instead, use the CreateCsv method, which permits passing of tolerance options")]
    public static TabularDataFormat Csv
    {
      get {
        return CreateCsv();
      }
    }
    
    /// <summary>
    /// Gets a <see cref="TabularDataFormat"/> instance that represents the popular TSV data format.
    /// </summary>
    /// <value>
    /// A format instance for TSV data.
    /// </value>
    [Obsolete("Instead, use the CreateTsv method, which permits passing of tolerance options")]
    public static TabularDataFormat Tsv
    {
      get {
        return CreateTsv();
      }
    }
    
    #endregion

    #region static methods

    /// <summary>
    /// Creates a <see cref="TabularDataFormat"/> representing the CSV specification.
    /// </summary>
    /// <returns>The format instance.</returns>
    /// <param name="tolerateEmptyRows"><c>true</c> if empty lines are to be tolerated; <c>false</c> otherwise.</param>
    public static TabularDataFormat CreateCsv(bool tolerateEmptyRows = false)
    {
      return new TabularDataFormat(rowDelimiter: "\r\n",
                                   columnDelimiter: ',',
                                   quotationCharacter: '"',
                                   quotationEscapeCharacter: '"',
                                   trimWhitespace: true);
    }

    /// <summary>
    /// Creates a <see cref="TabularDataFormat"/> representing the TSV specification.
    /// </summary>
    /// <returns>The format instance.</returns>
    public static TabularDataFormat CreateTsv()
    {
      return new TabularDataFormat(rowDelimiter: Environment.NewLine,
                                   columnDelimiter: '\t',
                                   disallowedCharacters: new HashSet<char>(new [] { '\t', '\n' }));
    }

    #endregion
  }
}

