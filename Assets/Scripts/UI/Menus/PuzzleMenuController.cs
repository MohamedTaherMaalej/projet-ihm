using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PuzzleMenuController : MenuController
{
    [SerializeField] private AudioClip bringUpSound;
    [SerializeField] private AudioClip resetSound;
    [SerializeField] private AudioClip skipSound;
    [SerializeField] private AudioClip backToMainSceneSound;
    [Tooltip("This is used to make the menu impossible to open once done")] public ProgressManager.ProgressPoint associatedProgressPoint;
    public List<GameObject> enableWhenShown;
    public InputAction bringUpPuzzleMenu;
    public event Action<bool> OnPuzzleMenuToggle;

    new void Start()
    {
        base.Start();
        enableWhenShown.Add(container);
        bringUpPuzzleMenu.Enable();
        bringUpPuzzleMenu.started += bringUp;
        ProgressManager.current.OnProgressUpdate += checkProgressPoint;
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return null;
        hide();
    }

    void OnDestroy()
    {
        bringUpPuzzleMenu.started -= bringUp;
        ProgressManager.current.OnProgressUpdate -= checkProgressPoint;
    }

    private void bringUp(InputAction.CallbackContext ctx)
    {
        bringUp();
    }
    public void bringUp()
    {
        if (!container.activeSelf)
            audioSource.PlayOneShot(bringUpSound);
        else
            audioSource.PlayOneShot(backSound);
        show(!container.activeSelf);
    }

    public void show(bool visible)
    {
        if (visible && ProgressManager.current.isDone(associatedProgressPoint))
            return; // abort when already done
        foreach (var go in enableWhenShown)
            go.SetActive(visible);
        OnPuzzleMenuToggle?.Invoke(visible);
        goToMainScreen(playSound: false);
    }

    public void hide()
    {
        show(false);
    }

    private void checkProgressPoint()
    {
        if (ProgressManager.current.isDone(associatedProgressPoint))
            hide();
    }

    public void resume()
    {
        audioSource.PlayOneShot(clickSound);
        hide();
    }

    public void resetPuzzle()
    {
        audioSource.PlayOneShot(resetSound);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name, LoadSceneMode.Single);
    }

    public void skipPuzzle()
    {
        audioSource.PlayOneShot(skipSound);
        UIEvents.OnPuzzleSkip?.Invoke();
    }

    public void backToMainScene()
    {
        audioSource.PlayOneShot(backToMainSceneSound);
        SceneManager.LoadScene("Bedroom", LoadSceneMode.Single);
    }
}
