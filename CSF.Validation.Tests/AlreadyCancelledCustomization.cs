using System.Threading;
using AutoFixture;

namespace CSF.Validation
{
    public class AlreadyCancelledCustomization : ICustomization
    {
        public void Customize(IFixture fixture)
        {
            fixture.Customize<CancellationToken>(c => c.FromFactory(() =>
            {
                var source = new CancellationTokenSource();
                source.Cancel();
                return source.Token;
            }));
        }
    }
}