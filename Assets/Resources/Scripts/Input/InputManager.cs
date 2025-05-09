using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Finger = UnityEngine.InputSystem.EnhancedTouch.Finger;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; set; }

    private Camera cam;
    private Transform selectedObject;
    private TrailRenderer trail;
    Rigidbody2D rb;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); // Destroy duplicate
        }
        else
        {
            Instance = this;
        }
    }
    private void OnEnable()
    {
        cam = Camera.main;
        EnhancedTouchSupport.Enable();
        Touch.onFingerDown += OnFingerDown;
        Touch.onFingerMove += OnFingerMove;
        Touch.onFingerUp += OnFingerUp;
    }

    private void OnDisable()
    {
        Touch.onFingerDown -= OnFingerDown;
        Touch.onFingerMove -= OnFingerMove;
        Touch.onFingerUp -= OnFingerUp;
        EnhancedTouchSupport.Disable();
    }

    void OnFingerDown(Finger finger)
    {
        Vector3 worldPos = ScreenToWorld(cam, finger.screenPosition);
        Collider2D hit = Physics2D.OverlapPoint(worldPos);

        if (hit.CompareTag("Player"))
        {
            selectedObject = hit.transform;
            trail = selectedObject.GetComponent<TrailRenderer>();
            if (trail != null)
                trail.enabled = true;
        }
        else if (hit == null)
        {
            return;
        }
    }

    void OnFingerMove(Finger finger)
    {
        if (selectedObject == null) return;

        Vector3 worldPos = ScreenToWorld(cam, finger.screenPosition);
        rb = selectedObject.GetComponent<Rigidbody2D>();
        rb.MovePosition(worldPos);
    }

    void OnFingerUp(Finger finger)
    {
        if (selectedObject != null && trail != null)
            trail.enabled = false;

        selectedObject = null;
        trail = null;
    }

    Vector3 ScreenToWorld(Camera cam, Vector2 screenPos)
    {
        Vector3 pos = new Vector3(screenPos.x, screenPos.y, cam.WorldToScreenPoint(Vector3.zero).z);
        return cam.ScreenToWorldPoint(pos);
    }
}
