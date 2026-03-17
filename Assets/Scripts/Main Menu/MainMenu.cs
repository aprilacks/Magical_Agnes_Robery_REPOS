using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{
    [Header("Buttons")]
    public Canvas _Main;
    public Canvas _Options;

    [Header("Sound")]
    public AudioMixer _AudioMixer;
    public Slider _MusicSlider;


    private void Start()
    {
        if(PlayerPrefs.HasKey("musicVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume();
        }
            SetMusicVolume();
        _Main.enabled = true;
        _Options.enabled = false;
    }

    // MUSIC
    public void SetMusicVolume()
    {
        float volume = _MusicSlider.value;
        _AudioMixer.SetFloat("music", Mathf.Log10((volume) * 20));
        PlayerPrefs.SetFloat("music", Mathf.Log10((volume) * 20));

    }
    public void LoadVolume()
    {
        _MusicSlider.value = PlayerPrefs.GetFloat("music");
        SetMusicVolume();

    }

    // STARTS GAMEPLAY
    public void StartGame()
    {
        SceneManager.LoadScene("0 - Tutorial");
    }

    // CLOSE APPLICATION
    public void CloseGame()
    {
        Application.Quit();
    }

    // OPTIONS CANVAS
    public void Option()
    {
        _Main.enabled = false;
        _Options.enabled = true;
    }

    public void Back()
    {
        _Main.enabled = true;
        _Options.enabled = false;
    }



}
