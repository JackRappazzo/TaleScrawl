using TaleScrawl.Core.DungeonScrawl;
using TaleSpire.Slab.V2;
using LeapingGorilla.Testing.Core.Attributes;

namespace TaleScrawl.UnitTests.ConvertTests
{
    public abstract class WhenTestingBuildSlab : WhenTestingTaleScrawlConverter
    {
        protected DsDocument Document = null!;
        protected Slab Result = null!;

        [When(DoNotRethrowExceptions: true)]
        public void BuildSlabIsCalled()
        {
            Result = Converter.BuildSlab(Document);
        }
    }
}
