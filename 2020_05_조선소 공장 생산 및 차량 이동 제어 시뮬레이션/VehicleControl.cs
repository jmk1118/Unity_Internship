using EVP;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// by MK, 차량의 움직임을 구현하는 클래스
/// </summary>
public class VehicleControl : MonoBehaviour
{
    [SerializeField] GameObject[] wheel = new GameObject[10]; // 차량에 달려있는 바퀴 오브젝트
    [SerializeField] int speed; // 바퀴의 회전속도

    // 바퀴를 회전시킨다.
    void Update()
    {
        if (GetComponent<NavMeshAgent>().isStopped == false && GetComponent<NavMeshAgent>().hasPath) 
        {
            for (int i = 0; i < 10; i++)
                wheel[i].transform.Rotate(Vector3.up * speed);
        }
    }
}
