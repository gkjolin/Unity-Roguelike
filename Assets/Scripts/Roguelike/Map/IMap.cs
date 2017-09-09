using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    public interface IMap
    {
        int Length { get; }
        int Width { get; }
        bool IsWalkable(Coord coord);
        bool IsWall(Coord coord);
    }
}