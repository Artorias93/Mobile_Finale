using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace NecromancerShop
{
    public class DraggableItem : MonoBehaviour
    {
        [SerializeField]
        private Image m_checkedImage;
        [SerializeField]
        private Sprite s_checked;

        public bool b_IsIngredient
        {
            set { b_isIngredient = value; }
        }
        public DraggableItem listElement;

        private NecromancerShopGM m_gameManager;
        private Image image;
        private RectTransform rectTransform;
        private bool b_isDraggable=true;
        private Canvas m_baseCanvas;
        private Vector2 v_startPosition;
        private bool b_isIngredient = false;
        void Start()
        {
            m_baseCanvas = GameObject.FindGameObjectWithTag("MainUI").GetComponent<Canvas>();
            rectTransform = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            m_gameManager = GameObject.FindGameObjectWithTag("MinigameManager").GetComponent<NecromancerShopGM>();


        }

        public void DragHandler(BaseEventData eventData)
        {
            if (b_isDraggable) {
                if (v_startPosition == Vector2.zero)
                {
                    v_startPosition = rectTransform.anchoredPosition;
                }
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
        }

        public void stopDraggable() {
            b_isDraggable = false;
        }
        public void DropHandler(BaseEventData eventData)
        {
            if (v_startPosition != Vector2.zero)
            {
                rectTransform.anchoredPosition = v_startPosition;

            }
        }
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.CompareTag("ShopCart"))
            {
                if (b_isIngredient)
                {
                    b_isDraggable = false;
                    Color tmpColor = image.color;
                    tmpColor.a = 0;
                    image.color = tmpColor;
                    listElement.setChecked();
                    m_gameManager.updateCorrectCounter();
                }
                else {
                    m_gameManager.updateErrorCounter();
                }
            }

        }

        public void setChecked()
        {
            Color tmpColor = m_checkedImage.color;
            tmpColor.a = 1;
            m_checkedImage.color = tmpColor;
            m_checkedImage.sprite = s_checked;
        }
    }

}
