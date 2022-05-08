using System;
using System.Reflection;
using AutoFixture;
using AutoFixture.Dsl;
using AutoFixture.Kernel;

namespace CSF.Validation
{
    public static class FixtureExtensions
    {
        public static void CustomizeForParameter<T>(this IFixture fixture, ParameterInfo parameter, Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
            => CustomizeForParameter<T>(fixture, parameter.ParameterType, parameter.Name, composerTransformation);

        public static void CustomizeForParameter<T>(this IFixture fixture, Type parameterType, string parameterName, Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation)
        {
            if (fixture is null)
                throw new ArgumentNullException(nameof(fixture));
            if (parameterType is null)
                throw new ArgumentNullException(nameof(parameterType));
            if (parameterName is null)
                throw new ArgumentNullException(nameof(parameterName));
            if (composerTransformation is null)
                throw new ArgumentNullException(nameof(composerTransformation));

            fixture.Customize(WrapWithParameterFilter(composerTransformation, parameterType, parameterName));
        }

        static Func<ICustomizationComposer<T>, ISpecimenBuilder> WrapWithParameterFilter<T>(Func<ICustomizationComposer<T>, ISpecimenBuilder> composerTransformation,
                                                                                            Type parameterType,
                                                                                            string parameterName)
        {
            var spec = new ParameterSpecification(parameterType, parameterName);
            return c => new FilteringSpecimenBuilder(new ParameterToSeededSpecimenBuilderAdapter(composerTransformation(c)), spec);
        }

        class ParameterToSeededSpecimenBuilderAdapter : ISpecimenBuilder
        {
            readonly ISpecimenBuilder wrapped;

            public object Create(object request, ISpecimenContext context)
            {
                if(request is ParameterInfo param)
                    return wrapped.Create(new SeededRequest(param.ParameterType, param.Name), context);
                return new NoSpecimen();
            }

            internal ParameterToSeededSpecimenBuilderAdapter(ISpecimenBuilder wrapped)
            {
                this.wrapped = wrapped ?? throw new ArgumentNullException(nameof(wrapped));
            }
        }
    }
}