using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FightCthulhu
{
    public class CthulhuTentacleManager : MonoBehaviour, IPointerDownHandler
    {
        public int i_HitIndex
        {
            get { return i_hitIndex;}
            set { i_hitIndex = value; }
        }
        private int i_hitIndex = -1;

        // Start is called before the first frame update
        public void resetHit() {
            i_hitIndex = -1;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            PlayerManager playerManager = PlayerManager.Instance;
            playerManager.setAttackTarget(this.gameObject);
        }
    }
}
