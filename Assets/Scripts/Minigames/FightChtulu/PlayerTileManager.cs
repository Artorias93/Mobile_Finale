using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace FightChtulu
{
    public class PlayerTileManager : MonoBehaviour, IPointerDownHandler
    {
        private PlayerManager playerManager;

        public void OnPointerDown(PointerEventData eventData)
        {
            playerManager = PlayerManager.Instance;
            playerManager.setPlayerRequestedPosition(this.gameObject);
        }
    }
}