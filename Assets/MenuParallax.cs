using UnityEngine;

public class MenuParallax : MonoBehaviour
{
    public float offsetMultiplier = 1f;
    public float smoothTime = 0.3f;
    
    public Camera targetCamera; 

    private Vector3 startPosition;
    private Vector3 velocity;

    private void Start()
    {
        startPosition = transform.position;
        
        if (targetCamera == null) 
            targetCamera = Camera.main;
    }

    private void LateUpdate()
    {
        if (targetCamera != null)
        {
            Vector3 viewportPoint = targetCamera.ScreenToViewportPoint(Input.mousePosition);
            
            Vector2 offset = new Vector2(viewportPoint.x - 0.5f, viewportPoint.y - 0.5f);
            
            Vector3 targetPosition = startPosition + new Vector3(offset.x * offsetMultiplier, offset.y * offsetMultiplier, 0f);
            
            transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothTime);
        }
    }
}