using TaleScrawl.Core.Ds;
using LeapingGorilla.Testing.Core.Attributes;

namespace TaleScrawl.UnitTests.ConvertTests
{
    public abstract class WhenTestingConvert : WhenTestingTaleScrawlConverter
    {
        protected DsDocument Document = null!;
        protected string Result = null!;

        [When]
        public void ConvertIsCalled()
        {
            Result = Converter.Convert(Document);
        }
    }
}
