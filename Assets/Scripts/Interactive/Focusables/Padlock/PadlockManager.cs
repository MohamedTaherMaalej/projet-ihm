using System.Collections;
using UnityEngine;

public class PadlockManager : FocusableObject
{
    public static PadlockManager current;
    public Transform newLocationOnWin;

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
        GetComponent<Padlock>().setWheelsInteractable(false);
    }

    void OnDestroy()
    {
        GetComponent<Padlock>().OnChangeOpenState -= handleWin;
        current = null;
    }

    public override void focus()
    {
        if (focused || PauseMenuController.current.shown
        || !ProgressManager.current.isDone(ProgressManager.ProgressPoint.TookEmptyMedicine)
        || ProgressManager.current.isDone(ProgressManager.ProgressPoint.OpenedPadlock))
            return;
        base.focus();
        float dyaw = Camera.main.transform.rotation.eulerAngles.y - Camera.main.transform.parent.parent.rotation.eulerAngles.y;
        Camera.main.transform.parent.parent.Rotate(0, -dyaw, 0);
        GetComponent<BoxCollider>().enabled = false;
        AudioManager.current.playPuzzleMusic();
        GetComponent<Padlock>().setWheelsInteractable(true);
        foreach (PadlockWheel wheel in GetComponent<Padlock>().wheels)
        {
            wheel.gameObject.layer = LayerMask.NameToLayer("Focusables");
            foreach (Transform t in wheel.transform)
                t.gameObject.layer = LayerMask.NameToLayer("Focusables");
        }
        GetComponent<Padlock>().OnChangeOpenState += handleWin;
    }

    public override void unfocus()
    {
        if (!focused)
            return;
        base.unfocus();
        GetComponent<BoxCollider>().enabled = true;
        AudioManager.current.playDefaultMusic();
        GetComponent<Padlock>().setWheelsInteractable(false);
        foreach (PadlockWheel wheel in GetComponent<Padlock>().wheels)
        {
            wheel.gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform t in wheel.transform)
                t.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        GetComponent<Padlock>().OnChangeOpenState -= handleWin;
    }

    private void handleWin(bool open)
    {
        if (!open)
            return;
        StartCoroutine(winSequence());
    }
    IEnumerator winSequence()
    {
        FocusableUIController.current.exitFocus.Disable();
        yield return new WaitForSeconds(1);
        originalCameraPosition = newLocationOnWin.position;
        originalCameraRotation = newLocationOnWin.rotation;
        ProgressManager.current.advance(ProgressManager.ProgressPoint.OpenedPadlock);
        unfocus();
        FocusableUIController.current.hide();
    }
}
