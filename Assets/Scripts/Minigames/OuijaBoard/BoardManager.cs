using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace OuijaBoardMinigame
{
    public class BoardManager : MonoBehaviour
    {
        [Header("Game Parameters")]
        [SerializeField] private char[] ouijaLetters;
        [SerializeField] private string[] ouijaWords;

        [Header("Prefabs and References")]
        [SerializeField] private GameObject m_letterPrefab;
        [SerializeField] private Selector m_selector;
        [SerializeField] private GameObject m_line1;
        [SerializeField] private GameObject m_line2;
        [SerializeField] private OuijaBoardUIManager m_ouijaBoardUIManager;
        [SerializeField] private OuijaBoardManager m_ouijaBoardManager;

        [Header("Letters' Settings")]
        [SerializeField] float f_maxRot = 30;
        [SerializeField] float f_maxHeight;
        [SerializeField] float f_lettersSpacing = 1.2f;
        [SerializeField] float f_lettersScaling = 30f;


        [SerializeField] private List<string> usedWords = new List<string>();
        private List<GameObject> m_letterList = new List<GameObject>();
        
        int i_collID;
        string s_word;
        bool b_finish;

        public void Initialize()
        {
            f_maxHeight = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CameraResolution>().scaleheight / 2;
            int i_letterId = 0;
            float f_countXpos = 0;

            foreach (char c_letter in ouijaLetters)
            {
                if (i_letterId == 13) f_countXpos = 0;
                if (i_letterId < 13) Spawn(m_line1, c_letter, f_countXpos);

                else Spawn(m_line2, c_letter, f_countXpos);
                f_countXpos += f_lettersSpacing;

                i_letterId++;
            }

            LayoutRebuilder.ForceRebuildLayoutImmediate(m_line1.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(m_line2.GetComponent<RectTransform>());
            m_line1.GetComponent<GridLayoutGroup>().enabled = false;
            m_line2.GetComponent<GridLayoutGroup>().enabled = false;

            foreach (Transform letter in m_line1.transform) letter.transform.position += new Vector3(0, Random.Range(-f_maxHeight, f_maxHeight), 0);
            foreach (Transform letter in m_line2.transform) letter.transform.position += new Vector3(0, Random.Range(-f_maxHeight, f_maxHeight), 0);


        }

        public void Spawn(GameObject line, char letter, float f_Xpos)
        {
            GameObject obj_letter = Instantiate(m_letterPrefab, line.transform);
            obj_letter.transform.SetParent(line.gameObject.transform);
            obj_letter.transform.localScale *= f_lettersScaling;
            obj_letter.transform.eulerAngles += new Vector3(0, 0, Random.Range(-f_maxRot, f_maxRot));
            obj_letter.name = letter.ToString();
            obj_letter.GetComponent<TMP_Text>().text = letter.ToString();
            obj_letter.GetComponent<TMP_Text>().outlineColor = new Color32(120, 65, 0, 255);


            m_letterList.Add(obj_letter);
        }

        public void CheckWord(int i_lettersCount, string s_lettersConcat)
        {
            m_ouijaBoardUIManager.results.GetComponent<TMP_Text>().text = s_lettersConcat;
            b_finish = false;

            if (s_word == s_lettersConcat)
            {
                b_finish = true;
                m_ouijaBoardManager.wordGuessed();
                ProposeWord();
            }
            else
            {
                i_collID = System.Array.IndexOf(ouijaLetters, char.ToUpper(s_word[i_lettersCount]));
                m_letterList[i_collID].GetComponent<TextMeshPro>().color = new Color32(230, 135, 0, 255);
                m_letterList[i_collID].GetComponent<Collider2D>().enabled = true;
            }
        }

        public void ProposeWord()
        {
            m_ouijaBoardUIManager.clearedWords.text = usedWords.Count.ToString() + "/" + m_ouijaBoardManager.WordsNum.ToString();
            s_word = ouijaWords[Random.Range(0, ouijaWords.Length)];
            m_selector.letters.Clear();

            if (usedWords.Count == m_ouijaBoardManager.WordsNum && b_finish) {
                m_ouijaBoardManager.GameCompleted();
            }
            else if (!usedWords.Contains(s_word) && usedWords.Count < m_ouijaBoardManager.WordsNum)
            {
                usedWords.Add(s_word);
                m_ouijaBoardUIManager.target.GetComponent<TMP_Text>().text = s_word;
                CheckWord(0, "");
            }
            else ProposeWord();
        }

        public void freezeSelector(bool b_isFreeze) {
            m_selector.freezeSelector(b_isFreeze);
        }
    }
}
