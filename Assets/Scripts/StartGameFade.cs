using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartGameFade : MonoBehaviour
{
    private bool b_canFade = false;
    private float f_currentAlpha = 0;
    [SerializeField] private float f_fadeSpeed = 2;

    // Start is called before the first frame update
    void Start()
    {
        var spriteRenderer = this.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer sr in spriteRenderer)
        {
            sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, 0);
        }

        var images = this.GetComponentsInChildren<Image>();
        foreach (Image img in images)
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        }

        var texts = this.GetComponentsInChildren<Text>();
        foreach (Text txt in texts)
        {
            txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 0);
        }
        
        StartMenuFade();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (b_canFade)
        {
            f_currentAlpha += Time.deltaTime * f_fadeSpeed;
            for (int i = 0; i < 2; i++)
            {
                GameObject menu = transform.GetChild(i).gameObject;

                var spriteRenderer = menu.GetComponentsInChildren<SpriteRenderer>();
                foreach (SpriteRenderer sr in spriteRenderer)
                {
                    sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, sr.color.a + (Time.deltaTime * f_fadeSpeed));
                }

                var images = menu.GetComponentsInChildren<Image>();
                foreach (Image img in images)
                {
                    img.color = new Color(img.color.r, img.color.g, img.color.b, img.color.a + (Time.deltaTime * f_fadeSpeed));
                }

                var texts = menu.GetComponentsInChildren<Text>();
                foreach (Text txt in texts)
                {
                    txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, txt.color.a + (Time.deltaTime * f_fadeSpeed));
                }

                if (f_currentAlpha > 1) b_canFade = false;
            }
        }
    }

    public void StartMenuFade()
    {
        b_canFade = true;
    }
}
