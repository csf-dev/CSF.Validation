using System;
using System.Collections.Generic;

namespace CSF.Validation.IntegrationTests
{
    public class Person
    {
        private ICollection<Pet> pets = new List<Pet>();

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public ICollection<Pet> Pets { get; set; }
    }
}