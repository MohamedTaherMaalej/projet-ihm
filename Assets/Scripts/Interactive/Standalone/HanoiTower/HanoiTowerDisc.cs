using System;
using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HanoiTowerDisc : MonoBehaviour
{
    public static event Action<HanoiTowerDisc> OnDiscReleased;
    public static event Action OnDiscMoved;
    public enum Size { None, Tiny, Small, Medium, Big }
    public Size size;
    private Quaternion originalRotation;
    private HanoiTowerPole currentPole;

    void Start()
    {
        currentPole = HanoiTowerManager.current.getPole(HanoiTowerPole.Kind.Left);
        originalRotation = transform.rotation;
    }

    public void pickUp()
    {
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotation;
        GetComponent<Rigidbody>().detectCollisions = false;
    }

    public void letGo()
    {
        StartCoroutine(letGoCoroutine());
    }
    IEnumerator letGoCoroutine()
    {
        GetComponent<XRGrabInteractable>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        GetComponent<Rigidbody>().detectCollisions = true;
        HanoiTowerPole newPole = HanoiTowerManager.current.whichPoleIsDiscAround(this);
        bool misplaced = newPole == null;
        bool changedPoles = false;
        // bool outsidePlayArea = !HanoiTowerManager.current.aroundBaseArea.bounds.Contains(transform.position);
        if (!misplaced)
        {
            HanoiTowerDisc highestDiscOnPole = newPole.getHighestDisc(otherThan: this);
            if (highestDiscOnPole != null && highestDiscOnPole.size < size)
                misplaced = true;
        }
        if (!misplaced)
        {
            if (newPole != currentPole)
                changedPoles = true;
            if (changedPoles)
                HanoiTowerManager.current.playValidSound();
            // else
            //     HanoiTowerManager.current.playInvalidSound(); // this tends to get infuriating (especially when it doesn't work properly)
            currentPole = newPole;
        }
        else
        {
            HanoiTowerManager.current.playInvalidSound();
            // if (outsidePlayArea)
            //     yield return new WaitForSeconds(2); // fall before fix (causes issues sometimes)
        }
        resetPolePosition();
        yield return null;
        OnDiscReleased?.Invoke(this);
        if (changedPoles)
            OnDiscMoved?.Invoke();
        GetComponent<XRGrabInteractable>().enabled = true;
    }

    private void resetPolePosition(bool fallFromHigherUp = false)
    {
        Rigidbody body = GetComponent<Rigidbody>();
        if (body != null)
            body.velocity = Vector3.zero;
        transform.rotation = originalRotation;
        float heightFactor = fallFromHigherUp ? 0.3f : 0.075f;
        transform.position = new Vector3(currentPole.transform.position.x, currentPole.transform.position.y + heightFactor * currentPole.transform.lossyScale.y, currentPole.transform.position.z);
    }

    public void togglePlane(bool activated)
    {
        // transform.Find("Bottom Plane Collider").gameObject.SetActive(activated);
    }
}
