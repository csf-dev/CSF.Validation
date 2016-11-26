//
// ValidationTests.cs
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
using System.Text.RegularExpressions;
using System.Net.Mail;

namespace CSF.Validation
{
  /// <summary>
  /// Helper type containing pre-written validation tests.
  /// </summary>
  public static class ValidationTests
  {
    #region fields
    
    /// <summary>
    /// Gets validation tests that may be performed on <see cref="System.String"/> members.
    /// </summary>
    public static readonly StringValidationTests String = new StringValidationTests();
    
    #endregion
    
    #region contained types
    
    /// <summary>
    /// String validation tests.
    /// </summary>
    public class StringValidationTests
    {
      private const string EMAIL_PATTERN = @"^[a-zA-Z0-9_+%-]+([a-zA-Z0-9._%+-]+[a-zA-Z0-9_+%-]+)?@([a-zA-Z0-9-]+\.)+[a-zA-Z0-9]{1,4}$";
      private static readonly Regex EmailAddressRegex = new Regex(EMAIL_PATTERN, RegexOptions.Compiled);
      
      /// <summary>
      /// Pre-written validation test for validating <see cref="System.String"/> email addresses.
      /// </summary>
      /// <value>
      /// A delegate that validates email addresses.
      /// </value>
      public ValidationFunction<string> EmailAddress
      {
        get {
          return (string x) => {
            bool output;
            
            if(x == null)
            {
              output = false;
            }
            else
            {
              try
              {
                /* Make an attempt to construct a MailAddress instance - if this throws an exception then it can't
                 * possibly be valid!
                 */
                new MailAddress(x);
                output = EmailAddressRegex.IsMatch(x);
              }
              catch(ArgumentException)
              {
                output = false;
              }
              catch(FormatException)
              {
                output = false;
              }
            }
            
            return output;
          };
        }
      }
    }
    
    #endregion  
  }
}

