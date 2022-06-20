using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fade : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartFade()
    {
        var spriteRenderes = this.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderes)
        {
            sr.color -= new Color(0, 0, 0, 0.5f);
        }

        var images = this.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            img.color -= new Color(0, 0, 0, 0.5f);
        }
    }

}
