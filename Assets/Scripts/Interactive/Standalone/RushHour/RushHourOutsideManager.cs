using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RushHourOutsideManager : MonoBehaviour
{
    public static RushHourOutsideManager current;
    public GameObject padlock;
    public GameObject puzzleTrigger;
    public GameObject targetCube;
    public GameObject[] showWhenUnsolved;
    public GameObject[] showWhenSolved;
    [Tooltip("Played when entering the puzzle's world/scene")] public AudioClip enterPuzzleSound;
    [Tooltip("Typically played when the puzzle is already complete")] public AudioClip accessDeniedSound;
    public Vector3 closedPosition;
    public Vector3 openPosition;
    private bool drawerOpen;
    private bool solved;

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
        ProgressManager.current.OnProgressUpdate += updateAppearance;
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return null;
        updateAppearance();
    }

    void OnDestroy()
    {
        ProgressManager.current.OnProgressUpdate -= updateAppearance;
        current = null;
    }

    public void jumpIn()
    {
        switch (puzzleTrigger.GetComponent<InteractableObjectEssential>().state)
        {
            case InteractableObjectEssential.InteractableState.InteractableNotDone:
                AudioManager.current.getSource().PlayOneShot(enterPuzzleSound);
                SceneManager.LoadScene("RushHour", LoadSceneMode.Single);
                break;
            case InteractableObjectEssential.InteractableState.InteractableDone:
                AudioManager.current.getSource().PlayOneShot(accessDeniedSound);
                break;
        }
    }

    private void updateAppearance()
    {
        drawerOpen = ProgressManager.current.isDone(ProgressManager.ProgressPoint.OpenedPadlock);
        solved = ProgressManager.current.isDone(ProgressManager.ProgressPoint.SolvedRushHour);
        padlock.SetActive(!drawerOpen);
        if (ProgressManager.current.getTemp() != 2)
            padlock.SetActive(false);
        if (drawerOpen)
            transform.localPosition = openPosition;
        else
            transform.localPosition = closedPosition;
        targetCube.SetActive(!solved);
        Scenario.current.toggleEndGameVisibilities();
    }
}
