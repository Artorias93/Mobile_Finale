using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EscapeDungeonMinigame
{
    public class Tile : MonoBehaviour
    {
        private Vector2Int v2_Coords;
        private HashSet<Tile> m_reachableTiles;

        public Vector2Int Coords
        {
            get { return v2_Coords; }
        }

        public HashSet<Tile> ReachableTiles
        {
            get { return m_reachableTiles; }
        }
       
        public void Initialize(int x, int y)
        {
            v2_Coords.Set(x, y);
            m_reachableTiles = new HashSet<Tile>();
            gameObject.name = "Tile (" + x + " " + y + ")";
        }

        public static void Connect(Tile tile1, Tile tile2)
        {
            if(tile1 == tile2)
            {
                Debug.Log("Invalid Connection: The two tiles are the same");
                return;
            }
            //Tiles must be adjacient, otherwise don't allow connection
            if ((tile1.Coords - tile2.Coords).magnitude == 1)
            {
                tile1.m_reachableTiles.Add(tile2);
                tile2.m_reachableTiles.Add(tile1);
            }
            else
            {
                Debug.Log("Invalid Connection: The two tiles aren't adjacent\nTile 1: " + tile1.Coords + " tile2: " + tile2.Coords);
            }
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            foreach (Tile tile in m_reachableTiles)
            {
                Gizmos.DrawLine(this.transform.position, tile.transform.position);
            }
        }
    }
}
