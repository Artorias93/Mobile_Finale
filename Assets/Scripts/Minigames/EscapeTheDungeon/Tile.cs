using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EscapeDungeonMinigame
{
    //Define TileSpriteType as flags enum to simplify selection later on
    [System.Serializable, System.Flags]
    enum TileSpriteType
    {
        FULL = 0, TOP = 1, RIGHT = 2, BOTTOM = 4, LEFT = 8, 
        TOP_BOTTOM = TOP | BOTTOM, LEFT_RIGHT = LEFT | RIGHT, 
        TOP_RIGHT = TOP | RIGHT, RIGHT_BOTTOM = RIGHT | BOTTOM, 
        BOTTOM_LEFT = BOTTOM | LEFT, LEFT_TOP = LEFT | TOP,
        TOP_RIGHT_BOTTOM = TOP | RIGHT | BOTTOM, RIGHT_BOTTOM_LEFT = RIGHT | BOTTOM | LEFT, 
        BOTTOM_LEFT_TOP = BOTTOM | LEFT | TOP , LEFT_TOP_RIGHT = LEFT | TOP | RIGHT, 
        EMPTY = TOP | RIGHT | BOTTOM | LEFT
    }

    [System.Serializable]
    struct TileSprite
    {
        public TileSpriteType Type;
        public Sprite sprite;   
    }


    public class Tile : MonoBehaviour, IPointerDownHandler
    {
        private Vector2Int v2_Coords;
        private HashSet<Tile> m_reachableTiles;
        private Image m_image;

        [SerializeField] private TileSprite[] m_TilesDictionaryEntries;
        private static Dictionary<TileSpriteType, Sprite> m_tileDictionary;

        private TileSpriteType type = TileSpriteType.FULL;

        public Vector2Int Coords
        {
            get { return v2_Coords; }
        }

        public HashSet<Tile> ReachableTiles
        {
            get { return m_reachableTiles; }
        }

        private bool b_occupied;

        public bool IsOccupied
        {
            get { return b_occupied; }
        }

        private void Awake()
        {
            m_image = GetComponent<Image>();
            if (m_tileDictionary == null) { //Initialize Dictionary Only once
                m_tileDictionary = new Dictionary<TileSpriteType, Sprite>();

                foreach (TileSprite t in m_TilesDictionaryEntries)
                {
                    m_tileDictionary.Add(t.Type, t.sprite);
                }

            } 
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
            if (m_reachableTiles == null) return;

            Gizmos.color = Color.red;
            foreach (Tile tile in m_reachableTiles)
            {
                Gizmos.DrawLine(this.transform.position, tile.transform.position);
            }
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            PlayerController.Instance.SetPosition(this);
        }

        public void SetOccupied(bool value)
        {
            if(b_occupied && value == true)
            {
                Debug.Log("Trying to occupy an already occupied tile");
                return;
            }
            b_occupied = value;
        }

        public void UpdateAppearance()
        {
            //Updates sprite based on adjacent tiles;
            if(m_reachableTiles == null)
            {
                Debug.LogWarning("Adjacent tiles list is null");
                return;
            }

            bool bFreeTop = false, bFreeRight = false, bFreeLeft = false, bFreeBottom = false;
            foreach(Tile tile in m_reachableTiles)
            {
                Vector2Int delta = tile.v2_Coords - this.v2_Coords;
                bFreeTop = (delta.y == 0 && delta.x == -1);
                bFreeRight = (delta.y == 1 && delta.x == 0);
                bFreeBottom = (delta.y == 0 && delta.x == 1);
                bFreeLeft = (delta.y == -1 && delta.x == 0);

                if (bFreeTop) type = type | TileSpriteType.TOP;
                if (bFreeRight) type = type | TileSpriteType.RIGHT;
                if (bFreeBottom) type = type | TileSpriteType.BOTTOM;
                if (bFreeLeft) type = type | TileSpriteType.LEFT; 
            }

            m_image.sprite = m_tileDictionary[type];

        }

    }
}
