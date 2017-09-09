/* Field of view is probably the most technical component of a traditional roguelike, and also one of the most important
 * in terms of replicating the roguelike 'feel'. The fact that we're using a mesh instead of a collection of tiles further
 * complicates the matter as we cannot hide/disable individual tiles by altering an individual gameobject or sprite 
 * renderer, and rebuilding a large mesh many times a second would be prohitively expensive. We handle this problem
 * by removing all triangles from the mesh, then adding them in as the player explores. We cache a lot of information
 * to ensure we perform these operations only when we actually need to, as they are still performance-heavy.
 * 
 * The algorithm to determine which tiles are in the field of view is done via recursive shadowcasting, and is handled
 * by the ShadowCaster class. See the top-level comments in that class for more information.*/

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using AKSaigyouji.Maps;

namespace AKSaigyouji.Roguelike
{
    public sealed class FieldOfView : GameBehaviour
    {
        [SerializeField] int range;
        [SerializeField] Transform player;
        [SerializeField] MeshFilter meshFilter;
        [SerializeField] Transform enemyAnchor;
        [SerializeField] MapFilter mapFilter;

        Mesh Mesh { get { return meshFilter.mesh; } }
        Vector2 PlayerPosition { get { return player.position; } set { player.position = value; } }

        IMap Map { get { return mapFilter.Map; } }

        readonly List<int> triangles = new List<int>();

        readonly ShadowCaster shadowCaster = new ShadowCaster();

        readonly Dictionary<Coord, int> indices = new Dictionary<Coord, int>(); // Indices into the mesh's vertices array
        readonly HashSet<Coord> revealedLocations = new HashSet<Coord>(); // These locations already added to the mesh
        readonly HashSet<Coord> previousLocations = new HashSet<Coord>(); // Don't need to update mesh if we step into one of these
        readonly HashSet<Coord> inFOVLocations = new HashSet<Coord>(); // Current line of sight

        Vector2 lastPosition = new Vector2(-1, -1);

        bool updateFOV = false;

        void LateUpdate()
        {
            if (updateFOV)
            {
                updateFOV = false;
                if (PlayerPosition != lastPosition) // Don't update map visibility if the player hasn't moved.
                {
                    lastPosition = PlayerPosition;
                    Coord positionAsCoord = (Coord)PlayerPosition;
                    if (!previousLocations.Contains(positionAsCoord)) // Update if the player moved to a new location
                    {
                        UpdateMapVisibility(positionAsCoord);
                    }
                }
                UpdateMonsterVisibility(); // Update monster visibility every time game time changes
            }
        }

        protected override void OnPlayerAction()
        {
            // The FOV is updated only when the game time changes, since the time changing is a necessary
            // condition for the FOV to become out of date. Note that we don't immediately update the FOV, but
            // rather we simply set this flag to true. Then, in the LateUpdate method, we update the FOV if this
            // flag is set. The reason for this is to ensure that the turn is 'over' before redrawing the FOV. 
            // In other words, we're avoiding a race condition between updating the FOV and between in-game events. 
            // LateUpdate doesn't get called until all the update methods are finished for a given turn/frame, 
            // making it the appropriate place to update visual information for rendering purposes.
            updateFOV = true;
        }

        protected override void OnNewMapGenerated()
        {
            lastPosition = new Vector2(-1, -1);
            indices.Clear();
            previousLocations.Clear();
            revealedLocations.Clear();
            triangles.Clear();
            // We map vertices to their index in the mesh's vertices array. When we reveal a square at a given position,
            // we need to rebuild two triangles corresponding to four consecutive vertices. This initialization step
            // allows us to do so very quickly
            Vector3[] vertices = Mesh.vertices;
            for (int i = 0; i < vertices.Length; i += 4)
            {
                indices[(Coord)vertices[i]] = i;
            }
            updateFOV = true;
        }

        void UpdateMonsterVisibility()
        {
            foreach (Transform enemy in enemyAnchor)
            {
                Coord position = (Coord)enemy.position;
                enemy.GetComponent<SpriteRenderer>().enabled = inFOVLocations.Contains(position);
            }
        }

        void UpdateMapVisibility(Coord center)
        {
            inFOVLocations.Clear();
            foreach (Coord coord in shadowCaster.CalculateFOV(Map, center.x, center.y, range))
            {
                inFOVLocations.Add(coord);
                if (indices.ContainsKey(coord) && !revealedLocations.Contains(coord))
                {
                    RevealSquare(coord);
                    revealedLocations.Add(coord);                }
            }
            Mesh.SetTriangles(triangles, 0);
        }
        
        void RevealSquare(Coord location)
        {
            // for this to work, the vertices must have been added four at a time as squares in the vertices array,
            // though if this is not the case, then we could have reordered the vertices array to obey this assumption
            // in the beginning. To reveal a square, then, we add two triangles to the mesh corresponding to those four
            // vertices.
            int index = indices[location];
            triangles.Add(index);
            triangles.Add(index + 1);
            triangles.Add(index + 2);
            triangles.Add(index);
            triangles.Add(index + 2);
            triangles.Add(index + 3);
        }

        #if UNITY_EDITOR
        [Tooltip("When the FOV game object is selected and the game is running, FOV gizmos will draw in the scene view.")]
        [SerializeField] bool drawFOVGrid = true;

        void OnDrawGizmosSelected()
        {
            if (drawFOVGrid)
            {
                Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
                foreach (var item in inFOVLocations)
                {
                    Gizmos.DrawWireCube(item, Vector3.one);
                }
            }
        }
        #endif
    } 
}