using System.Collections.Generic;
using TaleScrawl.Core.DungeonScrawl;
using TaleScrawl.Core.Geometry;

namespace TaleScrawl.UnitTests
{
    public static class TestDocuments
    {
        /// <summary>A single 4x4 room drawn in DungeonScrawl units (0..144), scaled down by the cell diameter.</summary>
        public static DsDocument FourByFourRoom(double cellDiameter = 36)
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

        public static DsDocument Empty()
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
                                Grid = new DsGrid { CellDiameter = 36 }
                            }
                        }
                    }
                },
                Data = new DsData
                {
                    Geometry = new Dictionary<string, DsGeometry>()
                }
            };
        }
    }
}
