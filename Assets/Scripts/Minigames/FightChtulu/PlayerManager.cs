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
        [SerializeField]
        private List<Sprite> attackSprite;

        public static PlayerManager Instance;
        private List<GameObject> m_positions= new List<GameObject>();
        private GameObject playerCurrentPosition=null;
        private GameObject requestedPosition=null;
        private int i_spriteIndex=0;
        private GameObject attackTarget = null;
        private bool b_isRightOriented = true;


        private void Awake()
        {
            Instance = this;
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
            MiniGameManager.OnUpdate += UpdateInTime;
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
            playerCurrentPosition.GetComponent<SpriteRenderer>().sprite = playerMovementSprites[i_spriteIndex];
        }
        private void setCurrentPosition(GameObject position, bool resetCurrent) {
            if (resetCurrent) {
                playerCurrentPosition.GetComponent<SpriteRenderer>().sprite = s_emptySpace;
                playerCurrentPosition.GetComponent<Collider2D>().enabled = true;
            }

            playerCurrentPosition = position;
            if (b_isRightOriented && playerCurrentPosition.transform.localScale.x < 0 || !b_isRightOriented && playerCurrentPosition.transform.localScale.x > 0)
            {
                tiltSprite();
            }           
            playerCurrentPosition.GetComponent<SpriteRenderer>().sprite = playerMovementSprites[i_spriteIndex];
            playerCurrentPosition.GetComponent<Collider2D>().enabled = false;
        }

        private void tiltSprite() {
            Vector3 tmpScale = playerCurrentPosition.transform.localScale;
            tmpScale.x *= -1;
            playerCurrentPosition.transform.localScale = tmpScale;
            b_isRightOriented = tmpScale.x < 0 ? false : true;
        }

        public void setPlayerRequestedPosition(GameObject obj) {

            if (obj != null && !obj.Equals(playerCurrentPosition)) {
                requestedPosition = obj;
            }
        }
        public void UpdateInTime()
        {
            changeSpriteIndex();
            if (attackTarget != null)
            { 
              int attackIndex = tryHit();
              attackTarget.GetComponent<CthulhuTentacleManager>().i_HitIndex = attackIndex;  
              attackTarget = null;
            }
            else {
                moveToRequestedPosition();
            }

        }

        private void moveToRequestedPosition() {
            if (m_positions.Contains(requestedPosition) && !playerCurrentPosition.Equals(requestedPosition))
            {
                Vector3 playerPosition = playerCurrentPosition.transform.position;
                Vector3 requestedTilePoisition = requestedPosition.transform.position;
                Vector3 dir = playerPosition - requestedTilePoisition;
                //If movement is straight (not diagonal)
                if (Mathf.Approximately((int)dir.x, 0) || Mathf.Approximately((int)dir.y, 0))
                {
                    float rayLength = 100f;
                    dir *= -1;

                    RaycastHit2D hit = Physics2D.Raycast(playerCurrentPosition.transform.position, dir, rayLength);

                    if (hit.collider == requestedPosition.GetComponent<Collider2D>())
                    {
                        //questo if serve a causa di un bug che per la tile 4 e 5 in verticale trova due valori di x diversi nonostante siano uguali
                        if (!Mathf.Approximately((int)dir.x, 0))
                        {
                            changeOrientationInMovement(playerPosition, requestedTilePoisition);
                        }
                        setCurrentPosition(requestedPosition, true);
                    }
                    else
                    {
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

        private void changeOrientationInMovement(Vector3 playerPosition, Vector3 requestedTilePoisition) {
                if (playerPosition.x < requestedTilePoisition.x)
                {
                    b_isRightOriented = true;
                }
                else if (playerPosition.x > requestedTilePoisition.x)
                {
                    b_isRightOriented = false;
                }
        }

        public void setAttackTarget(GameObject tentacle) {
            attackTarget = tentacle;
        }
        public int tryHit() {
            Vector3 dir;
            if (playerCurrentPosition.transform.position.x < attackTarget.transform.position.x)
            {
                dir = Vector3.right;
            }
            else {
                dir = Vector3.left;
            }
            float rayLength = 100f;

            //Get the first object hit by the ray
            RaycastHit2D hit = Physics2D.Raycast(playerCurrentPosition.transform.position, dir, rayLength);
            //Debug.DrawRay(playerCurrentPosition.transform.position, dir * rayLength, Color.red);

            GameObject tentacle=null;
            if (hit.collider != null) { 
                tentacle= hit.collider.transform.parent.gameObject;
            }
                
            if (tentacle != null && tentacle.Equals(attackTarget))
            {
                CthulhuManager cthulhuManager = CthulhuManager.Instance;
                Debug.LogWarning("HitIndex"+ cthulhuManager.i_CurrentTentacleIndex);
                if (b_isRightOriented && dir.Equals(Vector3.left) || !b_isRightOriented && dir.Equals(Vector3.right)) {
                    tiltSprite();
                }


                StartCoroutine(setAttackSprite());
                CthulhuTentacleManager tentacleManager= tentacle.GetComponent<CthulhuTentacleManager>();
                if (tentacleManager.g_GrabbedItem != null) {
                    tentacleManager.b_CanGrab = false;
                    tentacleManager.g_GrabbedItem.GetComponent<DefendableItem>().b_isGrabbed = false;
                    tentacleManager.g_GrabbedItem = null;

                }
                return cthulhuManager.i_CurrentTentacleIndex;
            }
            else {
                Debug.LogWarning("NoHit");
                return -1;
            }
        }
        private IEnumerator setAttackSprite() {
            playerCurrentPosition.GetComponent<SpriteRenderer>().sprite = attackSprite[0];
            yield return new WaitForSeconds(MiniGameManager.Instance.frameTime/2);
            playerCurrentPosition.GetComponent<SpriteRenderer>().sprite = attackSprite[1];
        }
    }
}
