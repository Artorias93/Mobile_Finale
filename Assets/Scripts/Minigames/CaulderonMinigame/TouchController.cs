using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;
public class TouchController : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    
    private Image img_cauldron;
    private Image img_ladle;
    private Vector3 m_vr3_input;

    void Start()
    {
        img_cauldron = GetComponent<Image>();
        img_ladle = transform.GetChild(0).GetComponent<Image>();
    }

    public virtual void OnDrag(PointerEventData pointerEventData)
    {
        Vector2 m_vt2_position;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(img_cauldron.rectTransform, pointerEventData.position, pointerEventData.pressEventCamera, out m_vt2_position))
        {
            m_vt2_position.x = (m_vt2_position.x / img_cauldron.rectTransform.sizeDelta.x);
            m_vt2_position.y = (m_vt2_position.y / img_cauldron.rectTransform.sizeDelta.y);
            Debug.Log(m_vt2_position);
            //normalized input
            m_vr3_input = new Vector3(m_vt2_position.x * 2 + 1, 0, m_vt2_position.y * 2 - 1);
            m_vr3_input = (m_vr3_input.magnitude > 1.0f) ? m_vr3_input.normalized : m_vr3_input;
            //ladle movement
            img_ladle.rectTransform.anchoredPosition = new Vector3(m_vr3_input.x * (img_cauldron.rectTransform.sizeDelta.x / 3),
                                                                    m_vr3_input.z * (img_cauldron.rectTransform.sizeDelta.y / 3));

        }
    }
    public virtual void OnPointerDown(PointerEventData pointerEventData)
    {
        OnDrag(pointerEventData);
    }
    public virtual void OnPointerUp(PointerEventData pointerEventData)
    {
        //reset
        //inputVector = Vector3.zero;
        //joystickImg.rectTransform.anchoredPosition = Vector3.zero;
    }

}


