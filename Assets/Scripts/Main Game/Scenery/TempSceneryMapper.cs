using System.Collections.Generic;
using UnityEngine;

public class TempSceneryMapper : MonoBehaviour
{
    [Tooltip("ORDER MATTERS IN BIG LIST! (doesn't matter in sub-lists\n\nHave one element in the big list for each temp")]
    public List<GameObjectList> tempSpecificObjectsOrdered = new List<GameObjectList>();

    void Start()
    {
        ProgressManager.current.OnTempUpdate += updateScenery;
    }

    void OnDestroy()
    {
        ProgressManager.current.OnTempUpdate -= updateScenery;
    }

    private void updateScenery()
    {
        for (int i = 0; i < tempSpecificObjectsOrdered.ToArray().Length; i++)
            foreach (GameObject go in tempSpecificObjectsOrdered[i].gameObjects)
                    go.SetActive(false);
        int temp = PlayerPrefs.GetInt("CurrentTemp");
        foreach (GameObject go in tempSpecificObjectsOrdered[temp - 1].gameObjects)
            go.SetActive(true);
    }
}
