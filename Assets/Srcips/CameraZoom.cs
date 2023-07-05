using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoom : MonoBehaviour

{
    private float zoomSpeed = 0.05f; // Giá trị zoomSpeed được giảm xuống
    public float minSize = 1f;
    public float maxSize = 37.78744f;

    private Camera mainCamera;
    private float initialDistance;
    private Vector3 initialCameraPosition;
    
    
    void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Input.touchCount == 2)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);

            if (touch1.phase == TouchPhase.Began || touch2.phase == TouchPhase.Began)
            {
                initialDistance = Vector2.Distance(touch1.position, touch2.position);
                initialCameraPosition = mainCamera.transform.position;
            }
            else if (touch1.phase == TouchPhase.Moved && touch2.phase == TouchPhase.Moved)
            {
                float currentDistance = Vector2.Distance(touch1.position, touch2.position);
                float distanceDifference = currentDistance - initialDistance;

                float zoomAmount = distanceDifference * zoomSpeed;
                float newOrthoSize = Mathf.Clamp(mainCamera.orthographicSize - zoomAmount, minSize, maxSize);

                Vector2 midPoint = (touch1.position + touch2.position) / 2f;
                Vector3 targetZoomPosition = mainCamera.ScreenToWorldPoint(new Vector3(midPoint.x, midPoint.y, mainCamera.nearClipPlane));

                mainCamera.orthographicSize = newOrthoSize;
                mainCamera.transform.position += initialCameraPosition - targetZoomPosition;
            }
        }
    }
}