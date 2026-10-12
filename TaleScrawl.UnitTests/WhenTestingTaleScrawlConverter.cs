using TaleScrawl.Core;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.NUnit.Composable;

namespace TaleScrawl.UnitTests
{
    public abstract class WhenTestingTaleScrawlConverter : ComposableTestingTheBehaviourOf
    {
        [ItemUnderTest]
        protected TaleScrawlConverter Converter = null!;
    }
}
