using System.Collections.Generic;
using TaleScrawl.Core.DungeonScrawl;
using TaleScrawl.Core.Geometry;

namespace TaleScrawl.UnitTests.DungeonScrawl.MapTests
{
    [TestFixture]
    public class DsMapMapperTests
    {
        [Test]
        public void Map_WithPageGrid_ScalesPolygonPointsByCellDiameter()
        {
            DsDocument document = CreateDocument(
                cellDiameter: 36,
                polygon: new List<Point> { new(0, 0), new(144, 0), new(144, 144), new(0, 144) });

            MapModel model = DsMapMapper.Map(document);

            Assert.That(model.CellDiameter, Is.EqualTo(36));
            Assert.That(model.UsedDefaultCellDiameter, Is.False);
            Assert.That(model.Polygons, Has.Count.EqualTo(1));

            IReadOnlyList<Point> contour = model.Polygons[0].Contours[0];
            Assert.That(contour[0], Is.EqualTo(new Point(0, 0)));
            Assert.That(contour[2], Is.EqualTo(new Point(4, 4)));
        }

        [Test]
        public void Map_WithoutGrid_UsesDefaultDiameterAndKeepsPointsUnscaled()
        {
            DsDocument document = new DsDocument
            {
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
                                    new List<Point> { new(0, 0), new(4, 0), new(4, 4), new(0, 4) }
                                }
                            }
                        }
                    }
                }
            };

            MapModel model = DsMapMapper.Map(document);

            Assert.That(model.CellDiameter, Is.EqualTo(1.0));
            Assert.That(model.UsedDefaultCellDiameter, Is.True);
            Assert.That(model.Polygons, Has.Count.EqualTo(1));

            IReadOnlyList<Point> contour = model.Polygons[0].Contours[0];
            Assert.That(contour[2], Is.EqualTo(new Point(4, 4)));
        }

        [Test]
        public void Map_WithEmptyGeometry_ReturnsNoPolygons()
        {
            DsDocument document = new DsDocument
            {
                Data = new DsData { Geometry = new Dictionary<string, DsGeometry>() }
            };

            MapModel model = DsMapMapper.Map(document);

            Assert.That(model.Polygons, Is.Empty);
        }

        private static DsDocument CreateDocument(double cellDiameter, List<Point> polygon)
        {
            return new DsDocument
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
                                Grid = new DsGrid { CellDiameter = cellDiameter }
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
                                new List<List<Point>> { polygon }
                            }
                        }
                    }
                }
            };
        }
    }
}
