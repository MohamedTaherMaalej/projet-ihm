using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuController : MenuController
{
    public static MainMenuController current;
    public Button continueButton;
    [SerializeField] private AudioClip enterGameSound;
    [SerializeField] private AudioClip quitSound;

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

    new void Start()
    {
        base.Start();
        goToMainScreen(playSound: false);
        continueButton.interactable = ProgressManager.current.saveDataExists();
    }

    void OnDestroy()
    {
        current = null;
    }

    new void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(LateOnEnable());
    }
    private IEnumerator LateOnEnable()
    {
        yield return null; // wait one frame
        float dyaw = Camera.main.transform.rotation.eulerAngles.y - Camera.main.transform.parent.parent.rotation.eulerAngles.y;
        Camera.main.transform.parent.parent.Rotate(0, -dyaw, 0);
        continueButton.interactable = ProgressManager.current.saveDataExists();
    }

    public void quit()
    {
        audioSource.PlayOneShot(quitSound);
        StartCoroutine(waitBeforeQuit());
    }
    IEnumerator waitBeforeQuit()
    {
        yield return new WaitForSeconds(0.5f);
        Debug.Log("Quitting Game!");
        Application.Quit();
    }

    public void newGame()
    {
        ProgressManager.current.startAnew();
        enterGame();
    }

    public void enterGame()
    {
        audioSource.PlayOneShot(enterGameSound);
        SceneManager.LoadScene("Bedroom", LoadSceneMode.Single);
    }
}
