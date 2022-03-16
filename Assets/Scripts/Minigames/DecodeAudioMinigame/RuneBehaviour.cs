using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class RuneBehaviour : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Image m_runeImage;
    [SerializeField] private Text m_nameTxt;

    private void Awake()
    {
        Assert.IsNotNull(m_runeImage, "No Rune Img Reference Selected");
        Assert.IsNotNull(m_nameTxt, "No Text Reference Selected");
    }

    public void Initialize(RuneEntry runeEntry)
    {
        m_runeImage.sprite = runeEntry.m_sprite;
        m_nameTxt.text = runeEntry.m_runeText;
    }
}
