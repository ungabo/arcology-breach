using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public Transform viewTransform;
    public float interactionRange = 3f;
    public float interactionRadius = 0.18f;
    public LayerMask interactionMask = ~0;
    public KeyCode interactKey = KeyCode.E;

    private IInteractable focusedInteractable;

    private void Awake()
    {
        if (viewTransform == null && Camera.main != null)
        {
            viewTransform = Camera.main.transform;
        }
    }

    private void Update()
    {
        if (GameStateController.Instance != null && !GameStateController.Instance.IsGameplayActive)
        {
            ClearFocus();
            return;
        }

        focusedInteractable = FindFocusedInteractable();
        if (focusedInteractable == null)
        {
            ClearFocus();
            return;
        }

        HUDController.Instance?.SetInteractionPrompt(focusedInteractable.Prompt);

        if (Input.GetKeyDown(interactKey))
        {
            focusedInteractable.Interact(gameObject);
        }
    }

    private IInteractable FindFocusedInteractable()
    {
        if (viewTransform == null)
        {
            return null;
        }

        Ray ray = new Ray(viewTransform.position, viewTransform.forward);
        if (!Physics.SphereCast(ray, interactionRadius, out RaycastHit hit, interactionRange, interactionMask, QueryTriggerInteraction.Collide))
        {
            return null;
        }

        IInteractable interactable = hit.collider.GetComponentInParent<IInteractable>();
        if (interactable == null || !interactable.CanInteract(gameObject))
        {
            return null;
        }

        return interactable;
    }

    private void ClearFocus()
    {
        focusedInteractable = null;
        HUDController.Instance?.ClearInteractionPrompt();
    }
}
