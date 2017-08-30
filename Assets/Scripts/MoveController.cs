using UnityEngine;
using System.Collections;

public class MoveController : MonoBehaviour {

    Animator ani;

    NavMeshAgent nav;

    float h, v;

    Vector3 moveVec;

    // Use this for initialization
    void Start () {
        ani = GetComponent<Animator>();
        nav = GetComponent<NavMeshAgent>();
    }
	
	// Update is called once per frame
	void Update () {

        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        moveVec = new Vector3(h, 0, v);

        if (h != 0 || v != 0)
        {
            ani.SetBool("Run", true);
            // 根据摄像机方向 进行移动
            moveVec = Quaternion.Euler(0, Camera.main.transform.eulerAngles.y, 0) * moveVec;
            nav.Move(moveVec * Time.deltaTime * 5);
            RotatePlayer();
        }
        else
        {
            ani.SetBool("Run", false);
        }
	}

    private void RotatePlayer()
    {
        //向量v围绕y轴旋转cameraAngle.y度
        Vector3 vec = Quaternion.Euler(0, 0, 0) * moveVec;
        if (vec == Vector3.zero)
            return;
        Quaternion qua = Quaternion.LookRotation(vec);
        transform.rotation = Quaternion.Lerp(transform.rotation, qua, Time.deltaTime * 100);
    }
}
