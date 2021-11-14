namespace CSF.Validation.IntegrationTests
{
    public class Pet
    {
        public string Name { get; set; }

        public string Type { get; set; }
        
        public int NumberOfLegs { get; set; }

        public int? MinimumAgeToOwn { get; set; }
    }
}