using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;


namespace DecodeAudioMinigame
{
    public class BookManager : MonoBehaviour
    {
        [Header("UI Prefabs and References")]
        [SerializeField] private GameObject m_runesContainer;
        [SerializeField] private GameObject m_runePrefab;

        List<RuneBehaviour> m_runeBehaviours;

        private void Awake()
        {
            m_runeBehaviours = new List<RuneBehaviour>();

            Assert.IsNotNull(m_runePrefab);
            if (m_runesContainer == null)
                m_runesContainer = this.gameObject;
        }

        public void Initialize(int runeCount)
        {
            for (int i = 0; i < runeCount; i++)
            {
                GameObject go = Instantiate(m_runePrefab, m_runesContainer.transform);
                go.name = "Rune (" + i + ")";

                RuneBehaviour rune = go.GetComponent<RuneBehaviour>();
                Assert.IsNotNull(rune, "Cannot find rune script on rune prefab gameobject");
                m_runeBehaviours.Add(rune);
            }
        }

        public void UpdateRunes(List<RuneEntry> runeEntries)
        {
            if (runeEntries.Count != m_runeBehaviours.Count)
            {
                Debug.LogError("Rune Entries list and Rune Gameobjects list aren't aligned");
            }

            for (int i = 0; i < runeEntries.Count; i++)
            {
                m_runeBehaviours[i].Initialize(runeEntries[i]);
            }
        }

        public void DisableAll()
        {
            foreach (RuneBehaviour rune in m_runeBehaviours)
            {
                rune.Disable();
            }
        }
    }
}

