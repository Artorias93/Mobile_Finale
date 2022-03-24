using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
        if (hit != false && hit.collider != null)
        {
            Debug.Log("I'm hitting " + hit.collider.name);
        }
        //if (transform.position.y <= -10) Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        print("COLPITO!!!");
    }

}
