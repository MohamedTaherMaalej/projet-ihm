using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit;

public class TriggerWall : MonoBehaviour
{
    public static TriggerWall current;
    public PuzzleMenuController menu;
    public AudioClip winSound;
    public AudioClip leaveSound;
    public CubeController targetCube;
    public GameObject[] showWhenComplete;
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
        if (!targetCube.isTarget)
        {
            targetCube = null;
            Debug.LogWarning("Rush Hour TriggerWall warning: Provided target cube is not marked as a target. It will be ignored.\nThe puzzle will still work fine, but the skip animation will look wrong.");
        }
        menu.OnPuzzleMenuToggle += handlePuzzleMenuToggle;
        UIEvents.OnPuzzleSkip += skip;
    }

    void OnDestroy()
    {
        menu.OnPuzzleMenuToggle -= handlePuzzleMenuToggle;
        UIEvents.OnPuzzleSkip -= skip;
        current = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (complete)
            return;
        CubeController cube = other.GetComponent<CubeController>();
        if (cube != null && cube.isTarget)
        {
            // No need to make objects uninteractable, as that is dealth with by the humongous end message text
            complete = true;
            menu.bringUpPuzzleMenu.Disable();
            StartCoroutine(endSequence());
        }
    }
    IEnumerator endSequence()
    {
        CubeSelectionManager.current.DeselectAllCubes();
        AudioManager.current.getSource().PlayOneShot(winSound);
        ProgressManager.current.advance(ProgressManager.ProgressPoint.SolvedRushHour);
        foreach (GameObject go in showWhenComplete)
            go.SetActive(true);
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
        ProgressManager.current.advance(ProgressManager.ProgressPoint.SolvedRushHour);
        yield return null;
        CubeSelectionManager.current.DeselectAllCubes();
        CubeController[] cubes = FindObjectsOfType<CubeController>();
        foreach (CubeController cube in cubes)
            cube.gameObject.SetActive(false);
        if (targetCube != null)
            targetCube.gameObject.SetActive(true);
        yield return new WaitForSeconds(1);
        foreach (GameObject go in showWhenComplete)
        {
            go.SetActive(true);
            if (go.name == "End Canvas")
                go.transform.Find("End Message").gameObject.SetActive(false);
        }
        yield return new WaitForSeconds(2);
        AudioManager.current.getSource().PlayOneShot(leaveSound);
        SceneManager.LoadScene("Bedroom", LoadSceneMode.Single);
    }

    private void handlePuzzleMenuToggle(bool visible)
    {
        CubeController[] cubes = FindObjectsOfType<CubeController>();
        foreach (CubeController cube in cubes)
            cube.GetComponent<XRSimpleInteractable>().enabled = !visible;
    }
}
