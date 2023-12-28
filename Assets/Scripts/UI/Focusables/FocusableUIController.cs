using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FocusableUIController : MonoBehaviour
{
    public static FocusableUIController current;
    public float menuDistance = 1;
    public float menuBumpUp = 0;
    public Button buttonExit;
    public AudioClip exitSound;
    public GameObject leftHandToBeHidden;
    public GameObject rightHandToBeHidden;
    public List<GameObject> enableWhenShown;
    public List<GameObject> disableWhenShown;
    public InputAction exitFocus = new InputAction();
    public GameObject container { get; private set; }
    public event Action exitFocusedState;
    protected AudioSource audioSource;

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
        try
        {
            container = gameObject.transform.parent.gameObject.transform.parent.gameObject;
            if (container == null)
                throw new Exception();
        }
        catch
        {
            Debug.LogWarning("Could not find container (2-level parent) of focusable UI controller object!");
        }
        // enableWhenShown.Add(container); // KEEP COMMENTED: this is dealth with by show() so the coroutine can work properly
        enableWhenShown.Add(buttonExit.gameObject);
        exitFocus.Enable(); // this is then disabled by the hide() call
        exitFocus.started += exit;
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return null;
        audioSource = AudioManager.current.getSource();
        disableWhenShown.Add(leftHandToBeHidden.GetComponentInChildren<SkinnedMeshRenderer>().gameObject);
        disableWhenShown.Add(rightHandToBeHidden.GetComponentInChildren<SkinnedMeshRenderer>().gameObject);
        hide();
    }

    void OnDestroy()
    {
        exitFocus.started -= exit;
        current = null;
    }

    private void exit(InputAction.CallbackContext ctx)
    {
        exit();
    }
    public void exit()
    {
        exitFocusedState?.Invoke();
        audioSource.PlayOneShot(exitSound);
        hide();
    }

    public void show(bool visible, Transform targetLocation)
    {
        Transform realLocation = Camera.main.transform;
        if (targetLocation != null)
            realLocation = targetLocation;
        UIEvents.OnMenuPaneChanged?.Invoke(); // just to hide any tooltips
        if (visible)
        {
            if (PauseMenuController.current.shown)
                return;
            container.SetActive(true);
            exitFocus.Enable();
            PauseMenuController.current.bringUpPauseMenu.Disable();
            container.transform.position = realLocation.position + menuDistance * realLocation.forward + menuBumpUp * realLocation.up;
            container.transform.rotation = realLocation.rotation;
        }
        else
        {
            exitFocus.Disable();
            // container.SetActive(false); // this is done later, further down the line, at the end of the coroutine
        }
        StartCoroutine(lateShow(visible));
    }
    IEnumerator lateShow(bool visible)
    {
        takeCareOfVisibleLists(visible); // done so it runs faster in the editor and skips the wait when leaving
        if (!visible)
        {
            yield return new WaitForSeconds(0.2f); // prevent the pause menu from popping right up when the button was just tapped to leave
            PauseMenuController.current.bringUpPauseMenu.Enable();
            container.SetActive(false);
        }
        takeCareOfVisibleLists(visible); // necessary to do this AFTER the above if
    }
    private void takeCareOfVisibleLists(bool visible)
    {
        foreach (var go in enableWhenShown)
            go.SetActive(visible); // does not include container yet
        foreach (var go in disableWhenShown)
            go.SetActive(!visible);
    }

    public void show(bool visible)
    {
        show(visible, null);
    }

    public void hide()
    {
        show(false);
    }
}
