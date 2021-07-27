using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// by MK, 차량의 네비게이션 기능을 활용해 초기 위치, 지정 공장으로 이동시키는 클래스
/// </summary>
public class CarNavi : MonoBehaviour
{
    NavMeshAgent agent; // 네비게이션 agent
    bool empty; // 차량이 부품을 지고있는지 여부
    int part; // 부품 넘버
    [SerializeField] GameObject startPosition; // 차량 위치 초기값
    Quaternion startRotation; // 차량 회전 초기값
    [SerializeField] GameObject[] Part = new GameObject[3]; // 차량에 올라간 부품 오브젝트
    GameObject nextFactory; // 차량이 향할 다음 공장
    GameObject nowPart; // 현재 차량이 지고있는 부품

    float startSpeed; // 차량 속도
    float startAngularSpeed; // 차량 회전속도
    
    // 초기값 설정
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        startRotation = transform.rotation;
        part = 0;
        empty = true;
        nextFactory = null;

        startSpeed = agent.speed;
        startAngularSpeed = agent.angularSpeed;
    }

    // 지고있던 부품을 없애고 초기 위치로 돌려보내는 함수
    public void GoHome()
    {
        Destroy(nowPart);
        agent.SetDestination(startPosition.transform.position);
        part = 0;
        nextFactory = null;
        empty = true;
        Debug.Log("집에 간다!");
    }

    // 인자로 받은 부품을 싣고 지정된 공장으로 보내는 함수
    public void GoFactory(int newpart, GameObject nextPositon)
    {
        Destroy(nowPart);
        part = newpart;
        nowPart = Instantiate(Part[newpart - 1], transform.GetChild(2).position, transform.rotation);
        nowPart.transform.parent = this.transform;
        nextFactory = nextPositon;
        empty = false;
        agent.SetDestination(nextPositon.transform.position);
    }

    // 현재 지고있는 부품을 반환하는 함수
    public int NowPart()
    {
        return part;
    }

    // 현재 부품이 있는지 여부를 반환하는 함수
    public bool NowEmpty()
    {
        return empty;
    }

    // 현재 향하고 있는 공장이 어딘지 반환하는 함수
    public GameObject NextFactory()
    {
        return nextFactory;
    }

    // 회전 초기값을 반환하는 함수
    public Quaternion StartRotation()
    {
        return startRotation;
    }

    // 위치 초기값을 반환하는 함수
    public Vector3 StartPosition()
    {
        return startPosition.transform.position;
    }

    // 속도, 회전속도를 조절하는 슬라이더에 할당된 함수
    public void SliderChange(GameObject slider)
    {
        agent.speed = startSpeed * slider.GetComponent<Slider>().value;
        agent.angularSpeed = startAngularSpeed * slider.GetComponent<Slider>().value;
    }
}