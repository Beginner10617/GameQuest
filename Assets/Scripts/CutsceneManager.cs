using System.Collections;
using Cinemachine;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [Header("Player Parameters")]
    public CinemachineVirtualCamera _virtualCamera;
    public PlayerMovement _playerMovement;
    public PlayerMovementStats _playerMovementStats;
    public Transform playerPos;

    [Header("Cutscene Parameters")]
    public static bool isCutSceneRunning;
    public Transform destinationPosition;

    public static CutsceneManager instance;

    [Header("Camera Settings")]
    public Vector3 cameraOffset = new Vector3(2f, 0f, 0f); // Adjust pan amount
    public float cameraPanSpeed = 2f;

    private CinemachineTransposer _transposer;
    private Vector3 _originalCameraOffset;

    [Header("Dialogue System")]
    public DialogueManager dialogueManager;  // Reference to your Dialogue Manager
    public Dialogue dialogue; // The dialogue to trigger after the pan

    private void Awake()
    {
        instance = this;

        _transposer = _virtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        if (_transposer == null)
        {
            Debug.LogError("CinemachineTransposer not found! Ensure the Virtual Camera's Body is set to 'Transposer'.");
            return;
        }

        _originalCameraOffset = _transposer.m_FollowOffset;
    }

    public void Cutscene1(Transform destPosition, Transform playerPos)
    {
        if (Mathf.Abs((playerPos.position - destPosition.position).x) < 0.1f)
        {
            isCutSceneRunning = false;
            _playerMovement.enabled = true;

            // Start the camera shift, then trigger dialogue
            //StartCoroutine(ShiftCameraForward());
            StartDialogue();
            return;
        }

        float x = (playerPos.position - destPosition.position).x > 0 ? -1 : 1;
        float y = 0f;
        Vector2 inputVector = new Vector2(x, y);
        _playerMovement.Move(instance._playerMovementStats.GroundAccleration, instance._playerMovementStats.GroundDeceleration, inputVector);
        _playerMovement.enabled = false;
    }

    public void FixedUpdate()
    {
        if (isCutSceneRunning)
        {
            Cutscene1(destinationPosition, playerPos);
        }
    }

    private IEnumerator ShiftCameraForward()
    {
        Vector3 targetOffset = _originalCameraOffset + cameraOffset;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            _transposer.m_FollowOffset = Vector3.Lerp(_transposer.m_FollowOffset, targetOffset, elapsedTime * cameraPanSpeed);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        _transposer.m_FollowOffset = targetOffset;

        // After panning, start the dialogue
        
    }

    private void StartDialogue()
    {
        if (dialogueManager != null && dialogue != null)
        {
            dialogueManager.StartDialogue(dialogue);
        }
        else
        {
            Debug.LogError("Dialogue Manager or Dialogue reference missing!");
        }
    }
}
