using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Tooltip : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Button button;
    public GameObject tooltip;

    void Start()
    {
        button = GetComponentInParent<Button>();
        if (button == null)
            Destroy(obj: this);
        hideTooltip();
        UIEvents.OnMenuPaneChanged += hideTooltip;
    }

    void OnDestroy()
    {
        UIEvents.OnMenuPaneChanged -= hideTooltip;
    }

    private void hideTooltip()
    {
        if (tooltip != null)
            tooltip.SetActive(false);
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (tooltip != null)
            tooltip.SetActive(false);
    }
}
