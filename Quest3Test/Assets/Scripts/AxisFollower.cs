using UnityEngine;

public class AxisFollower : MonoBehaviour
{
    [Header("跟随设置")]
    public Transform target;
    public float followSpeed = 5f;
    
    [Header("轴向选择")]
    public bool followX = false;
    public bool followY = false;
    public bool followZ = true;
    
    [Header("平滑选项")]
    public bool useSmoothDamp = true;
    public float smoothTime = 0.3f;  // 用于SmoothDamp
    
    private Vector3 velocity = Vector3.zero;
    
    void Update()
    {
        if (target == null) return;
        
        Vector3 currentPos = transform.position;
        Vector3 targetPos = target.position;
        
        // 根据设置选择要跟随的轴
        float newX = followX ? targetPos.x : currentPos.x;
        float newY = followY ? targetPos.y : currentPos.y;
        float newZ = followZ ? targetPos.z : currentPos.z;
        
        Vector3 newPosition = new Vector3(newX, newY, newZ);
        
        if (useSmoothDamp)
        {
            // 使用SmoothDamp实现更平滑的运动
            transform.position = Vector3.SmoothDamp(currentPos, newPosition, ref velocity, smoothTime);
        }
        else
        {
            // 使用Lerp平滑
            transform.position = Vector3.Lerp(currentPos, newPosition, followSpeed * Time.deltaTime);
        }
    }
}