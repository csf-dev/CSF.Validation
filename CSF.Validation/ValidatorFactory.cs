//
// ValidatorFactory.cs
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
using CSF.Validation.Manifest;
using CSF.Validation.Rules;
using CSF.Validation.ValidationRuns;

namespace CSF.Validation
{
  /// <summary>
  /// Default implementation of <see cref="IValidatorFactory"/>.
  /// </summary>
  public class ValidatorFactory : IValidatorFactory
  {
    readonly IValidationRunFactory runFactory;
    readonly IRuleResolver ruleResolver;
    readonly IManifestIdentityGenerator identityGenerator;
    readonly IValidationRunner validationRunner;

    /// <summary>
    /// Gets a validator instance for the given validation manifest.
    /// </summary>
    /// <returns>The validator.</returns>
    /// <param name="manifest">The validation manifest.</param>
    public virtual IValidator GetValidator(IValidationManifest manifest)
    {
      if(manifest == null)
        throw new ArgumentNullException(nameof(manifest));

      var run = GetValidationRunFactory().CreateRun(manifest);
      var runner = GetValidationRunner();

      return new Validator(run, runner);
    }

    /// <summary>
    /// Gets the validation run factory.
    /// </summary>
    /// <returns>The validation run factory.</returns>
    protected virtual IValidationRunFactory GetValidationRunFactory()
    {
      return runFactory?? new ValidationRunFactory(GetRuleResolver(), GetIdentityGenerator());
    }

    /// <summary>
    /// Gets the rule resolver.
    /// </summary>
    /// <returns>The rule resolver.</returns>
    protected virtual IRuleResolver GetRuleResolver()
    {
      return ruleResolver?? new SimpleRuleResolver();
    }

    /// <summary>
    /// Gets the identity generator.
    /// </summary>
    /// <returns>The identity generator.</returns>
    protected virtual IManifestIdentityGenerator GetIdentityGenerator()
    {
      return identityGenerator?? new DefaultManifestIdentityGenerator();
    }

    /// <summary>
    /// Gets the validation runner.
    /// </summary>
    /// <returns>The validation runner.</returns>
    protected virtual IValidationRunner GetValidationRunner()
    {
      return validationRunner?? new ValidationRunner();
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.ValidatorFactory"/> class.
    /// </summary>
    public ValidatorFactory() {}

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.ValidatorFactory"/> class.
    /// </summary>
    /// <param name="runFactory">Run factory.</param>
    /// <param name="validationRunner">Validation runner.</param>
    public ValidatorFactory(IValidationRunFactory runFactory, IValidationRunner validationRunner)
    {
      this.runFactory = runFactory;
      this.validationRunner = validationRunner;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="T:CSF.Validation.ValidatorFactory"/> class.
    /// </summary>
    /// <param name="ruleResolver">Rule resolver.</param>
    /// <param name="identityGenerator">Identity generator.</param>
    /// <param name="validationRunner">Validation runner.</param>
    public ValidatorFactory(IRuleResolver ruleResolver,
                            IManifestIdentityGenerator identityGenerator,
                            IValidationRunner validationRunner)
    {
      this.ruleResolver = ruleResolver;
      this.identityGenerator = identityGenerator;
      this.validationRunner = validationRunner;
    }
  }
}
