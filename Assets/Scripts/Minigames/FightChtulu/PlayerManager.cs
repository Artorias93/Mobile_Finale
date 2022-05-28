using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace FightCthulhu
{
    public class PlayerManager : MonoBehaviour
    {
        [SerializeField]
        private Sprite s_emptySpace;
        [SerializeField]
        private List<Sprite> playerMovementSprites;

        public static PlayerManager Instance;
        private List<GameObject> m_positions= new List<GameObject>();
        private GameObject playerCurrentPosition=null;
        private GameObject requestedPosition=null;
        private int i_spriteIndex=0;

        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            InitializePlayer();
        }

        public void InitializePlayer() {
            foreach (Transform child in transform)
            {
                m_positions.Add(child.gameObject);
            }

            GameObject startPosition = m_positions[Random.Range(0, m_positions.Count)];
            foreach (GameObject position in m_positions)
            {
                position.GetComponent<SpriteRenderer>().sprite = s_emptySpace;
            }
            setCurrentPosition(startPosition,false);
            MiniGameManager.OnUpdate += setPlayerPosition;
        }

        private void changeSpriteIndex() {
            if (i_spriteIndex == playerMovementSprites.Count - 1)
            {
                i_spriteIndex = 0;
            }
            else
            {
                i_spriteIndex++;
            }
        }
        private void setCurrentPosition(GameObject position, bool resetCurrent) {
            if (resetCurrent) {
                playerCurrentPosition.GetComponent<SpriteRenderer>().sprite = s_emptySpace;
                playerCurrentPosition.GetComponent<Collider2D>().enabled = true;
            }

            changeSpriteIndex();
            playerCurrentPosition = position;
            playerCurrentPosition.GetComponent<SpriteRenderer>().sprite = playerMovementSprites[i_spriteIndex];
            playerCurrentPosition.GetComponent<Collider2D>().enabled = false;
        }

        public void setPlayerRequestedPosition(GameObject obj) {

            if (obj != null && !obj.Equals(playerCurrentPosition)) {
                requestedPosition = obj;
            }
        }
        public void setPlayerPosition()
        {
            if (m_positions.Contains(requestedPosition) && !playerCurrentPosition.Equals(requestedPosition))
            {
                Vector3 dir = playerCurrentPosition.transform.position - requestedPosition.transform.position;
                //If movement is straight (not diagonal)
                if (Mathf.Approximately((int) dir.x,0)|| Mathf.Approximately((int)dir.y, 0))
                {
                    //Length of the ray
                    float rayLength = 100f;
                    dir *= -1;

                    //Get the first object hit by the ray
                    RaycastHit2D hit = Physics2D.Raycast(playerCurrentPosition.transform.position, dir, rayLength);

                    if (hit.collider == requestedPosition.GetComponent<Collider2D>())
                    {
                        setCurrentPosition(requestedPosition, true);
                    }
                    else {
                        requestedPosition = null;
                    }

                    //Debug.DrawRay(playerCurrentPosition.transform.position, dir * rayLength, Color.red);

                }
                else
                {
                    Debug.LogWarning("Can't move diagonal");
                    requestedPosition = null;

                }

            }
        }

        public int tryHit(GameObject tentacle) {
            Vector3 dir;
            if (playerCurrentPosition.transform.position.x < tentacle.transform.position.x)
            {
                dir = Vector3.right;
            }
            else {
                dir = Vector3.left;
            }
            float rayLength = 100f;
//            dir *= -1;

            //Get the first object hit by the ray
            RaycastHit2D hit = Physics2D.Raycast(playerCurrentPosition.transform.position, dir, rayLength);
            //Debug.DrawRay(playerCurrentPosition.transform.position, dir * rayLength, Color.red);
            if (hit.collider.CompareTag("Tentacle"))
            {
                CthulhuManager cthulhuManager = CthulhuManager.Instance;
                Debug.LogWarning("HitIndex"+ cthulhuManager.i_CurrentTentacleIndex);
                return cthulhuManager.i_CurrentTentacleIndex;
            }
            else {
                Debug.LogWarning("NoHit");
                return -1;
            }
        }
    }
}
