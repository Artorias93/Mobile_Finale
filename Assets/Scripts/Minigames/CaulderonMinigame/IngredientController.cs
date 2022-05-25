using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CouldronMinigame 
{ 
    public class IngredientController : MonoBehaviour
    {

        [SerializeField]
        private CauldronGameManager gameManager;


        [SerializeField]
        private GameObject ingredient;


        private float lives;
        private float spawnTimeSeconds;

        [SerializeField]
        public List<Sprite> listSpriteIngredients;

        public float Lives 
        {
        
            get { return lives; }
            set { lives = value; }
        }
        public float SpawnTimeSeconds
        {

            get { return spawnTimeSeconds; }
            set { spawnTimeSeconds = value; }
        }


        void Start()
        {

            spawnTimeSeconds = gameManager.m_gameParameters.spawnTimeSeconds;
            lives = gameManager.m_gameParameters.max_lives;

            StartCoroutine(ButtonTimer());

        }
        private void Update()
        {
            if (gameManager.GameFinished)
                StopAllCoroutines();
        }


        private IEnumerator ButtonTimer()
        {
            while (!ingredient.activeSelf && lives!=0)
            {
                yield return new WaitForSeconds(spawnTimeSeconds);

                ingredient.SetActive(true);
                SetImage();

                yield return new WaitForSeconds(spawnTimeSeconds);
                CheckLives();
            

                yield return new WaitForSeconds(spawnTimeSeconds);
            
            }
        }

        private void CheckLives()
        {
            
               
            if (ingredient.activeSelf) //se è true non hai premuto in tempo e disattiva
            {
                ingredient.SetActive(false);
                lives--;
                if (lives <= 0)
                {
                    EndGame();
                    StopCoroutine(ButtonTimer());                   
                }            
            }

        }

        private void EndGame()
        {
            gameManager.LoseMinigame();
        }
        private void SetImage()
        {
            int maxrand = listSpriteIngredients.Count;
            int rand = Random.Range(0, maxrand);
            Sprite s = (Sprite)listSpriteIngredients[rand];
            ingredient.GetComponent<Image>().sprite=s;
        }
    }
}
