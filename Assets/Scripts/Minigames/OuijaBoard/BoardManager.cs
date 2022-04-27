using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


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
        [SerializeField] float f_maxHeight = 1;
        [SerializeField] float f_lettersSpacing = 1.2f;

        [SerializeField] private List<string> usedWords = new List<string>();
        private List<GameObject> m_letterList = new List<GameObject>();
        
        int i_collID;
        string s_word;
        bool b_finish;


        private void Start()
        {   //Remove comment for debug
            //m_ouijaBoardManager.BeginMinigame(Difficulty.EASY);
        }

        public void Initialize()
        {
            int i_letterId = 0;
            float f_countXpos = 0;

            foreach (char c_letter in ouijaLetters)
            {
                f_countXpos += f_lettersSpacing;
                if (i_letterId == 13) f_countXpos = f_lettersSpacing;
                if (i_letterId < 13) Spawn(m_line1, c_letter, f_countXpos);
                else Spawn(m_line2, c_letter, f_countXpos);
                i_letterId++;
            }
        }

        public void Spawn(GameObject line, char letter, float f_Xpos)
        {
            GameObject obj_letter = Instantiate(m_letterPrefab, line.transform);
            Vector3 v3_parentPosition = line.gameObject.transform.position;
            obj_letter.transform.SetParent(line.gameObject.transform);
            obj_letter.transform.position = v3_parentPosition + new Vector3(f_Xpos, Random.Range(-f_maxHeight, f_maxHeight), 0);
            obj_letter.transform.eulerAngles += new Vector3(0, 0, Random.Range(-f_maxRot, f_maxRot));
            obj_letter.name = letter.ToString();
            obj_letter.GetComponent<TMP_Text>().text = letter.ToString();
            m_letterList.Add(obj_letter);
        }

        public void CheckWord(int i_lettersCount, string s_lettersConcat)
        {
            m_ouijaBoardUIManager.results.GetComponent<TMP_Text>().text = s_lettersConcat;
            b_finish = false;

            if (s_word == s_lettersConcat)
            {
                b_finish = true;
                ProposeWord();
            }
            else
            {
                i_collID = System.Array.IndexOf(ouijaLetters, char.ToUpper(s_word[i_lettersCount]));
                m_letterList[i_collID].GetComponent<TextMeshPro>().color = new Color32(200, 190, 0, 255);
                m_letterList[i_collID].GetComponent<Collider2D>().enabled = true;
            }
        }

        public void ProposeWord()
        {
            m_ouijaBoardUIManager.clearedWords.text = usedWords.Count.ToString() + "/" + m_ouijaBoardManager.WordsNum.ToString();
            s_word = ouijaWords[Random.Range(0, ouijaWords.Length)];
            m_selector.letters.Clear();

            if (usedWords.Count == m_ouijaBoardManager.WordsNum && b_finish) {
                m_ouijaBoardManager.b_gameFinished = true;
                m_ouijaBoardManager.b_win = true;
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
