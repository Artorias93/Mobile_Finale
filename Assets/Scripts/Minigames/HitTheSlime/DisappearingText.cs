using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisappearingText : MonoBehaviour
{

    [Header("Movement Parameters")]
    [SerializeField] private float f_disappearingSpeed = 5.0f;
    [SerializeField] private float f_scalingSpeed = 1.0f;
    [SerializeField] private float f_movementSpeed = 0.5f;
    [SerializeField] private float f_rotationSpeed = 0.1f;

    private Vector3 v2_randomDirection;

    [Header("Appearance")]
    public Color[] m_colors;
    [SerializeField] public Text m_txtComponent;
    [SerializeField] private RectTransform m_rectTransform;
    [SerializeField] private float killThreshold = 0.1f;

    private void Start()
    {
        //Assign Random Color and Direction
        v2_randomDirection = Random.insideUnitCircle.normalized;

        int randomIndex = Random.Range(0, m_colors.Length);
        m_txtComponent.color = m_colors[randomIndex];

    }

    public void AssignText(string txt)
    {
        m_txtComponent.text = txt;
    }

    void LateUpdate()
    {
        //Destroy text when it got too small, or too transparent
        bool readyForDestruction = m_rectTransform.localScale.x < 0 || m_rectTransform.localScale.y < 0 || m_txtComponent.color.a < killThreshold;
        if (readyForDestruction)
        {
            Destroy(gameObject);
            return;
        }
        float delta = Time.deltaTime;

        m_rectTransform.localScale -= f_scalingSpeed * delta * Vector3.one;
        m_rectTransform.localPosition += f_movementSpeed * delta * v2_randomDirection;

        Color newColor = m_txtComponent.color;
        newColor.a -= f_disappearingSpeed * delta;
        m_txtComponent.color = newColor;
        
    }
}
