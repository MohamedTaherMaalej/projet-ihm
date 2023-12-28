using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionEndGame : MonoBehaviour
{
    public static TransitionEndGame current;
    public AudioClip callAudio;
    public GameObject transitionText;
    public GameObject endMessage;
    public GameObject creditsScreen;

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
        StartCoroutine(transition());
    }
    IEnumerator transition()
    {
        transitionText.SetActive(true);
        endMessage.SetActive(false);
        creditsScreen.SetActive(false);
        yield return null;
        AudioManager.current.pauseMusic();
        AudioManager.current.getSource().PlayOneShot(callAudio);
        yield return new WaitForSeconds(8);
        transitionText.SetActive(false);
        endMessage.SetActive(true);
        yield return new WaitForSeconds(5);
        AudioManager.current.playEndCreditsMusic();
        endMessage.SetActive(false);
        creditsScreen.SetActive(true);
        yield return new WaitForSeconds(20);
        AudioManager.current.playDefaultMusic();
        ProgressManager.current.startAnew(); // delete save
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    void OnDestroy()
    {
        current = null;
    }

    void OnEnable()
    {
        StartCoroutine(LateOnEnable());
    }
    private IEnumerator LateOnEnable()
    {
        yield return null; // wait one frame
        float dyaw = Camera.main.transform.rotation.eulerAngles.y - Camera.main.transform.parent.parent.rotation.eulerAngles.y;
        Camera.main.transform.parent.parent.Rotate(0, -dyaw, 0);
    }
}
