using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightCthulhu
{
    public class CthulhuManager : MonoBehaviour
    {
        public int i_CurrentTentacleIndex
        {
            get { return i_currentTentaclesStepIndex; }
        }
        public int i_ErrorCount
        {
            get { return i_errorCount; }
            set { i_errorCount = value; }
        }
        public static CthulhuManager Instance;

        [SerializeField]
        private List<Sprite> idleSprites;

        private List<GameObject> m_tentacles = new List<GameObject>();
        private int i_nextTentaclesStepIndex = 0;
        private int i_currentTentaclesStepIndex = 0;
        private int maxChildIndex;
        private bool b_directionDown = true;
        private int i_ActiveTentacles;
        private int i_grabbingTentaclesCount;
        private List<GameObject> activeTentacles = new List<GameObject>();
        private bool b_tentaclesNeedReset = false;
        private int i_errorCount=0;
        private int i_spriteIndex = 0;

        private void Awake()
        {
            Instance = this;
        }

        public void InitializeCthulhu(int i_activeTentacles,int grabbingCount) {
            i_ActiveTentacles = i_activeTentacles;
            i_grabbingTentaclesCount = grabbingCount;
            foreach (Transform child in transform)
            {
                m_tentacles.Add(child.gameObject);
            }

            addActiveTentacles();

            maxChildIndex = m_tentacles[0].transform.childCount - 1;
            MiniGameManager.OnUpdate += UpdateInTime;
        }

        private void changeSpriteIndex()
        {
            if (i_spriteIndex == idleSprites.Count - 1)
            {
                i_spriteIndex = 0;
            }
            else
            {
                i_spriteIndex++;
            }
           GetComponent<SpriteRenderer>().sprite = idleSprites[i_spriteIndex];
        }
        private void addActiveTentacles() {
            int i_tmpGrabCount = i_grabbingTentaclesCount <= m_tentacles.Count? i_grabbingTentaclesCount: m_tentacles.Count;
            i_ActiveTentacles = i_ActiveTentacles <= m_tentacles.Count ? i_ActiveTentacles : m_tentacles.Count;
            while (activeTentacles.Count < i_ActiveTentacles)
            {
                int tentacleIdx = Random.Range(0, m_tentacles.Count);
                if (!activeTentacles.Contains(m_tentacles[tentacleIdx]))
                {
                    m_tentacles[tentacleIdx].GetComponent<CthulhuTentacleManager>().b_Hitted = false;
                    activeTentacles.Add(m_tentacles[tentacleIdx]);
                }
            }
            while (i_tmpGrabCount > 0) {
                int tentacleIdx = Random.Range(0, activeTentacles.Count);
                CthulhuTentacleManager tentacleManager = activeTentacles[tentacleIdx].GetComponent<CthulhuTentacleManager>();
                if (!tentacleManager.b_CanGrab) {
                    tentacleManager.b_CanGrab = true;
                    i_tmpGrabCount--;
                }
            }

        }

        private int findDeactivateIndex() {
            if (b_directionDown)
            {
                if (i_nextTentaclesStepIndex != 0)
                {
                    return i_nextTentaclesStepIndex - 1;
                }
            }
            else {
                if (i_nextTentaclesStepIndex != maxChildIndex) {
                    return i_nextTentaclesStepIndex + 1;
                }
            }
            return -1;
        }

        private void UpdateInTime() {
            MiniGameManager miniGameManager = MiniGameManager.Instance;
            if (miniGameManager != null && !miniGameManager.GameFinished)
            {
                int skipIdx = Random.Range(0, 5);
                changeSpriteIndex();
                if (skipIdx > 0)
                {
                    makeTentacleStep();
                }
            }
        }

        private void makeTentacleStep() {
            int deactivateIndex = findDeactivateIndex();

            foreach (GameObject tentacle in activeTentacles)
            {
                CthulhuTentacleManager tentacleManager = tentacle.GetComponent<CthulhuTentacleManager>();
                Transform tentacleStep = tentacle.transform.GetChild(i_nextTentaclesStepIndex);

                if (deactivateIndex != -1)
                {
                    tentacle.transform.GetChild(deactivateIndex).gameObject.SetActive(false);
                }
                tentacleStep.gameObject.SetActive(true);
                
                if (tentacleManager.b_CanGrab)
                {
                    tentacleStep.GetComponent<SpriteRenderer>().color = Color.red;
                }
                else {
                    tentacleStep.GetComponent<SpriteRenderer>().color = Color.white;
                }
                if (i_nextTentaclesStepIndex != 0) {
                    if (tentacleManager.b_Hitted)
                    {
                        tentacleStep.GetComponent<CapsuleCollider2D>().enabled = false;
                        Color tmpcolor = tentacleStep.GetComponent<SpriteRenderer>().color;
                        tmpcolor.a = 0.5f;
                        tentacleStep.GetComponent<SpriteRenderer>().color = tmpcolor;
                    }
                    else
                    {
                        tentacleStep.GetComponent<CapsuleCollider2D>().enabled = true;
                        Color tmpcolor = tentacleStep.GetComponent<SpriteRenderer>().color;
                        tmpcolor.a = 1;
                        tentacleStep.GetComponent<SpriteRenderer>().color = tmpcolor;
                    }
                }
                GameObject grabbedItem = tentacleManager.g_GrabbedItem;
                    if ( grabbedItem != null) {
                        grabbedItem.transform.position = tentacleStep.transform.GetChild(0).position;
                    }
            }
            if (b_tentaclesNeedReset) {

                foreach (GameObject tentacle in activeTentacles) {
                    CthulhuTentacleManager tentacleManager = tentacle.GetComponent<CthulhuTentacleManager>();
                    tentacleManager.b_CanGrab = false;
                    if (tentacleManager.g_GrabbedItem != null && m_tentacles.Contains(tentacle) ) {
                        m_tentacles.Remove(tentacle);
                        i_errorCount++;
                    }
                }
                activeTentacles.Clear();
                addActiveTentacles();
            }

            changeDirectionAndIndex();
            if (i_nextTentaclesStepIndex == 0)
            {
                b_tentaclesNeedReset = true;
            }
            else {
                b_tentaclesNeedReset = false;
            }
        }

        private void changeDirectionAndIndex() {
            if ((i_nextTentaclesStepIndex == 0 && !b_directionDown) || (i_nextTentaclesStepIndex == maxChildIndex && b_directionDown))
            {
                b_directionDown = !b_directionDown;
            }
            i_currentTentaclesStepIndex = i_nextTentaclesStepIndex;

            i_nextTentaclesStepIndex = b_directionDown ? i_nextTentaclesStepIndex + 1 : i_nextTentaclesStepIndex - 1;
        }

        private void OnDestroy()
        {
            MiniGameManager.OnUpdate -= UpdateInTime;
        }

    }
    
}