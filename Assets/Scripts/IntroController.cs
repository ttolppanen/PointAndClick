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
        "London, England 6th of January 1900:",

        "I suppose I should introduce who I am and what I’m supposed to do. My name is Alexis Haartman, son of " +
        "Alfred Haartman, who works as a Lutheran priest back at home.",

        "My father is a pious man who raised me in " +
        "a most loving and caring environment, I know the values of kindess, humility and understanding. I dare say " +
        "that my father is an upstanding beacon of light in our community! Not that that would, or should, reflect on " +
        "my character. ",

        "I’m going to be part of an expedition to the north pole, led by Sir Alister Barton. I’ve been terribly excited about all " +
        "this, you see, It’s been my true hearts desire to explore land that no human eyes have laid eyes upon " +
        "before, to see the majesty that our Lord left for us!",

        "I wish to share all that with the world, which is what this " +
        "journal is for. We’re wintering here in London while Sir Alister gathers a hearty crew for us to adventure " +
        "together with. ", 

        "The lodgings here are meagre, but I’ve been assured that we need all the pounds we can spare spent on the " +
        "equipment we will take with us. I’m no expert in explorers equipment so I will leave that to those who are. I " +
        "will be updating my journal at every significant point in our journey."
    };
    List<string> slide2Texts = new List<string>
    {
        "Port of Tyne, England 20th of February 1900:",

        "We’re in the port, looking for a suitable ship that will carry us toward the icy landscape up north. Here, at " +
        "the eve of our journey, I am reminded of why I chose to do this in the first place.",

        "I was a meek boy, studious, " +
        "but meek all the same, that is until in my studies I stumbled upon accounts by explorers such as Cada" +
        "Mosto and Vasco da Gama. What wonders they had seen, what beauty!",

        "And to think, they were among the " +
        "first civilized people to lay eyes upon that beauty, oh what a profound excitement it must have been! I " +
        "intend to follow the examples set by my ideological forefathers, and return home a wiser, more " +
        "experienced man.",

        "There is but two areas of note which carry the prestige of never having been seen by human eyes, the " +
        "Antarctica and the north pole. Being from the north myself, I thought it must be destined for me to be " +
        "among the first to conquer the north, which had eluded man for so long.",

        "(Ahm.. well anyway, uh) I needed " +
        "to join an expedition, so I began asking around, no one from my home country was wealthy enough to " +
        "worry about anything but their own.",

        "But since my language skills are good, and I had some money, I made " +
        "my way to England where, after a bit of searching, I found Sir Alister who just so happened to be searching " +
        "for a crew. He agreed to let me join his crew, after I contributed my fair share to the funding, of course.",

        "I must go now, I’m meeting the rest of the crew at the docks this afternoon, and I better get ready."
    };
    List<string> slide3Texts = new List<string>
    {
        "On board Aurora, somewhere in the Norwegian Sea 1st of March 1900.",

        "Well, we’ve set off, and I must say staying on a ship is a touch less romantic than it seems at first blush. I’ve been " +
        "heaving my guts out for the last week, but I’m feeling better. " +
        "Once we found the Aurora things moved quite fast, I was " +
        "introduced to the crew and we set off, taking with us all the equipment we could strap to ourselves along with a sled.",
        
        "Sir Alister has gathered a hardy crew of researchers and survivalists to aid in our venture to the north pole:",
        
        "Jack Cheshire: An Englishman, captain of the ship. A good man from what I’ve been told, doesn’t seem to " +
        "care for me though. I’m sure he will lead our way successfully to our destination.",
        
        "John Semmel: An American, he is a medical doctor. I’m not quite sure what he is doing on board a ship in all " +
        "the way over here, but he isn’t willing to talk about it, so I don’t proach the subject.",
        
        "Conall O’Rourke: As the name would suggest, he is from Ireland, there’s been tension between him and the " +
        "other English crew mates, but he and I get along fine. He is a geologist, I think that means he studies the " +
        "ground? Anyway, his help is appreciated.",
        
        "There are others in our fair crew, but their names escape me. I’m sure I’ll get to know all of them well " +
        "enough once this expedition is over, but I’m told that they’re all qualified in one way or another.",
        
        "We’ll make a little pitstop at Svalbard to restock any supplies we might need. After that it’s nothing but a " +
        "vast field of ice and snow, untouched by human hands! I can only imagine what we will uncover in that " +
        "mysterious land."
    };
    List<string> slide4Texts = new List<string>
    {
        "It’s cold.... got caught by a blizzard, no idea where we are. Some got separated, not found. down to 4 men, " +
        "not going to make it. I hope, someone finds us."
    };
    List<List<string>> allTexts;
    List<string> currentTexts;

    bool fading = false;
    bool writing = false;
    int slideIndex = 0;
    int textIndex = 0;

    private void Start()
    {
        allTexts = new List<List<string>> {slide1Texts, slide2Texts, slide3Texts, slide4Texts};
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
