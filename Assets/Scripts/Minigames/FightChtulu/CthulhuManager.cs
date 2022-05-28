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
        public static CthulhuManager Instance;

        private List<GameObject> m_tentacles = new List<GameObject>();
        private int i_nextTentaclesStepIndex = 0;
        private int i_currentTentaclesStepIndex = 0;
        private int maxChildIndex;
        private bool b_directionDown = true;

        private void Awake()
        {
            Instance = this;
        }
        void Start()
        {
            InitializeCthulhu();
        }

        public void InitializeCthulhu() {
            foreach (Transform child in transform)
            {
                m_tentacles.Add(child.gameObject);
            }
            maxChildIndex = m_tentacles[0].transform.childCount - 1;
            MiniGameManager.OnUpdate += tentacleStep;
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

        private void tentacleStep() {
            int deactivateIndex=findDeactivateIndex();

            foreach (GameObject tentacle in m_tentacles) {
                CthulhuTentacleManager tentacleManager = tentacle.GetComponent<CthulhuTentacleManager>();

                if (tentacleManager.i_HitIndex == i_nextTentaclesStepIndex) {
                    tentacleManager.resetHit();
                }

                if (tentacleManager.i_HitIndex == -1) {
                    if (deactivateIndex != -1)
                    {
                        tentacle.transform.GetChild(deactivateIndex).gameObject.SetActive(false);
                    }
                    tentacle.transform.GetChild(i_nextTentaclesStepIndex).gameObject.SetActive(true);
                }
            }

            if ((i_nextTentaclesStepIndex == 0 && !b_directionDown) || (i_nextTentaclesStepIndex == maxChildIndex && b_directionDown)) {
                b_directionDown = !b_directionDown;
            }
            i_currentTentaclesStepIndex = i_nextTentaclesStepIndex;

            i_nextTentaclesStepIndex = b_directionDown ? i_nextTentaclesStepIndex + 1 : i_nextTentaclesStepIndex - 1;
        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}