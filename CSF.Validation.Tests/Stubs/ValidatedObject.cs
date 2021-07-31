using System;

namespace CSF.Validation.Stubs
{
    public class ValidatedObject
    {
        public Guid Identity { get; set; } = Guid.NewGuid();

        public string AProperty { get; set; }
    }
}