using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// by MK, 세 부품을 받아 합성하는 공장을 다루는 클래스
/// </summary>
public class FactoryAssemble : MonoBehaviour
{
    int apart, bpart, cpart; // 부품 A, B, C
    [SerializeField] int max; // 공장에 쟁여둘 수 있는 최대 부품 수
    [SerializeField] float assembleTime; // 부품 합성에 걸리는 시간
    float firstAssembleTime; // 맨 처음 설정한 assembleTime
    [SerializeField] GameObject text; // 텍스트를 띄울 UI 오브젝트

    Queue<float> outTIme = new Queue<float>(); // 부품 3개가 모두 모인 시간을 넣는 큐. 

    // 텍스트와 firstAssembleTime을 초기화한다.
    private void Start()
    {
        text.GetComponent<TextMeshProUGUI>().text = "0/0/0";
        firstAssembleTime = assembleTime;
    }

    private void Update()
    {
        // 부품 3개가 모여 outTime큐에 값이 들어있는지 확인한다.
        if(outTIme.Count > 0)
            // outTime 맨 앞에 기록된 시간에서 assembleTime만큼의 시간이 지나면 부품 A,B,C를 하나씩 제거하고 outTime 맨 앞의 값을 제거한다.
            if (Time.time > outTIme.Peek() + assembleTime) 
            {
                apart--;
                bpart--;
                cpart--;
                text.GetComponent<TextMeshProUGUI>().text = apart + "/" + bpart + "/" + cpart;
                Debug.Log("선박이 완성되었습니다!");

                outTIme.Dequeue();
            }
    }

    // 공장에 차량이 들어오면 차량으로 옮겨진 오브젝트를 공장에 입력하고 차량을 초기위치로 돌려보낸다.
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            newPart(other.GetComponent<CarNavi>().NowPart(), other.gameObject);
            other.transform.Rotate(Vector3.up * 180);
            other.GetComponent<CarNavi>().GoHome();
        }
    }

    // 차량으로 옮겨진 오브젝트를 공장에 입력하고 부품 A, B, C가 모두 모였다면 outTime에 들어온 시간을 입력한다.
    public void newPart(int newpart, GameObject car)
    {
        switch(newpart)
        {
            case 1:
                apart++;
                break;
            case 2:
                bpart++;
                break;
            case 3:
                cpart++;
                break;
            default:
                break;
        }

        if (apart > outTIme.Count && bpart > outTIme.Count && cpart > outTIme.Count)
        {
            outTIme.Enqueue(Time.time);
        }

        text.GetComponent<TextMeshProUGUI>().text = apart + "/" + bpart + "/" + cpart;
    }

    // 현재 공장에 쌓인 부품의 갯수를 반환한다.
    public bool nowPart(int nextPart)
    {
        if (apart + bpart + cpart + nextPart <= max)
            return true;
        else
            return false;
    }

    // 슬라이더를 통해 assembleTime을 조작한다.
    public void SliderChange(GameObject slider)
    {
        assembleTime = firstAssembleTime / slider.GetComponent<Slider>().value;
    }
}
