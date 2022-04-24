using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using CSF.Validation.Rules;
using static CSF.Validation.Rules.CommonResults;

namespace CSF.Validation.IntegrationTests
{
    public class CantBeOwnedByUnderageChildren : IRule<Pet>
    {
        public Task<RuleResult> GetResultAsync(Pet validated, RuleContext context, CancellationToken token = default)
        {
            if(validated?.MinimumAgeToOwn.HasValue != true) return PassAsync();

            var owner = (Person) context.AncestorContexts.FirstOrDefault(x => x.ActualValue is Person)?.ActualValue;
            if(owner is null) throw new ArgumentException("The context must indicate an owner.", nameof(context));

            var ownerAge = GetAgeInYears(owner.Birthday);
            return ownerAge < validated.MinimumAgeToOwn.Value ? FailAsync() : PassAsync();
        }

        /// <summary>
        /// Algorithm to get a person's age in years copied directly from
        /// https://stackoverflow.com/a/1404/6221779
        /// </summary>
        /// <param name="birthdate">The person's date of birth</param>
        /// <returns>The person's age in years</returns>
        static int GetAgeInYears(DateTime birthdate)
        {
            // Save today's date.
            var today = DateTime.Today;

            // Calculate the age.
            var age = today.Year - birthdate.Year;

            // Go back to the year in which the person was born in case of a leap year
            if (birthdate.Date > today.AddYears(-age)) age--;

            return age;
        }
    }
}