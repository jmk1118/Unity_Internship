using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// by MK, 사용자에게 보여지는 카메라를 바꾸는 버튼 함수가 있는 클래스
/// </summary>
public class Camera_ButtonChange : MonoBehaviour
{
    public GameObject C1; // 카메라 1
    public GameObject C2; // 카메라 2
    public GameObject C3; // 카메라 3
    public GameObject C4; // 카메라 4
    
    /// <summary>
    /// 카메라 1을 활성화하는 함수
    /// </summary>
    public void CameraChangeC1()
    {
        C2.SetActive(false);
        C3.SetActive(false);
        C4.SetActive(false);
        

        C1.SetActive(true);
    }

    /// <summary>
    /// 카메라 2를 활성화하는 함수
    /// </summary>
    public void CameraChangeC2()
    {
        C1.SetActive(false);
        C3.SetActive(false);
        C4.SetActive(false);
        

        C2.SetActive(true);
    }

    /// <summary>
    /// 카메라 3을 활성화하는 함수
    /// </summary>
    public void CameraChangeC3()
    {
        C1.SetActive(false);
        C2.SetActive(false);
        C4.SetActive(false);
        

        C3.SetActive(true);
    }

    /// <summary>
    /// 카메라 4를 활성화하는 함수
    /// </summary>
    public void CameraChangeC4()
    {
        C1.SetActive(false);
        C2.SetActive(false);
        C3.SetActive(false);
        

        C4.SetActive(true);
    }
}
