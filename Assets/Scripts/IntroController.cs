using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class IntroController : MonoBehaviour
{
    public SpriteRenderer fade;
    public float fadeTime;
    public float textUpdateTime;
    public TextMeshProUGUI textComponent;
    public List<GameObject> slides;
    GameObject currentSlide;
    List<string> slide1Texts = new List<string>
    {
        "I suppose I should introduce who I am and what I’m supposed to do. My name is Alexis Haartman, son of " +
        "(TBD) Haartman, who works as a Lutheran priest back at home. ",

        "My father is a pious man who raised me in " +
        "a most loving and caring environment, I know the values of kindess, humility and understanding. I dare say " +
        "that my father is an upstanding beacon of light in our community! Not that that would, or should, reflect on " +
        "my character. ", 

        "I’m going to be part of an expedition to the north pole, led by Sir (TBD2). I’ve been terribly excited about all " +
        "this, you see, It’s been my true hearts desire to explore land that no human eyes have laid eyes upon " +
        "before, to see the majesty that our Lord left for us!",

        "I wish to share all that with the world, which is what this " +
        "journal is for. We’re wintering here in London while Sir (TBD2) gathers a hearty crew for us to adventure " +
        "together with. ", 

        "The lodgings here are meagre, but I’ve been assured that we need all the pounds we can spare spent on the " +
        "equipment we will take with us. I’m no expert in explorers equipment so I will leave that to those who are. I " +
        "will be updating my journal at every significant point in our journey."
    };
    List<string> slide2Texts = new List<string>
    {
        "Kuitenkin kaikesta tärkeintä on tehdä näin",
        "Jotta myöhemmin voidaan tehdä näin sitten lopulta kuitenkin."
    };
    List<List<string>> allTexts;
    List<string> currentTexts;

    bool fading = false;
    bool writing = false;
    int slideIndex = 0;
    int textIndex = 0;

    private void Start()
    {
        allTexts = new List<List<string>> {slide1Texts, slide2Texts};
        currentTexts = slide1Texts;
        currentSlide = slides[0];
        StartCoroutine(FadeIn());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !fading && !writing)
        {
            if (textIndex >= currentTexts.Count)
            {
                slideIndex += 1;
                if (slideIndex >= slides.Count)
                {
                    print("Vaihda scene");
                }
                else
                {
                    textIndex = 0;
                    currentTexts = allTexts[slideIndex];
                    StartCoroutine(FadeOut());
                }
            }
            else
            {
                writing = true;
                textComponent.text = "";
                StartCoroutine(WriteText(currentTexts[textIndex], 0));
                textIndex += 1;
            }
        }
    }

    public IEnumerator FadeOut()
    {
        fading = true;
        Color fadeColor = fade.color;
        if (fadeColor.a >= 1)
        {
            StartCoroutine(FadeIn());
            currentSlide.SetActive(false);
            currentSlide = slides[slideIndex];
            currentSlide.SetActive(true);
            yield break;
        }
        fadeColor.a += fadeTime / 100;
        fade.color = fadeColor;
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(FadeOut());
    }

    public IEnumerator FadeIn()
    {
        fading = true;
        Color fadeColor = fade.color;
        if (fadeColor.a <= 0)
        {
            fading = false;
            writing = true;
            textComponent.text = "";
            StartCoroutine(WriteText(currentTexts[textIndex], 0));
            textIndex += 1;
            yield break;
        }
        fadeColor.a -= fadeTime / 100;
        fade.color = fadeColor;
        yield return new WaitForSeconds(0.01f);
        StartCoroutine(FadeIn());
    }

    IEnumerator WriteText(string text, int i)
    {
        if (i >= text.Length)
        {
            writing = false;
            yield break;
        }
        textComponent.text = textComponent.text + text[i];
        yield return new WaitForSeconds(textUpdateTime);
        StartCoroutine(WriteText(text, i + 1));
    }
}
