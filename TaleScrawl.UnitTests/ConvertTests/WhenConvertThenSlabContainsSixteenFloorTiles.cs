using System;
using System.Collections.Generic;
using TaleScrawl.Core.Ds;
using TaleScrawl.Core.Geometry;
using TaleScrawl.Core.Slab;
using LeapingGorilla.Testing.Core.Attributes;
using LeapingGorilla.Testing.Core.Composable;
using LeapingGorilla.Testing.NUnit.Attributes;

namespace TaleScrawl.UnitTests.ConvertTests
{
    public class WhenConvertThenSlabContainsSixteenFloorTiles : WhenTestingConvert
    {
        protected override ComposedTest ComposeTest() => TestComposer
            .Given(ADocumentWithOneFourByFourRoomIsProvided)
            .When(ConvertIsCalled)
            .Then(ShouldContainSixteenFloorTiles);

        [Given]
        public void ADocumentWithOneFourByFourRoomIsProvided()
        {
            Document = new DsDocument
            {
                State = new DsState
                {
                    Document = new DsDocumentState
                    {
                        Nodes = new Dictionary<string, DsNode>
                        {
                            ["document"] = new DsNode
                            {
                                Type = "DOCUMENT",
                                Children = new List<string> { "page1" }
                            },
                            ["page1"] = new DsNode
                            {
                                Type = "PAGE",
                                Grid = new DsGrid { CellDiameter = 36 }
                            }
                        }
                    }
                },
                Data = new DsData
                {
                    Geometry = new Dictionary<string, DsGeometry>
                    {
                        ["g1"] = new DsGeometry
                        {
                            Polygons = new List<List<List<Point>>>
                            {
                                new List<List<Point>>
                                {
                                    new List<Point>
                                    {
                                        new(0, 0), new(144, 0), new(144, 144), new(0, 144)
                                    }
                                }
                            }
                        }
                    }
                }
            };
        }

        [Then]
        public void ShouldContainSixteenFloorTiles()
        {
            var decoder = new SlabDecoder();
            IReadOnlyList<SlabLayout> layouts = decoder.Decode(Result);

            Assert.That(layouts, Has.Count.EqualTo(1));
            Assert.That(layouts[0].AssetKindId, Is.EqualTo(Guid.Parse("cf6063bb-5c6e-4107-b3e9-9c0c5ac75768")));
            Assert.That(layouts[0].Assets, Has.Count.EqualTo(16));
        }
    }
}
