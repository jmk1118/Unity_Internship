using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 카메라 전환 버튼 클래스
/// </summary>
public class CameraChange : MonoBehaviour
{
    [SerializeField] GameObject camera1; // 카메라 1
    [SerializeField] GameObject camera2; // 카메라 2

    // 카메라 1을 키고 카메라 2를 끄는 함수
    public void cameraOne()
    {
        camera2.SetActive(false);
        camera1.SetActive(true);
    }

    // 카메라 1을 끄고 카메라 2를 키는 함수
    public void cameraTwo()
    {
        camera1.SetActive(false);
        camera2.SetActive(true);
    }
}
