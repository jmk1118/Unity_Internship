using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// by MK, 부품을 생산하는 공장 클래스
/// </summary>
public class FactoryMake : MonoBehaviour
{
    public GameObject firstFactory; // 첫 가공용 공장 오브젝트 
    public GameObject[] car = new GameObject[4]; // 차량 오브젝트

    int part; // 부품 번호
    float time; // 시간 저장용 변수
    public float waitTime; // 부품 생산 시간
    float firstWaitTime; // 부품 생산 시간 초기값
    int[] count = new int[4]; // 부품이 여러개가 나오도록 하기 위한 계산용 변수

    // 초기솨
    private void Start()
    {
        time = Time.time;
        count[1] = 3;
        count[2] = 3;
        count[3] = 3;
        firstWaitTime = waitTime;
    }

    void Update()
    {
        // 부품이 생산될 떄마다 차를 호출한다.
        // 단, 첫 가공 공장의 창고가 비어있어야 호출한다.
        if (Time.time > time + waitTime)
        {
            if(firstFactory.GetComponent<FactoryLock>().nowPart(3)) 
            {
                time = Time.time;
                SelectCar();
            }

        }
    }

    // 차량이 들어오면 랜덤하게 뽑은 부품을 차에 올리고 첫 가공공장으로 보낸다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player")) 
        {
            if(count[1] == 0 && count[2] == 0 && count[3] == 0) // 한 부품만 나오지 않도록 랜덤화
            {
                count[1] = 3;
                count[2] = 3;
                count[3] = 3;
            }

            part = Random.Range(1, 4);
            while (count[part] == 0) // 한 파트가 3번나오면 다른 파트가 나오도록 한다.
                part = Random.Range(1, 4);

            count[part]--;
            other.gameObject.GetComponent<CarNavi>().GoFactory(part, firstFactory);
            Debug.Log(other.name + "차량을" + firstFactory.name + "에 보냅니다.");
            // 오브젝트 위에 올리기

            other.transform.Rotate(Vector3.up * 180);
        }
    }

    int selectedCar; // 선택된 차량
    float selectedDistance; // 선택된 차량과 공장과의 거리
    float distance; // 거리 계산용 변수

    // 가장 가까운 자동차를 선택해서 데려오는 함수
    void SelectCar() 
    {
        selectedCar = 5; // 선택된 차 초기화
        selectedDistance = 100000; // 차와의 거리 초기화

        // 가장 가깝고 비어있는 차량을 탐색한다
        if (car[0].GetComponent<CarNavi>().NowEmpty() || car[0].GetComponent<CarNavi>().NextFactory() == null)
        {
            selectedCar = 0;
            selectedDistance = Vector3.Distance(transform.position, car[0].transform.position);
        }
        if (car[1].GetComponent<CarNavi>().NowEmpty() || car[1].GetComponent<CarNavi>().NextFactory() == null)
        {
            distance = Vector3.Distance(transform.position, car[1].transform.position);
            if(selectedDistance > distance)
            {
                selectedCar = 1;
                selectedDistance = distance;
            }
        }
        if (car[2].GetComponent<CarNavi>().NowEmpty() || car[2].GetComponent<CarNavi>().NextFactory() == null)
        {
            distance = Vector3.Distance(transform.position, car[2].transform.position);
            if (selectedDistance > distance)
            {
                selectedCar = 2;
                selectedDistance = distance;
            }
        }
        if (car[3].GetComponent<CarNavi>().NowEmpty() || car[3].GetComponent<CarNavi>().NextFactory() == null)
        {
            distance = Vector3.Distance(transform.position, car[3].transform.position);
            if (selectedDistance > distance)
            {
                selectedCar = 3;
                selectedDistance = distance;
            }
        }

        // 가장 가깝고 비어있는 차량이 존재하면 공장으로 호출한다
        if (selectedCar != 5)
        {
            car[selectedCar].GetComponent<NavMeshAgent>().SetDestination(transform.position);
            Debug.Log(car[selectedCar] + "를 제조공장으로 부릅니다.");
        }
    }

    // waitTime을 조절하는 슬라이더용 함수
    public void SliderChange(GameObject slider)
    {
        waitTime = firstWaitTime / slider.GetComponent<Slider>().value;
    }
}
