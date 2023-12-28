using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class is used to mark the Global Object as persistent.
/// It will also destroy any duplicates of that object.
/// </summary>
public class Global : MonoBehaviour
{
    public static Global current;

    private void Awake()
    {
        if (current == null)
        {
            current = this;
        }
        else
        {
            // Destroy(obj: this);
            Destroy(gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void OnDestroy()
    {
        // current = null; // this is global
    }
}
