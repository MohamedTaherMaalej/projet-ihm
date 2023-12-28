using System;
using System.Collections;
using UnityEngine;

public class MenuController : MonoBehaviour
{
    public GameObject mainMenuPane;
    public GameObject controlsScreenPane;
    public GameObject container { get; private set; }
    [SerializeField] protected AudioClip clickSound;
    [SerializeField] protected AudioClip backSound;
    protected AudioSource audioSource;

    protected void Start()
    {
        try
        {
            container = gameObject.transform.parent.gameObject.transform.parent.gameObject;
            if (container == null)
                throw new Exception();
        }
        catch
        {
            Debug.LogWarning("Could not find container (2-level parent) of a menu controller object!");
        }
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return null;
        audioSource = AudioManager.current.getSource();
    }

    protected void OnEnable()
    {
        StartCoroutine(LateOnEnable());
    }
    private IEnumerator LateOnEnable()
    {
        yield return null; // wait one frame otherwise the buttons and sprites are not properly recreated
        bool volumeMuted = PlayerPrefs.GetInt("VolumeMuted") == 1;
        UIEvents.UpdateVolumeMute?.Invoke(volumeMuted); // useful on menu reload
    }

    public void toggleMute()
    {
        audioSource.PlayOneShot(clickSound);
        bool volumeMuted = PlayerPrefs.GetInt("VolumeMuted") == 1;
        volumeMuted = !volumeMuted;
        UIEvents.UpdateVolumeMute?.Invoke(volumeMuted);
    }

    public void goToMainScreen(bool playSound)
    {
        if (playSound)
            audioSource.PlayOneShot(backSound);
        mainMenuPane.SetActive(true);
        controlsScreenPane.SetActive(false);
        UIEvents.OnMenuPaneChanged?.Invoke();
    }
    public void goToMainScreen()
    {
        goToMainScreen(true);
    }

    public void goToControlsScreen()
    {
        audioSource.PlayOneShot(clickSound);
        mainMenuPane.SetActive(false);
        controlsScreenPane.SetActive(true);
        UIEvents.OnMenuPaneChanged?.Invoke();
    }

    public void playBackSound()
    {
        audioSource.PlayOneShot(backSound);
    }
}
