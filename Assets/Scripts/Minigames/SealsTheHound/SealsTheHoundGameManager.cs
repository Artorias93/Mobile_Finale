using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace SealsTheHound
{
    [System.Serializable]
    public struct GameParameters
    {
        public int i_playTime;
        public int i_runeToComplete;
    }

    public enum Diff
    {
        easy,
        medium,
        hard
    }

    class DistanceFromPrev
    {
        public string s_previousNode { get; set; }
        public int i_cost { get; set; }
    }

    public class SealsTheHoundGameManager : AMinigameManager
    {
        [Header("References")]
        [SerializeField] private Grid grid;
        [SerializeField] private GameObject obj_houndRef;
        [SerializeField] private Tilemap tileMap;
        [SerializeField] private Tile water;
        [SerializeField] private Tile terrain;
        [SerializeField] private GameObject texto;
        [SerializeField] private GameObject obj_cellRef;
        [SerializeField] private GameObject obj_cellsContainer;


        [Header("Game Parameters")]
        [SerializeField, Range(3, 5)] private int i_gridHeight;
        [SerializeField, Range(4, 6)] private int i_gridWidth;
        [SerializeField] private GameParameters gp_easy, gp_medium, gp_hard;


        [Header("Debug")]
        [SerializeField] private bool b_enableDebug;
        [SerializeField] private Vector3Int gridPos = Vector3Int.zero;
        [SerializeField] private Diff diff;


        private GameParameters m_gameParameters;
        private Timer m_gameTimer;

        private Animator anim;
        private GameObject obj_rune;
        private Vector3Int gridPos2;
        private GameObject obj_cell;
        private GameObject obj_hound;
        private GameObject obj_currentNode;

        public List<GameObject> rg_adjacencyList = new List<GameObject> { };
        private Dictionary<string, GameObject> rg_cellsDict = new Dictionary<string, GameObject> { };
        private Dictionary<string, Tuple<string, int>> rg_pathCost = new Dictionary<string, Tuple<string, int>> { };
        public List<GameObject> rg_queue = new List<GameObject> { };
        public HashSet<GameObject> rg_markedNodes = new HashSet<GameObject> { };

        private int i_runeCompleted = 0;
        private int i_candlesPlaced = 0;
        private int i_candlesCount;
        private int i_childToCheckId;

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

        public int RunesCount
        {
            get { return i_runeCompleted; }
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
            gridPos2 = new Vector3Int(gridPos.y, gridPos.x, 0);
            obj_hound.transform.position = grid.GetCellCenterWorld(gridPos2 + Vector3Int.right);
        }

        public override void BeginMinigame(Difficulty difficulty)
        {
            m_difficulty = difficulty;
            if (difficulty == Difficulty.EASY)
            {
                m_gameParameters = gp_easy;
                i_childToCheckId = 0;
            }
            else if (difficulty == Difficulty.MEDIUM)
            {
                m_gameParameters = gp_medium;
                i_childToCheckId = 1;
            }
            else if (difficulty == Difficulty.HARD)
            {
                m_gameParameters = gp_hard;
                i_childToCheckId = 2;
            }

            gridPos = Vector3Int.FloorToInt(new Vector3(gridPos.y, gridPos.x, 0));

            m_gameTimer = Timer.Create(m_gameParameters.i_playTime, onTimeEnd, name: "Game Timer");
            m_gameTimer.StartTimer();

            GenerateBoard();
        }

        private void GenerateBoard()
        {
            obj_hound = Instantiate(obj_houndRef, grid.GetCellCenterWorld(Vector3Int.right), transform.rotation);

            for (int i = 0; i <= i_gridHeight; i++)
            {
                for (int j = 0; j <= i_gridWidth; j++)
                {
                    Vector3Int cellPos1 = new Vector3Int(i, j, 0);
                    Vector3Int cellPos2 = new Vector3Int(i, -j, 0);

                    if (!(i == 5 && j % 2 != 0))
                    {
                        if (!(rg_cellsDict.ContainsKey($"({j},{i})")))
                        {
                            CreateCells(cellPos1, j, i);
                            tileMap.SetTile(cellPos1, terrain);
                        }

                        if (!(rg_cellsDict.ContainsKey($"({-j},{i})")))
                        {
                            CreateCells(cellPos2, -j, i);
                            tileMap.SetTile(cellPos2, terrain);
                        }
                    }

                    if (!(rg_cellsDict.ContainsKey($"({-j},{-i})")))
                    {
                        CreateCells(-cellPos1, -j, -i);
                        tileMap.SetTile(-cellPos1, terrain);
                    }

                    if (!(rg_cellsDict.ContainsKey($"({j},{-i})")))
                    {
                        CreateCells(-cellPos2, j, -i);
                        tileMap.SetTile(-cellPos2, terrain);
                    }
                }
            }

            //rg_adjacencyList = rg_cellsDict.Values.ToList();
            //print(rg_adjacencyList[0].GetComponent<Cell>().rg_neighbors[0]);
            //BFS(rg_adjacencyList[0]);
        }

        private void CreateCells(Vector3Int cellPos, int x, int y)
        {
            obj_cell = Instantiate(obj_cellRef, grid.GetCellCenterWorld(cellPos) + Vector3.up / 20, transform.rotation);
            obj_cell.transform.SetParent(obj_cellsContainer.transform);
            obj_cell.GetComponent<Cell>().i_x = x;
            obj_cell.GetComponent<Cell>().i_y = y;
            obj_cell.name = $"({x},{y})";
            rg_cellsDict.Add($"({x},{y})", obj_cell);

            if (Mathf.Abs(x) == i_gridWidth || Mathf.Abs(y) == i_gridHeight) obj_cell.GetComponent<Cell>().b_isLeaf = true;
            if (y == i_gridHeight-1 && x % 2 != 0) obj_cell.GetComponent<Cell>().b_isLeaf = true;

            LabelCell(cellPos, x, y);
        }

        private void LabelCell(Vector3Int cellPos, int x, int y)
        {
            GameObject text = Instantiate(texto, grid.GetCellCenterWorld(cellPos) + Vector3.up / 20, transform.rotation);      //TEST
            text.transform.SetParent(obj_cell.transform);                                                                      //TEST
            text.name = $"TextCell({x},{y})";                                                                                  //TEST
            text.GetComponent<TextMesh>().text = (new Vector2Int(cellPos.y, cellPos.x)).ToString();                            //TEST
        }

        public void BFS(GameObject source)
        {
            Vector2Int houndPos = (Vector2Int)(grid.WorldToCell(obj_hound.transform.position) - Vector3Int.right);
            houndPos = new Vector2Int(houndPos.y, houndPos.x);
            rg_queue.Add(source);
            rg_markedNodes.Add(source);

            rg_pathCost.Add(source.name, new Tuple<string, int>(source.name, 0));

            while (rg_queue.Count!=0)
            {
                obj_currentNode = rg_queue[0];
                rg_queue.RemoveAt(0);

                print($"{obj_currentNode.name} has distance {rg_pathCost[obj_currentNode.name].Item2} from the source {source.name} and his previous node is {rg_pathCost[obj_currentNode.name].Item1}");

                foreach (string s_node in obj_currentNode.GetComponent<Cell>().rg_neighbors)
                {
                    if(!rg_markedNodes.Contains(rg_cellsDict[s_node]))
                    {
                        rg_queue.Add(rg_cellsDict[s_node]);
                        rg_markedNodes.Add(rg_cellsDict[s_node]);
                        rg_pathCost.Add(s_node, new Tuple<string, int>(obj_currentNode.name, rg_pathCost[obj_currentNode.name].Item2 + 1));
                    }
                }
            }


        }






        private void GenerateRound()
        {
            i_candlesPlaced = 0;
            i_candlesCount = 0;

            for (int i = 0; i <= i_childToCheckId; i++)
            {
                obj_rune.transform.GetChild(i).gameObject.SetActive(true);
                i_candlesCount += obj_rune.transform.GetChild(i).childCount;
            }

            SpawnCandles(i_candlesCount);
        }

        private void SpawnCandles(int i_candlesNum)
        {
            for (int i = 0; i <= i_candlesNum - 1; i++)
            {

            }
        }

        IEnumerator WaitGenerate()
        {
            if (i_runeCompleted == GameParameters.i_runeToComplete)
            {
                VictoryBehaviour();
                yield break;
            }

            yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);
            GenerateRound();
        }

        public void CountRunes()
        {
            i_runeCompleted++;
            anim.SetTrigger("Destroy");
            Destroy(obj_rune, anim.GetCurrentAnimatorStateInfo(0).length);
            StartCoroutine(WaitGenerate());
        }

        public void CountCandles()
        {
            i_candlesPlaced++;
            if (i_candlesPlaced == i_candlesCount) CountRunes();
        }

        public void LoseBehaviour()
        {
            EndMinigame();
        }

        protected override void EndMinigame()
        {
            m_gameTimer?.StopTimer();
            b_gameFinished = true;

            i_score = 3 - Mathf.FloorToInt((Mathf.Abs(((float)i_runeCompleted / (float)m_gameParameters.i_runeToComplete * 100) - 100) / 100 * 4));
            if (i_score > 3) i_score = 3;

        }

        private void VictoryBehaviour()
        {
            b_gameWon = true;
            EndMinigame();
        }
        private void onTimeEnd()
        {
            LoseBehaviour();
        }

    }
}

