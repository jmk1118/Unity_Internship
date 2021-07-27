using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// by MK, 차량이 초기위치에 들어왔을 때 초기 위치, 회전값으로 초기화하는 클래스
/// </summary>
public class Parking : MonoBehaviour
{
    // 차량이 초기 위치에 들어오면 위치, 회전값을 초기화한다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.GetComponent<NavMeshAgent>().ResetPath();
            other.transform.position = other.GetComponent<CarNavi>().StartPosition();
            other.transform.rotation = other.GetComponent<CarNavi>().StartRotation();
        }
    }
}
