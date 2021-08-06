using System;
using System.Collections.Generic;

namespace CSF.Validation.Stubs
{
    public class ComplexObject
    {
        public Guid Identity { get; } = Guid.NewGuid();

        public ComplexObject Associated { get; set; }

        public ICollection<ComplexObject> Children { get; } = new List<ComplexObject>();

        public string StringProperty { get; set; }

        public int IntegerMethod() => StringProperty?.Length ?? 0;
    }
}