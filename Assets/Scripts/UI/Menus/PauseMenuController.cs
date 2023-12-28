using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MenuController
{
    public static PauseMenuController current;
    [SerializeField] private AudioClip bringUpSound;
    [SerializeField] private AudioClip backToMainMenuSound;
    public float menuDistance = 1;
    public float menuBumpUp = 0;
    public GameObject leftHandToBeHidden;
    public GameObject rightHandToBeHidden;
    public List<GameObject> enableWhenShown;
    public List<GameObject> disableWhenShown;
    public InputAction bringUpPauseMenu = new InputAction();
    public bool shown { get; private set; }
    public event Action<bool> OnPauseMenuToggle;

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
        enableWhenShown.Add(container);
        bringUpPauseMenu.Enable();
        bringUpPauseMenu.started += bringUp;
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return null;
        disableWhenShown.Add(leftHandToBeHidden.GetComponentInChildren<SkinnedMeshRenderer>().gameObject);
        disableWhenShown.Add(rightHandToBeHidden.GetComponentInChildren<SkinnedMeshRenderer>().gameObject);
        shown = true;
        hide();
    }

    void OnDestroy()
    {
        bringUpPauseMenu.started -= bringUp;
        current = null;
    }

    private void bringUp(InputAction.CallbackContext ctx)
    {
        if (!container.activeSelf)
            audioSource.PlayOneShot(bringUpSound);
        else
            audioSource.PlayOneShot(backSound);
        show(!container.activeSelf);
    }

    public void show(bool visible)
    {
        if (shown == visible)
            return;
        shown = visible;
        if (visible)
        {
            Vector3 currentPosition = Camera.main.transform.position;
            Vector3 currentDirection = Camera.main.transform.forward;
            container.transform.position = currentPosition + menuDistance * new Vector3(currentDirection.x, 0, currentDirection.z).normalized + new Vector3(0, menuBumpUp, 0);
            float currentYaw = Camera.main.transform.rotation.eulerAngles.y;
            container.transform.rotation = Quaternion.Euler(0, currentYaw, 0);
        }
        foreach (var go in enableWhenShown)
            go.SetActive(visible);
        foreach (var go in disableWhenShown)
            go.SetActive(!visible);
        OnPauseMenuToggle?.Invoke(visible);
        goToMainScreen(playSound: false);
    }

    public void hide()
    {
        show(false);
    }

    public void resume()
    {
        audioSource.PlayOneShot(clickSound);
        hide();
    }

    public void backToMainMenu()
    {
        audioSource.PlayOneShot(backToMainMenuSound);
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}
