using UnityEngine;

public class ThirdCamera : MonoBehaviour
{
    public Transform target = null;     // 目标玩家
    [SerializeField]
    [Range(0, 360)]
    float horizontalAngle = 270f;      // 水平角度
    [SerializeField]
    [Range(0, 20)]
    float initialHeight = 2f;    // 人物在视野内屏幕中的位置设置

    [SerializeField]
    [Range(10, 90)]
    float initialAngle = 40f;   // 初始俯视角度
    [SerializeField]
    [Range(10, 90)]
    float maxAngle = 50f;     // 最高俯视角度
    [SerializeField]
    [Range(10, 90)]
    float minAngle = 35f;     // 最低俯视角度

    float initialDistance;    // 初始化相机与玩家的距离 根据角度计算
    [SerializeField]
    [Range(1, 100)]
    float maxDistance = 20f;        // 相机距离玩家最大距离
    [SerializeField]
    [Range(1, 100)]
    float minDistance = 5f;        // 相机距离玩家最小距离

    [SerializeField]
    [Range(1, 100)]
    float zoomSpeed = 50;       // 缩放速度

    [SerializeField]
    [Range(1f, 200)]
    float swipeSpeed = 50;      // 左右滑动速度

    float scrollWheel;        // 记录滚轮数值
    float tempAngle;          // 临时存储摄像机的初始角度
    Vector3 tempVector = new Vector3();

    void Start()
    {
        InitCamera();
    }

    void Update()
    {
        ZoomCamera();
        SwipeScreen();
    }    

    void LateUpdate()
    {
        FollowPlayer();
        RotateCamera();
    }

    /// <summary>
    /// 初始化 相机与玩家距离
    /// </summary>
    void InitCamera()
    {
        tempAngle = initialAngle;

        initialDistance = Mathf.Sqrt((initialAngle - minAngle) / Calculate()) + minDistance;

        initialDistance = Mathf.Clamp(initialDistance, minDistance, maxDistance);

    }

    /// <summary>
    /// 相机跟随玩家
    /// </summary>
    void FollowPlayer()
    {
        float upRidus = Mathf.Deg2Rad * initialAngle;
        float flatRidus = Mathf.Deg2Rad * horizontalAngle;

        float x = initialDistance * Mathf.Cos(upRidus) * Mathf.Cos(flatRidus);
        float z = initialDistance * Mathf.Cos(upRidus) * Mathf.Sin(flatRidus);
        float y = initialDistance * Mathf.Sin(upRidus);

        transform.position = Vector3.zero;
        tempVector.Set(x, y, z);
        tempVector = tempVector + target.position;
        transform.position = tempVector;
        tempVector.Set(target.position.x, target.position.y + initialHeight, target.position.z);

        transform.LookAt(tempVector);
    }

    /// <summary>
    /// 缩放相机与玩家距离
    /// </summary>
    void ZoomCamera()
    {
        scrollWheel = GetZoomValue();
        if (scrollWheel != 0)
        {
            tempAngle = initialAngle - scrollWheel * 2 * (maxAngle - minAngle);
            tempAngle = Mathf.Clamp(tempAngle, minAngle, maxAngle);
        }

        if (tempAngle != initialAngle)
        {
            initialAngle = Mathf.Lerp(initialAngle, tempAngle, Time.deltaTime * 10);

            initialDistance = Mathf.Sqrt((initialAngle - minAngle) / Calculate()) + minDistance;

            initialDistance = Mathf.Clamp(initialDistance, minDistance, maxDistance);
        }
    }

    float Calculate()
    {
        float dis = maxDistance - minDistance;
        float ang = maxAngle - minAngle;
        float line = ang / (dis * dis);
        return line;
    }

    bool isMousePress = false;
    Vector2 oldMousePos;
    Vector2 newMousePos;
    Vector2 mousePosOffset;
    /// <summary>
    /// 滑动屏幕 旋转相机和缩放视野
    /// </summary>
    public void SwipeScreen()
    {
        if (Input.GetMouseButtonDown(0) && !UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            oldMousePos = Vector2.zero;
            isMousePress = true;
        }
        else if(Input.GetMouseButtonUp(0))
        {
            mousePosOffset = Vector2.zero;
            isMousePress = false;
        }
        if (!isMousePress)
            return;

        newMousePos = Input.mousePosition;
        if(oldMousePos != Vector2.zero)
        {
            mousePosOffset = newMousePos - oldMousePos;
        }
        oldMousePos = newMousePos;
    }

    /// <summary>
    /// 获取缩放视野数值  1.鼠标滚轮 2.屏幕上下滑动
    /// </summary>
    /// <returns></returns>
    float GetZoomValue()
    {
        float zoomValue = 0;
        // 使用鼠标滚轮
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            zoomValue = Input.GetAxis("Mouse ScrollWheel");
        }
        else if (mousePosOffset != Vector2.zero)
        {
            zoomValue = mousePosOffset.y * Time.deltaTime * zoomSpeed * 0.01f;
        }

        return zoomValue;
    }

    float xVelocity = 0;
    /// <summary>
    /// 旋转相机
    /// </summary>
    void RotateCamera()
    {
        horizontalAngle = Mathf.SmoothDamp(horizontalAngle, horizontalAngle + mousePosOffset.x * Time.deltaTime * swipeSpeed, ref xVelocity, 0.1f);
    }
}