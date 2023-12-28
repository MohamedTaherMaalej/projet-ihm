using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ProgressManager : MonoBehaviour
{
    public static ProgressManager current;

    public enum ProgressPoint
    {
        StartedGame,
        ViewedLockedPhone,
        UnlockedPhone,
        SolvedHanoiTower,
        SleptToTemp2,
        TookEmptyMedicine,
        OpenedPadlock,
        SolvedRushHour,
        CalledCorrectNumber
    }

    public event Action OnProgressUpdate;
    public event Action OnTempUpdate;

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
        // Initialize progress PlayerPrefs
        foreach (int i in Enum.GetValues(typeof(ProgressPoint)))
            if (!PlayerPrefs.HasKey("Progress" + ((ProgressPoint)i).ToString()))
                PlayerPrefs.SetInt("Progress" + ((ProgressPoint)i).ToString(), 0);
        if (!PlayerPrefs.HasKey("CurrentTemp"))
            PlayerPrefs.SetInt("CurrentTemp", 1);

        // HACK: When starting the game from the bedroom (for testing purposes), forcefully start a save game
        if (SceneManager.GetActiveScene().name == "Bedroom")
            PlayerPrefs.SetInt("Progress" + ((ProgressPoint)0).ToString(), 1);
        
        // Force a reload on Bedroom scene reload
        SceneManager.activeSceneChanged += notify;
        StartCoroutine(waitBeforeNotify()); // initially notify (useful for testing launching the game within the bedroom in the Editor)
    }

    void OnDestroy()
    {
        SceneManager.activeSceneChanged -= notify;
        // current = null; // this should be global
    }

    private void notify(Scene oldScene, Scene newScene)
    {
        if (newScene.name == "Bedroom")
            StartCoroutine(waitBeforeNotify());
    }
    IEnumerator waitBeforeNotify()
    {
        yield return null;
        OnProgressUpdate?.Invoke();
        OnTempUpdate?.Invoke();
    }

    public bool saveDataExists()
    {
        // return PlayerPrefs.GetInt("Progress" + ((ProgressPoint)0).ToString()) == 1;
        return PlayerPrefs.GetInt("Progress" + ((ProgressPoint)1).ToString()) == 1; // HACK: To avoid uselss "Continue" buttons for no progress made
    }

    public void startAnew()
    {
        foreach (int i in Enum.GetValues(typeof(ProgressPoint)))
            setDone((ProgressPoint)i, i == 0 ? true : false);
        changeTemp(1);
    }

    public bool advance(ProgressPoint point)
    {
        // Make sure everything up to this point is complete
        for (int i = 0; i < (int)point; i++)
            if (!isDone((ProgressPoint)i))
                return false;

        // Abort if already done
        if (isDone(point))
            return false;
        
        // Register the change and notify subscribers, return true to indicate it was effective
        setDone(point, true);
        OnProgressUpdate?.Invoke();
        return true;
    }

    private void setDone(ProgressPoint point, bool done)
    {
        PlayerPrefs.SetInt("Progress" + point.ToString(), done ? 1 : 0);
    }

    public bool isDone(ProgressPoint point)
    {
        return PlayerPrefs.GetInt("Progress" + point.ToString()) != 0;
    }
    public bool getDone(ProgressPoint point) // synonym for isDone(ProgressPoint)
    {
        return isDone(point);
    }

    public void changeTemp(int temp)
    {
        if (getTemp() != temp)
        {
            if (temp == 0)
                temp = 1;
            PlayerPrefs.SetInt("CurrentTemp", temp);
            OnTempUpdate?.Invoke();
        }
    }
    public void setTemp(int temp) // synonym for changeTemp(int)
    {
        changeTemp(temp);
    }

    public int getTemp()
    {
        return PlayerPrefs.GetInt("CurrentTemp");
    }
}
