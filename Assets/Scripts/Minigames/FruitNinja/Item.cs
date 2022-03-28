using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{

    public GameObject obj_fruitSlicedPrefab;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

    }


    void OnTriggerEnter2D(Collider2D col)
    {

        if (col.CompareTag("Blade"))
        {
            //Vector3 direction = (col.transform.position - transform.position).normalized;

            //Quaternion rotation = Quaternion.LookRotation(direction);

            GameObject obj_slicedFruit = Instantiate(obj_fruitSlicedPrefab, transform.position, transform.rotation);
            Destroy(obj_slicedFruit, 3f);
            Destroy(gameObject);
        }
    }

}
