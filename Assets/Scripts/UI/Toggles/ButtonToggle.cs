using UnityEngine;
using UnityEngine.UI;

public class ButtonToggle : MonoBehaviour
{
    private Image image;
    public Sprite offSprite;
    public Sprite onSprite;

    protected void Start()
    {
        Button button = GetComponentInParent<Button>();
        if (button == null)
            Destroy(obj: this);
        Transform childTransform = button.transform.Find("Image");
        if (childTransform != null)
        {
            image = childTransform.GetComponent<Image>();
            if (image == null)
                Destroy(obj: this);
        }
        else
            Destroy(obj: this);
    }

    protected void selfUpdate(bool value)
    {
        if (image != null)
            image.sprite = value ? onSprite : offSprite;
    }
}
