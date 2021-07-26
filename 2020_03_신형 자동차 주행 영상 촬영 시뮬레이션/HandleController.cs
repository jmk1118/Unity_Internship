using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 좌우 방향(a, d)를 입력받아 차량의 핸들 오브젝트를 회전시키는 클래스
/// </summary>
public class HandleController : MonoBehaviour
{
    void Update()
    {
        // a를 입력받으면 핸들을 왼쪽으로 꺾는다
        if (Input.GetKey(KeyCode.A))
        {
            if (transform.localEulerAngles.z < 50 || transform.localEulerAngles.z > 310)
                transform.Rotate(0, 0, 5);
        }
        // d를 입력받으면 핸들을 오른쪽으로 꺾는다
        else if (Input.GetKey(KeyCode.D))
        {
            if (transform.localEulerAngles.z > 310 || transform.localEulerAngles.z < 50)
                transform.Rotate(0, 0, -5);
        }
        // 핸들이 꺾인 상태에서 입력값이 없으면 천천히 원래 상태로 돌아온다.
        else
        {
            if (transform.localEulerAngles.z < 150)
                transform.Rotate(0, 0, -transform.localRotation.eulerAngles.z * Time.deltaTime);
            else if (transform.localEulerAngles.z > 210)
                transform.Rotate(0, 0, (360 - transform.localRotation.eulerAngles.z) * Time.deltaTime);
        }
    }
}
