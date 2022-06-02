using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace SealsTheHound
{
    [System.Serializable]
    public struct GameParameters
    {
        public int i_playTime;
        public int i_unwalkableNodes;
        [Range(0.0f, 1.0f)] public float i_errorChance;
    }

    public enum Diff
    {
        easy,
        medium,
        hard
    }

    public class SealsTheHoundGameManager : AMinigameManager
    {
        [Header("References")]
        [SerializeField] private Grid grid;
        [SerializeField] private GameObject obj_houndRef;
        [SerializeField] private Tilemap tileMap;
        [SerializeField] private Tile water;
        [SerializeField] private Tile terrain;
        [SerializeField] private Tile savePoint;
        [SerializeField] private GameObject texto;
        [SerializeField] private GameObject obj_cellRef;
        [SerializeField] private GameObject obj_cellsContainer;


        [Header("Game Parameters")]
        [SerializeField] private Vector3 houndOffset = new Vector3(0.15f, -0.3f, 0);
        [SerializeField, Range(3, 5)] private int i_gridHeight;
        [SerializeField, Range(4, 6)] private int i_gridWidth;
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;


        [Header("Debug")]
        [SerializeField] private bool b_enableDebug;
        [SerializeField] private Vector3Int gridPos = Vector3Int.zero;
        [SerializeField] private Diff diff;


        private GameParameters m_gameParameters;
        private Timer m_gameTimer;

        private GameObject obj_cell;
        private GameObject obj_hound;
        private GameObject obj_currentNode;
        private Vector2Int houndPos;
        private Cell cellComp;

        private Dictionary<string, GameObject> rg_cellsDict = new Dictionary<string, GameObject> { };
        private Dictionary<GameObject, Tuple<GameObject, int>> rg_pathCost = new Dictionary<GameObject, Tuple<GameObject, int>> { };
        private List<GameObject> rg_queue = new List<GameObject> { };
        private HashSet<GameObject> rg_markedNodes = new HashSet<GameObject> { };
        private HashSet<GameObject> rg_leavesNodes = new HashSet<GameObject> { };
        private List<GameObject> rg_nearestLeaves = new List<GameObject> { };

        private bool b_gameWon = false;

        public static SealsTheHoundGameManager Instance;

#region PROPERTIES
        public bool GameFinished
        {
            get { return b_gameFinished; }
        }

        public bool GameWon
        {
            get { return b_gameWon; }
        }
        public int Score
        {
            get { return i_score; }
        }
        public Vector2Int HoundPosition
        {
            get { return houndPos; }
        }
        public int GridHeight
        {
            get { return i_gridHeight; }
        }
        public int GridWidth
        {
            get { return i_gridWidth; }
        }

        public Grid GetGrid
        {
            get { return grid; }
        }
        public Tilemap GetTilemap
        {
            get { return tileMap; }
        }

        public float RemainingTime
        {
            get { return m_gameTimer ? m_gameTimer.RemainingTime : -1; }
        }

        public GameParameters GameParameters
        {
            get { return m_gameParameters; }
        }
        #endregion


        private void Awake()
        {
            if (b_enableDebug)
            {
                if (diff == Diff.easy) BeginMinigame(Difficulty.EASY);
                else if (diff == Diff.medium) BeginMinigame(Difficulty.MEDIUM);
                else if (diff == Diff.hard) BeginMinigame(Difficulty.HARD);
            }

            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Update()
        {

        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            m_difficulty = difficulty;
            if (difficulty == Difficulty.EASY)
            {
                m_gameParameters = gp_easy;
            }
            else if (difficulty == Difficulty.MEDIUM)
            {
                m_gameParameters = gp_medium;
            }
            else if (difficulty == Difficulty.HARD)
            {
                m_gameParameters = gp_hard;
            }

            gridPos = Vector3Int.FloorToInt(new Vector3(gridPos.y, gridPos.x, 0));

            m_gameTimer = Timer.Create(m_gameParameters.i_playTime, onTimeEnd, name: "Game Timer");
            m_gameTimer.StartTimer();

            GenerateBoard();
            SetUnwalkableNodes();
        }

        private void GenerateBoard()
        {
            obj_hound = Instantiate(obj_houndRef, grid.GetCellCenterWorld(Vector3Int.right) + houndOffset, transform.rotation);

            for (int i = 0; i <= i_gridHeight; i++)
            {
                for (int j = 0; j <= i_gridWidth; j++)
                {
                    Vector3Int cellPos1 = new Vector3Int(i, j, 0);
                    Vector3Int cellPos2 = new Vector3Int(i, -j, 0);

                    if (!(i == 5 && j % 2 != 0))
                    {
                        if (!(rg_cellsDict.ContainsKey($"({j}, {i})")))
                        {
                            CreateCells(cellPos1, j, i);
                            tileMap.SetTile(cellPos1, terrain);
                        }

                        if (!(rg_cellsDict.ContainsKey($"({-j}, {i})")))
                        {
                            CreateCells(cellPos2, -j, i);
                            tileMap.SetTile(cellPos2, terrain);
                        }
                    }

                    if (!(rg_cellsDict.ContainsKey($"({-j}, {-i})")))
                    {
                        CreateCells(-cellPos1, -j, -i);
                        tileMap.SetTile(-cellPos1, terrain);
                    }

                    if (!(rg_cellsDict.ContainsKey($"({j}, {-i})")))
                    {
                        CreateCells(-cellPos2, j, -i);
                        tileMap.SetTile(-cellPos2, terrain);
                    }
                }
            }
        }

        private void CreateCells(Vector3Int cellPos, int x, int y)
        {
            obj_cell = Instantiate(obj_cellRef, grid.GetCellCenterWorld(cellPos) + Vector3.up / 20, transform.rotation);
            obj_cell.transform.SetParent(obj_cellsContainer.transform);
            cellComp = obj_cell.GetComponent<Cell>();
            cellComp.i_x = x;
            cellComp.i_y = y;
            obj_cell.name = $"({x}, {y})";
            rg_cellsDict.Add($"({x}, {y})", obj_cell);

            if (Mathf.Abs(x) == i_gridWidth || Mathf.Abs(y) == i_gridHeight) cellComp.b_isLeaf = true;
            if (y == i_gridHeight-1 && x % 2 != 0) cellComp.b_isLeaf = true;

            //LabelCell(cellPos, x, y);             //Labelize all Cells with (x, y) coords
        }

        private void LabelCell(Vector3Int cellPos, int x, int y)
        {
            GameObject text = Instantiate(texto, grid.GetCellCenterWorld(cellPos) + Vector3.up / 20, transform.rotation);      //TEST
            text.transform.SetParent(obj_cell.transform);                                                                      //TEST
            text.name = $"TextCell({x}, {y})";                                                                                 //TEST
            text.GetComponent<TextMesh>().text = (new Vector2Int(cellPos.y, cellPos.x)).ToString();                            //TEST
        }

        private void SetUnwalkableNodes()
        {
            int i_unwalkabledNode = 0;
            HashSet<Vector3Int> rg_unwalkableSet = new HashSet<Vector3Int> { };

            while (i_unwalkabledNode < m_gameParameters.i_unwalkableNodes)
            {
                int random_x = UnityEngine.Random.Range(-i_gridWidth, i_gridWidth);
                int random_y = UnityEngine.Random.Range(-i_gridHeight, i_gridHeight);
                
                Vector3Int unwalkableNode = new Vector3Int(random_y, random_x, 0);

                if (!rg_unwalkableSet.Contains(unwalkableNode) && (unwalkableNode != Vector3Int.zero))
                {
                    i_unwalkabledNode++;
                    rg_unwalkableSet.Add(unwalkableNode);
                    tileMap.SetTile(unwalkableNode, water);
                    rg_cellsDict[$"({random_x}, {random_y})"].GetComponent<Collider2D>().enabled = false;
                    rg_cellsDict.Remove($"({random_x}, {random_y})");
                }
            }
        }

        public void BFS(GameObject obj_touchedNode)
        {
            rg_queue.Clear();
            rg_pathCost.Clear();
            rg_markedNodes.Clear();
            rg_leavesNodes.Clear();

            Vector3Int houndPosCell = grid.WorldToCell(obj_hound.transform.position);
            houndPos = (Vector2Int)(houndPosCell - Vector3Int.right);
            houndPos = new Vector2Int(houndPos.y, houndPos.x);
            string s_sourceNode = houndPos.ToString();

            rg_queue.Add(rg_cellsDict[s_sourceNode]);
            rg_markedNodes.Add(rg_queue.Last());
            rg_cellsDict.Remove(obj_touchedNode.name);
            rg_pathCost.Add(rg_queue.Last(), new Tuple<GameObject, int>(rg_queue.Last(), 0));

            while (rg_queue.Count!=0)
            {
                obj_currentNode = rg_queue[0];
                rg_queue.RemoveAt(0);

                foreach (string s_node in obj_currentNode.GetComponent<Cell>().rg_neighbors)
                {
                    if (rg_cellsDict.ContainsKey(s_node))
                    {
                        GameObject obj_neighbor = rg_cellsDict[s_node];

                        if (!rg_markedNodes.Contains(obj_neighbor))
                        {
                            rg_queue.Add(obj_neighbor);
                            rg_markedNodes.Add(obj_neighbor);
                            rg_pathCost.Add(obj_neighbor, new Tuple<GameObject, int>(obj_currentNode, rg_pathCost[obj_currentNode].Item2 + 1));
                            if (obj_neighbor.GetComponent<Cell>().b_isLeaf) rg_leavesNodes.Add(obj_neighbor);
                        }
                    }
                }
            }

            if (rg_leavesNodes.Count == 0)
            {
                VictoryBehaviour();
                return;
            }

            MinLeavesDistance();

            GameObject obj_nearestNode = rg_nearestLeaves[UnityEngine.Random.Range(0, rg_nearestLeaves.Count)];                 //Choose Random Path between shortest ones

            while (rg_pathCost[obj_nearestNode].Item1.name != s_sourceNode)                                                     //Reverse Path to find first Node to walk
            {
                obj_nearestNode = rg_pathCost[obj_nearestNode].Item1;
            }

            HoundMoveAndError(s_sourceNode, obj_nearestNode.name);

            houndPosCell = grid.WorldToCell(obj_hound.transform.position);
            houndPos = new Vector2Int(houndPosCell.y, houndPosCell.x) + Vector2Int.down;

            if (rg_leavesNodes.Contains(rg_cellsDict[houndPos.ToString()])) LoseBehaviour();
        }

        private void MinLeavesDistance()                                                //Compute Leaves with Min Distance from SourceNode
        {
            int i_minDistance = rg_pathCost.Values.Max(x => x.Item2);

            foreach (GameObject obj_leaf in rg_leavesNodes)
            {
                if (rg_pathCost[obj_leaf].Item2 == i_minDistance) rg_nearestLeaves.Add(obj_leaf);

                if (rg_pathCost[obj_leaf].Item2 < i_minDistance)
                {
                    rg_nearestLeaves.Clear();
                    rg_nearestLeaves.Add(obj_leaf);
                    i_minDistance = rg_pathCost[obj_leaf].Item2;
                }
            }
        }

        private void HoundMoveAndError(string s_source, string s_nearestNode)           //Move Hound with Error Chance
        {
            if (UnityEngine.Random.Range(0f, 1f) < m_gameParameters.i_errorChance)
            {
                cellComp = rg_cellsDict[s_source].GetComponent<Cell>();
                int i_errorID = UnityEngine.Random.Range(0, cellComp.rg_neighbors.Count);

                while (!rg_cellsDict.ContainsKey(cellComp.rg_neighbors[i_errorID]))
                {
                    i_errorID = UnityEngine.Random.Range(0, cellComp.rg_neighbors.Count);
                }

                string s_errorNode = cellComp.rg_neighbors[i_errorID];
                cellComp = rg_cellsDict[s_errorNode].GetComponent<Cell>();
                obj_hound.transform.position = grid.GetCellCenterWorld(Vector3Int.FloorToInt(new Vector3(cellComp.i_y, cellComp.i_x, 0)) + Vector3Int.right) + houndOffset;
            }
            else
            {
                cellComp = rg_cellsDict[s_nearestNode].GetComponent<Cell>();
                obj_hound.transform.position = grid.GetCellCenterWorld(Vector3Int.FloorToInt(new Vector3(cellComp.i_y, cellComp.i_x, 0)) + Vector3Int.right) + houndOffset;
            }
        }

        public void LoseBehaviour()
        {
            tileMap.SetTile(new Vector3Int(houndPos.y, houndPos.x, 0), savePoint);
            obj_hound.transform.position = obj_hound.transform.position + new Vector3(-0.1f, 0.1f, 0);
            obj_hound.GetComponent<Animator>().SetTrigger("Wins");
            i_score = 0;
            EndMinigame();
        }

        protected override void EndMinigame()
        {
            m_gameTimer?.StopTimer();
            b_gameFinished = true;
        }

        private void VictoryBehaviour()
        {
            obj_hound.GetComponent<Animator>().SetTrigger("Death");
            b_gameWon = true;
            i_score = (int)(RemainingTime / m_gameParameters.i_playTime * 3) + 1;
            EndMinigame();
        }
        private void onTimeEnd()
        {
            LoseBehaviour();
        }

    }
}

