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


    [Header("Levels to load")]
    public string _newGameLevel;

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

    public void ResetButton(string MenuType)
    {
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
