using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections;

public class TaskCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerClickHandler
{
    public string taskName;
    public int stressImpact;

    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    [HideInInspector] public Transform originalParent;
    private bool droppedSuccessfully = false;
    private TimeSlot currentSlot = null;

    void Awake()
    {
        originalParent = transform.parent;
        canvasGroup = GetComponent<CanvasGroup>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (currentSlot != null)
        {
            // Free up the current slot if dragging again
            currentSlot.isFilled = false;
            StressManager.Instance.ApplyStressChange(-stressImpact); // Undo stress
            currentSlot = null;
        }

        transform.SetParent(transform.root); // Bring to front
        canvasGroup.blocksRaycasts = false;
        droppedSuccessfully = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / transform.root.localScale.x;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        StartCoroutine(CheckDropSuccess());
    }

    private IEnumerator CheckDropSuccess()
    {
        yield return new WaitForEndOfFrame(); // Let OnDrop complete

        if (!droppedSuccessfully)
        {
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;
        }
    }

    public void MarkDropped(TimeSlot slot)
    {
        droppedSuccessfully = true;
        currentSlot = slot;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (currentSlot != null)
        {
            // Return to original panel
            transform.SetParent(originalParent);
            transform.localPosition = Vector3.zero;

            // Free the slot
            currentSlot.isFilled = false;
            StressManager.Instance.ApplyStressChange(-stressImpact);

            currentSlot = null;
        }
    }
}
