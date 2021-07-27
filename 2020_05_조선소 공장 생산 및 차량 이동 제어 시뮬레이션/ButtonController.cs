using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// by MK, UI의 버튼에 할당할 함수들을 가지고 있는 클래스
/// </summary>
public class ButtonController : MonoBehaviour
{
    [SerializeField] GameObject[] Car = new GameObject[4]; // 차량 오브젝트 4대
    [SerializeField] GameObject[] Position = new GameObject[5]; // 목적지를 가리키는 오브젝트 5개
    NavMeshAgent agent; // 네비 agent
    Vector3 endPosition; // 목적지 포지션

    // 시작 시 0번 차량의 네비메이션 agent를 초기값으로 갖고있는다.
    private void Start()
    {
        agent = Car[0].GetComponent<NavMeshAgent>();
    }

    // 각 버튼을 누르면 해당 차량의 네비메이션 agent를 agent변수에 할당한다.
    public void Car1()
    {
        agent = Car[0].GetComponent<NavMeshAgent>();
    }
    public void Car2()
    {
        agent = Car[1].GetComponent<NavMeshAgent>();
    }
    public void Car3()
    {
        agent = Car[2].GetComponent<NavMeshAgent>();
    }
    public void Car4()
    {
        agent = Car[3].GetComponent<NavMeshAgent>();
    }

    // 각 버튼을 누르면 현재 agent에 할당된 차량을 원하는 목적지로 이동시킨다.
    public void ButtonP1()
    {
        endPosition = Position[0].transform.position;
        agent.SetDestination(endPosition);
    }
    public void ButtonP2()
    {
        endPosition = Position[1].transform.position;
        agent.SetDestination(endPosition);
    }
    public void ButtonP3()
    {
        endPosition = Position[2].transform.position;
        agent.SetDestination(endPosition);
    }
    public void ButtonP4()
    {
        endPosition = Position[3].transform.position;
        agent.SetDestination(endPosition);
    }
    public void ButtonP5()
    {
        endPosition = Position[4].transform.position;
        agent.SetDestination(endPosition);
    }
}
