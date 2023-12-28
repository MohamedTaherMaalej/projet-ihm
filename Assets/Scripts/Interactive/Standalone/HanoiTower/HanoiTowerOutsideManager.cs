using UnityEngine;
using UnityEngine.SceneManagement;

public class HanoiTowerOutsideManager : MonoBehaviour
{
    public static HanoiTowerOutsideManager current;
    [Tooltip("Played when entering the puzzle's world/scene")] public AudioClip enterPuzzleSound;
    [Tooltip("Typically played when the puzzle is already complete")] public AudioClip accessDeniedSound;
    public GameObject[] unsolvedDiscs;
    public GameObject[] solvedDiscs;
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
        updateAppearance();
        ProgressManager.current.OnProgressUpdate += updateAppearance;
    }

    void OnDestroy()
    {
        ProgressManager.current.OnProgressUpdate -= updateAppearance;
        current = null;
    }

    public void jumpIn()
    {
        switch (GetComponent<InteractableObjectEssential>().state)
        {
            case InteractableObjectEssential.InteractableState.InteractableNotDone:
                AudioManager.current.getSource().PlayOneShot(enterPuzzleSound);
                SceneManager.LoadScene("HanoiTower", LoadSceneMode.Single);
                break;
            case InteractableObjectEssential.InteractableState.InteractableDone:
                AudioManager.current.getSource().PlayOneShot(accessDeniedSound);
                break;
        }
    }

    private void updateAppearance()
    {
        solved = ProgressManager.current.isDone(ProgressManager.ProgressPoint.SolvedHanoiTower);
        foreach (GameObject go in solvedDiscs)
        {
            go.SetActive(solved);
        }
        foreach (GameObject go in unsolvedDiscs)
        {
            go.SetActive(!solved);
        }
    }
}
