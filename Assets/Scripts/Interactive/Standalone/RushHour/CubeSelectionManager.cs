using System.Collections;
using UnityEngine;

public class CubeSelectionManager : MonoBehaviour
{
    public static CubeSelectionManager current;
    public Material selectedMaterial, defaultMaterial, selectedMaterialForTarget, defaultMaterialForTarget;
    [HideInInspector] public bool somethingIsSelected = false;
    public AudioClip selectSound;
    public AudioClip deselectAllSound;

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
        DeselectAllCubes();
    }

    void OnEnable()
    {
        StartCoroutine(LateOnEnable());
    }
    private IEnumerator LateOnEnable()
    {
        yield return null; // wait one frame
        // No yaw correction here, as RushHour works differently in terms of orientation and I was lazy to find the right computation
    }

    void OnDestroy()
    {
        current = null;
    }

    public void DeselectAllCubes()
    {
        CubeController[] allCubes = FindObjectsOfType<CubeController>();
        foreach (CubeController cube in allCubes)
        {
            cube.DeselectCube();
        }
        somethingIsSelected = false;
    }
}
