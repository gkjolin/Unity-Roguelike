/* This is a port of the recursive shadowcasting algorithm found in
 * http://www.roguebasin.com/index.php?title=Improved_Shadowcasting_in_Java, with some tweaks and a bug fix. 
 * It's still messy for my liking, so I may come back to it later to clean it up. An explanation of the algorithm can
 * be found here: http://www.roguebasin.com/index.php?title=FOV_using_recursive_shadowcasting */

using System;
using System.Collections.Generic;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    public sealed class ShadowCaster
    {
        IMap map;
        int radius;
        int centerX;
        int centerY;

        readonly List<Coord> inFOV = new List<Coord>();

        readonly Coord[] directions = new Coord[]
        {
            new Coord(1,1), new Coord(1,-1), new Coord(-1, 1), new Coord(-1,-1)
        };

        public IEnumerable<Coord> CalculateFOV(IMap map, int centerX, int centerY, int radius)
        {
            if (map == null)
                throw new ArgumentNullException("map");

            this.centerX = centerX;
            this.centerY = centerY;
            this.radius = radius;
            this.map = map;

            inFOV.Clear();
            inFOV.Add(new Coord(centerX, centerY));
            foreach (Coord direction in directions)
            {
                CastLight(1, 1f, 0f, 0, direction.x, direction.y, 0);
                CastLight(1, 1f, 0f, direction.x, 0, 0, direction.y);
            }
            return inFOV;
        }

        void CastLight(int row, float start, float end, int xx, int xy, int yx, int yy)
        {
            float newStart = 0f;
            if (start < end)
            {
                return;
            }
            bool blocked = false;
            for (int distance = row; distance <= radius && !blocked; distance++)
            {
                int deltaY = -distance;
                for (int deltaX = -distance; deltaX <= 0; deltaX++)
                {
                    int currentX = centerX + deltaX * xx + deltaY * xy;
                    int currentY = centerY + deltaX * yx + deltaY * yy;
                    float leftSlope = (deltaX - 0.5f) / (deltaY + 0.5f);
                    float rightSlope = (deltaX + 0.5f) / (deltaY - 0.5f);
                    Coord coord = new Coord(currentX, currentY);

                    if (!(currentX >= 0 && currentY >= 0 && currentX < map.Length && currentY < map.Width) || start < rightSlope)
                    {
                        continue;
                    }
                    else if (end > leftSlope)
                    {
                        break;
                    }

                    if (Math.Sqrt(deltaX * deltaX + deltaY * deltaY) <= radius)
                    {
                        inFOV.Add(coord);
                    }

                    if (blocked)
                    {
                        if (map.IsWall(coord))
                        {
                            newStart = rightSlope;
                            continue;
                        }
                        else
                        {
                            blocked = false;
                            start = newStart;
                        }
                    }
                    else
                    {
                        if (map.IsWall(coord) && distance < radius)
                        {
                            blocked = true;
                            CastLight(distance + 1, start, leftSlope, xx, xy, yx, yy);
                            newStart = rightSlope;
                        }
                    }
                }
            }
        }
    } 
}