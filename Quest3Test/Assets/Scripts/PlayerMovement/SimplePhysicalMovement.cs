using UnityEngine;
using UnityEngine.XR;

public class SimplePhysicalMovement : MonoBehaviour
{
    [Header("设置")]
    public float movementSmoothness = 10f;
    public bool useCharacterController = true;
    public bool resetOnStart = true;
    
    private CharacterController characterController;
    private Vector3 lastHeadPosition;
    private Vector3 velocity;
    private bool isTracking = false;
    
    void Start()
    {
        Initialize();
        
        if (resetOnStart)
        {
            ResetPosition();
        }
    }
    
    void Initialize()
    {
        // 获取或添加CharacterController
        if (useCharacterController)
        {
            characterController = GetComponent<CharacterController>();
            if (characterController == null)
            {
                characterController = gameObject.AddComponent<CharacterController>();
                characterController.height = 1.8f;
                characterController.radius = 0.3f;
                characterController.center = new Vector3(0, 0.9f, 0);
            }
        }
        
        // 确保使用房间尺度追踪
        SetupTrackingSpace();
        
        // 尝试获取头显位置
        TryInitializeTracking();
    }
    
    void SetupTrackingSpace()
    {
        try
        {
            if (XRDevice.GetTrackingSpaceType() != TrackingSpaceType.RoomScale)
            {
                XRDevice.SetTrackingSpaceType(TrackingSpaceType.RoomScale);
                Debug.Log("已设置追踪空间为RoomScale");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("设置追踪空间时出错: " + e.Message);
        }
    }
    
    void TryInitializeTracking()
    {
        try
        {
            // 使用正确的API获取头显位置
            lastHeadPosition = InputTracking.GetLocalPosition(XRNode.Head);
            isTracking = true;
            Debug.Log("物理移动追踪已初始化");
        }
        catch (System.Exception e)
        {
            Debug.LogWarning("初始化追踪失败: " + e.Message);
            isTracking = false;
        }
    }
    
    void Update()
    {
        if (!isTracking) return;
        
        // 获取当前头显位置
        Vector3 currentHeadPosition;
        
        try
        {
            currentHeadPosition = InputTracking.GetLocalPosition(XRNode.Head);
        }
        catch
        {
            // 如果获取失败，尝试重新初始化
            TryInitializeTracking();
            return;
        }
        
        // 计算移动
        Vector3 headMovement = currentHeadPosition - lastHeadPosition;
        headMovement.y = 0; // 忽略垂直移动
        
        // 添加一个小的阈值来防止微小抖动
        if (headMovement.magnitude > 0.001f)
        {
            // 转换为世界空间移动
            Vector3 worldMovement = transform.TransformDirection(headMovement);
            
            // 应用移动
            if (useCharacterController && characterController != null)
            {
                characterController.Move(worldMovement);
            }
            else
            {
                transform.Translate(worldMovement, Space.World);
            }
            
            // 可选：平滑移动
            // transform.position = Vector3.SmoothDamp(transform.position, 
            //     transform.position + worldMovement, ref velocity, 0.1f);
        }
        
        lastHeadPosition = currentHeadPosition;
    }
    
    void OnEnable()
    {
        // 订阅XR事件
        Application.onBeforeRender += OnBeforeRender;
    }
    
    void OnDisable()
    {
        Application.onBeforeRender -= OnBeforeRender;
    }
    
    void OnBeforeRender()
    {
        // 在渲染前更新位置，确保平滑
        if (isTracking)
        {
            Update();
        }
    }
    
    [ContextMenu("重置位置")]
    public void ResetPosition()
    {
        try
        {
            InputTracking.Recenter();
            lastHeadPosition = InputTracking.GetLocalPosition(XRNode.Head);
            Debug.Log("位置已重置");
        }
        catch (System.Exception e)
        {
            Debug.LogError("重置位置失败: " + e.Message);
        }
    }
    
    // 获取当前移动状态（可用于UI显示）
    public Vector3 GetCurrentMovement()
    {
        if (!isTracking) return Vector3.zero;
        
        try
        {
            Vector3 currentHeadPosition = InputTracking.GetLocalPosition(XRNode.Head);
            return currentHeadPosition - lastHeadPosition;
        }
        catch
        {
            return Vector3.zero;
        }
    }
    
    // 检查是否在VR中运行
    // public bool IsVREnabled()
    // {
    //     return XRDevice.isPresent && isTracking;
    // }
}