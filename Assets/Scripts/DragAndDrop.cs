using UnityEngine;
using UnityEngine.InputSystem;

public class DragAndDrop : MonoBehaviour
{
    private Camera mainCamera;
    public GameObject selectedObject;
    private Vector3 offset;

    [SerializeField] private string draggableTag = "Draggable";

    private PlayerControls controls;

    private Vector2 mousePosition;
    private bool isClicking;

    private void Awake()
    {
        controls = new PlayerControls();

        controls.Gameplay.Point.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        controls.Gameplay.Click.started += ctx => OnClickStart();
        controls.Gameplay.Click.canceled += ctx => OnClickEnd();
    }

    private void OnEnable()
    {
        controls.Enable();
        mainCamera = Camera.main;
    }

    private void OnDisable()
    {
        controls.Disable();
    }

    void Update()
    {
        // Inside Update()
        if (selectedObject != null && isClicking)
        {
            Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePosition);
            Rigidbody2D rb = selectedObject.GetComponent<Rigidbody2D>();

            if (rb != null)
            {
                rb.MovePosition(worldPos + (Vector2)offset);
            }
            else
            {
                selectedObject.transform.position = (Vector3) worldPos + offset; // fallback
            }
        }

    }

    private void OnClickStart()
    {
        isClicking = true;

        Vector2 worldPos = mainCamera.ScreenToWorldPoint(mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector2.zero);

        if (hit.collider != null && hit.collider.CompareTag(draggableTag))
        {
            selectedObject = hit.collider.gameObject;
            offset = selectedObject.transform.position - (Vector3)worldPos;
        }
    }

    private void OnClickEnd()
    {
        isClicking = false;
        selectedObject = null;
    }
}
