using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class HanoiTowerManager : MonoBehaviour
{
    public static HanoiTowerManager current;
    public PuzzleMenuController menu;
    public BoxCollider aroundBaseArea;
    public HanoiTowerPole[] poles;
    public HanoiTowerDisc[] discs;
    public GameObject[] solvedDiscs;
    public GameObject[] showWhenComplete;
    public Light endSpotLight;
    public AudioClip winSound;
    public AudioClip endSpotLightSwitchOnSound;
    public AudioClip leaveSound;
    public AudioClip validMoveSound;
    public AudioClip invalidMoveSound;
    public bool complete { get; private set; } = false;

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
        showSolvedDiscs(false);
        HanoiTowerDisc.OnDiscReleased += updateInteractabilitiesLater;
        HanoiTowerDisc.OnDiscMoved += checkCompletion;
        menu.OnPuzzleMenuToggle += handlePuzzleMenuToggle;
        UIEvents.OnPuzzleSkip += skip;
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return null;
        updateInteractabilities();
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

    void OnDestroy()
    {
        HanoiTowerDisc.OnDiscReleased -= updateInteractabilitiesLater;
        HanoiTowerDisc.OnDiscMoved -= checkCompletion;
        menu.OnPuzzleMenuToggle -= handlePuzzleMenuToggle;
        UIEvents.OnPuzzleSkip -= skip;
        current = null;
    }

    public HanoiTowerPole whichPoleIsDiscAround(HanoiTowerDisc disc)
    {
        foreach (HanoiTowerPole pole in poles)
            if (pole.isDiscAroundMe(disc))
                return pole;
        return null;
    }

    public HanoiTowerPole getPole(HanoiTowerPole.Kind kind)
    {
        foreach (HanoiTowerPole pole in poles)
            if (pole.kind == kind)
                return pole;
        return null;
    }

    public HanoiTowerDisc getDisc(HanoiTowerDisc.Size size)
    {
        foreach (HanoiTowerDisc disc in discs)
            if (disc.size == size)
                return disc;
        return null;
    }

    private void showSolvedDiscs(bool visible)
    {
        foreach (GameObject go in solvedDiscs)
        {
            go.SetActive(visible);
        }
    }

    public void updateInteractabilitiesLater(HanoiTowerDisc movedDisc = null)
    {
        StartCoroutine(waitBeforeUpdatingInteractabilities(movedDisc));
    }
    IEnumerator waitBeforeUpdatingInteractabilities(HanoiTowerDisc movedDisc = null)
    {
        yield return new WaitForSeconds(0.5f);
        updateInteractabilities(movedDisc);
    }
    private void updateInteractabilities(HanoiTowerDisc movedDisc = null)
    {
        foreach (HanoiTowerDisc disc in discs)
        {
            if (movedDisc != null && ReferenceEquals(movedDisc, disc))
                continue;
            disc.GetComponent<XRGrabInteractable>().enabled = false;
            disc.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        }
        foreach (HanoiTowerPole pole in poles)
        {
            HanoiTowerDisc highestDisc = pole.getHighestDisc();
            if (highestDisc != null)
            {
                highestDisc.GetComponent<XRGrabInteractable>().enabled = true;
                highestDisc.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
            }
        }
        if (movedDisc != null)
        {
            movedDisc.GetComponent<XRGrabInteractable>().enabled = true;
            movedDisc.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
        }
    }

    private void checkCompletion()
    {
        if (complete)
            return;
        complete = true;
        HanoiTowerPole rightPole = getPole(HanoiTowerPole.Kind.Right);
        foreach (HanoiTowerDisc disc in discs)
        {
            if (!rightPole.isDiscAroundMe(disc))
            {
                complete = false;
                break;
            }
        }
        if (!complete)
            return;
        menu.bringUpPuzzleMenu.Disable();
        StartCoroutine(endSequence());
    }
    IEnumerator endSequence()
    {
        foreach (HanoiTowerDisc disc in discs)
            disc.GetComponent<XRGrabInteractable>().enabled = false;
        AudioManager.current.getSource().PlayOneShot(winSound);
        ProgressManager.current.advance(ProgressManager.ProgressPoint.SolvedHanoiTower);
        foreach (GameObject go in showWhenComplete)
            go.SetActive(true);
        yield return new WaitForSeconds(3);
        foreach (GameObject go in showWhenComplete)
            go.SetActive(false);
        yield return new WaitForSeconds(1);
        foreach (HanoiTowerDisc disc in discs)
            disc.gameObject.SetActive(false);
        foreach (GameObject disc in solvedDiscs)
            disc.SetActive(true);
        endSpotLight.gameObject.SetActive(true);
        AudioManager.current.getSource().PlayOneShot(endSpotLightSwitchOnSound);
        yield return new WaitForSeconds(3);
        AudioManager.current.getSource().PlayOneShot(leaveSound);
        SceneManager.LoadScene("Bedroom", LoadSceneMode.Single);
    }

    private void skip()
    {
        complete = true;
        menu.bringUpPuzzleMenu.Disable();
        menu.hide();
        StartCoroutine(skipSequence());
    }
    IEnumerator skipSequence()
    {
        AudioManager.current.getSource().PlayOneShot(winSound);
        ProgressManager.current.advance(ProgressManager.ProgressPoint.SolvedHanoiTower);
        yield return null;
        foreach (GameObject go in showWhenComplete)
            if (go.name != "End Canvas")
                go.SetActive(true);
        foreach (HanoiTowerDisc disc in discs)
            disc.gameObject.SetActive(false);
        foreach (GameObject disc in solvedDiscs)
            disc.SetActive(true);
        yield return new WaitForSeconds(2);
        foreach (GameObject go in showWhenComplete)
            go.SetActive(false);
        endSpotLight.gameObject.SetActive(true);
        AudioManager.current.getSource().PlayOneShot(endSpotLightSwitchOnSound);
        yield return new WaitForSeconds(3);
        AudioManager.current.getSource().PlayOneShot(leaveSound);
        SceneManager.LoadScene("Bedroom", LoadSceneMode.Single);
    }

    public void playValidSound()
    {
        AudioManager.current.getSource().PlayOneShot(validMoveSound);
    }

    public void playInvalidSound()
    {
        AudioManager.current.getSource().PlayOneShot(invalidMoveSound);
    }

    private void handlePuzzleMenuToggle(bool visible)
    {
        foreach (HanoiTowerDisc disc in discs)
            disc.GetComponent<XRGrabInteractable>().enabled = !visible;
    }
}
