using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 오브젝트가 바람에 흔들리는 것처럼 움직이게 하는 클래스
/// </summary>
public class Wind_Control : MonoBehaviour
{
    float wind; // 바람 속도

    // Update is called once per frame
    void Update()
    {
        wind = Mathf.Cos(Time.time); //코사인 함수를 이용하여 규칙적인 좌우운동을 만든다.
        transform.position += Vector3.right * 0.5f * wind * Time.deltaTime;
    }
}
