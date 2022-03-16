using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class BookManager : MonoBehaviour
{
    [Header("UI Prefabs and References")]
    [SerializeField] private GameObject m_runesContainer;
    [SerializeField] private GameObject m_runePrefab;

    private void Awake()
    {
        Assert.IsNotNull(m_runePrefab);

        if (m_runesContainer == null)
            m_runesContainer = this.gameObject;


    }

    public void Initialize(List<RuneEntry> runeEntries)
    {
        for(int i = 0; i < runeEntries.Count; i++)
        {
            GameObject go = Instantiate(m_runePrefab, m_runesContainer.transform);
            go.name = "Rune (" + i + ")";

            go.GetComponent<RuneBehaviour>().Initialize(runeEntries[i]);
        }
    }
}
