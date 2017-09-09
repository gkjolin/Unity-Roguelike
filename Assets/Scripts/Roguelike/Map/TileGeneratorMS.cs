/* This is a very specific way of laying out sprite tiles. It is based on marching squares triangulation.
 * This means that instead of placing a tile on each point in the map, a tile is placed for each group of four adjacent map
 * points. Given four map points, there are sixteen possible configurations based on whether those points are walls or floors.
 * For this generator to work, it must use a mesh renderer with a sprite material with a very specific sprite 
 * format: it must be 1 tile high, and 6 tiles across. The first tile represents a tile where all four map points are walls.
 * The second tile has walls at top left, top right, and bot left. The third has wall tiles at top left and bot right.
 * The fourth is empty (four floor tiles). The fifth has a wall tile at bottom right. The sixth has wall tiles at top left
 * and top right. 
 *
 * The rest of the configurations are inferred by rotating/flipping the UV coordinates associated with the above tiles. 
 *
 * In hindsight, it would be better to take individual tiles as constructor arguments (or exposed fields in the inspector)
 * and reconstruct the required texture algorithmically. That would be far easier to use, and the required algorithm
 * is not complex.*/

using UnityEngine;
using AKSaigyouji.Maps;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AKSaigyouji.Roguelike
{
    /// <summary>
    /// Arranges 16 types of tiles based on marching square configurations.
    /// </summary>
    public sealed class TileGeneratorMS : MonoBehaviour
    {
        public MeshFilter MeshFilter { get { return meshFilter; } }
        [SerializeField] MeshFilter meshFilter;

        List<Vector3> vertices = new List<Vector3>();
        List<int> triangles = new List<int>();
        List<Vector2> uvs = new List<Vector2>();

        public void Generate(Map map)
        {
            Generate(map, (x, y) => true);
        }

        public void Generate(Map map, Func<int, int, bool> mask)
        {
            if (map == null)
                throw new ArgumentNullException("map");

            if (mask == null)
                throw new ArgumentNullException("mask");

            foreach (Transform child in transform.Cast<Transform>().ToArray())
            {
                Destroy(child.gameObject);
            }
            var mesh = new Mesh();
            vertices.Clear();
            triangles.Clear();
            uvs.Clear();

            int vIndex = 0;

            for (int y = 0; y < map.Width - 1; y++)
            {
                for (int x = 0; x < map.Length - 1; x++)
                {
                    if (mask(x, y))
                    {
                        int botLeft = (int)map[x, y];
                        int botRight = (int)map[x + 1, y];
                        int topLeft = (int)map[x, y + 1];
                        int topRight = (int)map[x + 1, y + 1];
                        int configuration = botLeft + 2 * botRight + 4 * topRight + 8 * topLeft;

                        vertices.Add(new Vector3(x, y));
                        vertices.Add(new Vector3(x + 1, y));
                        vertices.Add(new Vector3(x + 1, y + 1));
                        vertices.Add(new Vector3(x, y + 1));

                        triangles.Add(vIndex);
                        triangles.Add(vIndex + 2);
                        triangles.Add(vIndex + 1);
                        triangles.Add(vIndex);
                        triangles.Add(vIndex + 3);
                        triangles.Add(vIndex + 2);

                        InsertUV(uvs, configuration);

                        vIndex += 4;
                    }
                }
            }

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);
            mesh.SetUVs(0, uvs);
            MeshFilter.mesh = mesh;
        }

        int tile = 0; // this determines which of the six tiles is chosen.
        float X { get { return tile / 6f; } } 
        const float X_DELTA = 1 / 6f;
        Vector2 BL { get { return new Vector2(X, 0); } }
        Vector2 BR { get { return new Vector2(X + X_DELTA, 0); } }
        Vector2 TR { get { return new Vector2(X + X_DELTA, 1); } }
        Vector2 TL { get { return new Vector2(X, 1); } }

        /// <summary>
        /// The xth tile.
        /// </summary>
        void Square(List<Vector2> uv)
        {
            uv.Add(BL); uv.Add(BR); uv.Add(TR); uv.Add(TL);
        }
        
        /// <summary>
        /// The xth tile rotated 90 degrees.
        /// </summary>
        void Square90(List<Vector2> uv)
        {
            uv.Add(TL); uv.Add(BL); uv.Add(BR); uv.Add(TR);
        }

        /// <summary>
        /// The xth tile rotated 180 degrees.
        /// </summary>
        void Square180(List<Vector2> uv)
        {
            uv.Add(TR); uv.Add(TL); uv.Add(BL); uv.Add(BR);
        }

        /// <summary>
        /// The xth tile rotated 270 degrees.
        /// </summary>
        void Square270(List<Vector2> uv)
        {
            uv.Add(BR); uv.Add(TR); uv.Add(TL); uv.Add(BL);
        }

        void InsertUV(List<Vector2> uv, int configuration)
        {
            switch (configuration)
            {
                // In each case, we establish which tile we want to use, then choose a suitable rotation of it.
                // Note that the case number corresponds to which four of the corners are walls, according to this
                // formula: 8 * topleft + 4 * topright + 2 * botright + 1 * botleft, where each corner is 1 (wall)
                // or 0 (floor).

                // A simpler approach would be to simply draw 16 tiles to handle each configuration, so as to not
                // worry about rotations at all. That has the benefit of being able to use different tiles for e.g. 
                // a single topright corner vs a single botright corner. 
                case 0:
                    tile = 3;
                    Square(uv);
                    break;
                case 1:
                    tile = 4;
                    Square270(uv);
                    break;
                case 2:
                    tile = 4;
                    Square(uv);
                    break;
                case 3:
                    tile = 5;
                    Square180(uv);
                    break;
                case 4:
                    tile = 4;
                    Square90(uv);
                    break;
                case 5:
                    tile = 2;
                    Square90(uv);
                    break;
                case 6:
                    tile = 5;
                    Square270(uv);
                    break;
                case 7:
                    tile = 1;
                    Square180(uv);
                    break;
                case 8:
                    tile = 4;
                    Square180(uv);
                    break;
                case 9:
                    tile = 5;
                    Square90(uv);
                    break;
                case 10:
                    tile = 2;
                    Square(uv);
                    break;
                case 11:
                    tile = 1;
                    Square90(uv);
                    break;
                case 12:
                    tile = 5;
                    Square(uv);
                    break;
                case 13:
                    tile = 1;
                    Square(uv);
                    break;
                case 14:
                    tile = 1;
                    Square270(uv);
                    break;
                case 15:
                    tile = 0;
                    Square(uv);
                    break;
                default:
                    break;
            }
        }
    } 
}