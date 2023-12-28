using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager current;
    public AudioClip defaultMusic;
    public AudioClip puzzleMusic;
    public AudioClip endCreditsMusic;
    public AudioClip enterPuzzleSound;
    public bool isMuted { get { return PlayerPrefs.GetInt("VolumeMuted") != 0; } }
    private AudioSource audioSource;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            Destroy(obj: this);
        }
    }

    void Start()
    {
        if (!PlayerPrefs.HasKey("VolumeMuted"))
        {
            PlayerPrefs.SetInt("VolumeMuted", 0);
        }
        audioSource = GetComponent<AudioSource>();
        changeMusic(SceneManager.GetActiveScene());
        SceneManager.activeSceneChanged += changedScene;
        UIEvents.UpdateVolumeMute += musteAudioListener;
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= changedScene;
        UIEvents.UpdateVolumeMute -= musteAudioListener;
        // current = null; // this should be global
    }

    private void changedScene(Scene oldScene, Scene newScene)
    {
        changeMusic(newScene);
    }

    private void changeMusic(Scene scene)
    {
        switch (scene.name)
        {
            case "HanoiTower":
            case "RushHour":
                audioSource.PlayOneShot(enterPuzzleSound);
                playPuzzleMusic();
                break;
            case "MainMenu":
            case "Bedroom":
            default:
                playDefaultMusic();
                break;
        }
    }

    private void musteAudioListener(bool muted)
    {
        PlayerPrefs.SetInt("VolumeMuted", muted ? 1 : 0);
        AudioListener.volume = muted ? 0 : 1;
    }

    public AudioSource getSource()
    {
        return audioSource;
    }

    public void playDefaultMusic()
    {
        audioSource.clip = defaultMusic;
        audioSource.Play();
        muteGlobalSource(false);
    }

    public void playPuzzleMusic()
    {
        audioSource.clip = puzzleMusic;
        audioSource.Play();
        muteGlobalSource(false);
    }

    public void playEndCreditsMusic()
    {
        audioSource.clip = endCreditsMusic;
        audioSource.Play();
        muteGlobalSource(false);
    }

    public void pauseMusic()
    {
        audioSource.Pause();
    }

    public void muteGlobalSource(bool muted)
    {
        audioSource.mute = muted;
    }
}
