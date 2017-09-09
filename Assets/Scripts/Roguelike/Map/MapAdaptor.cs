/* This class was written to provide an extension method for Map to convert to a simple interface permitting 
 * walkability queries. Rather than defining a public adaptor that has to be used, this hides the implementation 
 * and allows for a simple map.ToIMap() call to handle the details. */

using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    public static class MapAdaptor
    {
        public static IMap ToIMap(this Map map)
        {
            return new Adaptor(map);
        }

        sealed class Adaptor : IMap
        {
            readonly Map map;

            public int Length { get { return map.Length; } }
            public int Width { get { return map.Width; } }

            public Adaptor(Map map)
            {
                this.map = map;
            }

            public bool IsWalkable(Coord coord)
            {
                return !map.IsWallOrVoid(coord);
            }

            public bool IsWall(Coord coord)
            {
                return 0 <= coord.x && coord.x < Length && 0 <= coord.y && coord.y < Width && map.IsWall(coord);
            }
        }
    } 
}