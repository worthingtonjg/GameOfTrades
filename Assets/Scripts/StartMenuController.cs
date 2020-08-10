using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    public List<Image> images;
    public float fadeRate = .001f;
    public float waitTime = 3f;
    public int imageIndex = 0;
    public bool isFading;

    // Start is called before the first frame update
    void Start()
    {
        HideImages();

        imageIndex = Random.Range(0, images.Count);
        SetAlpha(images[imageIndex], 1);
        InvokeRepeating("StartFade", waitTime, waitTime);
    }

    // Update is called once per frame
    void Update()
    {
        if(isFading)
        {
            Color c1 = images[imageIndex].color;
            Color c2 = images[nextImage()].color;

            if(c1.a > 0)
            {
                SetAlpha(images[imageIndex], c1.a-= fadeRate);
                SetAlpha(images[nextImage()], c2.a+= fadeRate);
            }
            else
            {
                SetAlpha(images[imageIndex], 0);
                SetAlpha(images[nextImage()], 1);

                isFading = false;
                imageIndex = nextImage();
            }
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("02MainScene");
    }

    private void StartFade()
    {
        if(isFading) return;

        print("fade");
        isFading = true;
    }

    private void HideImages()
    {
        images.ForEach(i => 
        { 
            SetAlpha(i, 0);
        });
    }

    private void SetAlpha(Image i, float alpha)
    {
        Color c = i.color;
        c.a = alpha;
        i.color = c;
    }

    private int nextImage()
    {
        if(imageIndex >= images.Count - 1)
        {
            return 0;
        }

        return imageIndex + 1;
    }
}
