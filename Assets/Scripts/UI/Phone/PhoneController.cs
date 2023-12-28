using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PhoneController : MonoBehaviour
{
    public static PhoneController current;
    public float menuDistance = 10;
    public float menuBumpUp = 0;
    public GameObject frame;
    public GameObject lockedScreen; // must contain a NumericKeypad component
    public GameObject homeScreen;
    public GameObject messagesScreen;
    public GameObject telephoneScreen; // must contain a NumericKeypad component
    public GameObject helpScreen;
    [Tooltip("ORDER MATTERS!")] public List<GameObject> messagesTempPanesOrdered;
    public List<GameObject> conversationSelectionScreens;
    public List<GameObject> conversation1Screens;
    public List<GameObject> conversation2Screens;
    public List<GameObject> conversation3Screens;
    [Serializable] public struct HelpPaneMapping { public ProgressManager.ProgressPoint point; public GameObject helpPane; }
    public HelpPaneMapping[] helpPanes;
    public RawImage lockedSprite;
    public AudioClip clickSound;
    public AudioClip backSound;
    public AudioClip quitSound;
    public AudioClip enterHomeScreenSound;
    public AudioClip findCodeVoiceLine;
    public GameObject endGameBadge;
    public GameObject leftHandToBeHidden;
    public GameObject rightHandToBeHidden;
    public List<GameObject> enableWhenShown;
    public List<GameObject> disableWhenShown;
    public InputAction leavePhone = new InputAction();
    public InputAction bringUpPhoneTest = new InputAction();
    public GameObject container { get; private set; }
    public bool shown { get; private set; }
    protected AudioSource audioSource;
    protected NumericKeypad lockedKeypad;
    protected NumericKeypad telephoneKeypad;

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
            Debug.LogWarning("Could not find container (2-level parent) of phone controller object!");
        }
        lockedKeypad = lockedScreen.GetComponent<NumericKeypad>();
        telephoneKeypad = telephoneScreen.GetComponent<NumericKeypad>();
        // enableWhenShown.Add(container); // KEEP COMMENTED: this is dealth with by show() so the coroutine can work properly
        enableWhenShown.Add(frame);
        leavePhone.Enable(); // this is then disabled by the hide() call
        leavePhone.started += leave;
        bringUpPhoneTest.Enable();
        bringUpPhoneTest.started += bringUpTest;
        ProgressManager.current.OnProgressUpdate += updateHelpPanes;
        ProgressManager.current.OnTempUpdate += updateTempPanes;
        lockedKeypad.OnKeypadSubmitted += lockedKeypadSubmit;
        telephoneKeypad.OnKeypadSubmitted += telephoneKeypadSubmit;
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return null;
        audioSource = AudioManager.current.getSource();
        disableWhenShown.Add(leftHandToBeHidden.GetComponentInChildren<SkinnedMeshRenderer>().gameObject);
        disableWhenShown.Add(rightHandToBeHidden.GetComponentInChildren<SkinnedMeshRenderer>().gameObject);
        if (ProgressManager.current.isDone(ProgressManager.ProgressPoint.SolvedRushHour))
            endGameBadge.SetActive(true);
        shown = true;
        hide();
    }

    void OnDestroy()
    {
        leavePhone.started -= leave;
        bringUpPhoneTest.started -= bringUpTest;
        ProgressManager.current.OnProgressUpdate -= updateHelpPanes;
        ProgressManager.current.OnTempUpdate -= updateTempPanes;
        lockedKeypad.OnKeypadSubmitted -= lockedKeypadSubmit;
        telephoneKeypad.OnKeypadSubmitted -= telephoneKeypadSubmit;
        current = null;
    }

    private void bringUpTest(InputAction.CallbackContext ctx)
    {
        show(!container.activeSelf);
    }

    private void leave(InputAction.CallbackContext ctx)
    {
        leave();
    }
    public void leave()
    {
        audioSource.PlayOneShot(quitSound);
        hide();
    }

    public void show(bool visible)
    {
        if (shown == visible)
            return;
        shown = visible;
        if (visible)
        {
            if (PauseMenuController.current.shown)
                return;
            container.SetActive(true);
            leavePhone.Enable();
            PauseMenuController.current.bringUpPauseMenu.Disable();
            Vector3 currentPosition = Camera.main.transform.position;
            Vector3 currentDirection = Camera.main.transform.forward;
            container.transform.position = currentPosition + menuDistance * new Vector3(currentDirection.x, 0, currentDirection.z).normalized + new Vector3(0, menuBumpUp, 0);
            float currentYaw = Camera.main.transform.rotation.eulerAngles.y;
            container.transform.rotation = Quaternion.Euler(0, currentYaw, 0);
        }
        else
        {
            leavePhone.Disable();
            // container.SetActive(false); // this is done later, further down the line, at the end of the coroutine
        }
        if (ProgressManager.current.isDone(ProgressManager.ProgressPoint.UnlockedPhone))
            if (visible) goToHomeScreen();
            else goToHomeScreenSilent();
        else
            if (visible) goToLockedScreen();
            else goToLockedScreenSilent();
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

    public void hide()
    {
        show(false);
    }

    private void updateTempPanes()
    {
        int temp = ProgressManager.current.getTemp();
        foreach (GameObject tempPane in messagesTempPanesOrdered)
        {
            tempPane.SetActive(false);
        }
        messagesTempPanesOrdered[temp-1].SetActive(true);
    }

    private void updateHelpPanes()
    {
        ProgressManager.ProgressPoint maxPaneIndexReached = 0;
        HelpPaneMapping maxPaneReached = helpPanes[0];
        foreach (HelpPaneMapping helpPaneMapping in helpPanes)
        {
            if (ProgressManager.current.isDone(helpPaneMapping.point) && helpPaneMapping.point > maxPaneIndexReached)
            {
                maxPaneIndexReached = helpPaneMapping.point;
                maxPaneReached = helpPaneMapping;
            }
            helpPaneMapping.helpPane.SetActive(false);
        }
        maxPaneReached.helpPane.SetActive(true);
    }

    public void goToLockedScreenSilent()
    {
        lockedScreen.SetActive(true);
        homeScreen.SetActive(false);
        messagesScreen.SetActive(false);
        telephoneScreen.SetActive(false);
        helpScreen.SetActive(false);
        lockedSprite.enabled = true;
    }
    public void goToLockedScreen()
    {
        goToLockedScreenSilent();
        audioSource.PlayOneShot(enterHomeScreenSound);
        bool advanced = ProgressManager.current.advance(ProgressManager.ProgressPoint.ViewedLockedPhone);
        if (advanced)
        {
            StartCoroutine(waitBeforePlayingVoiceLine());
        }
    }
    IEnumerator waitBeforePlayingVoiceLine()
    {
        yield return new WaitForSeconds(3);
        AudioManager.current.getSource().PlayOneShot(findCodeVoiceLine);
    }

    public void goToHomeScreenSilent()
    {
        lockedScreen.SetActive(false);
        homeScreen.SetActive(true);
        messagesScreen.SetActive(false);
        telephoneScreen.SetActive(false);
        helpScreen.SetActive(false);
    }
    public void goToHomeScreen()
    {
        goToHomeScreenSilent();
        audioSource.PlayOneShot(enterHomeScreenSound);
    }

    public void goToMessagesScreen()
    {
        lockedScreen.SetActive(false);
        homeScreen.SetActive(false);
        messagesScreen.SetActive(true);
        telephoneScreen.SetActive(false);
        helpScreen.SetActive(false);
        goToConversationSelectionScreenSilent(); // important as a reset every time
        audioSource.PlayOneShot(clickSound);
        endGameBadge.SetActive(false); // mark as read
    }

    public void goToTelephoneScreen()
    {
        lockedScreen.SetActive(false);
        homeScreen.SetActive(false);
        messagesScreen.SetActive(false);
        telephoneScreen.SetActive(true);
        helpScreen.SetActive(false);
        audioSource.PlayOneShot(clickSound);
    }

    public void goToHelpScreen()
    {
        lockedScreen.SetActive(false);
        homeScreen.SetActive(false);
        messagesScreen.SetActive(false);
        telephoneScreen.SetActive(false);
        helpScreen.SetActive(true);
        audioSource.PlayOneShot(clickSound);
    }

    public void goToConversationSelectionScreenSilent()
    {
        foreach (GameObject go in conversationSelectionScreens)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in conversation1Screens)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in conversation2Screens)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in conversation3Screens)
        {
            go.SetActive(false);
        }
    }
    public void goToConversationSelectionScreen()
    {
        goToConversationSelectionScreenSilent();
        audioSource.PlayOneShot(backSound);
    }

    public void goToConversation1Screen()
    {
        foreach (GameObject go in conversationSelectionScreens)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in conversation1Screens)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in conversation2Screens)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in conversation3Screens)
        {
            go.SetActive(false);
        }
        audioSource.PlayOneShot(clickSound);
    }

    public void goToConversation2Screen()
    {
        foreach (GameObject go in conversationSelectionScreens)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in conversation1Screens)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in conversation2Screens)
        {
            go.SetActive(true);
        }
        foreach (GameObject go in conversation3Screens)
        {
            go.SetActive(false);
        }
        audioSource.PlayOneShot(clickSound);
    }

    public void goToConversation3Screen()
    {
        foreach (GameObject go in conversationSelectionScreens)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in conversation1Screens)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in conversation2Screens)
        {
            go.SetActive(false);
        }
        foreach (GameObject go in conversation3Screens)
        {
            go.SetActive(true);
        }
        audioSource.PlayOneShot(clickSound);
    }

    private void lockedKeypadSubmit(bool correct)
    {
        if (correct)
        {
            ProgressManager.current.advance(ProgressManager.ProgressPoint.UnlockedPhone);
            lockedSprite.enabled = false;
            StartCoroutine(unlock());
        }
    }
    IEnumerator unlock()
    {
        yield return new WaitForSeconds(1);
        goToHomeScreen();
    }

    private  void telephoneKeypadSubmit(bool correct)
    {
        if (correct)
        {
            ProgressManager.current.advance(ProgressManager.ProgressPoint.CalledCorrectNumber);
            Scenario.current.finishGame();
        }
    }
}
