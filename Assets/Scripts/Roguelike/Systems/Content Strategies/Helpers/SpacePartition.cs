/* When placing content randomly, we want to be careful about clustering too much content together, particularly
 * when it comes to enemies, for balance reasons. A naive approach would be to compare an object's location with
 * the location of all objects placed so far, but this has quadratic run-time in the number of objects. Partitioning
 * space into chunks offers a more efficient approach, and the following is a very simple implementation that 
 * will suffice for our purposes. Space is divided into chunks of a given size, and we keep track of whether a
 * given cell has an object.*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using AKSaigyouji.ArrayExtensions;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Represents a partition of a map into cells of a given uniform size.
    /// </summary>
    public sealed class SpacePartition
    {
        readonly int cellSize;
        readonly bool[,] content;

        public SpacePartition(int cellSize, int length, int width)
        {
            this.cellSize = cellSize;
            content = new bool[1 + length / cellSize, 1 + width / cellSize];
        }

        /// <summary>
        /// Does the cell containing this position already contain an object?
        /// </summary>
        public bool ContainsObject(int x, int y)
        {
            int xCell = GetCellCoordinate(x);
            int yCell = GetCellCoordinate(y);
            return content[xCell, yCell];
        }

        /// <summary>
        /// Do any cells adjacent to the cell containing this position already contain an object? Note that this includes
        /// both the cell containing this position, as well as diagonally adjacent cells. i.e. if cell (a,b) contains 
        /// position (x,y), then this will return true if any (a+i,b+j) contains an object, for i,j in {-1,0,1}.
        /// </summary>
        public bool IsAdjacentToObject(int x, int y)
        {
            int xCell = GetCellCoordinate(x);
            int yCell = GetCellCoordinate(y);
            for (int j = yCell - 1; j <= yCell + 1; j++)
            {
                for (int i = xCell - 1; i <= xCell + 1; i++)
                {
                    if (content.AreValidCoords(i, j) && content[i, j])
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void PlaceObject(int x, int y)
        {
            int xCell = GetCellCoordinate(x);
            int yCell = GetCellCoordinate(y);
            Assert.IsFalse(content[xCell, yCell]);
            content[xCell, yCell] = true;
        }

        int GetCellCoordinate(int coordinate)
        {
            return coordinate / cellSize;
        }
    } 
}