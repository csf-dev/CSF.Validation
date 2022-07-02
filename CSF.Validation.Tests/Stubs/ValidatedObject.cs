using System;
using System.Collections.Generic;

namespace CSF.Validation.Stubs
{
    public class ValidatedObject
    {
        public Guid Identity { get; set; } = Guid.NewGuid();

        public string AProperty { get; set; }

        public ICollection<string> Strings { get; set; } = new List<string>();
    }
}