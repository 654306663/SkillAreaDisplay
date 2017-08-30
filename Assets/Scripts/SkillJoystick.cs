using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

public class SkillJoystick : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IDragHandler {

    public float outerCircleRadius = 100;

    Transform innerCircleTrans;

    Vector2 outerCircleStartWorldPos = Vector2.zero;

    public Action<Vector2> onJoystickDownEvent;     // 按下事件
    public Action onJoystickUpEvent;     // 抬起事件
    public Action<Vector2> onJoystickMoveEvent;     // 滑动事件

    void Awake()
    {
        innerCircleTrans = transform.GetChild(0);
    }

    void Start()
    {
        outerCircleStartWorldPos = transform.position;
    }

    /// <summary>
    /// 按下
    /// </summary>
    public void OnPointerDown(PointerEventData eventData)
    {
        innerCircleTrans.position = eventData.position;
        if (onJoystickDownEvent != null)
            onJoystickDownEvent(innerCircleTrans.localPosition / outerCircleRadius);
    }

    /// <summary>
    /// 抬起
    /// </summary>
    public void OnPointerUp(PointerEventData eventData)
    {
        innerCircleTrans.localPosition = Vector3.zero;
        if (onJoystickUpEvent != null)
            onJoystickUpEvent();
    }

    /// <summary>
    /// 滑动
    /// </summary>
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 touchPos = eventData.position - outerCircleStartWorldPos;
        if (Vector3.Distance(touchPos, Vector2.zero) < outerCircleRadius)
            innerCircleTrans.localPosition = touchPos;
        else
            innerCircleTrans.localPosition = touchPos.normalized * outerCircleRadius;

        if (onJoystickMoveEvent != null)
            onJoystickMoveEvent(innerCircleTrans.localPosition / outerCircleRadius);
    }
}
