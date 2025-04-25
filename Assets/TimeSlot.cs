using UnityEngine;
using UnityEngine.EventSystems;

public class TimeSlot : MonoBehaviour, IDropHandler
{
    public bool isFilled = false;

    public void OnDrop(PointerEventData eventData)
    {
        if (isFilled) return;

        TaskCard droppedTask = eventData.pointerDrag.GetComponent<TaskCard>();
        if (droppedTask != null)
        {
            droppedTask.MarkDropped(this); // Pass reference to self
            droppedTask.transform.SetParent(transform);
            droppedTask.transform.localPosition = Vector3.zero;
            isFilled = true;

            StressManager.Instance.ApplyStressChange(droppedTask.stressImpact);
        }
    }
}
