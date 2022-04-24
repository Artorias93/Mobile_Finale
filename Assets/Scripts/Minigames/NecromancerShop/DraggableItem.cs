using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace NecromancerShop
{
    public class DraggableItem : MonoBehaviour
    {
        private Canvas m_baseCanvas;
        private Vector2 v_startPosition;
        private bool b_isIngredient = true;
        void Start()
        {
            m_baseCanvas = GameObject.FindGameObjectWithTag("MainUI").GetComponent<Canvas>();
            v_startPosition = transform.position;
        }

        public void DragHandler(BaseEventData eventData)
        {
            PointerEventData pointerData = (PointerEventData)eventData;
            Vector2 position;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                (RectTransform)m_baseCanvas.transform,
                pointerData.position,
                m_baseCanvas.worldCamera,
                out position
                );
            transform.position = m_baseCanvas.transform.TransformPoint(position);
        }

        public void DropHandler(BaseEventData eventData)
        {
                transform.position = v_startPosition;
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (b_isIngredient && collision.gameObject.CompareTag("ShopCart"))
            {
                Destroy(gameObject, 0.05f);
            }

        }
    }

}
