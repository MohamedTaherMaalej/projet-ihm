using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public Slider slider;

    void Start()
    {
        ProgressManager.current.OnProgressUpdate += UpdateProgressBar;
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return null;
        UpdateProgressBar();
    }

    void OnDestroy()
    {
        ProgressManager.current.OnProgressUpdate -= UpdateProgressBar;
    }

    private void UpdateProgressBar()
    {
        slider.value = 0;
        foreach (ProgressManager.ProgressPoint point in Enum.GetValues(typeof(ProgressManager.ProgressPoint)))
        {
            if (ProgressManager.current.isDone(point))
                slider.value++;
            else
                break;
        }
    }
}
