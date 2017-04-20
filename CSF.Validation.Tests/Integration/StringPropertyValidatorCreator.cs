//
// StandardFluentManifestCreator.cs
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
using CSF.Validation.Manifest.Fluent;

namespace CSF.Validation.Tests.Integration
{
  public class StringPropertyValidatorCreator : IValidatorCreator
  {
    public virtual IValidationManifest CreateManifest()
    {
      var builder = ManifestBuilder.Create<StubValidatedObject>();

      ConfigureManifest(builder);

      return builder.GetManifest();
    }

    protected virtual void ConfigureManifest(IManifestBuilder<StubValidatedObject> builder)
    {
      builder.AddMemberRule(x => x.StringProperty, RuleChooser.NotNull);

      builder.AddMemberRule(x => x.StringProperty, RuleChooser.StringLength, c => {
        c.Configure(r => {
          r.MinLength = 5;
          r.MaxLength = 10;
        });

        c.AddDependency(x => x.StringProperty, RuleChooser.NotNull);
      });
    }

    public virtual IValidator CreateValidator()
    {
      var manifest = CreateManifest();
      var factory = new ValidatorFactory();
      return factory.GetValidator(manifest);
    }
  }
}
