using System.Collections.Generic;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    public interface IPathFinder
    {
        bool FindPath(IMap map, Coord start, Coord end, int maxDistance, List<Coord> path);
    } 
}