using UnityEngine;

public class FocusableObject : MonoBehaviour
{
    public Transform focusedCameraLocation;
    public Transform focusedMenuLocation;
    public bool doSwitchToFocusablesLayer = true;
    public AudioClip focusSound;
    public bool focused { get; private set; } = false;
    protected Vector3 originalCameraPosition;
    protected Quaternion originalCameraRotation;

    public virtual void focus()
    {
        if (focused || PauseMenuController.current.shown)
            return;
        focused = true;
        AudioManager.current.getSource().PlayOneShot(focusSound);
        float distanceFromCameraToOrigin = Vector3.Distance(Camera.main.transform.position, Camera.main.transform.parent.parent.position);
        originalCameraPosition = Camera.main.transform.parent.parent.position;
        originalCameraRotation = Camera.main.transform.parent.parent.rotation;
        Camera.main.transform.parent.parent.position = focusedCameraLocation.position - distanceFromCameraToOrigin * focusedCameraLocation.up;
        Camera.main.transform.parent.parent.rotation = focusedCameraLocation.rotation;
        if (doSwitchToFocusablesLayer)
        {
            gameObject.layer = LayerMask.NameToLayer("Focusables");
            foreach (Transform t in GetComponentsInChildren<Transform>())
                t.gameObject.layer = LayerMask.NameToLayer("Focusables");
        }
        FocusableUIController.current.show(true, focusedMenuLocation);
        FocusableUIController.current.exitFocusedState += unfocus;
    }

    public virtual void unfocus()
    {
        if (!focused)
            return;
        focused = false;
        Camera.main.transform.parent.parent.position = originalCameraPosition;
        Camera.main.transform.parent.parent.rotation = originalCameraRotation;
        if (doSwitchToFocusablesLayer)
        {
            gameObject.layer = LayerMask.NameToLayer("Default");
            foreach (Transform t in GetComponentsInChildren<Transform>())
                t.gameObject.layer = LayerMask.NameToLayer("Default");
        }
        // the event comes from the button, which by design already hides the UI; so no need to do it here
        FocusableUIController.current.exitFocusedState -= unfocus;
        float dyaw = Camera.main.transform.rotation.eulerAngles.y - Camera.main.transform.parent.parent.rotation.eulerAngles.y;
        Camera.main.transform.parent.parent.Rotate(0, -dyaw, 0);
    }
}
