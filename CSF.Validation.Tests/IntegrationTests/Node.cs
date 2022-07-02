using System;
using System.Collections.Generic;

namespace CSF.Validation.IntegrationTests
{
    public class Node
    {
        internal const string ValidName = "FooBarBaz";

        public NodeChild Child { get; set; }

        public string Name { get; set; } = ValidName;

        public Guid Identity { get; set; } = Guid.NewGuid();
    }
}