//
// OutcomeAssert.cs
//
// Author:
//       Craig Fowler <craig@csf-dev.com>
//
// Copyright (c) 2017 Craig Fowler
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
using CSF.Validation.Rules;
using NUnit.Framework;

namespace Test.CSF
{
  public class OutcomeAssert
  {
    public static void IsSuccess(RuleOutcome outcome)
    {
      IsExpected(outcome, x => x == RuleOutcome.Success);
    }

    public static void IsFailure(RuleOutcome outcome)
    {
      IsExpected(outcome, x => x == RuleOutcome.Failure);
    }

    public static void IsError(RuleOutcome outcome)
    {
      IsExpected(outcome, x => x == RuleOutcome.Error);
    }

    public static void IsSuccess(IRuleResult result)
    {
      Assert.NotNull(result);
      IsSuccess(result.Outcome);
    }

    public static void IsFailure(IRuleResult result)
    {
      Assert.NotNull(result);
      IsFailure(result.Outcome);
    }

    public static void IsError(IRuleResult result)
    {
      Assert.NotNull(result);
      IsError(result.Outcome);
    }

    private static void IsExpected(RuleOutcome actualOutcome, Func<RuleOutcome,bool> predicate)
    {
      if(predicate == null)
      {
        throw new ArgumentNullException(nameof(predicate));
      }

      Assert.That(predicate(actualOutcome), $"Actual outcome: {actualOutcome}, was not as expected");
    }
  }
}
