using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scenario : MonoBehaviour
{
    public static Scenario current;
    public GameObject[] endGameShow;
    public GameObject[] endGameHide;
    public Transform enterGameLocation;
    public Transform defaultStartLocation;
    public Transform temp2StartLocation;
    public Transform afterRushHourStartLocation;
    public AudioClip notificationSound;
    public AudioClip newTempSound;
    public AudioClip changeTempSound;
    public AudioClip disappointedSound;
    public AudioClip findMoreMedicineVoiceLine;
    public AudioClip afterRushHourVoiceLine;
    public AudioClip beforeFinalPhoneCallVoiceLine;
    public InteractableObjectEssential diary;
    private bool alreadyPlayedNotificationSoundsTemp1 = false;

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
        StartCoroutine(Welcome()); // handle the player arriving/loading into the scene
        // HACK: Make the diary red at start
        switch (ProgressManager.current.getTemp())
        {
            case 2:
                diary.donePoint = ProgressManager.ProgressPoint.TookEmptyMedicine;
                break;
            case 1:
            default:
                diary.donePoint = ProgressManager.ProgressPoint.ViewedLockedPhone;
                break;
        }
    }
    IEnumerator Welcome()
    {
        yield return null;
        if (!ProgressManager.current.isDone(ProgressManager.ProgressPoint.ViewedLockedPhone))
        {
            Camera.main.transform.parent.parent.position = enterGameLocation.position;
            Camera.main.transform.parent.parent.rotation = enterGameLocation.rotation;
            playNotificationSounds(1);
        }
        else
        {
            switch (ProgressManager.current.getTemp())
            {
                case 2:
                    Camera.main.transform.parent.parent.position = temp2StartLocation.position;
                    Camera.main.transform.parent.parent.rotation = temp2StartLocation.rotation;
                    if (!ProgressManager.current.isDone(ProgressManager.ProgressPoint.TookEmptyMedicine))
                    {
                        playNotificationSounds(2);
                    }
                    else if (ProgressManager.current.isDone(ProgressManager.ProgressPoint.SolvedRushHour)
                    && !ProgressManager.current.isDone(ProgressManager.ProgressPoint.CalledCorrectNumber))
                    {
                        Camera.main.transform.parent.parent.position = afterRushHourStartLocation.position;
                        Camera.main.transform.parent.parent.rotation = afterRushHourStartLocation.rotation;
                        yield return new WaitForSeconds(1);
                        AudioManager.current.getSource().PlayOneShot(afterRushHourVoiceLine);
                        yield return new WaitForSeconds(2);
                        playNotificationSounds(2);
                        yield return new WaitForSeconds(6);
                        AudioManager.current.getSource().PlayOneShot(beforeFinalPhoneCallVoiceLine);
                    }
                    break;
                case 1:
                default:
                    Camera.main.transform.parent.parent.position = defaultStartLocation.position;
                    Camera.main.transform.parent.parent.rotation = defaultStartLocation.rotation;
                    break;
            }
            if (ProgressManager.current.isDone(ProgressManager.ProgressPoint.SolvedHanoiTower)
            && !ProgressManager.current.isDone(ProgressManager.ProgressPoint.SleptToTemp2))
            {
                yield return new WaitForSeconds(0.5f);
                AudioManager.current.getSource().PlayOneShot(newTempSound);
            }
        }
        float dyaw = Camera.main.transform.rotation.eulerAngles.y - Camera.main.transform.parent.parent.rotation.eulerAngles.y;
        Camera.main.transform.parent.parent.Rotate(0, -dyaw, 0);
    }

    void OnDestroy()
    {
        current = null;
    }

    public void playNotificationSounds(int temp)
    {
        if (temp == 1 && alreadyPlayedNotificationSoundsTemp1)
            return;
        alreadyPlayedNotificationSoundsTemp1 = true;
        StartCoroutine(playDelayedNotificationSounds(temp));
    }
    IEnumerator playDelayedNotificationSounds(int temp)
    {
        for (int i = 0; i < temp; i++)
        {
            yield return new WaitForSeconds(2);
            AudioManager.current.getSource().PlayOneShot(notificationSound);
            yield return new WaitForSeconds(0.5f);
            AudioManager.current.getSource().PlayOneShot(notificationSound);
        }
    }

    public void sleepToTemp2()
    {
        if (ProgressManager.current.getTemp() == 2)
            return;
        AudioManager.current.getSource().PlayOneShot(changeTempSound);
        ProgressManager.current.advance(ProgressManager.ProgressPoint.SleptToTemp2);
        SceneManager.LoadScene("TransitionToTemp2", LoadSceneMode.Single); // this takes care of calling ProgressManager to change temps
    }

    public void tookEmptyMedicine()
    {
        if (ProgressManager.current.isDone(ProgressManager.ProgressPoint.TookEmptyMedicine))
            return;
        AudioManager.current.getSource().PlayOneShot(disappointedSound);
    }

    public void releasedEmptyMedicine()
    {
        if (ProgressManager.current.isDone(ProgressManager.ProgressPoint.TookEmptyMedicine))
            return;
        AudioManager.current.getSource().PlayOneShot(findMoreMedicineVoiceLine);
        ProgressManager.current.advance(ProgressManager.ProgressPoint.TookEmptyMedicine);
    }

    public void toggleEndGameVisibilities()
    {
        bool solved = ProgressManager.current.isDone(ProgressManager.ProgressPoint.SolvedRushHour);
        foreach (GameObject go in endGameShow)
            go.SetActive(solved);
        foreach (GameObject go in endGameHide)
            go.SetActive(!solved);
    }

    public void finishGame()
    {
        StartCoroutine(endSequence());
    }
    IEnumerator endSequence()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("TransitionEndGame", LoadSceneMode.Single);
    }
}
