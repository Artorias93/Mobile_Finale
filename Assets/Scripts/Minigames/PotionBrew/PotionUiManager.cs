using System.Collections;
using System.Collections.Generic;
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


    // Start is called before the first frame update
    void Start()
    {
        
    }
    public List<GameObject> Initialize(int i_potionsCount)
    {
        Instantiate(m_shakingItemPrefab, m_shakeContainer.transform);
        //Instantiate(m_tiltingItemPrefab, m_tiltingContainer.transform);
        List<GameObject> selectableObjects = new List<GameObject>();
        for (int i = 0; i < i_potionsCount; i++)
        {
            GameObject go = Instantiate(m_selectableItemPrefab, m_selectablePotionsContainer.transform);
            go.name = "Selectable" + i;
            go.GetComponent<Image>().color = UnityEngine.Random.ColorHSV();
            selectableObjects.Add(go);
        }
        return selectableObjects;
    }

    public GameObject AddTiltable(GameObject tiltPotion) {
       GameObject go= Instantiate(m_tiltingItemPrefab, m_tiltingContainer.transform);
       go.GetComponent<SpriteRenderer>().color = tiltPotion.GetComponent<Image>().color;
       return go;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
