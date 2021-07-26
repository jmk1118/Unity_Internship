using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Intern2020;

/// <summary>
/// by MK, 차량 부품별로 지정 위치에 분해하여 차례대로 카메라에 비추는 클래스
/// </summary>
public class DoSomething : MonoBehaviour
{
    private PartsList p = new PartsList(); // 부품 리스트
    public GameObject car; // 차량 오브젝트
    private string path; // 부품 리스트가 저장되어있는 json파일 경로
    [SerializeField] bool useThree = true; // 어떤 json파일을 쓸 것인지 정하는 불리안 변수
    GameObject root; // 차량 오브젝트를 자식으로 넣을 빈 오브젝트
    [SerializeField] float nowChild = 0; // 카메라가 비추기 시작하는 자식 오브젝트 넘버
    [SerializeField] int videoSpeed = 0; // 카메라가 움직이는 속도
    [SerializeField] bool playVideo = false; // 일시정지용 불리안 변수

    private void Start()
    {
        // useThree 변수에 따라 가져오는 json 파일을 변경
        if (useThree)
            path = "SimplyPartsList3.json";
        else
            path = "SimplyPartsList.json";
        Debug.Log(path);

        // 차량오브젝트를 가져와서 부품 리스트에 따라 부품별로 정렬
        p.Deserialize(path);
        p.SplitPart(car);

        // 차량 오브젝트를 루트 오브젝트의 자식으로 넣음
        root = GameObject.Find("Root");
        for (int i = 0; i < root.transform.childCount; i++)
        {
            root.transform.GetChild(i).position += Vector3.right * 5 * i;
        }
        Debug.Log(root.transform.childCount);
    }

    private void Update()
    {
        // 카메라를 오른쪽(부품이 늘어진 방향)으로 이동
        transform.position = Vector3.right * nowChild * 5;

        // playVideo가 true라면 카메라를 오른쪽으로 이동
        if (playVideo)
            if (nowChild < root.transform.childCount)
                nowChild += videoSpeed * Time.deltaTime;
    }
}
