using UnityEngine;

/// <summary>
/// STARTER — A collectable orb for the "Orb Collector" lab goal.
/// Attach to a small sphere (scale 0.6), give it a Collider with
/// "Is Trigger" checked, and tag the object "Orb".
///
/// The orb gently bobs and spins every frame. Collection on trigger entry
/// is handled by the player (see PlayerController TODO 2).
/// </summary>
public class Orb : MonoBehaviour
{
    [Header("Bobbing & Spin")]
    [SerializeField] private float bobHeight = 0.15f;
    [SerializeField] private float spinSpeed = 150f;

    private Vector3 basePosition;
    private Camera dragCamera;
    private float dragDepth;
    private Vector3 dragOffset;
    private bool isDragging;

    void Awake()
    {
        basePosition = transform.position;
    }

    void Update()
    {
        if (!isDragging)
        {
            // Bob vertically around the orb's current resting position.
            float bob = Mathf.Sin(Time.time * 3f) * bobHeight;
            transform.position = basePosition + Vector3.up * bob;
        }

        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
    }

    private void OnMouseDown()
    {
        dragCamera = Camera.main;
        if (dragCamera == null)
        {
            Debug.LogWarning("[Orb] Cannot drag because no camera is tagged MainCamera.");
            return;
        }

        Vector3 screenPosition = dragCamera.WorldToScreenPoint(transform.position);
        dragDepth = screenPosition.z;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = dragDepth;
        dragOffset = transform.position - dragCamera.ScreenToWorldPoint(mousePosition);
        isDragging = true;
    }

    private void OnMouseDrag()
    {
        if (!isDragging || dragCamera == null) return;

        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z = dragDepth;

        // Make the dragged location the new bobbing centre so the orb stays
        // where it is released instead of snapping back to its old position.
        basePosition = dragCamera.ScreenToWorldPoint(mousePosition) + dragOffset;
        transform.position = basePosition;
    }

    private void OnMouseUp()
    {
        if (!isDragging) return;

        basePosition = transform.position;
        isDragging = false;
    }
}
