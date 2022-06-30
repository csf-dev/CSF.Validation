using System;
using System.Collections.Generic;

namespace CSF.Validation.IntegrationTests
{
    public class NodeChild
    {
        public Node Node { get; set; }

        public Guid Identity { get; set; } = Guid.NewGuid();
    }
}