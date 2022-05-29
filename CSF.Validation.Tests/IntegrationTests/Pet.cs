using System;

namespace CSF.Validation.IntegrationTests
{
    public class Pet
    {
        public Guid Identity { get; set; } = Guid.NewGuid();

        public string Name { get; set; }

        public string Type { get; set; }
        
        public int NumberOfLegs { get; set; }

        public int? MinimumAgeToOwn { get; set; }
    }
}