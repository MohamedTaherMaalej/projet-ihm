using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class Padlock : MonoBehaviour
{
    public event Action<bool> OnChangeOpenState;
    public string correctCombination;
    public List<PadlockWheel> wheels;
    [SerializeField] private AudioClip clickSound = null;
    [SerializeField] private AudioClip winSound = null;

    void Start()
    {
        char[] fixedCombination = {};
        foreach (var c in correctCombination)
        {
            if (!char.IsLetter(c))
            {
                Debug.LogError("Error initializing Padlock: combination must only include letters");
                Destroy(obj: this);
            }
            char l = char.ToUpper(c);
            if (!Enum.IsDefined(typeof(PadlockWheel.PadlockLetter), l.ToString()))
            {
                Debug.LogError("Error initializing Padlock: combination must only contain valid letters from padlock wheels");
                Destroy(obj: this);
            }
            fixedCombination = fixedCombination.Concat(new char[] { l }).ToArray();
        }
        if (fixedCombination.Length != wheels.ToArray().Length)
        {
            Debug.LogError("Error initializing Padlock: combination length much match number of wheels");
            Destroy(obj: this);
        }
        correctCombination = new string(fixedCombination);
        foreach (var wheel in wheels)
        {
            wheel.OnLetterChanged += checkCombination;
            wheel.OnFinishedRotating += playTickSound;
        }
        OnChangeOpenState += blockWheels;
    }
    void OnDestroy()
    {
        
        foreach (var wheel in wheels)
        {
            wheel.OnLetterChanged -= checkCombination;
            wheel.OnFinishedRotating -= playTickSound;
        }
        OnChangeOpenState -= blockWheels;
    }

    private void playTickSound()
    {
        AudioManager.current.getSource().PlayOneShot(clickSound);
    }

    private void checkCombination()
    {
        string combination = getCombination();
        bool correct = combination.Equals(correctCombination);
        OnChangeOpenState?.Invoke(correct);
        AudioManager.current.getSource().PlayOneShot(clickSound);
        if (correct)
        {
            AudioManager.current.getSource().PlayOneShot(winSound);
        }
    }

    public string getCombination()
    {
        string combination = "";
        foreach (var wheel in wheels)
            combination += wheel.getLetter().ToString();
        return combination;
    }

    private void blockWheels(bool blocked)
    {
        setWheelsInteractable(!blocked);
    }

    public void setWheelsInteractable(bool interactable)
    {
        foreach (PadlockWheel wheel in wheels)
        {
            wheel.interactable = interactable;
            wheel.GetComponent<XRSimpleInteractable>().enabled = interactable;
        }
    }
}
