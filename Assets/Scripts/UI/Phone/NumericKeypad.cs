using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <remarks>
/// Buttons must be TextMeshPro-type.
/// </remarks>
public class NumericKeypad : MonoBehaviour
{
    public List<Button> numberButtons;
    public Button backspaceButton;
    public Button submitButton;
    public bool useASubmitButton = true;
    public TextMeshProUGUI display;
    public int maxInputLength = 4;
    public string expectedCorrectValue;
    public AudioClip clickSound;
    public AudioClip deleteSound;
    public AudioClip submitCorrectSound;
    public AudioClip submitIncorrectSound;
    [Tooltip("While this progress point is not reached, the correct input will be treated as incorrect.")] public ProgressManager.ProgressPoint requiredToValidate;
    public event Action<string> OnKeypadInputChanged;
    public event Action<bool> OnKeypadSubmitted; // the boolean indicates the correctness of the attempt
    private Color originalColor;

    void Start()
    {
        if (expectedCorrectValue.Length > maxInputLength)
        {
            Debug.LogWarning("Warning with a NumericKeypad component: Expected value \"" + expectedCorrectValue + "\" is of length " + expectedCorrectValue.Length + ", which exceeds the max input length, which is only " + maxInputLength + ".");
        }
        foreach (char c in expectedCorrectValue)
        {
            bool typable = false;
            foreach (Button key in numberButtons)
            {
                if (key.GetComponentInChildren<TextMeshProUGUI>().text.Contains(c))
                {
                    typable = true;
                    break;
                }
            }
            if (!typable)
            {
                Debug.LogWarning("Warning with a NumericKeypad component: Expected value \"" + expectedCorrectValue + "\" contains character '" + c + "', which cannot be typed with the provided buttons. Make sure to correct their displayed text! (this is what they actually type)");
                break;
            }
        }
        foreach (Button key in numberButtons)
        {
            key.onClick.AddListener(() => { type(key); });
        }
        backspaceButton.onClick.AddListener(() => { delete(); });
        if (useASubmitButton)
            submitButton.onClick.AddListener(() => { checkInput(); });
        else
            OnKeypadInputChanged += checkInput;
    }
    void OnDestroy()
    {
        foreach (Button key in numberButtons)
        {
            key.onClick.RemoveAllListeners();
        }
        backspaceButton.onClick.RemoveAllListeners();
        if (useASubmitButton)
            submitButton.onClick.RemoveAllListeners();
        else
            OnKeypadInputChanged -= checkInput;
    }

    void OnEnable()
    {
        display.text = ""; // it is important to put this script/component on the actual panel that gets activated/deactivated
        backspaceButton.interactable = false;
    }

    private void type(Button key)
    {
        int currentDisplayLength = display.text.Length;
        if (currentDisplayLength < maxInputLength)
        {
            display.text += key.GetComponentInChildren<TextMeshProUGUI>().text;
            backspaceButton.interactable = true;
            AudioManager.current.getSource().PlayOneShot(clickSound);
            OnKeypadInputChanged?.Invoke(display.text);
        }
    }

    private void delete()
    {
        int currentDisplayLength = display.text.Length;
        if (display.text.Length > 0)
        {
            display.text = display.text.Remove(display.text.Length - 1, 1);
            if (currentDisplayLength - 1 == 0)
                backspaceButton.interactable = false;
            AudioManager.current.getSource().PlayOneShot(deleteSound);
            OnKeypadInputChanged?.Invoke(display.text);
        }
    }

    private void checkInput()
    {
        checkInput(display.text);
    }
    private void checkInput(string input)
    {
        bool correct = input.Equals(expectedCorrectValue);
        if (correct && ProgressManager.current.isDone(requiredToValidate))
        {
            AudioManager.current.getSource().PlayOneShot(submitCorrectSound);
            originalColor = display.color;
            display.color = Color.green;
        }
        else
        {
            AudioManager.current.getSource().PlayOneShot(submitIncorrectSound);
            StartCoroutine(flashIncorrect());
        }
        OnKeypadSubmitted?.Invoke(correct);
    }
    IEnumerator flashIncorrect()
    {
        originalColor = display.color;
        display.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        display.color = originalColor;
        yield return new WaitForSeconds(0.1f);
        display.color = Color.red;
        yield return new WaitForSeconds(0.1f);
        display.color = originalColor;
    }
}
