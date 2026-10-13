using System.IO;
using TaleScrawl.Core.DungeonScrawl;
using LeapingGorilla.Testing.Core.Attributes;

namespace TaleScrawl.UnitTests.DungeonScrawl.ReadTests
{
    public abstract class WhenTestingRead : WhenTestingDsFileReader
    {
        protected Stream Input = null!;
        protected DsDocument Result = null!;

        [When(DoNotRethrowExceptions: true)]
        public void ReadIsCalled()
        {
            Result = Reader.Read(Input);
        }
    }
}
