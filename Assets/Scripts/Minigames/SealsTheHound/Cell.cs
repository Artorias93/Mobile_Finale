using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

namespace SealsTheHound
{
    public class Cell : MonoBehaviour, IPointerDownHandler
    {
        [Header("References")]
        [SerializeField] private Tile water;

        public List<string> rg_neighbors = new List<string> { };
        [HideInInspector] public int i_x;
        [HideInInspector] public int i_y;

        public bool b_isLeaf;
        public bool b_isWalkable = true;
        public bool b_trigger = false;


        SealsTheHoundGameManager gm;
        int i_gridWidth;
        int i_gridHeight;

        // Start is called before the first frame update
        void Start()
        {
            gm = SealsTheHoundGameManager.Instance;
            i_gridWidth = gm.GridHeight;
            i_gridHeight = gm.GridWidth;

            rg_neighbors.Add($"({i_x + 1},{i_y})");
            rg_neighbors.Add($"({i_x - 1},{i_y})");
            rg_neighbors.Add($"({i_x},{i_y + 1})");
            rg_neighbors.Add($"({i_x},{i_y - 1})");
            
            if (i_x % 2 == 0)
            {
                rg_neighbors.Add($"({i_x + 1},{i_y - 1})");
                rg_neighbors.Add($"({i_x - 1},{i_y - 1})");
            }
            else
            {
                rg_neighbors.Add($"({i_x + 1},{i_y + 1})");
                rg_neighbors.Add($"({i_x - 1},{i_y + 1})");
            }


            if (Mathf.Abs(i_x) == i_gridWidth)
            {
                if (Mathf.Abs(i_y) == i_gridHeight)
                {
                    rg_neighbors.Remove($"({i_x},{i_y + Mathf.Sign(i_y)})");
                    rg_neighbors.Remove($"({i_x + Mathf.Sign(i_x)},{i_y})");

                    if (Mathf.Sign(i_y) < 0)
                    {
                        rg_neighbors.Remove($"({i_x - Mathf.Sign(i_x)},{i_y + Mathf.Sign(i_y)})");
                        rg_neighbors.Remove($"({i_x + Mathf.Sign(i_x)},{i_y + Mathf.Sign(i_y)})");
                    }
                    else
                    {
                        rg_neighbors.Remove($"({i_x - Mathf.Sign(i_x)},{i_y})");
                        rg_neighbors.Remove($"({i_x + Mathf.Sign(i_x)},{i_y - Mathf.Sign(i_y)})");
                    }
                }
                else
                {
                    rg_neighbors.Remove($"({i_x + Mathf.Sign(i_x)},{i_y})");
                    rg_neighbors.Remove($"({i_x + Mathf.Sign(i_x)},{i_y - 1})");
                }
            }
            else
            {
                if (Mathf.Abs(i_y) == i_gridHeight || ((i_y) == i_gridHeight-1 && i_x % 2 != 0))
                {
                    rg_neighbors.Remove($"({i_x},{i_y + Mathf.Sign(i_y)})");

                    if (Mathf.Abs(i_y) == i_gridHeight && i_x % 2 == 0)
                    {
                        if (Mathf.Sign(i_y) < 0)
                        {
                            rg_neighbors.Remove($"({i_x + 1},{i_y - 1})");
                            rg_neighbors.Remove($"({i_x - 1},{i_y - 1})");
                        }
                        else
                        {
                            rg_neighbors.Remove($"({i_x + 1},{i_y})");
                            rg_neighbors.Remove($"({i_x - 1},{i_y})");
                        }
                    }
                }
            }
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnMouseDown()
        {
            if (b_trigger)
            {

            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            b_trigger = true;
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            b_trigger = false;
        }

        public void OnPointerDown(PointerEventData pointerEventData)
        {
            b_trigger = true;
            GetComponent<Collider2D>().enabled = false;
            Vector3Int mouseWorldPos = gm.GetGrid.WorldToCell(Camera.main.ScreenToWorldPoint(Input.mousePosition));
            //Vector3Int coordinates = gm.GetGrid.GetCellCenterWorld(mouseWorldPos);
            gm.GetTilemap.SetTile(mouseWorldPos, water);
        }


    }
}
