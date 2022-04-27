using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IngredientController : MonoBehaviour
{
    
    private float spawnTimeSeconds=3;
    [SerializeField]
    private GameObject ingredient;
    
    //[Header("Ingredients Sprites")]
    [SerializeField]
    public List<Sprite> listSpriteIngredients;

    
    void Start()
    {
        StartCoroutine(ButtonTimer());

    }
    private void Update()
    {
        
    }
    private IEnumerator ButtonTimer()
    {
        while (!ingredient.activeSelf)
        {
            yield return new WaitForSeconds(spawnTimeSeconds);

            Debug.Log("in corutine");
            spawnTimeSeconds = Random.Range(4, 7);
            ingredient.SetActive(true);
            SetImage();

            yield return new WaitForSeconds(spawnTimeSeconds);
            CheckGameOver();
            

            spawnTimeSeconds = Random.Range(2, 4);
            yield return new WaitForSeconds(spawnTimeSeconds);
            
        }
    }

    private void CheckGameOver()
    {
        if (ingredient.activeSelf) //se è true non hai premuto in tempo e disattiva
        {
            ingredient.SetActive(false);
            //gameover o perdi una vita
        }

    }

    private void SetImage()
    {
        int maxrand = listSpriteIngredients.Count;
        int rand = Random.Range(0, maxrand);
        Sprite s = (Sprite)listSpriteIngredients[rand];
        ingredient.GetComponent<Image>().sprite=s;
    }
}
