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
        [SerializeField] private GameObject ingredientContainer;
        [SerializeField] private GameObject ingredientBackground;
        [SerializeField] private GameObject ingredient;
        [SerializeField]
        private List<Sprite> listSpriteIngredients;
        [SerializeField] private float m_scaleFactor = 0.2f;
        [SerializeField] private float f_timeToLerpScale = 1.0f;

        private float lives;
        private float spawnTimeSeconds;
        private float f_timeElapsed = 0;
        private Vector3 maxScale;
        private Vector3 minScale;
        private Vector3 startingScale;
        private Vector3 targetScale;

        private RectTransform ContainerRectTransform;

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

        private void Start()
        {
            ContainerRectTransform = ingredientBackground.GetComponent<RectTransform>();

            Vector3 scale = ContainerRectTransform.localScale;
            
            maxScale = scale * (1 + m_scaleFactor);
            minScale = scale * (1 - m_scaleFactor);

            startingScale = scale;
            targetScale = maxScale;
        }
        private void Update()
        {
            if (gameManager.GameFinished) {
                StopAllCoroutines();
                return;
            }

            if (ingredientContainer.gameObject.activeInHierarchy)
            {
                bool lerpCompleted = f_timeElapsed > f_timeToLerpScale;

                if (lerpCompleted)
                {
                    if (targetScale == maxScale)
                    {
                        startingScale = maxScale;
                        targetScale = minScale;
                    }
                        
                    else if (targetScale == minScale)
                    {
                        startingScale = minScale;
                        targetScale = maxScale;
                    }
                    f_timeElapsed = 0;
                }

                ContainerRectTransform.localScale= Vector3.Lerp(startingScale, targetScale, f_timeElapsed / f_timeToLerpScale);
                f_timeElapsed += Time.deltaTime;

            }

            
        }


        private IEnumerator ButtonTimer()
        {
            while (!ingredientContainer.activeSelf && lives!=0)
            {
                yield return new WaitForSeconds(spawnTimeSeconds);

                ingredientContainer.SetActive(true);
                SetImage();

                yield return new WaitForSeconds(spawnTimeSeconds);
                CheckLives();
            

                yield return new WaitForSeconds(spawnTimeSeconds);
            
            }
        }

        private void CheckLives()
        {
            
               
            if (ingredientContainer.activeSelf) //se è true non hai premuto in tempo e disattiva
            {
                ingredientContainer.SetActive(false);
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

        public void InitializeValue()
        {
            CauldronGameManager gm = CauldronGameManager.Instance;
            spawnTimeSeconds = gm.GameParameters.spawnTimeSeconds;

            spawnTimeSeconds = gm.GameParameters.spawnTimeSeconds;
            lives = gm.GameParameters.max_lives;
            StartCoroutine(ButtonTimer());
        }
    }
}
