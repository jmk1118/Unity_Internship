using Michsky.UI.ModernUIPack;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

/// <summary>
/// by MK, 부품을 받고 가공하여 다음 공장에 보내는 공장을 다루는 클래스
/// </summary>
public class FactoryLock : MonoBehaviour
{
    [SerializeField] int max; // 부품을 생산하고 쌓아둘 수 있는 최대량
    [SerializeField] float assembleTime; // 부품을 생산하는데 걸리는 시간
    float firstAssembleTime; // assembleTime 초기값
    [SerializeField] GameObject nextFactory; // 다음에 가야하는 공장 오브젝트
    [SerializeField] GameObject[] car = new GameObject[4]; // 차량 오브젝트 4대
    Queue<int> part = new Queue<int>(); // 들어온 부품들을 순서대로 넣는 큐
    Queue<float> startTime = new Queue<float>(); // 부품들이 들어온 시간을 기록하는 큐
    int sum; // 현재 가지고 있는 부품들의 수
    int oldpart; // 다음 공장으로 보낼 부품
    float time; // 시간 저장용 변수

    [SerializeField] GameObject text; // 현재 부품수를 표시할 텍스트 오브젝트

    // 초기화
    private void Start()
    {
        sum = 0;
        time = Time.time;
        firstAssembleTime = assembleTime;

        text.GetComponent<TextMeshProUGUI>().text = "0/" + max;
    }

    private void Update()
    {
        // 현재 공장이 부품을 가지고 있고 부품을 공장에 넣은지 일정 시간이 지났을 때, 차량을 호출한다.
        if (startTime.Count != 0 && Time.time > time + 5.0f)
        {
            time = Time.time;

            if(nextFactory.name == "Position_InFinal")
            {
                if (Time.time > startTime.Peek() + assembleTime && nextFactory.GetComponent<FactoryAssemble>().nowPart(part.Peek()))
                    SelectCar();
            }
            else
            {
                if (Time.time > startTime.Peek() + assembleTime && nextFactory.GetComponent<FactoryLock>().nowPart(part.Peek()))
                    SelectCar();
            }

        }
    }

    // 차량이 공장에 오면 차량이 가지고 있던 부품을 공장에 집어넣는다.
    // 공장에 가공이 끝난 부품이 있다면 부품을 차량에 얹어 다음 공장으로 보내고, 없다면 초기 위치로 돌려보낸다.
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            other.transform.Rotate(Vector3.up * 180);
            if (other.gameObject.GetComponent<CarNavi>().NowEmpty() == false)
                newPart(other.GetComponent<CarNavi>().NowPart());

            if (startTime.Count > 0 && Time.time > startTime.Peek() + assembleTime)
            {
                if (nextFactory.name == "Position_InFinal")
                    nextFactory.GetComponent<FactoryAssemble>().nowPart(part.Peek());
                else
                    nextFactory.GetComponent<FactoryLock>().nowPart(part.Peek());

                oldPart(other.gameObject);
            }
            else
            {
                other.GetComponent<CarNavi>().GoHome();
                Debug.Log(other.name + "는 집으로 갑니다");
            }
        }
    }

    // 차량으로부터 새 부품을 받는다
    public void newPart(int newpart)
    {
        part.Enqueue(newpart);
        startTime.Enqueue(Time.time);
        sum += newpart;

        text.GetComponent<TextMeshProUGUI>().text = sum + "/" + max;

    }

    // 가공이 끝난 부품을 차량에 얹어 다음 공장으로 보낸다
    public void oldPart(GameObject car)
    {
        oldpart = part.Dequeue();
        startTime.Dequeue();
        sum -= oldpart;
        //오브젝트를 생성해서 트랜스포터 위에 올리는것으로 변경
        car.GetComponent<CarNavi>().GoFactory(oldpart, nextFactory);

        text.GetComponent<TextMeshProUGUI>().text = sum + "/" + max;

        Debug.Log(startTime.Count + "시간 " + part.Count + "부품");
    }

    // 현재 공장에 새 부품이 들어갈 여유가 있는지 여부를 반환한다.
    public bool nowPart(int nextPart)
    {
        if (sum + nextPart <= max)
            return true;
        else
            return false;
    }

    
    int selectedCar; // 선택된 차량
    float selectedDistance; // 선택된 차량과 공장간의 거리
    float distance; // 거리 계산용 변수

    // 가장 가까운 자동차를 선택해서 데려오는 함수
    void SelectCar() 
    {
        // 이 공장으로 오고있는 차가 있다면 차를 부르지 않는다.
        if (car[0].GetComponent<CarNavi>().NextFactory() == gameObject) 
            return;
        if (car[1].GetComponent<CarNavi>().NextFactory() == gameObject)
            return;
        if (car[2].GetComponent<CarNavi>().NextFactory() == gameObject)
            return;
        if (car[3].GetComponent<CarNavi>().NextFactory() == gameObject)
            return;

        selectedCar = 5; //선택된 차 초기화
        selectedDistance = 100000; //차와의 거리 초기화

        // 가장 가깝고 비어있는 차량을 찾는다
        if (car[0].GetComponent<CarNavi>().NowEmpty())
        {
            if(Vector3.Distance(transform.position, car[0].transform.position) > 50)
            selectedCar = 0;
            selectedDistance = Vector3.Distance(transform.position, car[0].transform.position);
        }
        if (car[1].GetComponent<CarNavi>().NowEmpty())
        {
            distance = Vector3.Distance(transform.position, car[1].transform.position);
            if (selectedDistance > distance && distance > 50)
            {
                selectedCar = 1;
                selectedDistance = distance;
            }
        }
        if (car[2].GetComponent<CarNavi>().NowEmpty())
        {
            distance = Vector3.Distance(transform.position, car[2].transform.position);
            if (selectedDistance > distance && distance > 50)
            {
                selectedCar = 2;
                selectedDistance = distance;
            }
        }
        if (car[3].GetComponent<CarNavi>().NowEmpty())
        {
            distance = Vector3.Distance(transform.position, car[3].transform.position);
            if (selectedDistance > distance && distance > 50)
            {
                selectedCar = 3;
                selectedDistance = distance;
            }
        }

        // 가장 가깝고 비어있는 차량이 존재한다면 해당 차량을 공장으로 호출한다.
        if (selectedCar != 5)
        {
            car[selectedCar].GetComponent<NavMeshAgent>().SetDestination(transform.position);
            Debug.Log(car[selectedCar] + "를 " + gameObject.name +"으로 부릅니다.");
        }
    }

    // assembleTime을 조절하는 슬라이더용 함수
    public void SliderChange(GameObject slider)
    {
        assembleTime = firstAssembleTime / slider.GetComponent<Slider>().value;
    }
}
