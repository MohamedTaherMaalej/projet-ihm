using UnityEngine;

public class DiaryManager : FocusableObject
{
    public static DiaryManager current;
    public GameObject[] canvasObjects;

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

    void OnDestroy()
    {
        current = null;
    }

    public override void focus()
    {
        foreach (GameObject go in canvasObjects)
            go.SetActive(true);
        base.focus();
        // float dyaw = Camera.main.transform.rotation.eulerAngles.y - Camera.main.transform.parent.parent.rotation.eulerAngles.y;
        // Camera.main.transform.parent.parent.Rotate(0, -dyaw, 0);
    }

    public override void unfocus()
    {
        base.unfocus();
        foreach (GameObject go in canvasObjects)
            go.SetActive(false);
        // HACK: Update outline color
        InteractableObjectEssential outlineManager = GetComponent<InteractableObjectEssential>();
        if (outlineManager != null)
        {
            outlineManager.donePoint = ProgressManager.ProgressPoint.StartedGame;
            outlineManager.progressChanged(); // update because it would otherwise require the ProgressManager event
        }
    }
}
