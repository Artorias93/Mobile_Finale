using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

namespace EscapeDungeonMinigame
{
    [System.Serializable]
    public struct Edge
    {
        public enum Direction
        {
            North, West
        };

        public int x, y;
        public Direction direction;

        public Edge(int _x, int _y, Direction _dir)
        {
            x = _x;
            y = _y;
            direction = _dir;
        }
    };

    public class MazeManager : MonoBehaviour
    {
        [Header("Gameobjects and References")]
        [SerializeField] private GameObject m_GridGameObject;
        [SerializeField] private GridLayoutGroup m_GridLayout;
        [SerializeField] private GameObject m_CellPrefab;
        [Header("Maze settings")]
        [SerializeField] private int Width;
        [SerializeField] private int Height;
        [SerializeField] private int i_CellSize = 200;


        private Tile[,] m_tilesMatrix;

        private void Awake()
        {
            Assert.IsNotNull(m_GridGameObject, "No Grid Gameobject assigned");
            Assert.IsNotNull(m_CellPrefab, "No cell prefab assigned");

            if(m_GridLayout == null)
            {
                m_GridLayout = m_GridGameObject.GetComponent<GridLayoutGroup>();
                if (m_GridLayout == null)
                {
                    m_GridLayout = m_GridGameObject.AddComponent<GridLayoutGroup>();
                }
            }

            m_GridLayout.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            m_GridLayout.constraintCount = Width;
            m_GridLayout.cellSize = i_CellSize * Vector2.one;

            GenerateGrid();
            GenerateMaze();
        }

        private void GenerateGrid()
        {
            m_tilesMatrix = new Tile[Height, Width];
            for(int row = 0; row<Height; row++) { 
                for(int col = 0; col<Width; col++)
                {
                    GameObject tileGO = Instantiate(m_CellPrefab, m_GridGameObject.transform);
                    Tile tile = tileGO.GetComponent<Tile>();
                    m_tilesMatrix[row, col] = tile;
                    tile.Initialize(row, col);
                }
            }
        }

        private void GenerateMaze()
        {
            //To Implement different algorithms generation override this call
            KruskalRandomMaze();
        }

        private void KruskalRandomMaze()
        {
            HashSet<Tile>[,] sets = new HashSet<Tile>[Height, Width];
            for(int row = 0; row < Height; row++)
            {
                for(int col = 0; col < Width; col++)
                {
                     sets[row, col] = new HashSet<Tile>();
                    sets[row, col].Add(m_tilesMatrix[row, col]);
                }
            }

            //Start with fully walled maze, every cell has four walls
            List <Edge> edges = new List<Edge>();
            for(int col = 0; col < Width; col++)
            {
                for (int row = 0; row < Height; row++)
                {
                    Edge edge;
                    if (col > 0)
                    {
                        edge = new Edge(row, col, Edge.Direction.West);
                        edges.Add(edge);
                    }
                    if(row > 0)
                    {
                        edge = new Edge(row, col, Edge.Direction.North);
                        edges.Add(edge);
                    }
                }
            }
           
            while(edges.Count > 0)
            {
                //Get random edge, if the two cells are in different set join them; in any case eliminate it
                int randomEdgeIndex = Random.Range(0, edges.Count);
                Edge edge = edges[randomEdgeIndex];
                edges.RemoveAt(randomEdgeIndex);

                ref HashSet<Tile> setA = ref sets[edge.x, edge.y];

                Vector2Int d;
                if (edge.direction == Edge.Direction.North)
                    d = new Vector2Int(-1, 0);
                else
                    d = new Vector2Int(0, -1);

                ref HashSet<Tile> setB = ref sets[edge.x + d.x, edge.y + d.y];

                if(!setA.Overlaps(setB))
                {
                    setA.UnionWith(setB);
                    setB = setA;

                    Tile.Connect(m_tilesMatrix[edge.x, edge.y], m_tilesMatrix[edge.x + d.x, edge.y + d.y]);
                }
            }
        }

        void Start()
        {
            //Used to force correct positionitiong of player
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_GridGameObject.GetComponent<RectTransform>());
            PlayerController.Instance.Spawn(m_tilesMatrix[Height - 1, Width - 1]);
        }
    }
}
