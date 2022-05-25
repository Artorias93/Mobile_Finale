using System.Collections;
using System.Collections.Generic;
using PotionBrewMinigame;
using UnityEngine;
using UnityEngine.UI;

public class PotionUiManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_tiltingContainer;
    [SerializeField]
    private GameObject m_selectablePotionsContainer;
    [SerializeField]
    private GameObject m_shakeContainer;
    [SerializeField]
    private GameObject m_tiltingItemPrefab;
    [SerializeField]
    private GameObject m_selectableItemPrefab;
    [SerializeField]
    private GameObject m_shakingItemPrefab;
    [SerializeField]
    private Image selectPotionImage;


    public Text m_stepText;
    public Text m_flowText;
    public GameObject m_shakePotion;
    private List<string> s_colors = new List<string>{ "#00B3FF", "#00FF26", "#FAFF00", "#FF0022", "#B900FF" };


    public List<GameObject> Initialize(int i_potionsCount)
    {
        m_shakePotion= Instantiate(m_shakingItemPrefab, m_shakeContainer.transform);
        List<GameObject> selectableObjects = new List<GameObject>();
        Color potionColor;
        for (int i = 0; i < i_potionsCount; i++)
        {
            GameObject go = Instantiate(m_selectableItemPrefab, m_selectablePotionsContainer.transform);
            go.name = "Selectable" + i;
            
            int i_colorIndex = Random.Range(0,s_colors.Count);
            ColorUtility.TryParseHtmlString(s_colors[i_colorIndex], out potionColor);
            go.transform.GetChild(0).GetComponent<Image>().color =potionColor;
            s_colors.RemoveAt(i_colorIndex);
            selectableObjects.Add(go);
        }
        return selectableObjects;
    }

    public GameObject AddTiltable(GameObject tiltPotion) {
       GameObject go= Instantiate(m_tiltingItemPrefab, m_tiltingContainer.transform);
       go.GetComponent<SpriteRenderer>().color = tiltPotion.transform.GetChild(0).GetComponent<Image>().color;
       return go;
    }

    public void RemoveTiltable()
    {
        Destroy(m_tiltingContainer.transform.GetChild(0).gameObject);
    }

    public void setSelectImage(Color newColor) {
        selectPotionImage.transform.GetChild(0).GetComponent<Image>().color = newColor;
        selectPotionImage.gameObject.SetActive(true);
    }

    public void RemoveSelectImage() {
        selectPotionImage.gameObject.SetActive(false);
    }
}
