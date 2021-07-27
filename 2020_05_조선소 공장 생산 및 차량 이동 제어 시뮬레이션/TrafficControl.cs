using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// by MK, 교차로에서 차량을 통제하는 클래스
/// </summary>
public class TrafficControl : MonoBehaviour
{
    bool driving; // 현재 교차로에 차량이 있는지 여부
    Queue<GameObject> car = new Queue<GameObject>(); // 차량 오브젝트들
    GameObject nowCar; // 현재 주행중인 차량

    // 초기화
    private void Start()
    {
        driving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        // 차량이 교차로에 들어오면 실행된다.
        if (other.gameObject.CompareTag("Player")) 
        {
            // 도로에 아무도 없으면 driving을 true로 변환하여 주행중인 차가 있음을 표시한다.
            if (driving == false) 
            {
                driving = true;
                nowCar = other.gameObject;
                Debug.Log(this.name + "에" + other.name + " 들어왔습니다!");
            }
            // 도로에 주행중인 차량이 있으면 일시 정지한다.
            else
            {
                if(nowCar.GetComponent<NavMeshAgent>().isStopped == false && other.gameObject != nowCar)
                {
                    other.gameObject.GetComponent<NavMeshAgent>().isStopped = true;
                    car.Enqueue(other.gameObject);
                    Debug.Log(this.name + "에서" + other.name + "차량을 대기시킵니다!");
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // 차량이 교차로에 들어오면 실행된다.
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(this.name + "에서" + other + "차량이 나갔습니다.");
            // 주행중이던 차가 나간 후 대기중인 차량이 있으면 가장 먼저 대기한 차량을 주행상태로 바꾼다.
            if (car.Count != 0) 
            {
                nowCar = car.Dequeue();
                nowCar.GetComponent<NavMeshAgent>().isStopped = false;
                Debug.Log(this.name + "에서" + nowCar + "차량이 주행을 시작합니다.");
            }
            // 대기중인 차량이 없을 경우 주행중인 차가 없음을 표시한다.
            else
                driving = false;
        }
    }
}
