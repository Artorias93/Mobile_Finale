using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FightCthulhu
{
    public class CthulhuTentacleManager : MonoBehaviour, IPointerDownHandler
    {
        public bool b_CanGrab {
            get { return b_canGrab; }
            set { b_canGrab = value; }
        }
        public int i_HitIndex
        {
            get { return i_hitIndex;}
            set { i_hitIndex = value; }
        }
        public GameObject g_GrabbedItem
        {
            get { return grabbedItem; }
            set { grabbedItem = value; }
        }

        private int i_hitIndex = -1;
        private bool b_canGrab = false;
        private GameObject grabbedItem = null;

        // Start is called before the first frame update
        public void resetHit() {
            i_hitIndex = -1;
        }
        public void OnPointerDown(PointerEventData eventData)
        {
            PlayerManager playerManager = PlayerManager.Instance;
            playerManager.setAttackTarget(this.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (b_canGrab) {
                grabbedItem = collision.gameObject;
                grabbedItem.GetComponent<DefendableItem>().b_isGrabbed = true;
            }
        }
    }
}
