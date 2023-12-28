using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[SelectionBase]
public class PadlockWheel : MonoBehaviour, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    public static readonly Dictionary<string, int> rotations = new Dictionary<string, int>()
    {
        {"N", 0},
        {"K", 45},
        {"H", 90},
        {"E", 135},
        {"T", 180},
        {"S", 225},
        {"P", 270},
        {"O", 315}
    }; // all values should be between 0 (inclusive) and 360 (exclusive)
    public enum PadlockLetter { E, H, K, N, O, P, S, T }
    public event Action OnLetterChanged;
    public event Action OnFinishedRotating;
    public PadlockLetter startingLetter;
    private PadlockLetter letter;
    [HideInInspector] public bool interactable = true;
    private bool rotating = false;
    private static float rotationSpeed = 1000.0f;

    void Start()
    {
        setLetter(startingLetter);
    }

    void Update()
    {
        if (rotating)
        {
            float targetYaw = rotations[letter.ToString()];
            float realYaw = transform.localRotation.eulerAngles.y;
            float yaw = realYaw;
            if (Mathf.Abs(targetYaw - (realYaw - 360)) < Mathf.Abs(targetYaw - realYaw))
                yaw = realYaw - 360;
            if (Mathf.Abs(targetYaw - (realYaw + 360)) < Mathf.Abs(targetYaw - realYaw))
                yaw = realYaw + 360;
            rotating = Mathf.Abs(targetYaw - yaw) >= rotationSpeed * Time.deltaTime;
            if (rotating)
            {
                float dyaw = Mathf.Sign(targetYaw - yaw) * rotationSpeed * Time.deltaTime;
                yaw += dyaw;
                realYaw = (yaw % 360 + 360) % 360;
                transform.localRotation = Quaternion.Euler(0, realYaw, 0);
            }
            else
            {
                transform.localRotation = Quaternion.Euler(0, targetYaw, 0);
                OnFinishedRotating?.Invoke();
            }
        }
    }

    public void Click()
    {
        if (interactable && !rotating)
        {
            int letterIndex = (int)letter;
            int numberOfLetters = Enum.GetNames(typeof(PadlockLetter)).Length;
            letterIndex = ((letterIndex - 1) % numberOfLetters + numberOfLetters) % numberOfLetters;
            letter = (PadlockLetter)letterIndex;
            rotating = true;
            OnLetterChanged?.Invoke();
        }
    }

    public PadlockLetter getLetter()
    {
        return letter;
    }

    public void setLetter(PadlockLetter l)
    {
        letter = l;
        transform.localRotation = Quaternion.Euler(0, rotations[letter.ToString()], 0);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Click();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerUp(PointerEventData eventData)
    {
    }
}
