using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.InputSystem;

public class SequenceController : MonoBehaviour
{
    public TeleportationAnchor teleportAnchor;
    public GameObject firstObject;
    public GameObject secondObject;
    public GameObject thirdObject;
    public Animator secondObjectAnimator;
    public string animationTriggerName = "StartAnimation";
    public string idleStateName = "Idle";
    public float animationDuration = 10.0f;
    public float positionThreshold = 0.1f;

    public InputActionReference buttonPressAction;

    private bool isSequenceRunning = false;
    private bool playerAtAnchor = false;
    private float animationTimer = 0;

    void Start()
    {
        ResetSequence();
    }

    void OnEnable()
    {
        buttonPressAction.action.Enable();
        teleportAnchor.teleporting.AddListener(OnTeleporting);
    }

    void OnDisable()
    {
        buttonPressAction.action.Disable();
        teleportAnchor.teleporting.RemoveListener(OnTeleporting);
    }

    void Update()
    {
        if (playerAtAnchor && buttonPressAction.action.triggered)
        {
            HandleButtonPress();
        }

        if (isSequenceRunning)
        {
            animationTimer -= Time.deltaTime;

            if (animationTimer <= 0)
            {
                EndSequence();
            }
        }
    }

    void OnTeleporting(TeleportingEventArgs args)
    {
        Vector3 anchorPosition = teleportAnchor.teleportAnchorTransform.position;
        Vector3 destinationPosition = args.teleportRequest.destinationPosition;

        if (Vector3.Distance(anchorPosition, destinationPosition) <= positionThreshold)
        {
            if (!playerAtAnchor)
            {
                playerAtAnchor = true;
                ResetSequence();
            }
        }
        else
        {
            if (playerAtAnchor)
            {
                playerAtAnchor = false;
                ResetSequence();
            }
        }
    }

    private void HandleButtonPress()
    {
        if (!isSequenceRunning && !thirdObject.activeSelf)
        {
            HideObject(firstObject);
            ShowObject(secondObject);

            if (secondObjectAnimator != null)
            {
                secondObjectAnimator.SetTrigger(animationTriggerName);
            }

            animationTimer = animationDuration;
            isSequenceRunning = true;
        }
        else if (!isSequenceRunning && thirdObject.activeSelf)
        {
            HideObject(thirdObject);
            HideObject(secondObject);

            if (secondObjectAnimator != null)
            {
                secondObjectAnimator.SetTrigger(animationTriggerName);
            }

            animationTimer = animationDuration;
            isSequenceRunning = true;
        }
    }

    private void EndSequence()
    {
        isSequenceRunning = false;
        animationTimer = 0;

        HideObject(secondObject);
        ShowObject(thirdObject);
    }

    private void ResetSequence()
    {
        isSequenceRunning = false;
        animationTimer = 0;

        ShowObject(firstObject);
        HideObject(secondObject);
        HideObject(thirdObject);

        if (secondObjectAnimator != null)
        {
            secondObjectAnimator.ResetTrigger(animationTriggerName);
            secondObjectAnimator.Play(idleStateName, 0, 0);
        }
    }

    private void ShowObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(true);
        }
    }

    private void HideObject(GameObject obj)
    {
        if (obj != null)
        {
            obj.SetActive(false);
        }
    }
}