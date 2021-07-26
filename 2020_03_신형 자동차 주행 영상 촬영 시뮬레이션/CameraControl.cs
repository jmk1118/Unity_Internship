using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// by MK, 카메라가 차량 오브젝트를 따라가도록 하는 클래스
/// </summary>
public class CameraControl : MonoBehaviour
{
    [SerializeField] GameObject car; // 차량 오브젝트
    public GameObject[] cameras = new GameObject[2]; // 카메라
    [Range(0, 1)] public int choice; // 0이나 1의 값을 가지는 choice값. 게임을 실행할때 인스펙터상에서 설정한 카메라가 켜지도록 한다.

    private void Start()
    {
        // 카메라를 초기화하기 위해 두 카메라를 모두 비활성화하고, choice로 고른 카메라를 활성화한다.
        for (int i = 0; i < 2; i++)
        {
            cameras[i].SetActive(false);
        }
        cameras[choice].SetActive(true);   
    }

    void Update()
    {
        // 카메라가 차량 오브젝트를 따라간다.
        transform.position = car.transform.position;
    }
}
