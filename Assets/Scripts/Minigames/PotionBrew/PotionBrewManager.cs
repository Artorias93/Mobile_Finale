using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace PotionBrewMinigame
{
    public class PotionBrewManager : MonoBehaviour
    {
        [SerializeField]
        private PotionUiManager m_potionUIManager;

        private GameObject currentTilting;
        private float f_currentTiltWinAngleSeconds=0;
        private float f_currentShakeSeconds = 0;

        private bool b_canTilt=false;
        private bool b_canShake= false;
        private List<GameObject> selectablePotions;

        private void Start()
        {
            selectablePotions = m_potionUIManager.Initialize(3);
            for (int i = 0; i<selectablePotions.Count;i++) {
                GameObject elem = selectablePotions[i];
                elem.GetComponent<Button>().onClick.AddListener(() => { Check(elem.gameObject); });
            }
        }

        private void Check(GameObject tiltPotion) {
            if (currentTilting == null) {
                currentTilting = m_potionUIManager.AddTiltable(tiltPotion);
            }
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}