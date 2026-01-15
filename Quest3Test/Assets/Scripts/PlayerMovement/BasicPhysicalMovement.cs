using UnityEngine;
using UnityEngine.XR;

public class BasicPhysicalMovement : MonoBehaviour
{
    public Transform vrCamera;
    public float movementScale = 1.0f;
    
    private Vector3 lastHeadPosition;
    
    void Start()
    {
        // 自动获取VR相机
        if (vrCamera == null)
        {
            vrCamera = Camera.main?.transform;
        }
        
        if (vrCamera != null)
        {
            lastHeadPosition = vrCamera.localPosition;
        }
    }
    
    void Update()
    {
        if (vrCamera == null) return;
        
        Vector3 currentHeadPosition = vrCamera.localPosition;
        Vector3 movement = currentHeadPosition - lastHeadPosition;
        
        // 只应用水平移动
        movement.y = 0;
        
        // 移动整个游戏对象
        transform.Translate(movement * movementScale, Space.World);
        
        lastHeadPosition = currentHeadPosition;
    }
}