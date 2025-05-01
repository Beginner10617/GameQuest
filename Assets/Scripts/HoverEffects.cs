using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class HoverEffects : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI tmp;
    private string originalText;

    void Start()
    {
        tmp = GetComponent<TextMeshProUGUI>();
        originalText = tmp.text;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        tmp.text = $"<u>{originalText}</u>";
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        tmp.text = originalText;
    }
}
