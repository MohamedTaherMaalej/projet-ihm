using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class InteractableObjectEssential : MonoBehaviour
{
    public enum InteractableState { Deactivated, InteractableNotDone, InteractableDone };
    public ProgressManager.ProgressPoint donePoint;
    public InteractableState state { get; private set; }
    private Dictionary<InteractableState, int> stateToColor = new Dictionary<InteractableState, int>();

    void Start()
    {
        ProgressManager.current.OnProgressUpdate += progressChanged;
        stateToColor.Add(InteractableState.Deactivated, 0);
        stateToColor.Add(InteractableState.InteractableNotDone, 1);
        stateToColor.Add(InteractableState.InteractableDone, 2);

        Renderer[] allRenderers = GetComponentsInChildren<Renderer>(true);

        // Iterate through the array of renderers
        foreach (Renderer rendererComponent in allRenderers)
        {
            // Get the GameObject that has the Renderer component
            GameObject objectWithRenderer = rendererComponent.gameObject;
            objectWithRenderer.AddComponent<cakeslice.Outline>();
        }

        if (allRenderers.Length == 0)
        {
            Debug.Log("No Renderer components found on this GameObject and its children.");
        }
        changeStateTo(InteractableState.Deactivated);
        progressChanged();
    }
    void OnDestroy()
    {
        ProgressManager.current.OnProgressUpdate -= progressChanged;
    }

    public void changeStateTo(InteractableState state)
    {
        XRSimpleInteractable simpleInteractableScript = GetComponent<XRSimpleInteractable>();
        this.state = state;
        cakeslice.Outline[] scripts = GetComponentsInChildren<cakeslice.Outline>(true);

        // Iterate through the array of renderers
        if (state == InteractableState.Deactivated)
        {
            if (simpleInteractableScript != null)
            {
                simpleInteractableScript.enabled = false;
            }
            foreach (cakeslice.Outline script in scripts)
            {
                script.enabled = false;
            }
        }
        else
        {
            if (simpleInteractableScript != null)
            {
                simpleInteractableScript.enabled = true;
            }
            foreach (cakeslice.Outline script in scripts)
            {
                script.enabled = true;
                script.color = stateToColor[state];
            }
        }
    }

    public void progressChanged()
    {
        if (donePoint > 0)
        {
            if (ProgressManager.current.isDone(donePoint - 1))
            {
                changeStateTo(InteractableState.InteractableNotDone);
            }
            else
            {
                changeStateTo(InteractableState.Deactivated);
            }
        }
        else
        {
            changeStateTo(InteractableState.InteractableDone);
        }
        if (ProgressManager.current.isDone(donePoint))
        {
            changeStateTo(InteractableState.InteractableDone);
        }
    }
}
