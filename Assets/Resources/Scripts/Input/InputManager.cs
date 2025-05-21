using UnityEngine;
using System.Collections.Generic;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using Finger = UnityEngine.InputSystem.EnhancedTouch.Finger;

public class InputManager : MonoBehaviour
{
    public static InputManager Instance { get; set; }

    private Camera cam;


    Dictionary<Finger, Transform> selectedObjects = new Dictionary<Finger, Transform>();
    Dictionary<Finger, Rigidbody2D> rigidbodies = new Dictionary<Finger, Rigidbody2D>();
    Dictionary<Finger, TrailRenderer> trails = new Dictionary<Finger, TrailRenderer>();
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

        if (hit != null && hit.CompareTag("Player"))
        {
            Transform selected = hit.transform;
            selectedObjects[finger] = selected;

            TrailRenderer trail = selected.GetComponent<TrailRenderer>();
            if (trail != null)
            {
                trail.enabled = true;
                trails[finger] = trail;
            }
            
            Rigidbody2D rb = selected.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rigidbodies[finger] = rb;
            }
        }
        else
        {
            Debug.Log("Touch null");
        }
    }

    void OnFingerMove(Finger finger)
    {
        if (!selectedObjects.ContainsKey(finger)) return;

        Vector3 worldPos = ScreenToWorld(cam, finger.screenPosition);
        
        if (rigidbodies.TryGetValue(finger, out Rigidbody2D rb))
        {
            rb.MovePosition(worldPos);
        }
    }

    void OnFingerUp(Finger finger)
    {
        if (trails.TryGetValue(finger, out TrailRenderer trail))
        {
            trail.enabled = false;
        }

        selectedObjects.Remove(finger);
        trails.Remove(finger);
        rigidbodies.Remove(finger);
    }

    Vector3 ScreenToWorld(Camera cam, Vector2 screenPos)
    {
        Vector3 pos = new Vector3(screenPos.x, screenPos.y, cam.WorldToScreenPoint(Vector3.zero).z);
        return cam.ScreenToWorldPoint(pos);
    }
}
