using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuController : MonoBehaviour
{
    [Header("Volume Settings")]
    [SerializeField] private TMP_Text volumeTextValue = null;
    [SerializeField] private Slider volumeSlider = null;
    [SerializeField] private float defaultVolume = 1.0f;

    [Header("Confirmation")]
    [SerializeField] private GameObject confirmationPrompt = null;

    [Header("Gameplay Settings")]
    [SerializeField] private TMP_Text mouseSenTextValue = null;
    [SerializeField] private Slider mouseSenSlider = null;
    [SerializeField] private int defaultSen = 4;
    public int mainMouseSen = 4;
    //voor mouse sense te kunnen aanpassen moeten we nog een fake mouse createn.

    [Header("Toggle Settings")]
    //empty shit hier
    [Header("Graphics Settings")]
    [SerializeField] private Slider brightnessSlider = null;
    [SerializeField] private TMP_Text brightnessTextValue = null;
    [SerializeField] private float defaultBrightness = 0.5f;

    [Space(10)]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    [SerializeField] private Toggle fullScreenToggle;

    private int _qualityLevel;
    private bool _isFullScreen;
    private float _brightnessLevel;

    [Header("Levels to load")]
    public string _newGameLevel;

    [Header("Resolution Dropdowns")]
    public TMP_Dropdown resolutionDropdown;
    private Resolution[] resolutions;
    private Resolution SelectedResolution;
    void ResolutionValue(TMP_Dropdown change)
    {
        SelectedResolution = resolutions[change.value];
    }

    private void Start()
    {
        //Screen.SetResolution(1920, 1080, true);
        //Harcoded voor emergency
          resolutions = Screen.resolutions;
          resolutionDropdown.ClearOptions();

          List<string> options = new List<string>();
          int currentResolutionIndex = 0;

          for (int i = 0; i < resolutions.Length; i++)
          {
              string option = resolutions[i].width + " x " + resolutions[i].height;
              options.Add(option);

              if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
              {
                  currentResolutionIndex = i;
              }
          }
          resolutionDropdown.AddOptions(options);
          resolutionDropdown.value = currentResolutionIndex;
          resolutionDropdown.RefreshShownValue();
        resolutionDropdown.onValueChanged.AddListener(delegate {
            ResolutionValue(resolutionDropdown);
        });
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }


    public void StartGameYes()
    {
        SceneManager.LoadScene(_newGameLevel);
    }
    public void ExitButton()
    {
        Application.Quit();
    }

    public void SetVolume(float volume)
    {

        AudioListener.volume = volume;
        volumeTextValue.text = volume.ToString("0.0");
    }
    public void VolumeApply()
    {
        PlayerPrefs.SetFloat("masterVolume", AudioListener.volume);
        StartCoroutine(ConfirmationBox());
    }

    public void SetMouseSen(float sensitivity)
    {
        mainMouseSen = Mathf.RoundToInt(sensitivity);
        mouseSenTextValue.text = sensitivity.ToString("0");
    }
    
    public void GameplayApply()
    {
        PlayerPrefs.SetFloat("masterSen", mainMouseSen);
        StartCoroutine(ConfirmationBox());
    }

    public void GraphicsApply()
    {
        PlayerPrefs.SetFloat("masterBrightness", _brightnessLevel);
        //change brightness with post processing

        PlayerPrefs.SetInt("masterQuality", _qualityLevel);
        QualitySettings.SetQualityLevel(_qualityLevel);

        PlayerPrefs.SetInt("masterFullscreen", (_isFullScreen ? 1 : 0));
        Screen.fullScreen = _isFullScreen;

        Screen.SetResolution(SelectedResolution.width, SelectedResolution.height, true);
        
        StartCoroutine(ConfirmationBox());
    }

    public void SetBrightness(float brightness)
    {
        _brightnessLevel = brightness;
        brightnessTextValue.text = brightness.ToString("0.0");
    }

    public void SetFullScreen(bool isFullsceen)
    {
        _isFullScreen = isFullsceen;
    }

    public void SetQuality(int qualityIndex)
    {
        _qualityLevel = qualityIndex;
    }

    public void ResetButton(string MenuType)
    {
        if (MenuType == "Graphics")
        {
            //reset brightness
            brightnessSlider.value = defaultBrightness;
            brightnessTextValue.text = defaultBrightness.ToString("0.0");

            qualityDropdown.value = 3;
            QualitySettings.SetQualityLevel(3);

            fullScreenToggle.isOn = false;
            Screen.fullScreen = false;

            Resolution currenResolution = Screen.currentResolution;
            Screen.SetResolution(currenResolution.width, currenResolution.height, Screen.fullScreen);
            resolutionDropdown.value = resolutions.Length;
            GraphicsApply();
        }

        if (MenuType == "Audio")
        {
            AudioListener.volume = defaultVolume;
            volumeSlider.value = defaultVolume;
            volumeTextValue.text = defaultVolume.ToString("0.0");
            VolumeApply();
        }
        
        if (MenuType == "Gameplay")
        {
            mouseSenTextValue.text = defaultSen.ToString("0");
            mouseSenSlider.value = defaultSen;
            mainMouseSen = defaultSen;
            GameplayApply();
        }
    }
    public IEnumerator ConfirmationBox()
    {
        confirmationPrompt.SetActive(true);
        yield return new WaitForSeconds(2);
        confirmationPrompt.SetActive(false);

    }
}
