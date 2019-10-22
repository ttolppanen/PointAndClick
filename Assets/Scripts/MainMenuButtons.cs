using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    public Slider musicSlider;
    public Slider effectSlider;

    public void Begin()
    {
        SceneManager.LoadScene("Intro");
    }

    public void Settings()
    {
        print("Tee tämä!");
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void SetMusicVolume()
    { 
        GameManager.instance.SetMusicVolume(musicSlider.value);
    }

    public void SetEffectVolume()
    {
        GameManager.instance.SetEffectVolume(effectSlider.value);
    }
}
