using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public float smoothing = 5f;
    public Vector3 offset;

    private Transform target;
    private bool targetSet = false;
    
    void FixedUpdate()
    {
        if(!targetSet) return;
        
        Vector3 targetCameraPos = target.position + offset;
        transform.position = Vector3.Lerp(transform.position, 
            targetCameraPos, smoothing * Time.deltaTime);
    }

    public void StartFollow(Transform target)
    {
        this.target = target;
        targetSet = true;
    }
}