using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class DirectHandController : MonoBehaviour
{
    public Material passiveMaterial;
    public Material activeMaterial;
    private bool switchedToPassive;
    private SkinnedMeshRenderer modelRenderer;

    void Start()
    {
        makeHandPassive();
        switchedToPassive = false;
    }

    void OnEnable()
    {
        makeHandPassive();
        switchedToPassive = false;
    }

    void Update()
    {
        if (!switchedToPassive)
        {
            if (modelRenderer == null)
            {
                modelRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
                if (modelRenderer == null)
                    return;
            }
            makeHandPassive();
            switchedToPassive = true;
        }
    }

    public void makeHandPassive()
    {
        if (modelRenderer == null)
        {
            modelRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            if (modelRenderer == null)
                return;
        }
        modelRenderer.material = passiveMaterial;
    }

    public void makeHandActive()
    {
        if (modelRenderer == null)
        {
            modelRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
            if (modelRenderer == null)
                return;
        }
        modelRenderer.material = activeMaterial;
    }
}
