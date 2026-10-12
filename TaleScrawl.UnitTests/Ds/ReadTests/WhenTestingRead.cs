using System.IO;
using TaleScrawl.Core.Ds;
using LeapingGorilla.Testing.Core.Attributes;

namespace TaleScrawl.UnitTests.Ds.ReadTests
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
